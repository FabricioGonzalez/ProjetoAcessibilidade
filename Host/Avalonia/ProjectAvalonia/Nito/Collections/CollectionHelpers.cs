using System;
using System.Collections;
using System.Collections.Generic;
using ProjectAvalonia.Common.Helpers;

namespace Nito.Collections;

internal static class CollectionHelpers
{
    public static IReadOnlyCollection<T> ReifyCollection<T>(
        IEnumerable<T> source
    )
    {
        Guard.NotNull(parameterName: nameof(source), value: source);

        if (source is IReadOnlyCollection<T> result) return result;

        if (source is ICollection<T> collection) return new CollectionWrapper<T>(collection: collection);

        if (source is ICollection nongenericCollection)
            return new NongenericCollectionWrapper<T>(collection: nongenericCollection);

        return new List<T>(collection: source);
    }

    private sealed class NongenericCollectionWrapper<T> : IReadOnlyCollection<T>
    {
        private readonly ICollection _collection;

        public NongenericCollectionWrapper(
            ICollection collection
        )
        {
            _collection = collection ?? throw new ArgumentNullException(paramName: nameof(collection));
        }

        public int Count
        {
            get => _collection.Count;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in _collection) yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
    }

    private sealed class CollectionWrapper<T> : IReadOnlyCollection<T>
    {
        private readonly ICollection<T> _collection;

        public CollectionWrapper(
            ICollection<T> collection
        )
        {
            _collection = collection ?? throw new ArgumentNullException(paramName: nameof(collection));
        }

        public int Count
        {
            get => _collection.Count;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
    }
}