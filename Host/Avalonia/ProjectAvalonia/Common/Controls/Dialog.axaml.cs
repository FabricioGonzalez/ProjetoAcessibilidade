using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using ReactiveUI;

namespace ProjectAvalonia.Common.Controls;

/// <summary>
///     A simple overlay Dialog control.
/// </summary>
public class Dialog : ContentControl
{
    public static readonly StyledProperty<bool> IsDialogOpenProperty =
        AvaloniaProperty.Register<Dialog, bool>(name: nameof(IsDialogOpen));

    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<Dialog, bool>(name: nameof(IsActive));

    public static readonly StyledProperty<bool> IsBusyProperty =
        AvaloniaProperty.Register<Dialog, bool>(name: nameof(IsBusy));

    public static readonly StyledProperty<bool> IsBackEnabledProperty =
        AvaloniaProperty.Register<Dialog, bool>(name: nameof(IsBackEnabled));

    public static readonly StyledProperty<bool> IsCancelEnabledProperty =
        AvaloniaProperty.Register<Dialog, bool>(name: nameof(IsCancelEnabled));

    public static readonly StyledProperty<bool> EnableCancelOnPressedProperty =
        AvaloniaProperty.Register<Dialog, bool>(name: nameof(EnableCancelOnPressed));

    public static readonly StyledProperty<bool> EnableCancelOnEscapeProperty =
        AvaloniaProperty.Register<Dialog, bool>(name: nameof(EnableCancelOnEscape));

    public static readonly StyledProperty<double> MaxContentHeightProperty =
        AvaloniaProperty.Register<Dialog, double>(name: nameof(MaxContentHeight)
            , defaultValue: double.PositiveInfinity);

    public static readonly StyledProperty<double> MaxContentWidthProperty =
        AvaloniaProperty.Register<Dialog, double>(name: nameof(MaxContentWidth), defaultValue: double.PositiveInfinity);

    public static readonly StyledProperty<double> IncreasedWidthThresholdProperty =
        AvaloniaProperty.Register<Dialog, double>(name: nameof(IncreasedWidthThreshold), defaultValue: double.NaN);

    public static readonly StyledProperty<double> IncreasedHeightThresholdProperty =
        AvaloniaProperty.Register<Dialog, double>(name: nameof(IncreasedHeightThreshold), defaultValue: double.NaN);

    public static readonly StyledProperty<double> FullScreenHeightThresholdProperty =
        AvaloniaProperty.Register<Dialog, double>(name: nameof(FullScreenHeightThreshold), defaultValue: double.NaN);

    public static readonly StyledProperty<bool> FullScreenEnabledProperty =
        AvaloniaProperty.Register<Dialog, bool>(name: nameof(FullScreenEnabled));

    public static readonly StyledProperty<bool> IncreasedWidthEnabledProperty =
        AvaloniaProperty.Register<Dialog, bool>(name: nameof(IncreasedWidthEnabled));

    public static readonly StyledProperty<bool> IncreasedHeightEnabledProperty =
        AvaloniaProperty.Register<Dialog, bool>(name: nameof(IncreasedHeightEnabled));

    public static readonly StyledProperty<bool> IncreasedSizeEnabledProperty =
        AvaloniaProperty.Register<Dialog, bool>(name: nameof(IncreasedSizeEnabled));

    private bool _canCancelOnPointerPressed;
    private Panel? _dismissPanel;
    private Panel? _overlayPanel;

    public Dialog()
    {
        this.GetObservable(property: IsDialogOpenProperty).Subscribe(onNext: UpdateDelay);

        this.WhenAnyValue(property1: x => x.Bounds)
            .Subscribe(onNext: bounds =>
            {
                var width = bounds.Width;
                var height = bounds.Height;
                var increasedWidthThreshold = IncreasedWidthThreshold;
                var increasedHeightThreshold = IncreasedHeightThreshold;
                var fullScreenHeightThreshold = FullScreenHeightThreshold;
                var canIncreasedWidth = !double.IsNaN(d: increasedWidthThreshold)
                                        && width < increasedWidthThreshold;
                var canIncreasedHeight = !double.IsNaN(d: increasedHeightThreshold)
                                         && height < increasedHeightThreshold;
                var canGoToFullScreen = !double.IsNaN(d: fullScreenHeightThreshold)
                                        && height < fullScreenHeightThreshold;
                IncreasedWidthEnabled = canIncreasedWidth && !canIncreasedHeight;
                IncreasedHeightEnabled = !canIncreasedWidth && canIncreasedHeight;
                IncreasedSizeEnabled = canIncreasedWidth && canIncreasedHeight;
                FullScreenEnabled = canIncreasedWidth && canGoToFullScreen;
            });
    }

    public bool IsDialogOpen
    {
        get => GetValue(property: IsDialogOpenProperty);
        set => SetValue(property: IsDialogOpenProperty, value: value);
    }

    public bool IsActive
    {
        get => GetValue(property: IsActiveProperty);
        set => SetValue(property: IsActiveProperty, value: value);
    }

    public bool IsBusy
    {
        get => GetValue(property: IsBusyProperty);
        set => SetValue(property: IsBusyProperty, value: value);
    }

    public bool IsBackEnabled
    {
        get => GetValue(property: IsBackEnabledProperty);
        set => SetValue(property: IsBackEnabledProperty, value: value);
    }

