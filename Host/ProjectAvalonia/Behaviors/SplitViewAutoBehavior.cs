using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using ReactiveUI;

namespace ProjectAvalonia.Behaviors;

public class SplitViewAutoBehavior : DisposingBehavior<SplitView>
{
    public static readonly StyledProperty<double> CollapseThresholdProperty =
        AvaloniaProperty.Register<SplitViewAutoBehavior, double>(name: nameof(CollapseThreshold));

    public static readonly StyledProperty<Action> ToggleActionProperty =
        AvaloniaProperty.Register<SplitViewAutoBehavior, Action>(name: nameof(ToggleAction));

    public static readonly StyledProperty<Action> CollapseOnClickActionProperty =
        AvaloniaProperty.Register<SplitViewAutoBehavior, Action>(name: nameof(CollapseOnClickAction));

    private bool _sidebarWasForceClosed;

    public double CollapseThreshold
    {
        get => GetValue(property: CollapseThresholdProperty);
        set => SetValue(property: CollapseThresholdProperty, value: value);
    }

    public Action ToggleAction
    {
        get => GetValue(property: ToggleActionProperty);
        set => SetValue(property: ToggleActionProperty, value: value);
    }

    public Action CollapseOnClickAction
    {
        get => GetValue(property: CollapseOnClickActionProperty);
        set => SetValue(property: CollapseOnClickActionProperty, value: value);
    }

    protected override void OnAttached(
        CompositeDisposable disposables
    )
    {
        AssociatedObject!.WhenAnyValue(property1: x => x.Bounds)
            .DistinctUntilChanged()
            .Subscribe(onNext: SplitViewBoundsChanged)
            .DisposeWith(compositeDisposable: disposables);

        ToggleAction = OnToggleAction;
        CollapseOnClickAction = OnCollapseOnClickAction;
    }

    private void OnCollapseOnClickAction()
    {
        if (AssociatedObject!.Bounds.Width <= CollapseThreshold && AssociatedObject!.IsPaneOpen)
        {
            AssociatedObject!.IsPaneOpen = false;
        }
    }

    private void OnToggleAction()
    {
        if (AssociatedObject!.Bounds.Width > CollapseThreshold)
        {
            _sidebarWasForceClosed = AssociatedObject!.IsPaneOpen;
        }

        AssociatedObject!.IsPaneOpen = !AssociatedObject!.IsPaneOpen;
    }

    private void SplitViewBoundsChanged(
        Rect x
    )
    {
        if (AssociatedObject is null)
        {
            return;
        }

        if (x.Width <= CollapseThreshold)
        {
            AssociatedObject.DisplayMode = SplitViewDisplayMode.CompactOverlay;

            if (!_sidebarWasForceClosed && AssociatedObject.IsPaneOpen)
            {
                AssociatedObject.IsPaneOpen = false;
            }
        }
        else
        {
            AssociatedObject.DisplayMode = SplitViewDisplayMode.CompactInline;

            if (!_sidebarWasForceClosed && !AssociatedObject.IsPaneOpen)
            {
                AssociatedObject.IsPaneOpen = true;
            }
        }
    }
}