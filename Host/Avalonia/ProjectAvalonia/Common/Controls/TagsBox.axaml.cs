using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using Avalonia.Threading;
using Avalonia.VisualTree;
using ProjectAvalonia.Common.Helpers;
using ReactiveUI;

namespace ProjectAvalonia.Common.Controls;

public class TagsBox : TemplatedControl
{
    public static readonly StyledProperty<bool> IsCurrentTextValidProperty =
        AvaloniaProperty.Register<TagsBox, bool>(name: nameof(IsCurrentTextValid));

    public static readonly DirectProperty<TagsBox, bool> RequestAddProperty =
        AvaloniaProperty.RegisterDirect<TagsBox, bool>(name: nameof(RequestAdd), getter: o => o.RequestAdd);

    public static readonly StyledProperty<string> WatermarkProperty =
        TextBox.WatermarkProperty.AddOwner<TagsBox>();

    public static readonly StyledProperty<bool> RestrictInputToSuggestionsProperty =
        AvaloniaProperty.Register<TagsBox, bool>(name: nameof(RestrictInputToSuggestions));

    public static readonly StyledProperty<int> ItemCountLimitProperty =
        AvaloniaProperty.Register<TagsBox, int>(name: nameof(ItemCountLimit));

    public static readonly StyledProperty<int> MaxTextLengthProperty =
        AvaloniaProperty.Register<TagsBox, int>(name: nameof(MaxTextLength));

    public static readonly StyledProperty<char> TagSeparatorProperty =
        AvaloniaProperty.Register<TagsBox, char>(name: nameof(TagSeparator), defaultValue: ' ');

    public static readonly StyledProperty<bool> SuggestionsAreCaseSensitiveProperty =
        AvaloniaProperty.Register<TagsBox, bool>(name: nameof(SuggestionsAreCaseSensitive), defaultValue: true);

    public static readonly StyledProperty<bool> AllowDuplicationProperty =
        AvaloniaProperty.Register<TagsBox, bool>(name: nameof(AllowDuplication));

    public static readonly DirectProperty<TagsBox, IEnumerable<string>?> ItemsProperty =
        AvaloniaProperty.RegisterDirect<TagsBox, IEnumerable<string>?>(name: nameof(Items),
            getter: o => o.Items,
            setter: (
                o
                , v
            ) => o.Items = v,
            enableDataValidation: true);

    public static readonly DirectProperty<TagsBox, IEnumerable<string>?> TopItemsProperty =
        AvaloniaProperty.RegisterDirect<TagsBox, IEnumerable<string>?>(name: nameof(TopItems),
            getter: o => o.TopItems,
            setter: (
                o
                , v
            ) => o.TopItems = v);

    public static readonly DirectProperty<TagsBox, IList<string>?> SuggestionsProperty =
        AvaloniaProperty.RegisterDirect<TagsBox, IList<string>?>(
            name: nameof(Suggestions),
            getter: o => o.Suggestions,
            setter: (
                o
                , v
            ) => o.Suggestions = v);

    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<TagsBox, bool>(name: nameof(IsReadOnly));

    public static readonly StyledProperty<bool> EnableCounterProperty =
        AvaloniaProperty.Register<TagsBox, bool>(name: nameof(EnableCounter));

    public static readonly StyledProperty<bool> EnableDeleteProperty =
        AvaloniaProperty.Register<TagsBox, bool>(name: nameof(EnableDelete), defaultValue: true);

    private AutoCompleteBox? _autoCompleteBox;
    private CompositeDisposable? _compositeDisposable;
    private Control? _containerControl;
    private TextBox? _internalTextBox;
    private bool _isInputEnabled = true;
    private IEnumerable<string>? _items;
    private bool _requestAdd;
    private StringComparison _stringComparison;
    private IList<string>? _suggestions;
    private IEnumerable<string>? _topItems;
    private TextBlock? _watermark;

    [Content]
    public IEnumerable<string>? Items
    {
        get => _items;
        set => SetAndRaise(property: ItemsProperty, field: ref _items, value: value);
    }

