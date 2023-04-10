using System;
using System.Reactive.Linq;

namespace ProjectAvalonia.Common.Helpers;

public static class PrivacyModeHelper
{
    private static readonly TimeSpan RevealDelay = TimeSpan.FromSeconds(value: 0.75);
    private static readonly TimeSpan HideDelay = TimeSpan.FromSeconds(value: 10);

    public static IObservable<bool> DelayedRevealAndHide(
        IObservable<bool> isPointerOver
        , IObservable<bool> isPrivacyModeEnabled
        , IObservable<bool>? isVisibilityForced = null
    )
    {
        isVisibilityForced ??= Observable.Return(value: false);

        var shouldBeVisible = isPointerOver
            .Select(selector: Visibility)
            .Switch();

        var finalVisibility = isPrivacyModeEnabled
            .CombineLatest(
                source2: shouldBeVisible,
                source3: isVisibilityForced,
                resultSelector: (
                    privacyModeEnabled
                    , visible
                    , forced
                ) => !privacyModeEnabled || visible || forced);

        return finalVisibility;
    }

    private static IObservable<bool> Visibility(
        bool isPointerOver
    )
    {
        if (isPointerOver)
        {
            return ShowAfterDelayThenHide();
        }

        return Hide();
    }

    private static IObservable<bool> Hide() => Observable.Return(value: false);

    private static IObservable<bool> ShowAfterDelayThenHide()
    {
        var hideObs = Observable
            .Return(value: false)
            .Delay(dueTime: HideDelay);

        var showObs = Observable
            .Return(value: true)
            .Delay(dueTime: RevealDelay);

        return showObs.Concat(second: hideObs);
    }
}