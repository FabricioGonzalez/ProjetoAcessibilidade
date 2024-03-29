﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectAvalonia.Common.Extensions;

public static class TaskExtensions
{
    /// <summary>
    ///     Implements method that behaves as <see cref="Task{T}.WaitAsync(CancellationToken)" /> but
    ///     it throws <see cref="OperationCanceledException" /> instead of <see cref="TimeoutException" />.
    /// </summary>
    public static async Task<T> WithAwaitCancellationAsync<T>(
        this Task<T> task
        , CancellationToken cancellationToken
    )
    {
        try
        {
            return await task.WaitAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (TimeoutException e)
        {
            throw new OperationCanceledException(message: "Timed out.", innerException: e);
        }
    }

    /// <summary>
    ///     Implements method that behaves as <see cref="Task.WaitAsync(CancellationToken)" /> but
    ///     it throws <see cref="OperationCanceledException" /> instead of <see cref="TimeoutException" />.
    /// </summary>
    public static async Task WithAwaitCancellationAsync(
        this Task task
        , CancellationToken cancellationToken
    )
    {
        try
        {
            await task.WaitAsync(cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (TimeoutException e)
        {
            throw new OperationCanceledException(message: "Timed out.", innerException: e);
        }
    }

    /// <summary>
    ///     Implements method that behaves as <see cref="Task{T}.WaitAsync(TimeSpan)" /> but
    ///     it throws <see cref="OperationCanceledException" /> instead of <see cref="TimeoutException" />.
    /// </summary>
    public static async Task<T> WithAwaitCancellationAsync<T>(
        this Task<T> task
        , TimeSpan timeout
    )
    {
        try
        {
            return await task.WaitAsync(timeout: timeout).ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (TimeoutException e)
        {
            throw new OperationCanceledException(message: "Timed out.", innerException: e);
        }
    }

    /// <summary>
    ///     Implements method that behaves as <see cref="Task.WaitAsync(TimeSpan)" /> but
    ///     it throws <see cref="OperationCanceledException" /> instead of <see cref="TimeoutException" />.
    /// </summary>
    public static async Task WithAwaitCancellationAsync(
        this Task task
        , TimeSpan timeout
    )
    {
        try
        {
            await task.WaitAsync(timeout: timeout).ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (TimeoutException e)
        {
            throw new OperationCanceledException(message: "Timed out.", innerException: e);
        }
    }
}