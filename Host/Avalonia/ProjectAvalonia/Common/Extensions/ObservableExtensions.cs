using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ProjectAvalonia.Common.Extensions;

public static class ObservableExtensions
{
    public static IDisposable SubscribeAsync<T>(
        this IObservable<T> source
        , Func<T, Task> onNextAsync
    ) =>
        source
            .Select(selector: x => Observable.FromAsync(actionAsync: () => onNextAsync(arg: x)))
            .Concat()
            .Subscribe();

    public static IObservable<Unit> DoAsync<T>(
        this IObservable<T> source
        , Func<T, Task> onNextAsync
    ) =>
        source
            .Select(selector: x => Observable.FromAsync(actionAsync: () => onNextAsync(arg: x)))
            .Concat();

    public static IObservable<Unit> ToSignal<T>(
        this IObservable<T> source
    ) => source.Select(selector: _ => Unit.Default);

    public static IObservable<T> ReplayLastActive<T>(
        this IObservable<T> observable
    ) => observable.Replay(bufferSize: 1).RefCount();
}