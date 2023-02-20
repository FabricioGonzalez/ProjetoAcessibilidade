using System;
using System.Collections.Generic;
using System.Linq;

using Avalonia;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;

using SkiaSharp;

namespace QuestPDF.Previewer;

class InteractiveCanvas : ICustomDrawOperation
{
    public Rect Bounds
    {
        get; set;
    }
    public ICollection<PreviewPage> Pages
    {
        get; set;
    } = new List<PreviewPage>();

    public float ViewportWidth => (float)Bounds.Width;
    public float ViewportHeight => (float)Bounds.Height;

    public float Scale { get; private set; } = 1;
    public float TranslateX
    {
        get; set;
    }
    public float TranslateY
    {
        get; set;
    }

    private const float MinScale = 0.1f;
    private const float MaxScale = 10f;

    private const float PageSpacing = 25f;
    private const float SafeZone = 25f;

    public float TotalPagesHeight => Pages.Sum(x => x.Size.Height) + (Pages.Count - 1) * PageSpacing;
    public float TotalHeight => TotalPagesHeight + SafeZone * 2 / Scale;
    public float MaxWidth => Pages.Any() ? Pages.Max(x => x.Size.Width) : 0;

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
            return Math.Clamp(viewPortSize, 0, 1);
        }
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

    public (int pageNumber, float x, float y)? FindClickedPointOnThePage(float x, float y)
    {
        x -= ViewportWidth / 2;
        x /= Scale;
        x += TranslateX;

        y /= Scale;
        y += TranslateY;

        var location = GetPagePosition().FirstOrDefault
            (p => p.beginY <= y && y <= p.endY);

        if (location == default)
            return null;

        var page = Pages.ElementAt(location.pageNumber - 1);

        x += page.Size.Width / 2;

        if (x < 0 || page.Size.Width < x)
            return null;

        y -= location.beginY;

        return (location.pageNumber, x, y);
    }


    #region transformations

    private void LimitScale()
    {
        Scale = Math.Max(Scale, MinScale);
        Scale = Math.Min(Scale, MaxScale);
    }

    private void LimitTranslate()
    {
        if (TotalPagesHeight > ViewportHeight / Scale)
        {
            TranslateY = Math.Min(TranslateY, MaxTranslateY);
            TranslateY = Math.Max(TranslateY, 0);
        }
        else
        {
            TranslateY = (TotalPagesHeight - ViewportHeight / Scale) / 2;
        }

        if (ViewportWidth / Scale < MaxWidth)
        {
            var maxTranslateX = (ViewportWidth / 2 - SafeZone) / Scale - MaxWidth / 2;

            TranslateX = Math.Min(TranslateX, -maxTranslateX);
            TranslateX = Math.Max(TranslateX, maxTranslateX);
        }
        else
        {
            TranslateX = 0;
        }
    }

    public void TranslateWithCurrentScale(float x, float y)
    {
        TranslateX += x / Scale;
        TranslateY += y / Scale;

        LimitTranslate();
    }

    public void ZoomToPoint(float x, float y, float factor)
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

    public void Render(IDrawingContextImpl context)
    {
        if (!Pages.Any())
            return;

        LimitScale();
        LimitTranslate();

        /*        // Avalonia 11.0.0 preview feature method
                var skia = context.GetFeature<ISkiaSharpApiLeaseFeature>();
                using var lease = skia.Lease();

                SKCanvas canvas = lease.SkCanvas;*/
        var canvas = (context as ISkiaDrawingContextImpl)?.SkCanvas;

        if (canvas == null)
            throw new InvalidOperationException($"Context needs to be ISkiaDrawingContextImpl but got {nameof(context)}");
        var originalMatrix = canvas.TotalMatrix;

        canvas.Translate(ViewportWidth / 2, 0);

        canvas.Scale(Scale);
        canvas.Translate(TranslateX, -TranslateY + SafeZone / Scale);

        foreach (var page in Pages)
        {
            canvas.Translate(-page.Size.Width / 2f, 0);
            DrawBlankPage(canvas, page.Size);
            canvas.DrawPicture(page.Picture);
            canvas.Translate(page.Size.Width / 2f, page.Size.Height + PageSpacing);
        }

        canvas.SetMatrix(originalMatrix);
        DrawInnerGradient(canvas);
    }

    public void Dispose()
    {
    }
    public bool Equals(ICustomDrawOperation? other) => false;
    public bool HitTest(Point p) => true;

    #endregion

    #region blank page

    private static SKPaint BlankPagePaint = new SKPaint
    {
        Color = SKColors.White
    };

    private static SKPaint BlankPageShadowPaint = new SKPaint
    {
        ImageFilter = SKImageFilter.CreateBlendMode(
            SKBlendMode.Overlay,
            SKImageFilter.CreateDropShadowOnly(0, 6, 6, 6, SKColors.Black.WithAlpha(64)),
            SKImageFilter.CreateDropShadowOnly(0, 10, 14, 14, SKColors.Black.WithAlpha(32)))
    };

    private void DrawBlankPage(SKCanvas canvas, Infrastructure.Size size)
    {
        canvas.DrawRect(0, 0, size.Width, size.Height, BlankPageShadowPaint);
        canvas.DrawRect(0, 0, size.Width, size.Height, BlankPagePaint);
    }

    #endregion

    #region inner viewport gradient

    private const int InnerGradientSize = (int)SafeZone;
    private static readonly SKColor InnerGradientColor = SKColor.Parse("#666");

    private void DrawInnerGradient(SKCanvas canvas)
    {
        // gamma correction
        var colors = Enumerable
            .Range(0, InnerGradientSize)
            .Select(x => 1f - x / (float)InnerGradientSize)
            .Select(x => Math.Pow(x, 2f))
            .Select(x => (byte)(x * 255))
            .Select(x => InnerGradientColor.WithAlpha(x))
            .ToArray();

        using var fogPaint = new SKPaint
        {
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                new SKPoint(0, InnerGradientSize),
                colors,
                SKShaderTileMode.Clamp)
        };

        canvas.DrawRect(0, 0, ViewportWidth, InnerGradientSize, fogPaint);
    }

    #endregion
}
