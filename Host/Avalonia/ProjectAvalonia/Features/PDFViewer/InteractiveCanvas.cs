using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using QuestPDF.Previewer;
using SkiaSharp;
using Size = QuestPDF.Infrastructure.Size;

namespace ProjectAvalonia.Features.PDFViewer;

internal class InteractiveCanvas : ICustomDrawOperation
{
    private const float MinScale = 0.1f;
    private const float MaxScale = 10f;

    private const float PageSpacing = 25f;
    private const float SafeZone = 25f;

    public ICollection<PreviewPage> Pages
    {
        get;
        set;
    } = new List<PreviewPage>();

    public float ViewportWidth => (float)Bounds.Width;
    public float ViewportHeight => (float)Bounds.Height;

    public float Scale
    {
        get;
        private set;
    } = 1;

    public float TranslateX
    {
        get;
        set;
    }

    public float TranslateY
    {
        get;
        set;
    }

    public float TotalPagesHeight => Pages.Sum(selector: x => x.Size.Height) + (Pages.Count - 1) * PageSpacing;
    public float TotalHeight => TotalPagesHeight + SafeZone * 2 / Scale;
    public float MaxWidth => Pages.Any() ? Pages.Max(selector: x => x.Size.Width) : 0;

    public float MaxTranslateY => TotalHeight - ViewportHeight / Scale;

    public float ScrollPercentY
    {
        get => TranslateY / MaxTranslateY;
        set => TranslateY = value * MaxTranslateY;
    }

    public float ScrollViewportSizeY
    {
        get
        {
            var viewPortSize = ViewportHeight / Scale / TotalHeight;
            return Math.Clamp(value: viewPortSize, min: 0, max: 1);
        }
    }

    public Rect Bounds
    {
        get;
        set;
    }

    public IEnumerable<(int pageNumber, float beginY, float endY)> GetPagePosition()
    {
        var pageNumber = 1;
        var currentPagePosition = SafeZone / Scale;

        foreach (var page in Pages)
        {
            yield return (pageNumber, currentPagePosition, currentPagePosition + page.Size.Height);
            currentPagePosition += page.Size.Height + PageSpacing;
            pageNumber++;
        }
    }

    public (int pageNumber, float x, float y)? FindClickedPointOnThePage(
        float x
        , float y
    )
    {
        x -= ViewportWidth / 2;
        x /= Scale;
        x += TranslateX;

        y /= Scale;
        y += TranslateY;

        var location = GetPagePosition().FirstOrDefault
            (predicate: p => p.beginY <= y && y <= p.endY);

        if (location == default)
        {
            return null;
        }

        var page = Pages.ElementAt(index: location.pageNumber - 1);

        x += page.Size.Width / 2;

        if (x < 0 || page.Size.Width < x)
        {
            return null;
        }

        y -= location.beginY;

        return (location.pageNumber, x, y);
    }


    #region transformations

    private void LimitScale()
    {
        Scale = Math.Max(val1: Scale, val2: MinScale);
        Scale = Math.Min(val1: Scale, val2: MaxScale);
    }

    private void LimitTranslate()
    {
        if (TotalPagesHeight > ViewportHeight / Scale)
        {
            TranslateY = Math.Min(val1: TranslateY, val2: MaxTranslateY);
            TranslateY = Math.Max(val1: TranslateY, val2: 0);
        }
        else
        {
            TranslateY = (TotalPagesHeight - ViewportHeight / Scale) / 2;
        }

        if (ViewportWidth / Scale < MaxWidth)
        {
            var maxTranslateX = (ViewportWidth / 2 - SafeZone) / Scale - MaxWidth / 2;

            TranslateX = Math.Min(val1: TranslateX, val2: -maxTranslateX);
            TranslateX = Math.Max(val1: TranslateX, val2: maxTranslateX);
        }
        else
        {
            TranslateX = 0;
        }
    }

    public void TranslateWithCurrentScale(
        float x
        , float y
    )
    {
        TranslateX += x / Scale;
        TranslateY += y / Scale;

        LimitTranslate();
    }

