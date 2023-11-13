using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using ReactiveUI;

namespace ProjectAvalonia.Common.Controls;

public class ClipboardCopyButton : TemplatedControl
{
    public static readonly StyledProperty<ReactiveCommand<Unit, Unit>> CopyCommandProperty =
        AvaloniaProperty.Register<ClipboardCopyButton, ReactiveCommand<Unit, Unit>>(name: nameof(CopyCommand));

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<ClipboardCopyButton, string>(name: nameof(Text));

    public ClipboardCopyButton()
    {
        var canCopy = this.WhenAnyValue(property1: x => x.Text, selector: text => text is not null);
        CopyCommand = ReactiveCommand.CreateFromTask(execute: CopyToClipboardAsync, canExecute: canCopy);
    }

    public ReactiveCommand<Unit, Unit> CopyCommand
    {
        get => GetValue(property: CopyCommandProperty);
        set => SetValue(property: CopyCommandProperty, value: value);
    }

    public string Text
    {
        get => GetValue(property: TextProperty);
        set => SetValue(property: TextProperty, value: value);
    }

    private async Task CopyToClipboardAsync()
    {
        if (TopLevel.GetTopLevel(this) is { Clipboard: { } clipboard })
        {
            await clipboard.SetTextAsync(text: Text);
            await Task.Delay(
                millisecondsDelay: 1000); // Introduces a delay while the animation is playing (1s). This will make the command 'busy' while being animated, avoiding reentrancy.
        }
    }
}