using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace ProjectAvalonia.Common.Controls;

[TemplatePart(name: "PART_TrimmedTextBlock", type: typeof(TextBlock))]
[TemplatePart(name: "PART_NoTrimTextBlock", type: typeof(FadeOutTextBlock))]
public class FadeOutTextControl : TemplatedControl
{
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<FadeOutTextControl, string?>(nameof(Text));

    private FadeOutTextBlock? _noTrimTextBlock;

    private TextBlock? _trimmedTextBlock;

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(property: TextProperty, value: value);
    }

    protected override void OnApplyTemplate(
        TemplateAppliedEventArgs e
    )
    {
        base.OnApplyTemplate(e);

        _trimmedTextBlock = e.NameScope.Find<TextBlock>("PART_TrimmedTextBlock");
        _noTrimTextBlock = e.NameScope.Find<FadeOutTextBlock>("PART_NoTrimTextBlock");

        if (_trimmedTextBlock is not null && _noTrimTextBlock is not null)
        {
            _noTrimTextBlock.TrimmedTextBlock = _trimmedTextBlock;
        }
    }
}