using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Diagnostics;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ReactiveUI;

namespace ProjectAvalonia.Behaviors;

public class ShowAttachedFlyoutWhenFocusedBehavior : AttachedToVisualTreeBehavior<Control>
{
    public static readonly DirectProperty<ShowAttachedFlyoutWhenFocusedBehavior, bool> IsFlyoutOpenProperty =
        AvaloniaProperty.RegisterDirect<ShowAttachedFlyoutWhenFocusedBehavior, bool>(
            name: "IsFlyoutOpen",
            getter: o => o.IsFlyoutOpen,
            setter: (
                o
                , v
            ) => o.IsFlyoutOpen = v);

    private bool _isFlyoutOpen;

    public bool IsFlyoutOpen
    {
        get => _isFlyoutOpen;
        set => SetAndRaise(property: IsFlyoutOpenProperty, field: ref _isFlyoutOpen, value: value);
    }

    protected override void OnAttachedToVisualTree(
        CompositeDisposable disposable
    )
    {
        if (AssociatedObject?.GetVisualRoot() is not Control visualRoot)
        {
            return;
        }

        var flyoutBase = FlyoutBase.GetAttachedFlyout(element: AssociatedObject);
        if (flyoutBase is null)
        {
            return;
        }

        var controller =
            new FlyoutShowController(parent: AssociatedObject, flyout: flyoutBase).DisposeWith(
                compositeDisposable: disposable);

        FocusBasedFlyoutOpener(associatedObject: AssociatedObject, flyoutBase: flyoutBase)
            .DisposeWith(compositeDisposable: disposable);
        IsOpenPropertySynchronizer(controller: controller).DisposeWith(compositeDisposable: disposable);

        // EDGE CASES
        // Edge case when the Visual Root becomes active and the Associated object is focused.
        ActivateOpener(associatedObject: AssociatedObject, visualRoot: visualRoot, controller: controller)
            .DisposeWith(compositeDisposable: disposable);
        DeactivateCloser(visualRoot: visualRoot, controller: controller).DisposeWith(compositeDisposable: disposable);

        // This is a workaround for the case when the user switches theme. The same behavior is detached and re-attached on theme changes.
        // If you don't close it, the Flyout will show in an incorrect position. Maybe bug in Avalonia?
        if (IsFlyoutOpen)
        {
            IsFlyoutOpen = false;
        }
    }

    private static IDisposable DeactivateCloser(
        Control visualRoot
        , FlyoutShowController controller
    ) =>
        Observable.FromEventPattern(target: visualRoot, eventName: nameof(Window.Deactivated))
            .Do(onNext: _ => controller.SetIsForcedOpen(value: false))
            .Subscribe();

    private static IDisposable ActivateOpener(
        IInputElement associatedObject
        , Control visualRoot
        , FlyoutShowController controller
    ) =>
        Observable.FromEventPattern(target: visualRoot, eventName: nameof(Window.Activated))
            .Where(predicate: _ => associatedObject.IsFocused)
            .Do(onNext: _ => controller.SetIsForcedOpen(value: true))
            .Subscribe();

    private static IObservable<bool> GetPopupIsFocused(
        FlyoutBase flyoutBase
    )
    {
        var currentPopupHost = Observable
            .FromEventPattern(target: flyoutBase, eventName: nameof(flyoutBase.Opened))
            .Select(selector: _ => ((IPopupHostProvider)flyoutBase).PopupHost?.Presenter)
            .WhereNotNull();

        var popupGotFocus = currentPopupHost.Select(selector: x => x.OnEvent(routedEvent: InputElement.GotFocusEvent))
            .Switch().ToSignal();
        var popupLostFocus = currentPopupHost.Select(selector: x => x.OnEvent(routedEvent: InputElement.LostFocusEvent))
            .Switch().ToSignal();
        var flyoutGotFocus = popupGotFocus.Select(selector: _ => true)
            .Merge(second: popupLostFocus.Select(selector: _ => false));
        return flyoutGotFocus;
    }

    private IDisposable IsOpenPropertySynchronizer(
        FlyoutShowController controller
    ) =>
        this
            .WhenAnyValue(property1: x => x.IsFlyoutOpen)
            .Do(onNext: controller.SetIsForcedOpen)
            .Subscribe();

    private IDisposable FocusBasedFlyoutOpener(
        IAvaloniaObject associatedObject
        , FlyoutBase flyoutBase
    )
    {
        var isPopupFocused = GetPopupIsFocused(flyoutBase: flyoutBase);
        var isAssociatedObjectFocused = associatedObject.GetObservable(property: InputElement.IsFocusedProperty);

        var mergedFocused = isAssociatedObjectFocused.Merge(second: isPopupFocused);

        var weAreFocused = mergedFocused
            .Throttle(dueTime: TimeSpan.FromSeconds(value: 0.1))
            .DistinctUntilChanged();

        return weAreFocused
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Do(onNext: isOpen => IsFlyoutOpen = isOpen)
            .Subscribe();
    }
}