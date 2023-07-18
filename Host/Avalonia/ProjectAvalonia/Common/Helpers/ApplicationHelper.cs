using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

namespace ProjectAvalonia.Common.Helpers;

public static class ApplicationHelper
{
    public static Window? MainWindow =>
        (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;

    public static IObservable<string> ClipboardTextChanged(
        IScheduler? scheduler = default
    ) =>
        Observable.Interval(period: TimeSpan.FromSeconds(value: 0.2), scheduler: scheduler ?? Scheduler.Default)
            .SelectMany(
                selector: _ => TopLevel.GetTopLevel(MainWindow)?.Clipboard?.GetTextAsync()
                    .ToObservable() ?? Observable.Return<string?>(value: null)
                    .WhereNotNull())
            .DistinctUntilChanged();
}