    public bool IsCurrentTextValid
    {
        get => GetValue(property: IsCurrentTextValidProperty);
        private set => SetValue(property: IsCurrentTextValidProperty, value: value);
    }

    public bool RequestAdd
    {
        get => _requestAdd;
        set => SetAndRaise(property: RequestAddProperty, field: ref _requestAdd, value: value);
    }

    public IEnumerable<string>? TopItems
    {
        get => _topItems;
        set => SetAndRaise(property: TopItemsProperty, field: ref _topItems, value: value);
    }

    public string Watermark
    {
        get => GetValue(property: WatermarkProperty);
        set => SetValue(property: WatermarkProperty, value: value);
    }

    public bool RestrictInputToSuggestions
    {
        get => GetValue(property: RestrictInputToSuggestionsProperty);
        set => SetValue(property: RestrictInputToSuggestionsProperty, value: value);
    }

    public int ItemCountLimit
    {
        get => GetValue(property: ItemCountLimitProperty);
        set => SetValue(property: ItemCountLimitProperty, value: value);
    }

    public char TagSeparator
    {
        get => GetValue(property: TagSeparatorProperty);
        set => SetValue(property: TagSeparatorProperty, value: value);
    }

    public IList<string>? Suggestions
    {
        get => _suggestions;
        set => SetAndRaise(property: SuggestionsProperty, field: ref _suggestions, value: value);
    }

    public bool IsReadOnly
    {
        get => GetValue(property: IsReadOnlyProperty);
        set => SetValue(property: IsReadOnlyProperty, value: value);
    }

    public bool SuggestionsAreCaseSensitive
    {
        get => GetValue(property: SuggestionsAreCaseSensitiveProperty);
        set => SetValue(property: SuggestionsAreCaseSensitiveProperty, value: value);
    }

    public bool AllowDuplication
    {
        get => GetValue(property: AllowDuplicationProperty);
        set => SetValue(property: AllowDuplicationProperty, value: value);
    }

    public bool EnableCounter
    {
        get => GetValue(property: EnableCounterProperty);
        set => SetValue(property: EnableCounterProperty, value: value);
    }

    public bool EnableDelete
    {
        get => GetValue(property: EnableDeleteProperty);
        set => SetValue(property: EnableDeleteProperty, value: value);
    }

    public int MaxTextLength
    {
        get => GetValue(property: MaxTextLengthProperty);
        set => SetValue(property: MaxTextLengthProperty, value: value);
    }

    private string CurrentText => _autoCompleteBox?.Text ?? "";

