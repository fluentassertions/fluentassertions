using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Formatting;

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Represents a collection of data items that are associated with an <see cref="AssertionScope"/>.
    /// </summary>
    internal class ContextDataItems
    {
        private readonly List<DataItem> items = new List<DataItem>();

        public IDictionary<string, object> GetReportable()
        {
            return items.Where(item => item.Reportable).ToDictionary(item => item.Key, item => item.Value);
        }

        public string AsStringOrDefault(string key)
        {
            DataItem item = items.SingleOrDefault(i => i.Key == key);
            if (item != null)
            {
                if (item.RequiresFormatting)
                {
                    return Formatter.ToString(item.Value);
                }

                return item.Value.ToString();
            }

            return null;
        }

        public void Add(ContextDataItems contextDataItems)
        {
            foreach (DataItem item in contextDataItems.items)
            {
                Add(item.Clone());
            }
        }

        public void Add(DataItem item)
        {
            DataItem existingItem = items.SingleOrDefault(i => i.Key == item.Key);
            if (existingItem != null)
            {
                items.Remove(existingItem);
            }

            items.Add(item);
        }

        public T Get<T>(string key)
        {
            DataItem item = items.SingleOrDefault(i => i.Key == key);
            return (T)(item?.Value ?? default(T));
        }

        internal class DataItem
        {
            public DataItem(string key, object value, bool reportable, bool requiresFormatting)
            {
                Key = key;
                Value = value;
                Reportable = reportable;
                RequiresFormatting = requiresFormatting;
            }

            public string Key { get; }

            public object Value { get; }

            public bool Reportable { get; }

            public bool RequiresFormatting { get; }

            public DataItem Clone()
            {
                object value = (Value is ICloneable2 cloneable) ? cloneable.Clone() : Value;
                return new DataItem(Key, value, Reportable, RequiresFormatting);
            }
        }
    }
}
