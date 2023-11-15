using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;

namespace ProjectAvalonia.Common.EventHelpers;

/// <summary>
///     Source: https://stackoverflow.com/a/42117130/2061103
/// </summary>
public class EventsAwaiter<TEventArgs>
{
    public EventsAwaiter(
        Action<EventHandler<TEventArgs>> subscribe
        , Action<EventHandler<TEventArgs>> unsubscribe
        , int count
    )
    {
        Guard.MinimumAndNotNull(parameterName: nameof(count), value: count, smallest: 0);

        var eventsArrived = new List<TaskCompletionSource<TEventArgs>>(capacity: count);

        for (var i = 0; i < count; i++)
        {
            eventsArrived.Add(item: new TaskCompletionSource<TEventArgs>());
        }

        EventsArrived = eventsArrived;
        Tasks = EventsArrived.Select(selector: x => x.Task).ToArray();
        Unsubscribe = unsubscribe;

        subscribe(obj: SubscriptionEventHandler);
    }

    /// <remarks>Guards <see cref="EventsArrived" />.</remarks>
    private object Lock
    {
        get;
    } = new();

    /// <remarks>Guarded by <see cref="Lock" />.</remarks>
    protected IReadOnlyList<TaskCompletionSource<TEventArgs>> EventsArrived
    {
        get;
    }

    protected IReadOnlyList<Task<TEventArgs>> Tasks
    {
        get;
    }

    private Action<EventHandler<TEventArgs>> Unsubscribe
    {
        get;
    }

    private void SubscriptionEventHandler(
        object? sender
        , TEventArgs e
    )
    {
        lock (Lock)
        {
            var firstUnfinished = EventsArrived.FirstOrDefault(predicate: x => !x.Task.IsCompleted);
            firstUnfinished?.TrySetResult(result: e);

            // This is guaranteed to happen only once.
            if (Tasks.All(predicate: x => x.IsCompleted))
            {
                Unsubscribe(obj: SubscriptionEventHandler);
            }
        }
    }

    public async Task<IEnumerable<TEventArgs>> WaitAsync(
        TimeSpan timeout
    )
        => await Task.WhenAll(tasks: Tasks).WithAwaitCancellationAsync(timeout: timeout)
            .ConfigureAwait(continueOnCapturedContext: false);

    public async Task<IEnumerable<TEventArgs>> WaitAsync(
        CancellationToken token
    )
        => await Task.WhenAll(tasks: Tasks).WithAwaitCancellationAsync(cancellationToken: token)
            .ConfigureAwait(continueOnCapturedContext: false);
}