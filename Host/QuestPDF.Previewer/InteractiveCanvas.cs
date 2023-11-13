using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;

namespace QuestPDF.Previewer;

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
    }

    private float Width => (float)Bounds.Width;
    private float Height => (float)Bounds.Height;

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

    public float TotalPagesHeight => Pages.Sum(selector: x => x.Height) + (Pages.Count - 1) * PageSpacing;
    public float TotalHeight => TotalPagesHeight + SafeZone * 2 / Scale;
    public float MaxWidth => Pages.Any() ? Pages.Max(selector: x => x.Width) : 0;

    public float MaxTranslateY => TotalHeight - Height / Scale;

    public float ScrollPercentY
    {
        get => TranslateY / MaxTranslateY;
        set => TranslateY = value * MaxTranslateY;
    }

    public float ScrollViewportSizeY
    {
        get
        {
            var viewPortSize = Height / Scale / TotalHeight;
            return Math.Clamp(value: viewPortSize, min: 0, max: 1);
        }
    }

    public Rect Bounds
    {
        get;
        set;
    }

    #region transformations

    private void LimitScale()
    {
        Scale = Math.Max(val1: Scale, val2: MinScale);
        Scale = Math.Min(val1: Scale, val2: MaxScale);
    }

    private void LimitTranslate()
    {
        if (TotalPagesHeight > Height / Scale)
        {
            TranslateY = Math.Min(val1: TranslateY, val2: MaxTranslateY);
            TranslateY = Math.Max(val1: TranslateY, val2: 0);
        }
        else
        {
            TranslateY = (TotalPagesHeight - Height / Scale) / 2;
        }

        if (Width / Scale < MaxWidth)
        {
            var maxTranslateX = (Width / 2 - SafeZone) / Scale - MaxWidth / 2;

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

        TranslateX -= x / Scale - x / oldScale;
        TranslateY -= y / Scale - y / oldScale;

        LimitTranslate();
    }

    #endregion

    #region rendering

    public void Render(
        ImmediateDrawingContext context
    )
    {
        if (Pages.Count <= 0)
        {
            return;
        }

        LimitScale();
        LimitTranslate();

        var skia = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
        using var lease = skia.Lease();

        var canvas = lease.SkCanvas;

        if (canvas == null)
        {
            throw new InvalidOperationException(
                message: $"Context needs to be ISkiaDrawingContextImpl but got {nameof(context)}");
        }

        var originalMatrix = canvas.TotalMatrix;

        canvas.Translate(dx: Width / 2, dy: 0);

        canvas.Scale(s: Scale);
        canvas.Translate(dx: TranslateX, dy: -TranslateY + SafeZone / Scale);

        foreach (var page in Pages)
        {
            canvas.Translate(dx: -page.Width / 2f, dy: 0);
            DrawBlankPage(canvas: canvas, width: page.Width, height: page.Height);
            canvas.DrawPicture(picture: page.Picture);
            canvas.Translate(dx: page.Width / 2f, dy: page.Height + PageSpacing);
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
        , float width
        , float height
    )
    {
        canvas.DrawRect(x: 0, y: 0, w: width, h: height, paint: BlankPageShadowPaint);
        canvas.DrawRect(x: 0, y: 0, w: width, h: height, paint: BlankPagePaint);
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

        canvas.DrawRect(x: 0, y: 0, w: Width, h: InnerGradientSize, paint: fogPaint);
    }

    #endregion
}