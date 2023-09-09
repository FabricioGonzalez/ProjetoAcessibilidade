using System.Diagnostics.Contracts;

namespace Common.Optional;

public readonly struct Optional<T> : IEquatable<Optional<T>>
{
    internal readonly T? Value;
    internal readonly bool isSome;


    internal Optional(T? value)
    {
        Value = value;

        isSome = value is not null;
    }

    public bool Equals(
        Optional<T> other
    ) =>
        Value is null
            ? other.Value is null
            : Value.Equals(other.Value);

    public static Optional<T> Some(
        T obj
    ) => new Optional<T>(obj);


    [Pure]
    public bool IsSome =>
           isSome;

    /// <summary>
    /// Is the option in a None state
    /// </summary>
    [Pure]
    public bool IsNone =>
        !isSome;

    public static Optional<T?> None() => new(default);

    public Optional<TResult> Map<TResult>(
        Func<T, TResult> map
    )
        where TResult : class =>
        new(Value is not null
                ? map(Value)
                : null);
    public async Task<Optional<TResult>> MapAsync<TResult>(
          Func<T, Task<TResult>> mapAsync
      )
          where TResult : class =>
          Value is not null ? Optional<TResult>.Some(await mapAsync(Value)) : Optional<TResult>.None();


    public ValueOption<TResult> MapValue<TResult>(
        Func<T, TResult> map
    )
        where TResult : struct =>
        Value is not null
            ? ValueOption<TResult>.Some(map(Value))
            : ValueOption<TResult>.None();

    public Optional<TResult> MMaMapOptional<TResult>(
        Func<T, Optional<TResult>> map
    )
        where TResult : class =>
        Value is not null
            ? map(Value)
            : Optional<TResult>.None();

    public ValueOption<TResult> MapOptionalValue<TResult>(
        Func<T, ValueOption<TResult>> map
    )
        where TResult : struct =>
        Value is not null
            ? map(Value)
            : ValueOption<TResult>.None();

    public T Reduce(
        T orElse
    ) => Value ?? orElse;

    public T Reduce(
        Func<T> orElse
    ) => Value ?? orElse();

    public Optional<T> Where(
        Func<T, bool> predicate
    ) =>
        Value is not null && predicate(Value)
            ? this
            : None();

    public Optional<T> WhereNot(
        Func<T, bool> predicate
    ) =>
        Value is not null && !predicate(Value)
            ? this
            : None();

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public override bool Equals(
        object? other
    ) => other is Optional<T> option && Equals(option);

    public static bool operator ==(
        Optional<T>? a,
        Optional<T>? b
    ) => a is null
        ? b is null
        : a.Equals(b);

    public static bool operator !=(
        Optional<T>? a,
        Optional<T>? b
    ) => !(a == b);
}