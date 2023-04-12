using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using ProjectAvalonia.Common.Extensions;

namespace ProjectAvalonia.Behaviors;

public class NumberBoxBehavior : DisposingBehavior<TextBox>
{
    protected override void OnAttached(
        CompositeDisposable disposables
    )
    {
        if (AssociatedObject is null)
        {
            return;
        }

        AssociatedObject
            .AddDisposableHandler(routedEvent: InputElement.TextInputEvent, handler: (
                _
                , e
            ) =>
            {
                if (e.Text is not null)
                {
                    e.Text = CorrectInput(input: e.Text);
                }
            }, routes: RoutingStrategies.Tunnel)
            .DisposeWith(compositeDisposable: disposables);

        Observable
            .FromEventPattern<RoutedEventArgs>(target: AssociatedObject
                , eventName: nameof(AssociatedObject.PastingFromClipboard))
            .Select(selector: x => x.EventArgs)
            .SubscribeAsync(onNextAsync: async e =>
            {
                e.Handled = true;

                if (Application.Current is { Clipboard: { } clipboard })
                {
                    AssociatedObject.Text = CorrectInput(input: await clipboard.GetTextAsync());
                }
            })
            .DisposeWith(compositeDisposable: disposables);
    }

    private string CorrectInput(
        string input
    ) => new(value: input.Where(predicate: c => char.IsDigit(c: c) || c == '.').ToArray());
}