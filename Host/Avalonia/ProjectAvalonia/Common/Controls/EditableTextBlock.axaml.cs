using System;
using System.Collections;
using System.Reactive;
using System.Windows.Input;

using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using Avalonia.Threading;

using ReactiveUI;

namespace ProjectAvalonia.Common.Controls;

public class EditableTextBlock : TemplatedControl
{
    public static readonly DirectProperty<EditableTextBlock, string> TextProperty =
        TextBlock.TextProperty.AddOwner<EditableTextBlock>(
            getter: o => o.Text,
            setter: (
                o,
                v
            ) => o.Text = v,
            defaultBindingMode: BindingMode.TwoWay,
            enableDataValidation: true);

    public static readonly DirectProperty<EditableTextBlock, ICommand?> CommandProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, ICommand?>(
            name: nameof(Command),
            getter: component => component.Command,
            setter: (
                component,
                value
            ) => component.Command = value);

    public static readonly DirectProperty<EditableTextBlock, bool?> HasActionsProperty =
       AvaloniaProperty.RegisterDirect<EditableTextBlock, bool?>(
           name: nameof(HasActions),
           getter: component => component.HasActions,
           setter: (
               component,
               value
           ) => component.HasActions = value,
           unsetValue: false);

    public static readonly DirectProperty<EditableTextBlock, IEnumerable?> ActionsProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, IEnumerable?>(
            name: nameof(Actions),
            getter: x => x.Actions,
            setter: (
                x,
                v
            ) => x.Actions = v);

    public static readonly DirectProperty<EditableTextBlock, object?> CommandParameterProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, object?>(
            name: nameof(CommandParameter),
            getter: component => component.CommandParameter,
            setter: (
                component,
                value
            ) => component.CommandParameter = value);

    public static readonly DirectProperty<EditableTextBlock, string> EditTextProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, string>(
            name: nameof(EditText),
            getter: o => o.EditText,
            setter: (
                o,
                v
            ) => o.EditText = v);

    public static readonly StyledProperty<bool> InEditModeProperty =
        AvaloniaProperty.Register<EditableTextBlock, bool>(
            name: nameof(InEditMode),
            defaultBindingMode: BindingMode.TwoWay);

    private readonly DispatcherTimer _editClickTimer;
    private IEnumerable? _actions;
    private bool? _hasActions;

    private ICommand? _command;


    private object? _commandParameter;
    private string _editText = string.Empty;
    private string _text = string.Empty;
    private TextBox _textBox;

    public EditableTextBlock()
    {
        RenameItem = ReactiveCommand.Create(
            execute: () =>
            {
                InEditMode = !InEditMode;
            });

        if (_hasActions.GetValueOrDefault(false))
        {
            _actions = new AvaloniaList<object>
        {
            new MenuItem
            {
                Command = RenameItem,
                Header = "Renomear"
            }
        };

            if (Actions is not null)
            {
                ContextFlyout = new MenuFlyout
                {
                    Items = _actions
                };
            }
        }

        _editClickTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(value: 500)
        };

        _editClickTimer.Tick += (
            _,
            _
        ) =>
        {
            _editClickTimer.Stop();

            if (IsFocused && !InEditMode)
            {
                EnterEditMode();
            }
        };

        this.GetObservable(property: TextProperty)
            .Subscribe(
                onNext: t =>
                {
                    EditText = t;
                });

        this.GetObservable(property: InEditModeProperty)
            .Subscribe(
                onNext: mode =>
                {
                    if (mode && _textBox != null)
                    {
                        EnterEditMode();
                    }
                });

