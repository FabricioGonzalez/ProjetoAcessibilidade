using System;
using System.Threading.Tasks;

namespace ProjectAvalonia.Nito.AsyncEx;

public class RunningTasks : IDisposable
{
    public RunningTasks(
        AbandonedTasks taskCollection
    )
    {
        taskCollection.AddAndClearCompleted(Completion.Task);
    }

    private bool DisposedValue { get; set; }
    private TaskCompletionSource Completion { get; } = new();

    public void Dispose()
    {
        Dispose(disposing: true);
    }

    public static IDisposable RememberWith(
        AbandonedTasks taskCollection
    )
    {
        return new RunningTasks(taskCollection: taskCollection);
    }

    protected virtual void Dispose(
        bool disposing
    )
    {
        if (!DisposedValue)
        {
            if (disposing) Completion.TrySetResult();

            DisposedValue = true;
        }
    }
}