    protected override void OnApplyTemplate(
        TemplateAppliedEventArgs e
    )
    {
        base.OnApplyTemplate(e: e);

        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();

        _watermark = e.NameScope.Find<TextBlock>(name: "PART_Watermark");
        var presenter = e.NameScope.Find<ItemsPresenter>(name: "PART_ItemsPresenter");
        presenter.ApplyTemplate();
        _containerControl = presenter.Panel;
        _autoCompleteBox = (_containerControl as ConcatenatingWrapPanel)?.ConcatenatedChildren.OfType<AutoCompleteBox>()
            .FirstOrDefault();

        if (_autoCompleteBox is null)
        {
            return;
        }

        Observable.FromEventPattern<TemplateAppliedEventArgs>(target: _autoCompleteBox
                , eventName: nameof(TemplateApplied))
            .Subscribe(onNext: args =>
            {
                _internalTextBox = args.EventArgs.NameScope.Find<TextBox>(name: "PART_TextBox");
                var suggestionListBox = args.EventArgs.NameScope.Find<ListBox>(name: "PART_SelectingItemsControl");

                _internalTextBox.WhenAnyValue(property1: x => x.IsFocused)
                    .Where(predicate: isFocused => isFocused == false)
                    .Subscribe(onNext: _ => RequestAdd = true)
                    .DisposeWith(compositeDisposable: _compositeDisposable);

                Observable
                    .FromEventPattern(target: suggestionListBox, eventName: nameof(PointerReleased))
                    .Subscribe(onNext: _ => RequestAdd = true)
                    .DisposeWith(compositeDisposable: _compositeDisposable);
            })
            .DisposeWith(compositeDisposable: _compositeDisposable);

        _autoCompleteBox
            .AddDisposableHandler(routedEvent: TextInputEvent, handler: OnTextInput, routes: RoutingStrategies.Tunnel)
            .DisposeWith(compositeDisposable: _compositeDisposable);

        _autoCompleteBox
            .AddDisposableHandler(routedEvent: KeyDownEvent, handler: OnKeyDown, routes: RoutingStrategies.Tunnel)
            .DisposeWith(compositeDisposable: _compositeDisposable);

        LayoutUpdated += OnLayoutUpdated;

        _autoCompleteBox.WhenAnyValue(property1: x => x.Text)
            .WhereNotNull()
            .Where(predicate: text => text.Contains(value: TagSeparator))
            .Subscribe(onNext: _ => RequestAdd = true)
            .DisposeWith(compositeDisposable: _compositeDisposable);

        this.WhenAnyValue(property1: x => x.RequestAdd)
            .Where(predicate: x => x)
            .Throttle(dueTime: TimeSpan.FromMilliseconds(value: 10))
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Select(selector: _ => CurrentText)
            .Subscribe(onNext: currentText =>
            {
                Dispatcher.UIThread.Post(action: () => RequestAdd = false);
                ClearInputField();

                var tags = GetFinalTags(input: currentText, tagSeparator: TagSeparator);

                foreach (var tag in tags)
                {
                    AddTag(tag: tag);
                }
            });

        _autoCompleteBox.WhenAnyValue(property1: x => x.Text)
            .Subscribe(onNext: _ =>
            {
                InvalidateWatermark();
                CheckIsCurrentTextValid();
            })
            .DisposeWith(compositeDisposable: _compositeDisposable);
    }

    private void CheckIsCurrentTextValid()
    {
        var correctedInput = CurrentText.ParseLabel();

        if (RestrictInputToSuggestions && Suggestions is { } suggestions)
        {
            IsCurrentTextValid = suggestions.Any(predicate: x =>
                x.Equals(value: correctedInput, comparisonType: _stringComparison));
            return;
        }

        if (!RestrictInputToSuggestions)
        {
            IsCurrentTextValid = !string.IsNullOrEmpty(value: correctedInput);
            return;
        }

        IsCurrentTextValid = false;
    }

    private void OnKeyDown(
        object? sender
        , KeyEventArgs e
    )
    {
        if (!_isInputEnabled && e.Key != Key.Back)
        {
            return;
        }

        var emptyInputField = string.IsNullOrEmpty(value: CurrentText);

        switch (e.Key)
        {
            case Key.Back when emptyInputField:
                RemoveLastTag();
                break;

            case Key.Enter or Key.Tab when !emptyInputField:
                RequestAdd = true;
                e.Handled = true;
                break;
        }
    }

    private void ClearInputField()
    {
        _autoCompleteBox?.ClearValue(property: AutoCompleteBox.SelectedItemProperty);
        Dispatcher.UIThread.Post(action: () => _autoCompleteBox?.ClearValue(property: AutoCompleteBox.TextProperty));
    }

    private IEnumerable<string> GetFinalTags(
        string input
        , char tagSeparator
    )
    {
        var tags = input.Split(separator: tagSeparator);

        foreach (var tag in tags)
        {
            var correctedTag = tag.ParseLabel();

            if (!string.IsNullOrEmpty(value: correctedTag))
            {
                yield return correctedTag;
            }
        }
    }

    private void OnLayoutUpdated(
        object? sender
        , EventArgs e
    ) => UpdateCounters();

    private void UpdateCounters()
    {
        var tagItems = _containerControl.GetVisualDescendants().OfType<TagControl>().ToArray();

        for (var i = 0; i < tagItems.Length; i++)
        {
            tagItems[i].OrdinalIndex = i + 1;
        }
    }

    protected override void OnGotFocus(
        GotFocusEventArgs e
    )
    {
        base.OnGotFocus(e: e);

        _internalTextBox?.Focus();
    }

