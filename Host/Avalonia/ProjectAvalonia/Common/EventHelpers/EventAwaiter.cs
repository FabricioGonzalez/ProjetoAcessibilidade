using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectAvalonia.Common.Extensions;

namespace ProjectAvalonia.Common.EventHelpers;

public class EventAwaiter<TEventArgs> : EventsAwaiter<TEventArgs>
{
    public EventAwaiter(
        Action<EventHandler<TEventArgs>> subscribe
        , Action<EventHandler<TEventArgs>> unsubscribe
    ) : base(subscribe: subscribe, unsubscribe: unsubscribe, count: 1)
    {
        Task = Tasks[index: 0];
    }

    protected Task<TEventArgs> Task
    {
        get;
    }

    public async new Task<TEventArgs> WaitAsync(
        TimeSpan timeout
    )
        => await Task.WithAwaitCancellationAsync(timeout: timeout).ConfigureAwait(continueOnCapturedContext: false);

    public async new Task<TEventArgs> WaitAsync(
        CancellationToken token
    )
        => await Task.WithAwaitCancellationAsync(cancellationToken: token)
            .ConfigureAwait(continueOnCapturedContext: false);
}