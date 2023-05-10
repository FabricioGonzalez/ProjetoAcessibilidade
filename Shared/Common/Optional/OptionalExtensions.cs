namespace Common.Optional;

public static class OptionalExtensions
{
    public static Optional<T> Where<T>(
        this T? obj
        , Func<T, bool> predicate
    )
        where T : class =>
        obj is not null && predicate(obj) ? Optional<T>.Some(obj) : Optional<T>.None();

    public static Optional<T> WhereNot<T>(
        this T? obj
        , Func<T, bool> predicate
    )
        where T : class =>
        obj is not null && !predicate(obj) ? Optional<T>.Some(obj) : Optional<T>.None();

    public static Optional<T> ToOption<T>(
        this T? obj
    )
        where T : class => obj is null ? Optional<T>.None() : Optional<T>.Some(obj);
}