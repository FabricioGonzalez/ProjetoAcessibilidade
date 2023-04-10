using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Media;

namespace ProjectAvalonia.Common.Controls;

public class ContentArea : ContentControl
{
    public static readonly StyledProperty<object> TitleProperty =
        AvaloniaProperty.Register<ContentArea, object>(name: nameof(Title));

    public static readonly StyledProperty<object> CaptionProperty =
        AvaloniaProperty.Register<ContentArea, object>(name: nameof(Caption));

    public static readonly StyledProperty<bool> EnableBackProperty =
        AvaloniaProperty.Register<ContentArea, bool>(name: nameof(EnableBack));

    public static readonly StyledProperty<bool> EnableCancelProperty =
        AvaloniaProperty.Register<ContentArea, bool>(name: nameof(EnableCancel));

    public static readonly StyledProperty<bool> EnableNextProperty =
        AvaloniaProperty.Register<ContentArea, bool>(name: nameof(EnableNext));

    public static readonly StyledProperty<bool> EnableSkipProperty =
        AvaloniaProperty.Register<ContentArea, bool>(name: nameof(EnableSkip));

    public static readonly StyledProperty<bool> FocusNextProperty =
        AvaloniaProperty.Register<ContentArea, bool>(name: nameof(FocusNext));

    public static readonly StyledProperty<bool> FocusCancelProperty =
        AvaloniaProperty.Register<ContentArea, bool>(name: nameof(FocusCancel));

    public static readonly StyledProperty<object> CancelContentProperty =
        AvaloniaProperty.Register<ContentArea, object>(name: nameof(CancelContent), defaultValue: "Cancel");

    public static readonly StyledProperty<object> NextContentProperty =
        AvaloniaProperty.Register<ContentArea, object>(name: nameof(NextContent), defaultValue: "Done");

    public static readonly StyledProperty<object> SkipContentProperty =
        AvaloniaProperty.Register<ContentArea, object>(name: nameof(SkipContent), defaultValue: "Skip");

    public static readonly StyledProperty<object> FooterContentProperty =
        AvaloniaProperty.Register<ContentArea, object>(name: nameof(FooterContent));

    public static readonly StyledProperty<bool> IsBusyProperty =
        AvaloniaProperty.Register<ContentArea, bool>(name: nameof(IsBusy));

    public static readonly StyledProperty<IBrush> HeaderBackgroundProperty =
        AvaloniaProperty.Register<ContentArea, IBrush>(name: nameof(HeaderBackground));

    private IContentPresenter? _captionPresenter;

    private IContentPresenter? _titlePresenter;

    public object Title
    {
        get => GetValue(property: TitleProperty);
        set => SetValue(property: TitleProperty, value: value);
    }

    public object Caption
    {
        get => GetValue(property: CaptionProperty);
        set => SetValue(property: CaptionProperty, value: value);
    }

    public bool EnableBack
    {
        get => GetValue(property: EnableBackProperty);
        set => SetValue(property: EnableBackProperty, value: value);
    }

    public bool EnableCancel
    {
        get => GetValue(property: EnableCancelProperty);
        set => SetValue(property: EnableCancelProperty, value: value);
    }

    public bool EnableNext
    {
        get => GetValue(property: EnableNextProperty);
        set => SetValue(property: EnableNextProperty, value: value);
    }

    public bool EnableSkip
    {
        get => GetValue(property: EnableSkipProperty);
        set => SetValue(property: EnableSkipProperty, value: value);
    }

    public bool FocusNext
    {
        get => GetValue(property: FocusNextProperty);
        set => SetValue(property: FocusNextProperty, value: value);
    }

    public bool FocusCancel
    {
        get => GetValue(property: FocusCancelProperty);
        set => SetValue(property: FocusCancelProperty, value: value);
    }

    public object CancelContent
    {
        get => GetValue(property: CancelContentProperty);
        set => SetValue(property: CancelContentProperty, value: value);
    }

    public object NextContent
    {
        get => GetValue(property: NextContentProperty);
        set => SetValue(property: NextContentProperty, value: value);
    }

    public object SkipContent
    {
        get => GetValue(property: SkipContentProperty);
        set => SetValue(property: SkipContentProperty, value: value);
    }

    public object FooterContent
    {
        get => GetValue(property: FooterContentProperty);
        set => SetValue(property: FooterContentProperty, value: value);
    }

    public bool IsBusy
    {
        get => GetValue(property: IsBusyProperty);
        set => SetValue(property: IsBusyProperty, value: value);
    }

    public IBrush HeaderBackground
    {
        get => GetValue(property: HeaderBackgroundProperty);
        set => SetValue(property: HeaderBackgroundProperty, value: value);
    }

    protected override bool RegisterContentPresenter(
        IContentPresenter presenter
    )
    {
        var result = base.RegisterContentPresenter(presenter: presenter);

        switch (presenter.Name)
        {
            case "PART_TitlePresenter":
                if (_titlePresenter is not null)
                {
                    _titlePresenter.PropertyChanged -= PresenterOnPropertyChanged;
                }

                _titlePresenter = presenter;
                _titlePresenter.PropertyChanged += PresenterOnPropertyChanged;
                result = true;
                break;

            case "PART_CaptionPresenter":
                if (_captionPresenter is not null)
                {
                    _captionPresenter.PropertyChanged -= PresenterOnPropertyChanged;
                }

                _captionPresenter = presenter;
                _captionPresenter.PropertyChanged += PresenterOnPropertyChanged;
                _captionPresenter.IsVisible = Caption is not null;
                result = true;
                break;
        }

        return result;
    }

    private void PresenterOnPropertyChanged(
        object? sender
        , AvaloniaPropertyChangedEventArgs e
    )
    {
        if (e.Property == ContentPresenter.ChildProperty)
        {
            var className = sender == _captionPresenter ? "caption" : "title";

            if (e.OldValue is IStyledElement oldValue)
            {
                oldValue.Classes.Remove(name: className);
            }

            if (e.NewValue is IStyledElement newValue)
            {
                newValue.Classes.Add(name: className);
            }
        }
        else if (e.Property == CaptionProperty && _captionPresenter is not null)
        {
            _captionPresenter.IsVisible = e.NewValue is not null;
        }
    }
}