namespace Common.Optional;

public class Optional<T> : IEquatable<Optional<T>>
    where T : class
{
    private T? _content;

    public bool Equals(
        Optional<T> other
    ) =>
        _content is null
            ? other._content is null
            : _content.Equals(other._content);

    public static Optional<T> Some(
        T obj
    ) => new() { _content = obj };

    public static Optional<T> None() => new();

    public Optional<TResult> Map<TResult>(
        Func<T, TResult> map
    )
        where TResult : class =>
        new()
        {
            _content = _content is not null
                ? map(_content)
                : null
        };

    public ValueOption<TResult> MapValue<TResult>(
        Func<T, TResult> map
    )
        where TResult : struct =>
        _content is not null
            ? ValueOption<TResult>.Some(map(_content))
            : ValueOption<TResult>.None();

    public Optional<TResult> MapOptional<TResult>(
        Func<T, Optional<TResult>> map
    )
        where TResult : class =>
        _content is not null
            ? map(_content)
            : Optional<TResult>.None();

    public ValueOption<TResult> MapOptionalValue<TResult>(
        Func<T, ValueOption<TResult>> map
    )
        where TResult : struct =>
        _content is not null
            ? map(_content)
            : ValueOption<TResult>.None();

    public T Reduce(
        T orElse
    ) => _content ?? orElse;

    public T Reduce(
        Func<T> orElse
    ) => _content ?? orElse();

    public Optional<T> Where(
        Func<T, bool> predicate
    ) =>
        _content is not null && predicate(_content)
            ? this
            : None();

    public Optional<T> WhereNot(
        Func<T, bool> predicate
    ) =>
        _content is not null && !predicate(_content)
            ? this
            : None();

    public override int GetHashCode() => _content?.GetHashCode() ?? 0;

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