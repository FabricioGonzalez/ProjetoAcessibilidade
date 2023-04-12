using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ProjectAvalonia.Common.EventHelpers;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Logging;

namespace ProjectAvalonia.Common.Bases;

/// <summary>
///     <see cref="PeriodicRunner" /> is an extension of <see cref="BackgroundService" /> that is useful for tasks
///     that are supposed to repeat regularly.
/// </summary>
public abstract class PeriodicRunner : BackgroundService
{
    private volatile TaskCompletionSource<bool>? _tcs;

    protected PeriodicRunner(
        TimeSpan period
    )
    {
        Period = period;
        ExceptionTracker = new LastExceptionTracker();
    }

    public TimeSpan Period
    {
        get;
    }

    private LastExceptionTracker ExceptionTracker
    {
        get;
    }

    /// <summary>
    ///     Action successfully executed. Returns how long it took.
    /// </summary>
    public event EventHandler<TimeSpan>? Tick;

    /// <summary>
    ///     Normally, <see cref="ActionAsync(CancellationToken)" /> user-action is called every time that <see cref="Period" />
    ///     elapses.
    ///     This method allows to expedite the process by interrupting the waiting process.
    /// </summary>
    /// <remarks>
    ///     If <see cref="ExecuteAsync(CancellationToken)" /> is not actually in waiting phase, this method call makes
    ///     sure that next waiting process will be omitted altogether.
    ///     <para>Note that when the <see cref="PeriodicRunner" /> has not been started, this call is ignored.</para>
    /// </remarks>
    public void TriggerRound() =>
        // Note: All members of TaskCompletionSource<TResult> are thread-safe and may be used from multiple threads concurrently.
        _tcs?.TrySetResult(result: true);

    /// <summary>
    ///     Triggers and waits for the action to execute.
    /// </summary>
    public async Task TriggerAndWaitRoundAsync(
        CancellationToken token
    )
    {
        EventAwaiter<TimeSpan> eventAwaiter = new(
            subscribe: h => Tick += h,
            unsubscribe: h => Tick -= h);
        TriggerRound();
        await eventAwaiter.WaitAsync(token: token).ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task TriggerAndWaitRoundAsync(
        TimeSpan timeout
    )
    {
        using CancellationTokenSource cancellationTokenSource = new(delay: timeout);
        await TriggerAndWaitRoundAsync(token: cancellationTokenSource.Token)
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>
    ///     Abstract method that is called every <see cref="Period" /> or sooner when <see cref="TriggerRound" /> is called.
    /// </summary>
    /// <remarks>Exceptions are handled in <see cref="ExecuteAsync(CancellationToken)" />.</remarks>
    protected abstract Task ActionAsync(
        CancellationToken cancel
    );

    /// <inheritdoc />
    protected async override Task ExecuteAsync(
        CancellationToken stoppingToken
    )
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _tcs = new TaskCompletionSource<bool>();

            try
            {
                // Do user action.
                var before = DateTimeOffset.UtcNow;
                await ActionAsync(cancel: stoppingToken).ConfigureAwait(continueOnCapturedContext: false);
                Tick?.Invoke(sender: this, e: DateTimeOffset.UtcNow - before);

                var info = ExceptionTracker.LastException;

                // Log previous exception if any.
                if (info is not null)
                {
                    Logger.LogInfo(message: $"Exception stopped coming. It came for " +
                                            $"{(DateTimeOffset.UtcNow - info.FirstAppeared).TotalSeconds} seconds, " +
                                            $"{info.ExceptionCount} times: {info.Exception.ToTypeMessageString()}");
                    ExceptionTracker.Reset();
                }
            }
            catch (Exception ex) when (ex is OperationCanceledException or TimeoutException)
            {
                Logger.LogTrace(exception: ex);
            }
            catch (Exception ex)
            {
                // Exception encountered, process it.
                var info = ExceptionTracker.Process(currentException: ex);
                if (info.IsFirst)
                {
                    Logger.LogError(exception: info.Exception);
                }
            }

            // Wait for the next round.
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(token: stoppingToken);
                var linkedTcs = _tcs; // Copy reference so it cannot change.

                if (linkedTcs.Task == await Task
                        .WhenAny(task1: linkedTcs.Task, task2: Task.Delay(delay: Period, cancellationToken: cts.Token))
                        .ConfigureAwait(continueOnCapturedContext: false))
                {
                    cts.Cancel(); // Ensure that the Task.Delay task is cleaned up.
                }
                else
                {
                    linkedTcs.TrySetCanceled(
                        cancellationToken: stoppingToken); // Ensure that the tcs.Task is cleaned up.
                }
            }
            catch (TaskCanceledException ex)
            {
                Logger.LogTrace(exception: ex);
            }
        }
    }
}