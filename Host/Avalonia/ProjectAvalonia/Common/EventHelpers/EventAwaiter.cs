using System;
using System.Threading;
using System.Threading.Tasks;

using ProjectAvalonia.Common.Extensions;

namespace ProjectAvalonia.Common.EventHelpers;
public class EventAwaiter<TEventArgs> : EventsAwaiter<TEventArgs>
{
    public EventAwaiter(Action<EventHandler<TEventArgs>> subscribe, Action<EventHandler<TEventArgs>> unsubscribe) : base(subscribe, unsubscribe, count: 1)
    {
        Task = Tasks[0];
    }

    protected Task<TEventArgs> Task
    {
        get;
    }

    public new async Task<TEventArgs> WaitAsync(TimeSpan timeout)
        => await Task.WithAwaitCancellationAsync(timeout).ConfigureAwait(false);

    public new async Task<TEventArgs> WaitAsync(CancellationToken token)
        => await Task.WithAwaitCancellationAsync(token).ConfigureAwait(false);
}
