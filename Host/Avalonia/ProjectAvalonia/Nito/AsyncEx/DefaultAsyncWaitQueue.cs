using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Nito.Collections;

namespace Nito.AsyncEx;

/// <summary>
///     The default wait queue implementation, which uses a double-ended queue.
/// </summary>
/// <typeparam name="T">The type of the results. If this is not needed, use <see cref="object" />.</typeparam>
[DebuggerDisplay(value: "Count = {Count}")]
[DebuggerTypeProxy(type: typeof(DefaultAsyncWaitQueue<>.DebugView))]
public sealed class DefaultAsyncWaitQueue<T> : IAsyncWaitQueue<T>
{
    private readonly Deque<TaskCompletionSource<T>> _queue = new();

    private int Count
    {
        get => _queue.Count;
    }

    bool IAsyncWaitQueue<T>.IsEmpty
    {
        get => Count == 0;
    }

    Task<T> IAsyncWaitQueue<T>.Enqueue()
    {
        var tcs = TaskCompletionSourceExtensions.CreateAsyncTaskSource<T>();
        _queue.AddToBack(value: tcs);
        return tcs.Task;
    }

    void IAsyncWaitQueue<T>.Dequeue(
        T result
    )
    {
        _queue.RemoveFromFront().TrySetResult(result: result);
    }

    bool IAsyncWaitQueue<T>.TryCancel(
        Task task
        , CancellationToken cancellationToken
    )
    {
        for (var i = 0; i != _queue.Count; ++i)
            if (_queue[index: i].Task == task)
            {
                _queue[index: i].TrySetCanceled(cancellationToken: cancellationToken);
                _queue.RemoveAt(index: i);
                return true;
            }

        return false;
    }

    [DebuggerNonUserCode]
    internal sealed class DebugView
    {
        private readonly DefaultAsyncWaitQueue<T> _queue;

        public DebugView(
            DefaultAsyncWaitQueue<T> queue
        )
        {
            _queue = queue;
        }

        [DebuggerBrowsable(state: DebuggerBrowsableState.RootHidden)]
        public Task<T>[] Tasks
        {
            get
            {
                var result = new List<Task<T>>(capacity: _queue._queue.Count);
                foreach (var entry in _queue._queue) result.Add(item: entry.Task);

                return result.ToArray();
            }
        }
    }
}