        AddHandler(
            routedEvent: PointerPressedEvent,
            handler: (
                _,
                e
            ) =>
            {
                _editClickTimer.Stop();

                if (!InEditMode)
                {
                    var properties = e.GetCurrentPoint(relativeTo: this)
                        .Properties;
                    if (e.ClickCount == 1 &&
                        properties.IsLeftButtonPressed &&
                        IsFocused)
                    {
                        _editClickTimer.Start();
                    }
                }
                else
                {
                    var hit = this.InputHitTest(p: e.GetPosition(relativeTo: this));

                    if (hit == null)
                    {
                        ExitEditMode();
                    }
                }
            },
            routes: RoutingStrategies.Tunnel);
    }


    public IEnumerable? Actions
    {
        get => _actions;
        set => SetAndRaise(
            property: ActionsProperty,
            field: ref _actions,
            value: value);
    }

    public ICommand? Command
    {
        get => _command;
        set => SetAndRaise(
            property: CommandProperty,
            field: ref _command,
            value: value);
    }
    public bool? HasActions
    {
        get => _hasActions;
        set => SetAndRaise(
            property: HasActionsProperty,
            field: ref _hasActions,
            value: value);
    }

    private ReactiveCommand<Unit, Unit>? CommitCommand
    {
        get;
    }

    public object? CommandParameter
    {
        get => _commandParameter;
        set => SetAndRaise(
            property: CommandParameterProperty,
            field: ref _commandParameter,
            value: value);
    }

    [Content]
    public string Text
    {
        get => _text;
        set => SetAndRaise(
            property: TextProperty,
            field: ref _text,
            value: value);
    }

    public string EditText
    {
        get => _editText;
        set => SetAndRaise(
            property: EditTextProperty,
            field: ref _editText,
            value: value);
    }

    public bool InEditMode
    {
        get => GetValue(property: InEditModeProperty);
        set => SetValue(
            property: InEditModeProperty,
            value: value);
    }

    private ReactiveCommand<Unit, Unit> RenameItem
    {
        get;
    }

    protected override void OnApplyTemplate(
        TemplateAppliedEventArgs e
    )
    {
        base.OnApplyTemplate(e: e);

        _textBox = e.NameScope.Find<TextBox>(name: "PART_TextBox");

        if (InEditMode)
        {
            EnterEditMode();
        }
    }

    protected override void OnKeyUp(
        KeyEventArgs e
    )
    {
        switch (e.Key)
        {
            case Key.Enter:
                ExitEditMode();
                e.Handled = true;
                break;

            case Key.Escape:
                ExitEditMode(restore: true);
                e.Handled = true;
                break;
        }

        base.OnKeyUp(e: e);
    }

    private void EnterEditMode()
    {
        EditText = Text;
        InEditMode = true;
        /*((VisualRoot as IInputRoot).MouseDevice as IPointer).Capture(_textBox);*/
        /*(VisualRoot as IInputRoot).MouseDevice.Capture(_textBox);*/
        /*(VisualRoot as IPointer).Capture(_textBox);*/
        _textBox.CaretIndex = Text.Length;
        _textBox.SelectionStart = 0;
        _textBox.SelectionEnd = Text.Length;

        Dispatcher.UIThread.InvokeAsync(
            action: () =>
            {
                _textBox.Focus();
            });
    }

    private void ExitEditMode(
        bool restore = false
    )
    {
        if (restore)
        {
            EditText = Text;
        }
        else
        {
            Text = EditText;
            if (Command is not null)
            {
                if (CommandParameter is not null)
                {
                    Command.Execute(parameter: CommandParameter);
                }
                else
                {
                    Command.Execute(parameter: null);
                }
            }
        }

        InEditMode = false;
        /*((VisualRoot as IInputRoot).MouseDevice).Capture(null);*/
        /*  var element = (VisualRoot as IInputRoot).PointerOverElement;*/
        /*(VisualRoot as IPointer).Capture(null);*/
    }

    protected override void OnPropertyChanged<T>(
        AvaloniaPropertyChangedEventArgs<T> change
    )
    {
        base.OnPropertyChanged(change: change);

        if (change.Property == InEditModeProperty)
        {
            PseudoClasses.Set(
                name: ":editing",
                value: change.NewValue.GetValueOrDefault<bool>());
        }
    }
}