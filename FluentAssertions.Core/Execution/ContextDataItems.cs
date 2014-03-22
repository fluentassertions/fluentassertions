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

        public IDictionary<string, object> Reportable
        {
            get { return items.Where(item => item.Reportable).ToDictionary(item => item.Key, item => item.Value); }
        }

        public string AsStringOrDefault(string key)
        {
            DataItem item = items.SingleOrDefault(i => i.Key == key);
            if (item != null)
            {
                if ((key == "subject") || (key == "expectation"))
                {
                    return Formatter.ToString(item.Value);
                }
                
                return item.Value.ToString();
            }

            return null;
        }

        public void Add(ContextDataItems contextDataItems)
        {
            foreach (var item in contextDataItems.items)
            {
                Add(item.Clone());
            }
        }

        public void Add(string key, object value, Reportability reportability)
        {
            Add(new DataItem(key, value, reportability));
        }

        private void Add(DataItem item)
        {
            var existingItem = items.SingleOrDefault(i => i.Key == item.Key);
            if (existingItem != null)
            {
                items.Remove(existingItem);
            }

            items.Add(item);
        }

        public T Get<T>(string key)
        {
            DataItem item = items.SingleOrDefault(i => i.Key == key);
            return (T)((item != null) ? item.Value : default(T));
        }

        internal class DataItem
        {
            private readonly Reportability reportability;

            public DataItem(string key, object value, Reportability reportability)
            {
                Key = key;
                Value = value;
                this.reportability = reportability;
            }

            public string Key { get; private set; }

            public object Value { get; private set; }

            public bool Reportable
            {
                get { return reportability == Reportability.Reportable; }
            }

            public DataItem Clone()
            {
                var cloneable = Value as ICloneable2;
                return new DataItem(Key, (cloneable != null) ? cloneable.Clone() : Value, reportability);
            }
        }
    }
}