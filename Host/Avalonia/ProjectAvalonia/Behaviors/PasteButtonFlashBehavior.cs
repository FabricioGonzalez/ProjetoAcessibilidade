using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ProjectAvalonia.Common.Controls;
using ProjectAvalonia.Common.Extensions;
using ReactiveUI;

namespace ProjectAvalonia.Behaviors;

public class PasteButtonFlashBehavior : AttachedToVisualTreeBehavior<AnimatedButton>
{
    public static readonly StyledProperty<string> FlashAnimationProperty =
        AvaloniaProperty.Register<PasteButtonFlashBehavior, string>(name: nameof(FlashAnimation));

    public static readonly StyledProperty<string> CurrentAddressProperty =
        AvaloniaProperty.Register<PasteButtonFlashBehavior, string>(name: nameof(CurrentAddress));

    private string? _lastFlashedOn;

    public string FlashAnimation
    {
        get => GetValue(property: FlashAnimationProperty);
        set => SetValue(property: FlashAnimationProperty, value: value);
    }

    public string CurrentAddress
    {
        get => GetValue(property: CurrentAddressProperty);
        set => SetValue(property: CurrentAddressProperty, value: value);
    }

    protected override void OnAttachedToVisualTree(
        CompositeDisposable disposables
    )
    {
        /* RxApp.MainThreadScheduler.Schedule(async () => await CheckClipboardForValidAddressAsync());*/

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            var mainWindow = lifetime.MainWindow;

            Observable
                .FromEventPattern(target: mainWindow, eventName: nameof(mainWindow.Activated))
                .Select(selector: _ => Unit.Default)
                .Merge(second: this.WhenAnyValue(property1: x => x.CurrentAddress).Select(selector: _ => Unit.Default))
                .Throttle(dueTime: TimeSpan.FromMilliseconds(value: 100))
                .ObserveOn(scheduler: RxApp.MainThreadScheduler)
                .SubscribeAsync(onNextAsync: async _ =>
                {
                    await Task.Run(function: () => "");
                    /*    await CheckClipboardForValidAddressAsync();*/
                })
                .DisposeWith(compositeDisposable: disposables);

            Observable
                .Interval(period: TimeSpan.FromMilliseconds(value: 500))
                .ObserveOn(scheduler: RxApp.MainThreadScheduler)
                .SubscribeAsync(onNextAsync: async _ =>
                {
                    if (!mainWindow.IsActive)
                    {
                        return;
                    }

                    await Task.Run(function: () => "");
                    /*    await CheckClipboardForValidAddressAsync();*/
                })
                .DisposeWith(compositeDisposable: disposables);
        }

        AssociatedObject?.WhenAnyValue(property1: x => x.AnimateIcon)
            .Where(predicate: x => x)
            .Subscribe(onNext: _ => AssociatedObject.Classes.Remove(name: FlashAnimation))
            .DisposeWith(compositeDisposable: disposables);
    }

    /* private async Task CheckClipboardForValidAddressAsync(bool forceCheck = false)
     {
         if (Application.Current is { Clipboard: { } clipboard })
         {
             var clipboardValue = (await clipboard.GetTextAsync()) ?? "";

             // Yes, it can be null, the software crashed without this condition.
             if (clipboardValue is null)
             {
                 return;
             }

             if (AssociatedObject is null)
             {
                 return;
             }

             if (_lastFlashedOn == clipboardValue && !forceCheck)
             {
                 return;
             }

             AssociatedObject.Classes.Remove(FlashAnimation);

             clipboardValue = clipboardValue.Trim();

             // ClipboardValue might not match CurrentAddress, but it might be a PayJoin address pointing to the CurrentAddress
             // Hence we need to compare both string value and parse result
             if (clipboardValue != CurrentAddress &&
                 AddressStringParser.TryParse(clipboardValue, Services.WalletManager.Network, out var address) &&
                 address?.Address?.ToString() != CurrentAddress)
             {
                 AssociatedObject.Classes.Add(FlashAnimation);
                 _lastFlashedOn = clipboardValue;
                 ToolTip.SetTip(AssociatedObject, $"Paste BTC Address:\r\n{clipboardValue}");
             }
             else
             {
                 ToolTip.SetTip(AssociatedObject, "Paste");
             }
         }
     }*/
}