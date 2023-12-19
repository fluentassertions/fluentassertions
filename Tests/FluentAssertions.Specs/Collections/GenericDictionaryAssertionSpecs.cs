using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericDictionaryAssertionSpecs
{
    // If you try to implement support for IReadOnlyDictionary, these tests should still succeed.
    public class SanityChecks
    {
        [Fact]
        public void When_the_same_dictionaries_are_expected_to_be_the_same_it_should_not_fail()
        {
            // Arrange
            IDictionary<int, string> dictionary = new Dictionary<int, string>();
            IDictionary<int, string> referenceToDictionary = dictionary;

            // Act / Assert
            dictionary.Should().BeSameAs(referenceToDictionary);
        }

        [Fact]
        public void When_the_same_custom_dictionaries_are_expected_to_be_the_same_it_should_not_fail()
        {
            // Arrange
            IDictionary<int, string> dictionary = new DictionaryNotImplementingIReadOnlyDictionary<int, string>();
            IDictionary<int, string> referenceToDictionary = dictionary;

            // Act / Assert
            dictionary.Should().BeSameAs(referenceToDictionary);
        }

        [Fact]
        public void When_object_type_is_exactly_equal_to_the_specified_type_it_should_not_fail()
        {
            // Arrange
            IDictionary<int, string> dictionary = new DictionaryNotImplementingIReadOnlyDictionary<int, string>();

            // Act / Assert
            dictionary.Should().BeOfType<DictionaryNotImplementingIReadOnlyDictionary<int, string>>();
        }

        [Fact]
        public void When_a_dictionary_does_not_implement_the_read_only_interface_it_should_have_dictionary_assertions()
        {
            // Arrange
            IDictionary<int, string> dictionary = new DictionaryNotImplementingIReadOnlyDictionary<int, string>();

            // Act / Assert
            dictionary.Should().NotContainKey(0, "Dictionaries not implementing IReadOnlyDictionary<TKey, TValue> "
                + "should be supported at least until Fluent Assertions 6.0.");
        }
    }

    public class OtherDictionaryAssertions
    {
        [Theory]
        [MemberData(nameof(SingleDictionaryData))]
        public void When_a_dictionary_like_collection_contains_the_expected_key_and_value_it_should_succeed<T>(T subject)
            where T : IEnumerable<KeyValuePair<int, int>>
        {
            // Assert
            subject.Should().Contain(1, 42);
        }

        [Theory]
        [MemberData(nameof(SingleDictionaryData))]
        public void When_using_a_dictionary_like_collection_it_should_preserve_reference_equality<T>(T subject)
            where T : IEnumerable<KeyValuePair<int, int>>
        {
            // Act
            Action act = () => subject.Should().BeSameAs(subject);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [MemberData(nameof(SingleDictionaryData))]
        public void When_a_dictionary_like_collection_contains_the_expected_key_it_should_succeed<T>(T subject)
            where T : IEnumerable<KeyValuePair<int, int>>
        {
            // Act
            Action act = () => subject.Should().ContainKey(1);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [MemberData(nameof(SingleDictionaryData))]
        public void When_a_dictionary_like_collection_contains_the_expected_value_it_should_succeed<T>(T subject)
            where T : IEnumerable<KeyValuePair<int, int>>
        {
            // Act
            Action act = () => subject.Should().ContainValue(42);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [MemberData(nameof(DictionariesData))]
        public void When_comparing_dictionary_like_collections_for_equality_it_should_succeed<T1, T2>(T1 subject, T2 expected)
            where T1 : IEnumerable<KeyValuePair<int, int>>
            where T2 : IEnumerable<KeyValuePair<int, int>>
        {
            // Act
            Action act = () => subject.Should().Equal(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [MemberData(nameof(DictionariesData))]
        public void When_comparing_dictionary_like_collections_for_inequality_it_should_throw<T1, T2>(T1 subject, T2 expected)
            where T1 : IEnumerable<KeyValuePair<int, int>>
            where T2 : IEnumerable<KeyValuePair<int, int>>
        {
            // Act
            Action act = () => subject.Should().NotEqual(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        public static IEnumerable<object[]> SingleDictionaryData() =>
            Dictionaries().Select(x => new[] { x });

        public static object[] Dictionaries()
        {
            return new object[]
            {
                new Dictionary<int, int> { [1] = 42 },
                new TrueReadOnlyDictionary<int, int>(new Dictionary<int, int> { [1] = 42 }),
                new List<KeyValuePair<int, int>> { new(1, 42) }
            };
        }

        public static IEnumerable<object[]> DictionariesData()
        {
            return
                from x in Dictionaries()
                from y in Dictionaries()
                select new[] { x, y };
        }
    }

    public class Miscellaneous
    {
        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            dictionary.Should()
                .HaveCount(2)
                .And.ContainKey(1)
                .And.ContainValue("Two");
        }
    }

    /// <summary>
    /// This class only implements <see cref="IReadOnlyDictionary{TKey, TValue}"/>,
    /// as <see cref="ReadOnlyDictionary{TKey,TValue}"/> also implements <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    private class TrueReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly IReadOnlyDictionary<TKey, TValue> dictionary;

        public TrueReadOnlyDictionary(IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = dictionary;
        }

        public TValue this[TKey key] => dictionary[key];

        public IEnumerable<TKey> Keys => dictionary.Keys;

        public IEnumerable<TValue> Values => dictionary.Values;

        public int Count => dictionary.Count;

        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();

        public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => dictionary.GetEnumerator();
    }

    internal class TrackingTestDictionary : IDictionary<int, string>
    {
        private readonly IDictionary<int, string> entries;

        public TrackingTestDictionary(params KeyValuePair<int, string>[] entries)
        {
            this.entries = entries.ToDictionary(e => e.Key, e => e.Value);
            Enumerator = new TrackingDictionaryEnumerator(entries);
        }

        public TrackingDictionaryEnumerator Enumerator { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<int, string>> GetEnumerator()
        {
            Enumerator.IncreaseEnumerationCount();
            Enumerator.Reset();
            return Enumerator;
        }

        public void Add(KeyValuePair<int, string> item)
        {
            entries.Add(item);
        }

        public void Clear()
        {
            entries.Clear();
        }

        public bool Contains(KeyValuePair<int, string> item)
        {
            return entries.Contains(item);
        }

        public void CopyTo(KeyValuePair<int, string>[] array, int arrayIndex)
        {
            entries.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<int, string> item)
        {
            return entries.Remove(item);
        }

        public int Count
        {
            get { return entries.Count; }
        }

        public bool IsReadOnly
        {
            get { return entries.IsReadOnly; }
        }

        public bool ContainsKey(int key)
        {
            return entries.ContainsKey(key);
        }

        public void Add(int key, string value)
        {
            entries.Add(key, value);
        }

        public bool Remove(int key)
        {
            return entries.Remove(key);
        }

        public bool TryGetValue(int key, out string value)
        {
            return entries.TryGetValue(key, out value);
        }

        public string this[int key]
        {
            get { return entries[key]; }
            set { entries[key] = value; }
        }

        public ICollection<int> Keys
        {
            get { return entries.Keys; }
        }

        public ICollection<string> Values
        {
            get { return entries.Values; }
        }
    }

    internal sealed class TrackingDictionaryEnumerator : IEnumerator<KeyValuePair<int, string>>
    {
        private readonly KeyValuePair<int, string>[] values;
        private int index;

        public TrackingDictionaryEnumerator(KeyValuePair<int, string>[] values)
        {
            index = -1;
            this.values = values;
        }

        public int LoopCount { get; private set; }

        public void IncreaseEnumerationCount()
        {
            LoopCount++;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            index++;
            return index < values.Length;
        }

        public void Reset()
        {
            index = -1;
        }

        public KeyValuePair<int, string> Current
        {
            get { return values[index]; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }

    internal class DictionaryNotImplementingIReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> dictionary = new();

        public TValue this[TKey key] { get => dictionary[key]; set => throw new NotImplementedException(); }

        public ICollection<TKey> Keys => dictionary.Keys;

        public ICollection<TValue> Values => dictionary.Values;

        public int Count => dictionary.Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).IsReadOnly;

        public void Add(TKey key, TValue value) => throw new NotImplementedException();

        public void Add(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

        public void Clear() => throw new NotImplementedException();

        public bool Contains(KeyValuePair<TKey, TValue> item) => dictionary.Contains(item);

        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw new NotImplementedException();

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();

        public bool Remove(TKey key) => throw new NotImplementedException();

        public bool Remove(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

        public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => dictionary.GetEnumerator();
    }

    public class MyClass
    {
        public int SomeProperty { get; set; }

        protected bool Equals(MyClass other)
        {
            return SomeProperty == other.SomeProperty;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((MyClass)obj);
        }

        public override int GetHashCode()
        {
            return SomeProperty;
        }
    }
}
