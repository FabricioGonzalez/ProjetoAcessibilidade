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

namespace ProjectAvalonia.Controls;

public class EditableTextBlock : TemplatedControl
{
    private string _text;
    private string _editText;
    private TextBox _textBox;
    private readonly DispatcherTimer _editClickTimer;

    public EditableTextBlock()
    {
        _editClickTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(500),
        };

        _editClickTimer.Tick += (sender, e) =>
         {
             _editClickTimer.Stop();

             if (IsFocused && !InEditMode)
             {
                 EnterEditMode();
             }
         };

        this.GetObservable(TextProperty).Subscribe(t =>
        {
            EditText = t;
        });

        this.GetObservable(InEditModeProperty).Subscribe(mode =>
        {
            if (mode && _textBox != null)
            {
                EnterEditMode();
            }
        });

        AddHandler(PointerPressedEvent, (sender, e) =>
        {
            _editClickTimer.Stop();

            if (!InEditMode)
            {
                var properties = e.GetCurrentPoint(this).Properties;
                if (e.ClickCount == 1 && properties.IsLeftButtonPressed && IsFocused)
                {
                    _editClickTimer.Start();
                }
            }
            else
            {
                var hit = this.InputHitTest(e.GetPosition(this));

                if (hit == null)
                {
                    ExitEditMode();
                }
            }
        }, RoutingStrategies.Tunnel);
    }

    public static readonly DirectProperty<EditableTextBlock, string> TextProperty = TextBlock.TextProperty.AddOwner<EditableTextBlock>(
            o => o.Text,
            (o, v) => o.Text = v,
            defaultBindingMode: BindingMode.TwoWay,
            enableDataValidation: true);

    public static readonly DirectProperty<EditableTextBlock, ICommand> CommandProperty = AvaloniaProperty.RegisterDirect<EditableTextBlock, ICommand>(
        nameof(Command),
        component => component.Command,
        (component, value) => component.Command = value);

    public static readonly DirectProperty<EditableTextBlock, object> CommandParameterProperty = AvaloniaProperty.RegisterDirect<EditableTextBlock, object>(
        nameof(Command),
        component => component.CommandParameter,
        (component, value) => component.CommandParameter = value);

    private ICommand command;
    public ICommand Command
    {
        get => command;
        set => SetAndRaise(CommandProperty, ref command, value);
    }

    private object commandParameter;
    public object CommandParameter
    {
        get => commandParameter;
        set => SetAndRaise(CommandParameterProperty, ref commandParameter, value);
    }

    [Content]
    public string Text
    {
        get => _text;
        set => SetAndRaise(TextProperty, ref _text, value);
    }

    public string EditText
    {
        get => _editText;
        set => SetAndRaise(EditTextProperty, ref _editText, value);
    }

    public static readonly DirectProperty<EditableTextBlock, string> EditTextProperty =
            AvaloniaProperty.RegisterDirect<EditableTextBlock, string>(nameof(EditText),
                o => o.EditText,
                (o, v) => o.EditText = v);

    public static readonly StyledProperty<bool> InEditModeProperty =
        AvaloniaProperty.Register<EditableTextBlock, bool>(
            nameof(InEditMode),
            defaultBindingMode: BindingMode.TwoWay);

    public bool InEditMode
    {
        get => GetValue(InEditModeProperty);
        set => SetValue(InEditModeProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _textBox = e.NameScope.Find<TextBox>("PART_TextBox");

        if (InEditMode)
        {
            EnterEditMode();
        }
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
                ExitEditMode();
                e.Handled = true;
                break;

            case Key.Escape:
                ExitEditMode(true);
                e.Handled = true;
                break;
        }

        base.OnKeyUp(e);
    }

    private void EnterEditMode()
    {
        EditText = Text;
        InEditMode = true;
        /*((VisualRoot as IInputRoot).MouseDevice as IPointer).Capture(_textBox);*/
        (VisualRoot as IInputRoot).MouseDevice.Capture(_textBox);
        /*(VisualRoot as IPointer).Capture(_textBox);*/
        _textBox.CaretIndex = Text.Length;
        _textBox.SelectionStart = 0;
        _textBox.SelectionEnd = Text.Length;

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            _textBox.Focus();
        });
    }

    private void ExitEditMode(bool restore = false)
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
                    Command.Execute(CommandParameter);
                }
                else
                    Command.Execute(null);
            }
        }

        InEditMode = false;
        ((VisualRoot as IInputRoot).MouseDevice).Capture(null);
        /*  var element = (VisualRoot as IInputRoot).PointerOverElement;*/
        /*(VisualRoot as IPointer).Capture(null);*/
    }

    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == InEditModeProperty)
        {
            PseudoClasses.Set(":editing", change.NewValue.GetValueOrDefault<bool>());
        }
    }
}
