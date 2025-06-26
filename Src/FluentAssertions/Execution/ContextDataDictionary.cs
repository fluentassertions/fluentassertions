using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Formatting;

namespace FluentAssertions.Execution;

/// <summary>
/// Represents a collection of data items that are associated with an <see cref="AssertionScope"/>.
/// </summary>
internal class ContextDataDictionary
{
    private readonly List<DataItem> items = [];

    public IDictionary<string, object> GetReportable()
    {
        return items
            .Where(item => item.Reportable)
            .ToDictionary(item => item.Key, item => item.Value, StringComparer.Ordinal);
    }

    public string AsStringOrDefault(string key)
    {
        DataItem item = items.SingleOrDefault(i => i.Key == key);

        if (item is not null)
        {
            if (item.RequiresFormatting)
            {
                return Formatter.ToString(item.Value);
            }

            return item.Value.ToString();
        }

        return null;
    }

    public void Add(ContextDataDictionary contextDataDictionary)
    {
        foreach (DataItem item in contextDataDictionary.items)
        {
            Add(item.Clone());
        }
    }

    public void Add(DataItem item)
    {
        int existingItemIndex = items.FindIndex(i => i.Key == item.Key);
        if (existingItemIndex >= 0)
        {
            items[existingItemIndex] = item;
        }
        else
        {
            items.Add(item);
        }
    }

    public class DataItem(string key, object value)
    {
        public string Key { get; } = key;

        public object Value { get; } = value;

        public bool Reportable { get; init; }

        public bool RequiresFormatting { get; init; }

        public DataItem Clone()
        {
            object clone = Value is ICloneable2 cloneable ? cloneable.Clone() : Value;
            return new DataItem(Key, clone)
            {
                Reportable = Reportable,
                RequiresFormatting = RequiresFormatting
            };
        }
    }
}
