using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ReactiveUI;

namespace ProjectAvalonia.Common.Controls;

public enum ReplacementMode
{
    Text
    , Icon
}

public class PrivacyContentControl : ContentControl
{
    public static readonly StyledProperty<ReplacementMode> PrivacyReplacementModeProperty =
        AvaloniaProperty.Register<PrivacyContentControl, ReplacementMode>(name: nameof(PrivacyReplacementMode));

    public static readonly StyledProperty<bool> ForceShowProperty =
        AvaloniaProperty.Register<PrivacyContentControl, bool>(name: nameof(ForceShow));

    public static readonly StyledProperty<bool> UseOpacityProperty =
        AvaloniaProperty.Register<PrivacyContentControl, bool>(name: nameof(UseOpacity), defaultValue: true);

    public PrivacyContentControl()
    {
        if (Design.IsDesignMode)
        {
            return;
        }

        var displayContent = PrivacyModeHelper.DelayedRevealAndHide(
            isPointerOver: this.WhenAnyValue(property1: x => x.IsPointerOver),
            isPrivacyModeEnabled: ServicesConfig.UiConfig.WhenAnyValue(property1: x => x.PrivacyMode),
            isVisibilityForced: this.WhenAnyValue(property1: x => x.ForceShow));

        IsContentRevealed = displayContent
            .ReplayLastActive();
    }

    private IObservable<bool> IsContentRevealed
    {
        get;
    } = Observable.Empty<bool>();

    public ReplacementMode PrivacyReplacementMode
    {
        get => GetValue(property: PrivacyReplacementModeProperty);
        set => SetValue(property: PrivacyReplacementModeProperty, value: value);
    }

    public bool ForceShow
    {
        get => GetValue(property: ForceShowProperty);
        set => SetValue(property: ForceShowProperty, value: value);
    }

    public bool UseOpacity
    {
        get => GetValue(property: UseOpacityProperty);
        set => SetValue(property: UseOpacityProperty, value: value);
    }
}