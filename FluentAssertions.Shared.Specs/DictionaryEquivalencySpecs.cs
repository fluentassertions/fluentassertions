using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class DictionaryEquivalencySpecs
    {
        #region Non-Generic Dictionaries

        [TestMethod]
        public void When_asserting_equivalence_of_dictionaries_it_should_respect_the_declared_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IDictionary dictionary1 = new NonGenericDictionary { { 2001, new Car() } };
            IDictionary dictionary2 = new NonGenericDictionary { { 2001, new Customer() } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.ShouldBeEquivalentTo(dictionary2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow("the declared type of the items is object");
        }

        [TestMethod]
        public void When_asserting_equivalence_of_dictionaries_and_configured_to_use_runtime_properties_it_should_respect_the_runtime_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IDictionary dictionary1 = new NonGenericDictionary { { 2001, new Car() } };
            IDictionary dictionary2 = new NonGenericDictionary { { 2001, new Customer() } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                dictionary1.ShouldBeEquivalentTo(dictionary2,
                    opts => opts.IncludingAllRuntimeProperties());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>("the types have different properties");
        }

        [TestMethod]
        public void When_a_non_generic_dictionary_is_typed_as_object_and_runtime_typing_has_not_been_specified_the_declared_type_should_be_respected()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object object1 = new NonGenericDictionary();
            object object2 = new NonGenericDictionary();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => object1.ShouldBeEquivalentTo(object2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<InvalidOperationException>("the type of the subject is object");
        }

        [TestMethod]
        public void When_a_non_generic_dictionary_is_typed_as_object_and_runtime_typing_is_specified_the_runtime_type_should_be_respected()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object object1 = new NonGenericDictionary { { "greeting", "hello" } };
            object object2 = new NonGenericDictionary { { "greeting", "hello" } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => object1.ShouldBeEquivalentTo(object2, opts => opts.IncludingAllRuntimeProperties());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow("the runtime type is a dictionary and the dictionaries are equivalent");
        }

        #endregion

        [TestMethod]
        public void When_an_object_implements_two_IDictionary_interfaces_it_should_fail_descriptively()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var object1 = new ClassWithTwoDictionaryImplementations();
            var object2 = new ClassWithTwoDictionaryImplementations();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => object1.ShouldBeEquivalentTo(object2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Subject implements multiple dictionary types.  "
                    + "It is not known which type should be use for equivalence.*");
        }

        [TestMethod]
        public void When_two_dictionaries_asserted_to_be_equivalent_have_different_lengths_it_should_fail_descriptively()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<string, string> { { "greeting", "hello" } };
            var dictionary2 = new Dictionary<string, string> { { "greeting", "hello" }, {"farewell", "goodbye"} };
                

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act1 = () => dictionary1.ShouldBeEquivalentTo(dictionary2);
            Action act2 = () => dictionary2.ShouldBeEquivalentTo(dictionary1);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act1.ShouldThrow<AssertFailedException>().WithMessage("Expected subject to be a dictionary with 2 item(s), but found 1 item(s)*");
            act2.ShouldThrow<AssertFailedException>().WithMessage("Expected subject to be a dictionary with 1 item(s), but found 2 item(s)*");
        }

        [TestMethod]
        public void When_a_dictionary_does_not_implement_IDictionary_it_should_still_be_treated_as_a_dictionary()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IDictionary<string, int> dictionary = new GenericDictionaryNotImplementingIDictionary<string, int> { { "hi", 1 } };
            ICollection<KeyValuePair<string, int>> collection = new List<KeyValuePair<string, int>> { new KeyValuePair<string, int>( "hi", 1 ) };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.ShouldBeEquivalentTo(collection);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("*is a dictionary and cannot be compared with a non-dictionary type.*");
        }

        [TestMethod]
        public void When_asserting_equivalence_of_generic_dictionaries_and_the_expectation_key_type_is_assignable_from_the_subjects_it_should_pass_if_compatible()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<string, string> { { "greeting", "hello" } };
            var dictionary2 = new Dictionary<object, string> { { "greeting", "hello" } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.ShouldBeEquivalentTo(dictionary2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow("the keys are still strings");
        }

        [TestMethod]
        public void When_asserting_equivalence_of_generic_dictionaries_and_the_expectation_key_type_is_assignable_from_the_subjects_it_should_fail_if_incompatible()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<string, string> { { "greeting", "hello" } };
            var dictionary2 = new Dictionary<object, string> { { new object(), "hello" } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.ShouldBeEquivalentTo(dictionary2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage("Subject contains unexpected key \"greeting\"*");
        }

        [TestMethod]
        public void When_asserting_equivalence_of_generic_dictionaries_and_the_expectation_key_type_is_not_assignable_from_the_subjects_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<string, string> { { "greeting", "hello" } };
            var dictionary2 = new Dictionary<int, string> { { 1234, "hello" } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.ShouldBeEquivalentTo(dictionary2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    String.Format(
                        "*The subject dictionary has keys of type System.String; however, the expected dictionary is not keyed with any compatible types.*{0}*",
                        typeof(IDictionary<int, string>)));
        }

        [TestMethod]
        public void When_a_generic_dictionary_is_typed_as_object_and_runtime_typing_has_not_been_specified_the_declared_type_should_be_respected()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object object1 = new Dictionary<string, string> { { "greeting", "hello" } };
            object object2 = new Dictionary<string, string> { { "greeting", "hello" } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => object1.ShouldBeEquivalentTo(object2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<InvalidOperationException>("the type of the subject is object");
        }

        [TestMethod]
        public void When_a_generic_dictionary_is_typed_as_object_and_runtime_typing_has_is_specified_it_should_use_the_runtime_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object object1 = new Dictionary<string, string> { { "greeting", "hello" } };
            object object2 = new Dictionary<string, string> { { "greeting", "hello" } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => object1.ShouldBeEquivalentTo(object2, opts => opts.IncludingAllRuntimeProperties());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow("the runtime type is a dictionary and the dictionaries are equivalent");
        }

        [TestMethod]
        public void When_asserting_the_equivalence_of_generic_dictionaries_it_should_respect_the_declared_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<int, CustomerType> { { 0, new DerivedCustomerType("123") } };
            var dictionary2 = new Dictionary<int, CustomerType> { { 0, new CustomerType("123") } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.ShouldBeEquivalentTo(dictionary2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow("the objects are equivalent according to the members on the declared type");
        }

        [TestMethod]
        public void When_asserting_equivalence_of_generic_dictionaries_and_configured_to_use_runtime_properties_it_should_respect_the_runtime_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<int, CustomerType> { { 0, new DerivedCustomerType("123") } };
            var dictionary2 = new Dictionary<int, CustomerType> { { 0, new CustomerType("123") } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                dictionary1.ShouldBeEquivalentTo(dictionary2,
                    opts => opts.IncludingAllRuntimeProperties());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>("the runtime types have different properties");
        }

        [TestMethod]
        public void When_asserting_equivalence_of_generic_dictionaries_the_type_information_should_be_preserved_for_other_equivalency_steps()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Guid userId = Guid.NewGuid();

            var dictionary1 = new Dictionary<Guid, IEnumerable<string>> { { userId, new List<string> { "Admin", "Special" } } };
            var dictionary2 = new Dictionary<Guid, IEnumerable<string>> { { userId, new List<string> { "Admin", "Other" } } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.ShouldBeEquivalentTo(dictionary2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_equivalence_of_non_generic_dictionaries_the_lack_of_type_information_should_be_preserved_for_other_equivalency_steps()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Guid userId = Guid.NewGuid();

            var dictionary1 = new NonGenericDictionary { { userId, new List<string> { "Admin", "Special" } } };
            var dictionary2 = new NonGenericDictionary { { userId, new List<string> { "Admin", "Other" } } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.ShouldBeEquivalentTo(dictionary2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow("the declared type of the values is 'object'");
        }

        public class UserRolesLookupElement
        {
            private readonly Dictionary<Guid, List<string>> innerRoles = new Dictionary<Guid, List<string>>();

            public virtual Dictionary<Guid, IEnumerable<string>> Roles
            {
                get { return innerRoles.ToDictionary(x => x.Key, y => y.Value.Select(z => z)); }
            }

            public void Add(Guid userId, params string[] roles)
            {
                innerRoles[userId] = roles.ToList();
            }
        }

        private class NonGenericDictionary : IDictionary
        {
            private readonly IDictionary dictionary = new Dictionary<object, object>();

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void CopyTo(Array array, int index)
            {
                dictionary.CopyTo(array, index);
            }

            public int Count
            {
                get
                {
                    return dictionary.Count;
                }
            }

            public bool IsSynchronized
            {
                get
                {
                    return dictionary.IsSynchronized;
                }
            }

            public object SyncRoot
            {
                get
                {
                    return dictionary.SyncRoot;
                }
            }

            public void Add(object key, object value)
            {
                dictionary.Add(key, value);
            }

            public void Clear()
            {
                dictionary.Clear();
            }

            public bool Contains(object key)
            {
                return dictionary.Contains(key);
            }

            public IDictionaryEnumerator GetEnumerator()
            {
                return dictionary.GetEnumerator();
            }

            public void Remove(object key)
            {
                dictionary.Remove(key);
            }

            public bool IsFixedSize
            {
                get
                {
                    return dictionary.IsFixedSize;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return dictionary.IsReadOnly;
                }
            }

            public object this[object key]
            {
                get
                {
                    return dictionary[key];
                }
                set
                {
                    dictionary[key] = value;
                }
            }

            public ICollection Keys
            {
                get
                {
                    return dictionary.Keys;
                }
            }

            public ICollection Values
            {
                get
                {
                    return dictionary.Values;
                }
            }
        }

        private class GenericDictionaryNotImplementingIDictionary<TKey, TValue> : IDictionary<TKey, TValue>
        {
            private readonly Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                return dictionary.GetEnumerator();
            }

            void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
            {
                ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Add(item);
            }

            public void Clear()
            {
                dictionary.Clear();
            }

            bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Contains(item);
            }

            void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            {
                ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).CopyTo(array, arrayIndex);
            }

            bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Remove(item);
            }

            public int Count
            {
                get
                {
                    return dictionary.Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).IsReadOnly;
                }
            }

            public bool ContainsKey(TKey key)
            {
                return dictionary.ContainsKey(key);
            }

            public void Add(TKey key, TValue value)
            {
                dictionary.Add(key, value);
            }

            public bool Remove(TKey key)
            {
                return dictionary.Remove(key);
            }

            public bool TryGetValue(TKey key, out TValue value)
            {
                return dictionary.TryGetValue(key, out value);
            }

            public TValue this[TKey key]
            {
                get
                {
                    return dictionary[key];
                }
                set
                {
                    dictionary[key] = value;
                }
            }

            public ICollection<TKey> Keys
            {
                get
                {
                    return dictionary.Keys;
                }
            }

            public ICollection<TValue> Values
            {
                get
                {
                    return dictionary.Values;
                }
            }
        }

        /// <summary>
        /// FakeItEasy can probably handle this in a couple lines, but then it would not be portable.
        /// </summary>
        private class ClassWithTwoDictionaryImplementations : Dictionary<int, object>, IDictionary<string, object>
        {
            IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
            {
                return
                    ((ICollection<KeyValuePair<int, object>>)this).Select(
                        item =>
                        new KeyValuePair<string, object>(
                            item.Key.ToString(CultureInfo.InvariantCulture),
                            item.Value)).GetEnumerator();
            }

            public void Add(KeyValuePair<string, object> item)
            {
                ((ICollection<KeyValuePair<int, object>>)this).Add(new KeyValuePair<int, object>(Parse(item.Key), item.Value));
            }

            public bool Contains(KeyValuePair<string, object> item)
            {
                return
                    ((ICollection<KeyValuePair<int, object>>)this).Contains(
                        new KeyValuePair<int, object>(Parse(item.Key), item.Value));
            }

            public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
            {
                ((ICollection<KeyValuePair<int, object>>)this).Select(
                    item =>
                    new KeyValuePair<string, object>(item.Key.ToString(CultureInfo.InvariantCulture), item.Value))
                    .ToArray()
                    .CopyTo(array, arrayIndex);
            }

            public bool Remove(KeyValuePair<string, object> item)
            {
                return
                    ((ICollection<KeyValuePair<int, object>>)this).Remove(
                        new KeyValuePair<int, object>(Parse(item.Key), item.Value));
            }

            bool ICollection<KeyValuePair<string, object>>.IsReadOnly
            {
                get
                {
                    return ((ICollection<KeyValuePair<int, object>>)this).IsReadOnly;
                }
            }

            public bool ContainsKey(string key)
            {
                return ContainsKey(Parse(key));
            }

            public void Add(string key, object value)
            {
                Add(Parse(key), value);
            }

            public bool Remove(string key)
            {
                return Remove(Parse(key));
            }

            public bool TryGetValue(string key, out object value)
            {
                return TryGetValue(Parse(key), out value);
            }

            public object this[string key]
            {
                get
                {
                    return this[Parse(key)];
                }
                set
                {
                    this[Parse(key)] = value;
                }
            }

            ICollection<string> IDictionary<string, object>.Keys
            {
                get
                {
                    return Keys.Select(_ => _.ToString(CultureInfo.InvariantCulture)).ToList();
                }
            }

            ICollection<object> IDictionary<string, object>.Values
            {
                get
                {
                    return Values;
                }
            }

            private int Parse(string key)
            {
                return Int32.Parse(key);
            }
        }
    }
}