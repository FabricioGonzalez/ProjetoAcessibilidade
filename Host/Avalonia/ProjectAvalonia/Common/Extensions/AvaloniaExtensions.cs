using System;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Interactivity;

namespace ProjectAvalonia.Common.Extensions;

public static class AvaloniaExtensions
{
    public static IObservable<EventPattern<TEventArgs>> OnEvent<TEventArgs>(
        this IInteractive target
        , RoutedEvent<TEventArgs> routedEvent
        , RoutingStrategies routingStrategies = RoutingStrategies.Bubble
    )
        where TEventArgs : RoutedEventArgs =>
        Observable.FromEventPattern<TEventArgs>(
            addHandler: add => target.AddHandler(routedEvent: routedEvent, handler: add, routes: routingStrategies),
            removeHandler: remove => target.RemoveHandler(routedEvent: routedEvent, handler: remove));
}