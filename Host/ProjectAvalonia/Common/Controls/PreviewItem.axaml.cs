using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using ReactiveUI;

namespace ProjectAvalonia.Common.Controls;

public class PreviewItem : ContentControl
{
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<PreviewItem, string>(name: nameof(Label));

    public static readonly StyledProperty<Geometry> IconProperty =
        AvaloniaProperty.Register<PreviewItem, Geometry>(name: nameof(Icon));

    public static readonly StyledProperty<double> IconSizeProperty =
        AvaloniaProperty.Register<PreviewItem, double>(name: nameof(IconSize), defaultValue: 24);

    public static readonly StyledProperty<bool> IsIconVisibleProperty =
        AvaloniaProperty.Register<PreviewItem, bool>(name: nameof(IsIconVisible));

    public static readonly StyledProperty<string> TextValueProperty =
        AvaloniaProperty.Register<PreviewItem, string>(name: nameof(TextValue));

    public static readonly StyledProperty<ICommand> CopyCommandProperty =
        AvaloniaProperty.Register<PreviewItem, ICommand>(name: nameof(CopyCommand));

    public static readonly StyledProperty<bool> IsCopyButtonVisibleProperty =
        AvaloniaProperty.Register<PreviewItem, bool>(name: nameof(IsCopyButtonVisible));

    public static readonly StyledProperty<bool> PrivacyModeEnabledProperty =
        AvaloniaProperty.Register<PreviewItem, bool>(name: nameof(PrivacyModeEnabled));

    public string Label
    {
        get => GetValue(property: LabelProperty);
        set => SetValue(property: LabelProperty, value: value);
    }

    public Geometry Icon
    {
        get => GetValue(property: IconProperty);
        set => SetValue(property: IconProperty, value: value);
    }

    public double IconSize
    {
        get => GetValue(property: IconSizeProperty);
        set => SetValue(property: IconSizeProperty, value: value);
    }

    public bool IsIconVisible
    {
        get => GetValue(property: IsIconVisibleProperty);
        set => SetValue(property: IsIconVisibleProperty, value: value);
    }

    public string TextValue
    {
        get => GetValue(property: TextValueProperty);
        set => SetValue(property: TextValueProperty, value: value);
    }

    public ICommand CopyCommand
    {
        get => GetValue(property: CopyCommandProperty);
        set => SetValue(property: CopyCommandProperty, value: value);
    }

    public bool IsCopyButtonVisible
    {
        get => GetValue(property: IsCopyButtonVisibleProperty);
        set => SetValue(property: IsCopyButtonVisibleProperty, value: value);
    }

    public bool PrivacyModeEnabled
    {
        get => GetValue(property: PrivacyModeEnabledProperty);
        set => SetValue(property: PrivacyModeEnabledProperty, value: value);
    }

    protected override void OnApplyTemplate(
        TemplateAppliedEventArgs e
    )
    {
        var button = e.NameScope.Find<ClipboardCopyButton>(name: "PART_ClipboardCopyButton");

        var hasBeenJustCopied = Observable.Return(value: false)
            .Concat(second: button.CopyCommand
                .Select(selector: _ => Observable.Return(value: true)
                    .Concat(second: Observable.Timer(dueTime: TimeSpan.FromSeconds(value: 1))
                        .Select(selector: _ => false)))
                .Switch());

        var isCopyButtonVisible = this
            .WhenAnyValue(property1: item => item.IsPointerOver, property2: item => item.TextValue, selector: (
                a
                , b
            ) => a && !string.IsNullOrWhiteSpace(value: b))
            .CombineLatest(second: hasBeenJustCopied, resultSelector: (
                over
                , justCopied
            ) => over || justCopied);

        this.Bind(property: IsCopyButtonVisibleProperty, source: isCopyButtonVisible);

        base.OnApplyTemplate(e: e);
    }
}