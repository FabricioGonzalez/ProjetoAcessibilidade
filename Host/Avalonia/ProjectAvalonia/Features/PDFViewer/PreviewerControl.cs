using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;

namespace QuestPDF.Previewer;

internal class PreviewerControl : Control
{
    public static readonly StyledProperty<ObservableCollection<PreviewPage>> PagesProperty =
        AvaloniaProperty.Register<PreviewerControl, ObservableCollection<PreviewPage>>(
            name: nameof(Pages),
            defaultValue: new ObservableCollection<PreviewPage>());

    public static readonly StyledProperty<float> CurrentScrollProperty
        = AvaloniaProperty.Register<PreviewerControl, float>(name: nameof(CurrentScroll));

    public static readonly StyledProperty<float> ScrollViewportSizeProperty
        = AvaloniaProperty.Register<PreviewerControl, float>(name: nameof(ScrollViewportSize));

    public PreviewerControl()
    {
        PagesProperty.Changed.Subscribe(onNext: x =>
        {
            InteractiveCanvas.Pages = x.NewValue.Value;
            InvalidateVisual();
        });

        CurrentScrollProperty.Changed.Subscribe(onNext: x =>
        {
            InteractiveCanvas.ScrollPercentY = x.NewValue.Value;
            Dispatcher.UIThread.Post(action: () => InvalidateVisual());
        });

        ClipToBounds = true;
    }

    private InteractiveCanvas InteractiveCanvas
    {
        get;
    } = new();

    public ObservableCollection<PreviewPage>? Pages
    {
        get => GetValue(property: PagesProperty);
        set => SetValue(property: PagesProperty, value: value);
    }

    public float CurrentScroll
    {
        get => GetValue(property: CurrentScrollProperty);
        set => SetValue(property: CurrentScrollProperty, value: value);
    }

    public float ScrollViewportSize
    {
        get => GetValue(property: ScrollViewportSizeProperty);
        set => SetValue(property: ScrollViewportSizeProperty, value: value);
    }

    private bool IsMousePressed
    {
        get;
        set;
    }

    private Vector MousePosition
    {
        get;
        set;
    }

    protected override void OnPointerWheelChanged(
        PointerWheelEventArgs e
    )
    {
        base.OnPointerWheelChanged(e: e);

        if ((e.KeyModifiers & KeyModifiers.Control) != 0)
        {
            var scaleFactor = 1 + e.Delta.Y / 10f;
            var point = new Point(x: Bounds.Center.X, y: Bounds.Top) - e.GetPosition(relativeTo: this);

            InteractiveCanvas.ZoomToPoint(x: (float)point.X, y: -(float)point.Y, factor: (float)scaleFactor);
            InvalidateVisual();
        }

        if (e.KeyModifiers == KeyModifiers.None)
        {
            var translation = (float)e.Delta.Y * 25;
            InteractiveCanvas.TranslateWithCurrentScale(x: 0, y: -translation);
            InvalidateVisual();
        }
    }

    protected override void OnPointerMoved(
        PointerEventArgs e
    )
    {
        base.OnPointerMoved(e: e);

        if (IsMousePressed)
        {
            var currentPosition = e.GetPosition(relativeTo: this);
            var translation = currentPosition - MousePosition;
            InteractiveCanvas.TranslateWithCurrentScale(x: (float)translation.X, y: -(float)translation.Y);

            InvalidateVisual();
        }

        MousePosition = e.GetPosition(relativeTo: this);
    }

    protected override void OnPointerPressed(
        PointerPressedEventArgs e
    )
    {
        base.OnPointerPressed(e: e);

        IsMousePressed = true;
    }

    protected override void OnPointerReleased(
        PointerReleasedEventArgs e
    )
    {
        base.OnPointerReleased(e: e);
        IsMousePressed = false;
    }

    public override void Render(
        DrawingContext context
    )
    {
        CurrentScroll = InteractiveCanvas.ScrollPercentY;
        ScrollViewportSize = InteractiveCanvas.ScrollViewportSizeY;

        InteractiveCanvas.Bounds = new Rect(x: 0, y: 0, width: Bounds.Width, height: Bounds.Height);

        context.Custom(custom: InteractiveCanvas);
        base.Render(context: context);
    }
}