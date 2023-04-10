using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using Avalonia.Threading;

namespace ProjectAvalonia.Common.Controls;

public class EditableTextBlock : TemplatedControl
{
    public static readonly DirectProperty<EditableTextBlock, string> TextProperty =
        TextBlock.TextProperty.AddOwner<EditableTextBlock>(
            getter: o => o.Text,
            setter: (
                o
                , v
            ) => o.Text = v,
            defaultBindingMode: BindingMode.TwoWay,
            enableDataValidation: true);

    public static readonly DirectProperty<EditableTextBlock, ICommand> CommandProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, ICommand?>(
            name: nameof(Command),
            getter: component => component.Command,
            setter: (
                component
                , value
            ) => component.Command = value);

    public static readonly DirectProperty<EditableTextBlock, object?> CommandParameterProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, object?>(
            name: nameof(CommandParameter),
            getter: component => component.CommandParameter,
            setter: (
                component
                , value
            ) => component.CommandParameter = value);

    public static readonly DirectProperty<EditableTextBlock, string> EditTextProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, string>(name: nameof(EditText),
            getter: o => o.EditText,
            setter: (
                o
                , v
            ) => o.EditText = v);

    public static readonly StyledProperty<bool> InEditModeProperty =
        AvaloniaProperty.Register<EditableTextBlock, bool>(
            name: nameof(InEditMode),
            defaultBindingMode: BindingMode.TwoWay);

    private readonly DispatcherTimer _editClickTimer;
    private string _editText;
    private string _text;
    private TextBox _textBox;

    private ICommand? command;

    private object? commandParameter;

    public EditableTextBlock()
    {
        _editClickTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(value: 500)
        };

        _editClickTimer.Tick += (
            sender
            , e
        ) =>
        {
            _editClickTimer.Stop();

            if (IsFocused && !InEditMode)
            {
                EnterEditMode();
            }
        };

        this.GetObservable(property: TextProperty).Subscribe(onNext: t =>
        {
            EditText = t;
        });

        this.GetObservable(property: InEditModeProperty).Subscribe(onNext: mode =>
        {
            if (mode && _textBox != null)
            {
                EnterEditMode();
            }
        });

        AddHandler(routedEvent: PointerPressedEvent, handler: (
            sender
            , e
        ) =>
        {
            _editClickTimer.Stop();

            if (!InEditMode)
            {
                var properties = e.GetCurrentPoint(relativeTo: this).Properties;
                if (e.ClickCount == 1 && properties.IsLeftButtonPressed && IsFocused)
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
        }, routes: RoutingStrategies.Tunnel);
    }

    public ICommand? Command
    {
        get => command;
        set => SetAndRaise(property: CommandProperty, field: ref command, value: value);
    }

    public object? CommandParameter
    {
        get => commandParameter;
        set => SetAndRaise(property: CommandParameterProperty, field: ref commandParameter, value: value);
    }

    [Content]
    public string Text
    {
        get => _text;
        set => SetAndRaise(property: TextProperty, field: ref _text, value: value);
    }

    public string EditText
    {
        get => _editText;
        set => SetAndRaise(property: EditTextProperty, field: ref _editText, value: value);
    }

    public bool InEditMode
    {
        get => GetValue(property: InEditModeProperty);
        set => SetValue(property: InEditModeProperty, value: value);
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

        Dispatcher.UIThread.InvokeAsync(action: () =>
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
            PseudoClasses.Set(name: ":editing", value: change.NewValue.GetValueOrDefault<bool>());
        }
    }
}