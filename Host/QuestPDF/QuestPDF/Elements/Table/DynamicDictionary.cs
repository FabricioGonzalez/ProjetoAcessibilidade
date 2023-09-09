using System.Collections.Generic;
using System.Linq;

namespace QuestPDF.Elements.Table
{
    /// <summary>
    ///     This dictionary allows to access key that does not exist.
    ///     Instead of throwing an exception, it returns a default value.
    /// </summary>
    public class DynamicDictionary<TKey, TValue>
    {
        public DynamicDictionary()
        {
        }

        public DynamicDictionary(
            TValue defaultValue
        )
        {
            Default = defaultValue;
        }

        private TValue Default
        {
            get;
        }

        private IDictionary<TKey, TValue> Dictionary
        {
            get;
        } = new Dictionary<TKey, TValue>();

        public TValue this[
            TKey key
        ]
        {
            get => Dictionary.TryGetValue(key: key, value: out var value) ? value : Default;
            set => Dictionary[key: key] = value;
        }

        public List<KeyValuePair<TKey, TValue>> Items => Dictionary.ToList();
    }
}