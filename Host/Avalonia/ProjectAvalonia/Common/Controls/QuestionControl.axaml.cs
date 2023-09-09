using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ProjectAvalonia.Common.Controls;

public enum HighlightedButton
{
    None
    , YesButton
    , NoButton
}

public class QuestionControl : ContentControl
{
    public static readonly StyledProperty<ICommand> YesCommandProperty =
        AvaloniaProperty.Register<QuestionControl, ICommand>(name: nameof(YesCommand));

    public static readonly StyledProperty<ICommand> NoCommandProperty =
        AvaloniaProperty.Register<QuestionControl, ICommand>(name: nameof(NoCommand));

    public static readonly StyledProperty<IImage> ImageIconProperty =
        AvaloniaProperty.Register<QuestionControl, IImage>(name: nameof(ImageIcon));

    public static readonly StyledProperty<object?> IconContentProperty =
        AvaloniaProperty.Register<QuestionControl, object?>(name: nameof(IconContent));

    public static readonly StyledProperty<HighlightedButton> HighlightButtonProperty =
        AvaloniaProperty.Register<QuestionControl, HighlightedButton>(name: nameof(HighlightButton));

    public static readonly StyledProperty<bool> IsYesButtonProperty =
        AvaloniaProperty.Register<QuestionControl, bool>(name: nameof(IsYesButton));

    public static readonly StyledProperty<bool> IsNoButtonProperty =
        AvaloniaProperty.Register<QuestionControl, bool>(name: nameof(IsNoButton));

    public QuestionControl()
    {
        UpdateHighlightedButton(highlightedButton: HighlightButton);
    }

    public ICommand YesCommand
    {
        get => GetValue(property: YesCommandProperty);
        set => SetValue(property: YesCommandProperty, value: value);
    }

    public ICommand NoCommand
    {
        get => GetValue(property: NoCommandProperty);
        set => SetValue(property: NoCommandProperty, value: value);
    }

    public IImage ImageIcon
    {
        get => GetValue(property: ImageIconProperty);
        set => SetValue(property: ImageIconProperty, value: value);
    }

    public bool IsYesButton
    {
        get => GetValue(property: IsYesButtonProperty);
        set => SetValue(property: IsYesButtonProperty, value: value);
    }

    public bool IsNoButton
    {
        get => GetValue(property: IsNoButtonProperty);
        set => SetValue(property: IsNoButtonProperty, value: value);
    }

    public object? IconContent
    {
        get => GetValue(property: IconContentProperty);
        set => SetValue(property: IconContentProperty, value: value);
    }

    public HighlightedButton HighlightButton
    {
        get => GetValue(property: HighlightButtonProperty);
        set => SetValue(property: HighlightButtonProperty, value: value);
    }

    protected override void OnPropertyChanged(
        AvaloniaPropertyChangedEventArgs change
    )
    {
        base.OnPropertyChanged(change: change);

        if (change.Property == HighlightButtonProperty)
        {
            UpdateHighlightedButton(highlightedButton: change.GetNewValue<HighlightedButton>());
        }
    }

    private void UpdateHighlightedButton(
        HighlightedButton highlightedButton
    )
    {
        IsYesButton = highlightedButton == HighlightedButton.YesButton;
        IsNoButton = highlightedButton == HighlightedButton.NoButton;
    }
}