    public bool IsCancelEnabled
    {
        get => GetValue(property: IsCancelEnabledProperty);
        set => SetValue(property: IsCancelEnabledProperty, value: value);
    }

    public bool EnableCancelOnPressed
    {
        get => GetValue(property: EnableCancelOnPressedProperty);
        set => SetValue(property: EnableCancelOnPressedProperty, value: value);
    }

    public bool EnableCancelOnEscape
    {
        get => GetValue(property: EnableCancelOnEscapeProperty);
        set => SetValue(property: EnableCancelOnEscapeProperty, value: value);
    }

    public double MaxContentHeight
    {
        get => GetValue(property: MaxContentHeightProperty);
        set => SetValue(property: MaxContentHeightProperty, value: value);
    }

    public double MaxContentWidth
    {
        get => GetValue(property: MaxContentWidthProperty);
        set => SetValue(property: MaxContentWidthProperty, value: value);
    }

    public double IncreasedWidthThreshold
    {
        get => GetValue(property: IncreasedWidthThresholdProperty);
        set => SetValue(property: IncreasedWidthThresholdProperty, value: value);
    }

    public double IncreasedHeightThreshold
    {
        get => GetValue(property: IncreasedHeightThresholdProperty);
        set => SetValue(property: IncreasedHeightThresholdProperty, value: value);
    }

    public double FullScreenHeightThreshold
    {
        get => GetValue(property: FullScreenHeightThresholdProperty);
        set => SetValue(property: FullScreenHeightThresholdProperty, value: value);
    }

    private bool FullScreenEnabled
    {
        get => GetValue(property: FullScreenEnabledProperty);
        set => SetValue(property: FullScreenEnabledProperty, value: value);
    }

    private bool IncreasedWidthEnabled
    {
        get => GetValue(property: IncreasedWidthEnabledProperty);
        set => SetValue(property: IncreasedWidthEnabledProperty, value: value);
    }

    private bool IncreasedHeightEnabled
    {
        get => GetValue(property: IncreasedHeightEnabledProperty);
        set => SetValue(property: IncreasedHeightEnabledProperty, value: value);
    }

    private bool IncreasedSizeEnabled
    {
        get => GetValue(property: IncreasedSizeEnabledProperty);
        set => SetValue(property: IncreasedSizeEnabledProperty, value: value);
    }

    private CancellationTokenSource? CancelPointerPressedDelay
    {
        get;
        set;
    }

    private void UpdateDelay(
        bool isDialogOpen
    )
    {
        try
        {
            _canCancelOnPointerPressed = false;
            CancelPointerPressedDelay?.Cancel();

            if (isDialogOpen)
            {
                CancelPointerPressedDelay = new CancellationTokenSource();

                Task.Delay(delay: TimeSpan.FromSeconds(value: 1), cancellationToken: CancelPointerPressedDelay.Token)
                    .ContinueWith(continuationFunction: _ => _canCancelOnPointerPressed = true);
            }
        }
        catch (OperationCanceledException)
        {
            // ignored
        }
    }

    protected override void OnPropertyChanged<T>(
        AvaloniaPropertyChangedEventArgs<T> change
    )
    {
        base.OnPropertyChanged(change: change);

        if (change.Property == IsDialogOpenProperty)
        {
            PseudoClasses.Set(name: ":open", value: change.NewValue.GetValueOrDefault<bool>());
        }

        if (change.Property == IsBusyProperty)
        {
            PseudoClasses.Set(name: ":busy", value: change.NewValue.GetValueOrDefault<bool>());
        }
    }

    protected override void OnApplyTemplate(
        TemplateAppliedEventArgs e
    )
    {
        base.OnApplyTemplate(e: e);

        _dismissPanel = e.NameScope.Find<Panel>(name: "PART_Dismiss");
        _overlayPanel = e.NameScope.Find<Panel>(name: "PART_Overlay");

        if (this.GetVisualRoot() is TopLevel topLevel)
        {
            topLevel.AddHandler(routedEvent: PointerPressedEvent, handler: CancelPointerPressed
                , routes: RoutingStrategies.Tunnel);
            topLevel.AddHandler(routedEvent: KeyDownEvent, handler: CancelKeyDown, routes: RoutingStrategies.Tunnel);
        }
    }

    private void Close() => IsDialogOpen = false;

    private void CancelPointerPressed(
        object? sender
        , PointerPressedEventArgs e
    )
    {
        if (IsDialogOpen && IsActive && EnableCancelOnPressed && !IsBusy && _dismissPanel is not null &&
            _overlayPanel is not null && _canCancelOnPointerPressed)
        {
            var point = e.GetPosition(relativeTo: _dismissPanel);
            var isPressedOnTitleBar = e.GetPosition(relativeTo: _overlayPanel).Y < 30;

            if (!_dismissPanel.Bounds.Contains(p: point) && !isPressedOnTitleBar)
            {
                e.Handled = true;
                Close();
            }
        }
    }

    private void CancelKeyDown(
        object? sender
        , KeyEventArgs e
    )
    {
        if (e.Key == Key.Escape && EnableCancelOnEscape && !IsBusy && IsActive)
        {
            e.Handled = true;
            Close();
        }
    }
}