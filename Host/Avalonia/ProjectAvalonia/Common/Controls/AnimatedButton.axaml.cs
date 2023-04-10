using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace ProjectAvalonia.Common.Controls;

public class AnimatedButton : TemplatedControl
{
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<AnimatedButton, ICommand>(name: nameof(Command));

    public static readonly StyledProperty<Geometry> NormalIconProperty =
        AvaloniaProperty.Register<AnimatedButton, Geometry>(name: nameof(NormalIcon));

    public static readonly StyledProperty<Geometry> ClickIconProperty =
        AvaloniaProperty.Register<AnimatedButton, Geometry>(name: nameof(ClickIcon));

    public static readonly StyledProperty<object> CommandParameterProperty =
        AvaloniaProperty.Register<AnimatedButton, object>(name: nameof(CommandParameter));

    public static readonly StyledProperty<double> InitialOpacityProperty =
        AvaloniaProperty.Register<AnimatedButton, double>(name: nameof(InitialOpacity), defaultValue: 0.6);

    public static readonly StyledProperty<double> PointerOverOpacityProperty =
        AvaloniaProperty.Register<AnimatedButton, double>(name: nameof(PointerOverOpacity), defaultValue: 1);

    public static readonly StyledProperty<bool> AnimateIconProperty =
        AvaloniaProperty.Register<AnimatedButton, bool>(name: nameof(AnimateIcon));

    public static readonly StyledProperty<bool> ExecuteOnOpenProperty =
        AvaloniaProperty.Register<AnimatedButton, bool>(name: nameof(ExecuteOnOpen));

    static AnimatedButton()
    {
        AffectsRender<AnimatedButton>(InitialOpacityProperty);
    }

    public ICommand Command
    {
        get => GetValue(property: CommandProperty);
        set => SetValue(property: CommandProperty, value: value);
    }

    public object CommandParameter
    {
        get => GetValue(property: CommandParameterProperty);
        set => SetValue(property: CommandParameterProperty, value: value);
    }

    public Geometry NormalIcon
    {
        get => GetValue(property: NormalIconProperty);
        set => SetValue(property: NormalIconProperty, value: value);
    }

    public Geometry ClickIcon
    {
        get => GetValue(property: ClickIconProperty);
        set => SetValue(property: ClickIconProperty, value: value);
    }

    public double InitialOpacity
    {
        get => GetValue(property: InitialOpacityProperty);
        set => SetValue(property: InitialOpacityProperty, value: value);
    }

    public double PointerOverOpacity
    {
        get => GetValue(property: PointerOverOpacityProperty);
        set => SetValue(property: PointerOverOpacityProperty, value: value);
    }

    public bool AnimateIcon
    {
        get => GetValue(property: AnimateIconProperty);
        set => SetValue(property: AnimateIconProperty, value: value);
    }

    public bool ExecuteOnOpen
    {
        get => GetValue(property: ExecuteOnOpenProperty);
        set => SetValue(property: ExecuteOnOpenProperty, value: value);
    }

    protected override void OnAttachedToVisualTree(
        VisualTreeAttachmentEventArgs e
    )
    {
        base.OnAttachedToVisualTree(e: e);

        AnimateIcon = ExecuteOnOpen;

        if (ExecuteOnOpen)
        {
            Command.Execute(parameter: default);
        }
    }
}