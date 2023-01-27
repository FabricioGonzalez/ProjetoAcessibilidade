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
    private string? _lastFlashedOn;

    public static readonly StyledProperty<string> FlashAnimationProperty =
        AvaloniaProperty.Register<PasteButtonFlashBehavior, string>(nameof(FlashAnimation));

    public static readonly StyledProperty<string> CurrentAddressProperty =
        AvaloniaProperty.Register<PasteButtonFlashBehavior, string>(nameof(CurrentAddress));

    public string FlashAnimation
    {
        get => GetValue(FlashAnimationProperty);
        set => SetValue(FlashAnimationProperty, value);
    }

    public string CurrentAddress
    {
        get => GetValue(CurrentAddressProperty);
        set => SetValue(CurrentAddressProperty, value);
    }

    protected override void OnAttachedToVisualTree(CompositeDisposable disposables)
    {
        /* RxApp.MainThreadScheduler.Schedule(async () => await CheckClipboardForValidAddressAsync());*/

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            var mainWindow = lifetime.MainWindow;

            Observable
                .FromEventPattern(mainWindow, nameof(mainWindow.Activated)).Select(_ => Unit.Default)
                .Merge(this.WhenAnyValue(x => x.CurrentAddress).Select(_ => Unit.Default))
                .Throttle(TimeSpan.FromMilliseconds(100))
                .ObserveOn(RxApp.MainThreadScheduler)
                .SubscribeAsync(async _ =>
                {
                    await Task.Run(() => "");
                    /*    await CheckClipboardForValidAddressAsync();*/
                })
                .DisposeWith(disposables);

            Observable
                .Interval(TimeSpan.FromMilliseconds(500))
                .ObserveOn(RxApp.MainThreadScheduler)
                .SubscribeAsync(async _ =>
                {
                    if (!mainWindow.IsActive)
                    {
                        return;
                    }
                    await Task.Run(() => "");
                    /*    await CheckClipboardForValidAddressAsync();*/
                })
                .DisposeWith(disposables);
        }

        AssociatedObject?.WhenAnyValue(x => x.AnimateIcon)
            .Where(x => x)
            .Subscribe(_ => AssociatedObject.Classes.Remove(FlashAnimation))
            .DisposeWith(disposables);
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
