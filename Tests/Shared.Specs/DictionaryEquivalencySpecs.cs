using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    [Collection("Equivalency")]
    public class DictionaryEquivalencySpecs
    {
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

            public int Count => dictionary.Count;

            public bool IsSynchronized => dictionary.IsSynchronized;

            public object SyncRoot => dictionary.SyncRoot;

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

            public bool IsFixedSize => dictionary.IsFixedSize;

            public bool IsReadOnly => dictionary.IsReadOnly;

            public object this[object key]
            {
                get => dictionary[key];
                set => dictionary[key] = value;
            }

            public ICollection Keys => dictionary.Keys;

            public ICollection Values => dictionary.Values;
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

            public int Count => dictionary.Count;

            public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).IsReadOnly;

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
                get => dictionary[key];
                set => dictionary[key] = value;
            }

            public ICollection<TKey> Keys => dictionary.Keys;

            public ICollection<TValue> Values => dictionary.Values;
        }

        /// <summary>
        ///     FakeItEasy can probably handle this in a couple lines, but then it would not be portable.
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

            bool ICollection<KeyValuePair<string, object>>.IsReadOnly =>
                ((ICollection<KeyValuePair<int, object>>)this).IsReadOnly;

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
                get => this[Parse(key)];
                set => this[Parse(key)] = value;
            }

            ICollection<string> IDictionary<string, object>.Keys
            {
                get { return Keys.Select(_ => _.ToString(CultureInfo.InvariantCulture)).ToList(); }
            }

            ICollection<object> IDictionary<string, object>.Values => Values;

            private int Parse(string key)
            {
                return int.Parse(key);
            }
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

        public class ClassWithMemberDictionary
        {
            public Dictionary<string, string> Dictionary { get; set; }
        }

        [Fact]
        public void When_a_dictionary_does_not_implement_the_dictionary_interface_it_should_still_be_treated_as_a_dictionary()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IDictionary<string, int> dictionary = new GenericDictionaryNotImplementingIDictionary<string, int> {{"hi", 1}};
            ICollection<KeyValuePair<string, int>> collection =
                new List<KeyValuePair<string, int>> {new KeyValuePair<string, int>("hi", 1)};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().BeEquivalentTo(collection);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_a_dictionary_property_is_detected_it_should_ignore_the_order_of_the_pairs
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new
            {
                Customers = new Dictionary<string, string>
                {
                    {"Key2", "Value2"},
                    {"Key1", "Value1"}
                }
            };

            var subject = new
            {
                Customers = new Dictionary<string, string>
                {
                    {"Key1", "Value1"},
                    {"Key2", "Value2"}
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_a_generic_dictionary_is_typed_as_object_and_runtime_typing_has_is_specified_it_should_use_the_runtime_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object object1 = new Dictionary<string, string> {{"greeting", "hello"}};
            object object2 = new Dictionary<string, string> {{"greeting", "hello"}};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => object1.Should().BeEquivalentTo(object2, opts => opts.RespectingRuntimeTypes());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow("the runtime type is a dictionary and the dictionaries are equivalent");
        }

        [Fact]
        public void When_a_generic_dictionary_is_typed_as_object_it_should_respect_the_runtime_typed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object object1 = new Dictionary<string, string> {{"greeting", "hello"}};
            object object2 = new Dictionary<string, string> {{"greeting", "hello"}};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => object1.Should().BeEquivalentTo(object2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_a_non_generic_dictionary_is_typed_as_object_and_runtime_typing_is_specified_the_runtime_type_should_be_respected()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object object1 = new NonGenericDictionary {{"greeting", "hello"}};
            object object2 = new NonGenericDictionary {{"greeting", "hello"}};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => object1.Should().BeEquivalentTo(object2, opts => opts.RespectingRuntimeTypes());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow("the runtime type is a dictionary and the dictionaries are equivalent");
        }

        [Fact]
        public void When_a_non_generic_dictionary_is_typed_as_object_it_should_respect_the_runtime_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object object1 = new NonGenericDictionary();
            object object2 = new NonGenericDictionary();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => object1.Should().BeEquivalentTo(object2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
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
            Action act = () => object1.Should().BeEquivalentTo(object2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_equivalence_of_dictionaries_and_configured_to_respect_runtime_type_it_should_respect_the_runtime_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IDictionary dictionary1 = new NonGenericDictionary {{2001, new Car()}};
            IDictionary dictionary2 = new NonGenericDictionary {{2001, new Customer()}};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                    dictionary1.Should().BeEquivalentTo(dictionary2,
                        opts => opts.RespectingRuntimeTypes());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>("the types have different properties");
        }

        [Fact]
        public void When_asserting_equivalence_of_dictionaries_it_should_respect_the_declared_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var actual = new Dictionary<int, CustomerType> {{0, new CustomerType("123")}};
            var expectation = new Dictionary<int, CustomerType> {{0, new DerivedCustomerType("123")}};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => actual.Should().BeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow("because it should ignore the properties of the derived type");
        }

        [Fact]
        public void
            When_asserting_equivalence_of_generic_dictionaries_and_configured_to_use_runtime_properties_it_should_respect_the_runtime_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var actual = new Dictionary<int, CustomerType> {{0, new CustomerType("123")}};
            var expectation = new Dictionary<int, CustomerType> {{0, new DerivedCustomerType("123")}};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                    actual.Should().BeEquivalentTo(expectation,
                        opts => opts.RespectingRuntimeTypes());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>("the runtime types have different properties");
        }

        [Fact]
        public void
            When_asserting_equivalence_of_generic_dictionaries_and_the_expectation_key_type_is_assignable_from_the_subjects_it_should_fail_if_incompatible()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var actual = new Dictionary<object, string> {{new object(), "hello"}};
            var expected = new Dictionary<string, string> {{"greeting", "hello"}};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => actual.Should().BeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to contain key \"greeting\"*");
        }

        [Fact]
        public void When_the_subjects_key_type_is_compatible_with_the_expected_key_type_it_should_match()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<object, string> {{"greeting", "hello"}};
            var dictionary2 = new Dictionary<string, string> {{"greeting", "hello"}};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.Should().BeEquivalentTo(dictionary2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow("the keys are still strings");
        }

        [Fact]
        public void When_the_subjects_key_type_is_not_compatible_with_the_expected_key_type_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var actual = new Dictionary<int, string> {{1234, "hello"}};
            var expectation = new Dictionary<string, string> {{"greeting", "hello"}};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => expectation.Should().BeEquivalentTo(actual);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>()
                .WithMessage("*subject*keys*Int32*compatible types*IDictionary`2[System.String,System.String]*");
        }

        [Fact]
        public void
            When_asserting_equivalence_of_generic_dictionaries_the_type_information_should_be_preserved_for_other_equivalency_steps()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var userId = Guid.NewGuid();

            var dictionary1 = new Dictionary<Guid, IEnumerable<string>> {{userId, new List<string> {"Admin", "Special"}}};
            var dictionary2 = new Dictionary<Guid, IEnumerable<string>> {{userId, new List<string> {"Admin", "Other"}}};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.Should().BeEquivalentTo(dictionary2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_equivalence_of_non_generic_dictionaries_the_lack_of_type_information_should_be_preserved_for_other_equivalency_steps()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var userId = Guid.NewGuid();

            var dictionary1 = new NonGenericDictionary {{userId, new List<string> {"Admin", "Special"}}};
            var dictionary2 = new NonGenericDictionary {{userId, new List<string> {"Admin", "Other"}}};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.Should().BeEquivalentTo(dictionary2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>()
                .WithMessage("*Other*Special*");
        }

        [Fact]
        public void When_asserting_the_equivalence_of_generic_dictionaries_it_should_respect_the_declared_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var actual = new Dictionary<int, CustomerType>
            {
                {0, new DerivedCustomerType("123")}
            };

            var expectation = new Dictionary<int, CustomerType> {{0, new CustomerType("123")}};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => actual.Should().BeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow("the objects are equivalent according to the members on the declared type");
        }

        [Fact]
        public void When_the_both_properties_are_null_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new ClassWithMemberDictionary
            {
                Dictionary = null
            };

            var subject = new ClassWithMemberDictionary
            {
                Dictionary = null
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void
            When_the_dictionary_values_are_handled_by_the_enumerable_equivalency_step_the_type_information_should_be_preserved()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var userId = Guid.NewGuid();

            var actual = new UserRolesLookupElement();
            actual.Add(userId, "Admin", "Special");

            var expected = new UserRolesLookupElement();
            expected.Add(userId, "Admin", "Other");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => actual.Should().BeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*Roles[*][1]*Other*Special*");
        }

        [Fact]
        public void
            When_the_other_dictionary_does_not_contain_enough_items_it_should_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new
            {
                Customers = new Dictionary<string, string>
                {
                    {"Key1", "Value1"},
                    {"Key2", "Value2"}
                }
            };

            var subject = new
            {
                Customers = new Dictionary<string, string>
                {
                    {"Key1", "Value1"}
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*Customers*dictionary*2 item(s)*but*1 item(s)*");
        }

        [Fact]
        public void When_the_other_property_is_not_a_dictionary_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Customers = "I am a string"
            };

            var expected = new
            {
                Customers = new Dictionary<string, string>
                {
                    {"Key2", "Value2"},
                    {"Key1", "Value1"}
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*Customers*Dictionary`2[System.String,System.String], but found*String*");
        }

        [Fact]
        public void When_the_other_property_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ClassWithMemberDictionary
            {
                Dictionary = new Dictionary<string, string>
                {
                    {"Key2", "Value2"},
                    {"Key1", "Value1"}
                }
            };

            var expected = new ClassWithMemberDictionary
            {
                Dictionary = null
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>()
                .WithMessage("*member*Dictionary to be <null>, but found *{*}*");
        }

        [Fact]
        public void When_two_dictionaries_asserted_to_be_equivalent_have_different_lengths_it_should_fail_descriptively()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<string, string> {{"greeting", "hello"}};
            var dictionary2 = new Dictionary<string, string> {{"greeting", "hello"}, {"farewell", "goodbye"}};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act1 = () => dictionary1.Should().BeEquivalentTo(dictionary2);
            Action act2 = () => dictionary2.Should().BeEquivalentTo(dictionary1);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act1.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be a dictionary with 2 item(s), but found 1 item(s).*Missing key(s): \"farewell\"*");
            
            act2.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be a dictionary with 1 item(s), but found 2 item(s).*Additional key(s): \"farewell\"*");
        }

        [Fact]
        public void When_two_equivalent_dictionaries_are_compared_directly_as_if_it_is_a_collection_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var result = new Dictionary<string, int?>
            {
                {"C", null},
                {"B", 0},
                {"A", 0}
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => result.Should().BeEquivalentTo(new Dictionary<string, int?>
            {
                {"A", 0},
                {"B", 0},
                {"C", null}
            });

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void When_two_equivalent_dictionaries_are_compared_directly_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var result = new Dictionary<string, int>
            {
                {"C", 0},
                {"B", 0},
                {"A", 0}
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => result.Should().BeEquivalentTo(new Dictionary<string, int>
            {
                {"A", 0},
                {"B", 0},
                {"C", 0}
            });

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void When_two_nested_dictionaries_contain_null_values_it_should_not_crash()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var projection = new
            {
                ReferencedEquipment = new Dictionary<int, string>
                {
                    {1, null}
                }
            };

            var persistedProjection = new
            {
                ReferencedEquipment = new Dictionary<int, string>
                {
                    {1, null}
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => persistedProjection.Should().BeEquivalentTo(projection);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void When_two_nested_dictionaries_do_not_match_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var projection = new
            {
                ReferencedEquipment = new Dictionary<int, string>
                {
                    {1, "Bla1"}
                }
            };

            var persistedProjection = new
            {
                ReferencedEquipment = new Dictionary<int, string>
                {
                    {1, "Bla2"}
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => persistedProjection.Should().BeEquivalentTo(projection);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*ReferencedEquipment[1]*Bla1*Bla2*2*index 3*");
        }
    }
}
