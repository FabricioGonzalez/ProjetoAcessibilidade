using System.Diagnostics.Contracts;

using Common.Models;

namespace Common.Result;
public enum ResultState : byte
{
    Faulted,
    Success
}

/// <summary>
/// Represents the result of an operation:
/// 
///     A | Exception
/// 
/// </summary>
/// <typeparam name="A">Bound value type</typeparam>
/// <remarks>
/// `Result<A>` (and `OptionalResult<A>`) is purely there to represent a concrete result value of a invoked lazy operation 
/// (like `Try<A>`).  You're not really meant to consume it directly.
/// 
/// For example:
/// 
///     var ma = Try(...);
///     var ra = ma(); // This will produce a `Result<A>` because that's what the `Try` delegate returns
/// 
/// But you should be matching on the result, or using any of the other convenient extension methods to get a concrete value:
/// 
///     var ma = Try(...);
///     var ra1 = ma.IfFail(0);
///     var ra1 = ma.Match(Succ: x => x + 10, Fail: 0);
///     // ... etc ...
/// </remarks>
public readonly struct Result<A>
{
    internal readonly ResultState State;
    internal readonly A Value;
    private readonly Exception _error;

    internal Exception Error => _error;

    /// <summary>
    /// Constructor of a concrete value
    /// </summary>
    /// <param name="value"></param>
    internal Result(A value)
    {
        State = ResultState.Success;
        Value = value;
        _error = null;
    }

    /// <summary>
    /// Constructor of an error value
    /// </summary>
    /// <param name="e"></param>
    internal Result(Exception e)
    {
        State = ResultState.Faulted;
        _error = e;
        Value = default;
    }

    public static Result<TValue> Success<TValue>(TValue Value) => new Result<TValue>(Value);

    public static Result<A> Failure(Exception excpt) => new Result<A>(excpt);


    /// <summary>
    /// Implicit conversion operator from A to Result<A>
    /// </summary>
    /// <param name="value">Value</param>
    [Pure]
    public static implicit operator Result<A>(A value) =>
        new Result<A>(value);

    /// <summary>
    /// True if the result is faulted
    /// </summary>
    [Pure]
    public bool IsFaulted =>
        State == ResultState.Faulted;

    /// <summary>
    /// True if the struct is in an invalid state
    /// </summary>
    [Pure]
    public bool IsBottom =>
        State == ResultState.Faulted && (_error == null);

    /// <summary>
    /// True if the struct is in an success
    /// </summary>
    [Pure]
    public bool IsSuccess =>
        State == ResultState.Success;

    /// <summary>
    /// Convert the value to a showable string
    /// </summary>
    [Pure]
    public override string ToString() =>
        IsFaulted
            ? _error?.ToString() ?? "(Bottom)"
            : Value?.ToString() ?? "(null)";

    [Pure]
    public A IfFail(A defaultValue) =>
        IsFaulted
            ? defaultValue
            : Value;

    [Pure]
    public A IfFail(Func<Exception, A> f) =>
        IsFaulted
            ? f(Error)
            : Value;

    public Empty IfFail(Action<Exception> f)
    {
        if (IsFaulted) f(Error);
        return Empty.Value;
    }

    public Empty IfSucc(Action<A> f)
    {
        if (IsSuccess) f(Value);
        return Empty.Value;
    }

    [Pure]
    public async Task<Result<A>> DoAsync(Func<A, Task> SuccAsync, Func<Exception, Task> FailAsync)
    {
        if (IsFaulted)
        {

            await FailAsync(Error);
            return this;
        }
        await SuccAsync(Value);
        return this;

    }
    [Pure]
    public Result<A> Do(Action<A> Succ, Action<Exception> Fail)
    {
        if (IsFaulted)
        {

            Fail(Error);
            return this;
        }
        Succ(Value);
        return this;

    }

    [Pure]
    public R Match<R>(Func<A, R> Succ, Func<Exception, R> Fail) =>
        IsFaulted
            ? Fail(Error)
            : Succ(Value);


    [Pure]
    public async Task<R> MatchAsync<R>(Func<A, Task<R>> Succ, Func<Exception, Task<R>> Fail) =>
        IsFaulted
            ? await Fail(Error)
            : await Succ(Value);


    [Pure]
    public Result<B> Map<B>(Func<A, B> f) =>
        IsFaulted
            ? new Result<B>(Error)
            : new Result<B>(f(Value));

    [Pure]
    public async Task<Result<B>> MapAsync<B>(Func<A, Task<B>> f) =>
        IsFaulted
            ? new Result<B>(Error)
            : new Result<B>(await f(Value));

}