    public void ZoomToPoint(
        float x
        , float y
        , float factor
    )
    {
        var oldScale = Scale;
        Scale *= factor;

        LimitScale();

        factor = Scale / oldScale;

        TranslateX -= x / (oldScale * factor) - x / oldScale;
        TranslateY -= y / (oldScale * factor) - y / oldScale;

        LimitTranslate();
    }

    #endregion

    #region rendering

    public void Render(
        IDrawingContextImpl context
    )
    {
        if (!Pages.Any())
        {
            return;
        }

        LimitScale();
        LimitTranslate();

        /*        // Avalonia 11.0.0 preview feature method
                var skia = context.GetFeature<ISkiaSharpApiLeaseFeature>();
                using var lease = skia.Lease();

                SKCanvas canvas = lease.SkCanvas;*/
        var canvas = (context as ISkiaDrawingContextImpl)?.SkCanvas;

        if (canvas == null)
        {
            throw new InvalidOperationException(
                message: $"Context needs to be ISkiaDrawingContextImpl but got {nameof(context)}");
        }

        var originalMatrix = canvas.TotalMatrix;

        canvas.Translate(dx: ViewportWidth / 2, dy: 0);

        canvas.Scale(s: Scale);
        canvas.Translate(dx: TranslateX, dy: -TranslateY + SafeZone / Scale);

        foreach (var page in Pages)
        {
            canvas.Translate(dx: -page.Size.Width / 2f, dy: 0);
            DrawBlankPage(canvas: canvas, size: page.Size);
            canvas.DrawPicture(picture: page.Picture);
            canvas.Translate(dx: page.Size.Width / 2f, dy: page.Size.Height + PageSpacing);
        }

        canvas.SetMatrix(matrix: originalMatrix);
        DrawInnerGradient(canvas: canvas);
    }

    public void Dispose()
    {
    }

    public bool Equals(
        ICustomDrawOperation? other
    ) => false;

    public bool HitTest(
        Point p
    ) => true;

    #endregion

    #region blank page

    private static readonly SKPaint BlankPagePaint = new()
    {
        Color = SKColors.White
    };

    private static readonly SKPaint BlankPageShadowPaint = new()
    {
        ImageFilter = SKImageFilter.CreateBlendMode(
            mode: SKBlendMode.Overlay,
            background: SKImageFilter.CreateDropShadowOnly(dx: 0, dy: 6, sigmaX: 6, sigmaY: 6
                , color: SKColors.Black.WithAlpha(alpha: 64)),
            foreground: SKImageFilter.CreateDropShadowOnly(dx: 0, dy: 10, sigmaX: 14, sigmaY: 14
                , color: SKColors.Black.WithAlpha(alpha: 32)))
    };

    private void DrawBlankPage(
        SKCanvas canvas
        , Size size
    )
    {
        canvas.DrawRect(x: 0, y: 0, w: size.Width, h: size.Height, paint: BlankPageShadowPaint);
        canvas.DrawRect(x: 0, y: 0, w: size.Width, h: size.Height, paint: BlankPagePaint);
    }

    #endregion

    #region inner viewport gradient

    private const int InnerGradientSize = (int)SafeZone;
    private static readonly SKColor InnerGradientColor = SKColor.Parse(hexString: "#666");

    private void DrawInnerGradient(
        SKCanvas canvas
    )
    {
        // gamma correction
        var colors = Enumerable
            .Range(start: 0, count: InnerGradientSize)
            .Select(selector: x => 1f - x / (float)InnerGradientSize)
            .Select(selector: x => Math.Pow(x: x, y: 2f))
            .Select(selector: x => (byte)(x * 255))
            .Select(selector: x => InnerGradientColor.WithAlpha(alpha: x))
            .ToArray();

        using var fogPaint = new SKPaint
        {
            Shader = SKShader.CreateLinearGradient(
                start: new SKPoint(x: 0, y: 0),
                end: new SKPoint(x: 0, y: InnerGradientSize),
                colors: colors,
                mode: SKShaderTileMode.Clamp)
        };

        canvas.DrawRect(x: 0, y: 0, w: ViewportWidth, h: InnerGradientSize, paint: fogPaint);
    }

    #endregion
}