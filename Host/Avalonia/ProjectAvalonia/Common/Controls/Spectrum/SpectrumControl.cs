using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Threading;
using SkiaSharp;

namespace ProjectAvalonia.Common.Controls.Spectrum;

public class SpectrumControl
    : TemplatedControl
        , ICustomDrawOperation
{
    private const int NumBins = 64;
    private const double TextureHeight = 32;
    private const double TextureWidth = 32;

    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<SpectrumControl, bool>(name: nameof(IsActive));

    public static readonly StyledProperty<bool> IsDockEffectVisibleProperty =
        AvaloniaProperty.Register<SpectrumControl, bool>(name: nameof(IsDockEffectVisible));

    private readonly AuraSpectrumDataSource _auraSpectrumDataSource;

    private readonly SKPaint _blur = new()
    {
        ImageFilter = SKImageFilter.CreateBlur(sigmaX: 24, sigmaY: 24, tileMode: SKShaderTileMode.Clamp)
        , FilterQuality = SKFilterQuality.Low
    };

    private readonly DispatcherTimer _invalidationTimer;

    private readonly SpectrumDataSource[] _sources;
    private readonly SplashEffectDataSource _splashEffectDataSource;

    private float[] _data;

    private bool _isAuraActive;
    private bool _isSplashActive;

    private IBrush? _lineBrush;

    private SKColor _lineColor;
    private SKSurface? _surface;

    public SpectrumControl()
    {
        SetVisibility();
        _data = new float[NumBins];
        _auraSpectrumDataSource = new AuraSpectrumDataSource(numBins: NumBins);
        _splashEffectDataSource = new SplashEffectDataSource(numBins: NumBins);

        _auraSpectrumDataSource.GeneratingDataStateChanged += OnAuraGeneratingDataStateChanged;
        _splashEffectDataSource.GeneratingDataStateChanged += OnSplashGeneratingDataStateChanged;

        _sources = new SpectrumDataSource[] { _auraSpectrumDataSource, _splashEffectDataSource };

        Background = new RadialGradientBrush
        {
            GradientStops =
            {
                new GradientStop { Color = Color.Parse(s: "#00000D21"), Offset = 0 }
                , new GradientStop { Color = Color.Parse(s: "#FF000D21"), Offset = 1 }
            }
        };

        _invalidationTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(value: 1000.0 / 15.0)
        };

        _invalidationTimer.Tick += (
            sender
            , args
        ) => InvalidateVisual();
    }

    public bool IsActive
    {
        get => GetValue(property: IsActiveProperty);
        set => SetValue(property: IsActiveProperty, value: value);
    }

    public bool IsDockEffectVisible
    {
        get => GetValue(property: IsDockEffectVisibleProperty);
        set => SetValue(property: IsDockEffectVisibleProperty, value: value);
    }

    void IDisposable.Dispose()
    {
        // nothing to do.
    }

    public bool HitTest(
        Point p
    ) => Bounds.Contains(p: p);

    public void Render(
        ImmediateDrawingContext context
    )
    {
        var bounds = Bounds;

        var skia = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();

        var lease = skia?.Lease();

        if (lease is null)
        {
            return;
        }

        if (_surface is null)
        {
            if (lease.GrContext is { } grContext)
            {
                _surface =
                    SKSurface.Create(context: grContext, budgeted: false
                        , info: new SKImageInfo(width: (int)TextureWidth, height: (int)TextureHeight));
            }
            else
            {
                _surface = SKSurface.Create(
                    info: new SKImageInfo(
                        width: (int)Math.Ceiling(a: TextureWidth),
                        height: (int)Math.Ceiling(a: TextureHeight),
                        colorType: SKImageInfo.PlatformColorType,
                        alphaType: SKAlphaType.Premul));
            }
        }

        RenderBars(context: _surface.Canvas);

        using var snapshot = _surface.Snapshot();

        lease.SkCanvas.DrawImage(
            image: snapshot,
            source: new SKRect(left: 0, top: 0, right: (float)TextureWidth, bottom: (float)TextureHeight),
            dest: new SKRect(left: 0, top: 0, right: (float)bounds.Width, bottom: (float)bounds.Height), paint: _blur);
    }

    bool IEquatable<ICustomDrawOperation>.Equals(
        ICustomDrawOperation? other
    ) => false;

    private void OnSplashGeneratingDataStateChanged(
        object? sender
        , bool e
    )
    {
        _isSplashActive = e;
        SetVisibility();
    }

    private void OnAuraGeneratingDataStateChanged(
        object? sender
        , bool e
    )
    {
        _isAuraActive = e;
        SetVisibility();
    }

    private void SetVisibility() => IsVisible = _isSplashActive || _isAuraActive;

    private void OnIsActiveChanged()
    {
        _auraSpectrumDataSource.IsActive = IsActive;

        if (IsActive)
        {
            _auraSpectrumDataSource.Start();
            _invalidationTimer.Start();
        }
        else
        {
            _invalidationTimer.Stop();
        }
    }

    protected override void OnPropertyChanged(
        AvaloniaPropertyChangedEventArgs change
    )
    {
        base.OnPropertyChanged(change: change);

        if (change.Property == IsActiveProperty)
        {
            OnIsActiveChanged();
        }
        else if (change.Property == IsDockEffectVisibleProperty)
        {
            if (change.GetNewValue<bool>() && !IsActive)
            {
                _splashEffectDataSource.Start();
            }
        }
        else if (change.Property == ForegroundProperty)
        {
            _lineBrush = Foreground ?? Brushes.Magenta;

            if (_lineBrush is ImmutableSolidColorBrush brush)
            {
                _lineColor = brush.Color.ToSKColor();
            }
        }
    }

    public override void Render(
        DrawingContext context
    )
    {
        base.Render(context: context);

        for (var i = 0; i < NumBins; i++)
        {
            _data[i] = 0;
        }

        foreach (var source in _sources)
        {
            source.Render(data: ref _data);
        }

        context.Custom(custom: this);
    }

    private void RenderBars(
        SKCanvas context
    )
    {
        context.Clear();
        var width = TextureWidth;
        var height = TextureHeight;
        var thickness = width / NumBins;
        var center = width / 2;

        double x = 0;

        using var linePaint = new SKPaint
        {
            Color = _lineColor, IsAntialias = false, Style = SKPaintStyle.Fill
        };

        using var path = new SKPath();

        for (var i = 0; i < NumBins; i++)
        {
            var dCenter = Math.Abs(value: x - center);
            var multiplier = 1 - dCenter / center;
            var rect = new SKRect(
                left: (float)x,
                top: (float)height,
                right: (float)(x + thickness),
                bottom: (float)(height - multiplier * _data[i] * (height * 0.8)));
            path.AddRect(rect: rect);

            x += thickness;
        }

        context.DrawPath(path: path, paint: linePaint);
    }
}