    private void CheckIsInputEnabled()
    {
        if (Items is IList items && ItemCountLimit > 0)
        {
            _isInputEnabled = items.Count < ItemCountLimit;
        }
    }

    private void InvalidateWatermark()
    {
        if (_watermark is not null)
        {
            _watermark.IsVisible = (Items is null || (Items is not null && !Items.Any())) &&
                                   string.IsNullOrEmpty(value: CurrentText);
        }
    }

    private void OnTextInput(
        object? sender
        , TextInputEventArgs e
    )
    {
        if (sender is not AutoCompleteBox autoCompleteBox)
        {
            return;
        }

        var typedFullText = autoCompleteBox.SearchText + e.Text;

        if (!_isInputEnabled ||
            (typedFullText is { Length: 1 } && typedFullText.StartsWith(value: TagSeparator)) ||
            string.IsNullOrEmpty(value: typedFullText.ParseLabel()))
        {
            e.Handled = true;
            return;
        }

        var suggestions = Suggestions?.ToArray();

        if (RestrictInputToSuggestions &&
            suggestions is not null &&
            !suggestions.Any(predicate: x => x.StartsWith(value: typedFullText, comparisonType: _stringComparison)))
        {
            if (!typedFullText.EndsWith(value: TagSeparator) ||
                (typedFullText.EndsWith(value: TagSeparator) &&
                 !suggestions.Contains(value: autoCompleteBox.SearchText)))
            {
                e.Handled = true;
                return;
            }
        }

        if (e.Text is { Length: 1 } && e.Text.StartsWith(value: TagSeparator))
        {
            autoCompleteBox.Text = autoCompleteBox.SearchText;
            RequestAdd = true;
            e.Handled = true;
        }
    }

    /*protected override void UpdateDataValidation(
        AvaloniaProperty property
        , BindingValue value,
        Exception error
    )
    {
        if (property == ItemsProperty)
        {
            DataValidationErrors.SetError(control: this, error: value.Error);
        }
    }*/
    protected override void UpdateDataValidation(
        AvaloniaProperty property
        , BindingValueType state
        , Exception? error
    )
    {
        if (property == ItemsProperty)
        {
            DataValidationErrors.SetError(control: this, error: error);
        }
    }

    protected override void OnPropertyChanged(
        AvaloniaPropertyChangedEventArgs e
    )
    {
        base.OnPropertyChanged(change: e);

        if (e.Property == IsReadOnlyProperty)
        {
            PseudoClasses.Set(name: ":readonly", value: IsReadOnly);
        }
        else if (e.Property == SuggestionsAreCaseSensitiveProperty)
        {
            _stringComparison = SuggestionsAreCaseSensitive
                ? StringComparison.CurrentCulture
                : StringComparison.CurrentCultureIgnoreCase;
        }
    }

    private void RemoveLastTag()
    {
        if (Items is IList { Count: > 0 } items)
        {
            RemoveAt(index: items.Count - 1);
        }
    }

    public void RemoveAt(
        int index
    )
    {
        if (Items is not IList items)
        {
            return;
        }

        items.RemoveAt(index: index);
        CheckIsInputEnabled();
        InvalidateWatermark();
    }

    public void AddTag(
        string tag
    )
    {
        var inputTag = tag;

        if (Items is not IList items)
        {
            return;
        }

        if (ItemCountLimit > 0 && items.Count + 1 > ItemCountLimit)
        {
            return;
        }

        if (!AllowDuplication && items.Contains(value: tag))
        {
            return;
        }

        if (Suggestions is { } suggestions)
        {
            if (RestrictInputToSuggestions &&
                !suggestions.Any(predicate: x => x.Equals(value: tag, comparisonType: _stringComparison)))
            {
                return;
            }

            // When user tries to commit a tag,
            // check if it's already in the suggestions list
            // by comparing it case-insensitively.
            var result = suggestions.FirstOrDefault(predicate: x =>
                x.Equals(value: tag, comparisonType: StringComparison.CurrentCultureIgnoreCase));

            if (result is not null)
            {
                inputTag = result;
            }
        }

        items.Add(value: inputTag);
        CheckIsInputEnabled();
        InvalidateWatermark();
    }
}