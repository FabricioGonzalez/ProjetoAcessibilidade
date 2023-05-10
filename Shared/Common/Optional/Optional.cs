namespace Common.Optional;

public class Optional<T> : IEquatable<Optional<T>>
    where T : class
{
    private T? _content;

    public bool Equals(
        Optional<T>? other
    ) =>
        other is not null && (_content?.Equals(other._content) ?? false);

    public static Optional<T> Some(
        T obj
    ) => new() { _content = obj };

    public static Optional<T> None() => new();

    public Optional<TResult> Map<TResult>(
        Func<T, TResult> map
    )
        where TResult : class => new() { _content = _content is not null ? map(_content) : null };

    public T Reduce(
        Func<T> defaultContent
    ) => _content ?? defaultContent();

    public override int GetHashCode() => _content?.GetHashCode() ?? 0;

    public override bool Equals(
        object? other
    ) =>
        Equals(other as Optional<T>);

    public static bool operator ==(
        Optional<T>? a
        , Optional<T>? b
    ) => a?.Equals(b) ?? b is null;

    public static bool operator !=(
        Optional<T>? a
        , Optional<T>? b
    ) => !(a == b);
}