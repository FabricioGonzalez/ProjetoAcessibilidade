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
    Text,
    Icon
}

public class PrivacyContentControl : ContentControl
{
    public static readonly StyledProperty<ReplacementMode> PrivacyReplacementModeProperty =
        AvaloniaProperty.Register<PrivacyContentControl, ReplacementMode>(nameof(PrivacyReplacementMode));

    public static readonly StyledProperty<bool> ForceShowProperty =
        AvaloniaProperty.Register<PrivacyContentControl, bool>(nameof(ForceShow));

    public static readonly StyledProperty<bool> UseOpacityProperty =
        AvaloniaProperty.Register<PrivacyContentControl, bool>(nameof(UseOpacity), defaultValue: true);

    public PrivacyContentControl()
    {
        if (Design.IsDesignMode)
        {
            return;
        }

        var displayContent = PrivacyModeHelper.DelayedRevealAndHide(
            this.WhenAnyValue(x => x.IsPointerOver),
            ServicesConfig.UiConfig.WhenAnyValue(x => x.PrivacyMode),
            this.WhenAnyValue(x => x.ForceShow));

        IsContentRevealed = displayContent
            .ReplayLastActive();
    }

    private IObservable<bool> IsContentRevealed { get; } = Observable.Empty<bool>();

    public ReplacementMode PrivacyReplacementMode
    {
        get => GetValue(PrivacyReplacementModeProperty);
        set => SetValue(PrivacyReplacementModeProperty, value);
    }

    public bool ForceShow
    {
        get => GetValue(ForceShowProperty);
        set => SetValue(ForceShowProperty, value);
    }

    public bool UseOpacity
    {
        get => GetValue(UseOpacityProperty);
        set => SetValue(UseOpacityProperty, value);
    }
}
