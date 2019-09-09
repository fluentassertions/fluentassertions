using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using FluentAssertions.Equivalency;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
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

        public class SomeBaseKeyClass : IEquatable<SomeBaseKeyClass>
        {
            public SomeBaseKeyClass(int id)
            {
                Id = id;
            }

            public int Id { get; }

            public override int GetHashCode()
            {
                return Id;
            }

            public bool Equals(SomeBaseKeyClass other)
            {
                if (other is null)
                {
                    return false;
                }

                return this.Id == other.Id;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as SomeBaseKeyClass);
            }

            public override string ToString()
            {
                return $"BaseKey {Id}";
            }
        }

        public class SomeDerivedKeyClass : SomeBaseKeyClass
        {
            public SomeDerivedKeyClass(int id) : base(id)
            {
            }
        }

        [Fact]
        public void When_a_dictionary_does_not_implement_the_dictionary_interface_it_should_still_be_treated_as_a_dictionary()
        {
            // Arrange
            IDictionary<string, int> dictionary = new GenericDictionaryNotImplementingIDictionary<string, int>
            {
                ["hi"] = 1
            };

            ICollection<KeyValuePair<string, int>> collection =
                new List<KeyValuePair<string, int>> { new KeyValuePair<string, int>("hi", 1) };

            // Act
            Action act = () => dictionary.Should().BeEquivalentTo(collection);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_read_only_dictionary_matches_the_expectation_it_should_succeed()
        {
            // Arrange
            IReadOnlyDictionary<string, IEnumerable<string>> dictionary =
                new ReadOnlyDictionary<string, IEnumerable<string>>(
                new Dictionary<string, IEnumerable<string>>()
                {
                    ["Key2"] = new[] { "Value2" },
                    ["Key1"] = new[] { "Value1" }
                });

            // Act
            Action act = () => dictionary.Should().BeEquivalentTo(new Dictionary<string, IEnumerable<string>>()
            {
                ["Key1"] = new[] { "Value1" },
                ["Key2"] = new[] { "Value2" }
            });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_read_only_dictionary_does_not_match_the_expectation_it_should_throw()
        {
            // Arrange
            IReadOnlyDictionary<string, IEnumerable<string>> dictionary =
                new ReadOnlyDictionary<string, IEnumerable<string>>(
                new Dictionary<string, IEnumerable<string>>()
                {
                    ["Key2"] = new[] { "Value2" },
                    ["Key1"] = new[] { "Value1" }
                });

            // Act
            Action act = () => dictionary.Should().BeEquivalentTo(new Dictionary<string, IEnumerable<string>>()
            {
                ["Key2"] = new[] { "Value3" },
                ["Key1"] = new[] { "Value1" }
            });

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected pair[Key2][0]*Value3*Value2*");
        }

        [Fact]
        // #930
        public void When_a_dictionary_is_compared_to_null_it_should_not_throw_a_NullReferenceException()
        {
            // Arrange
            Dictionary<int, int> subject = null;
            Dictionary<int, int> expectation = new Dictionary<int, int>();

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        // #930
        public void When_a_null_dictionary_is_compared_to_null_it_should_not_throw()
        {
            // Arrange
            Dictionary<int, int> subject = null;
            Dictionary<int, int> expectation = null;

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_dictionary_is_compared_to_a_dictionary_it_should_allow_chaining()
        {
            // Arrange
            Dictionary<int, int> subject = new Dictionary<int, int> { [42] = 1337 };
            Dictionary<int, int> expectation = new Dictionary<int, int> { [42] = 1337 };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation)
                .And.ContainKey(42);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_dictionary_is_compared_to_a_dictionary_with_a_config_it_should_allow_chaining()
        {
            // Arrange
            Dictionary<int, int> subject = new Dictionary<int, int> { [42] = 1337 };
            Dictionary<int, int> expectation = new Dictionary<int, int> { [42] = 1337 };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, opt => opt)
                .And.ContainKey(42);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_a_dictionary_property_is_detected_it_should_ignore_the_order_of_the_pairs
            ()
        {
            // Arrange
            var expected = new
            {
                Customers = new Dictionary<string, string>
                {
                    ["Key2"] = "Value2",
                    ["Key1"] = "Value1"
                }
            };

            var subject = new
            {
                Customers = new Dictionary<string, string>
                {
                    ["Key1"] = "Value1",
                    ["Key2"] = "Value2"
                }
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_a_generic_dictionary_is_typed_as_object_and_runtime_typing_has_is_specified_it_should_use_the_runtime_type()
        {
            // Arrange
            object object1 = new Dictionary<string, string> { ["greeting"] = "hello" };
            object object2 = new Dictionary<string, string> { ["greeting"] = "hello" };

            // Act
            Action act = () => object1.Should().BeEquivalentTo(object2, opts => opts.RespectingRuntimeTypes());

            // Assert
            act.Should().NotThrow("the runtime type is a dictionary and the dictionaries are equivalent");
        }

        [Fact]
        public void When_a_generic_dictionary_is_typed_as_object_it_should_respect_the_runtime_typed()
        {
            // Arrange
            object object1 = new Dictionary<string, string> { ["greeting"] = "hello" };
            object object2 = new Dictionary<string, string> { ["greeting"] = "hello" };

            // Act
            Action act = () => object1.Should().BeEquivalentTo(object2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_a_non_generic_dictionary_is_typed_as_object_and_runtime_typing_is_specified_the_runtime_type_should_be_respected()
        {
            // Arrange
            object object1 = new NonGenericDictionary { ["greeting"] = "hello" };
            object object2 = new NonGenericDictionary { ["greeting"] = "hello" };

            // Act
            Action act = () => object1.Should().BeEquivalentTo(object2, opts => opts.RespectingRuntimeTypes());

            // Assert
            act.Should().NotThrow("the runtime type is a dictionary and the dictionaries are equivalent");
        }

        [Fact]
        public void
            When_a_non_generic_dictionary_is_decided_to_be_equivalent_to_expected_trace_is_still_written()
        {
            // Arrange
            object object1 = new NonGenericDictionary { ["greeting"] = "hello" };
            object object2 = new NonGenericDictionary { ["greeting"] = "hello" };
            var traceWriter = new StringBuilderTraceWriter();

            // Act
            object1.Should().BeEquivalentTo(object2, opts => opts.RespectingRuntimeTypes().WithTracing(traceWriter));

            // Assert
            string trace = traceWriter.ToString();
            trace.Should().Contain("Recursing into dictionary item greeting at root");
        }

        [Fact]
        public void When_a_non_generic_dictionary_is_typed_as_object_it_should_respect_the_runtime_type()
        {
            // Arrange
            object object1 = new NonGenericDictionary();
            object object2 = new NonGenericDictionary();

            // Act
            Action act = () => object1.Should().BeEquivalentTo(object2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_object_implements_two_IDictionary_interfaces_it_should_fail_descriptively()
        {
            // Arrange
            var object1 = new ClassWithTwoDictionaryImplementations();
            var object2 = new ClassWithTwoDictionaryImplementations();

            // Act
            Action act = () => object1.Should().BeEquivalentTo(object2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_equivalence_of_dictionaries_and_configured_to_respect_runtime_type_it_should_respect_the_runtime_type()
        {
            // Arrange
            IDictionary dictionary1 = new NonGenericDictionary { [2001] = new Car() };
            IDictionary dictionary2 = new NonGenericDictionary { [2001] = new Customer() };

            // Act
            Action act =
                () =>
                    dictionary1.Should().BeEquivalentTo(dictionary2,
                        opts => opts.RespectingRuntimeTypes());

            // Assert
            act.Should().Throw<XunitException>("the types have different properties");
        }

        [Fact]
        public void When_asserting_equivalence_of_dictionaries_it_should_respect_the_declared_type()
        {
            // Arrange
            var actual = new Dictionary<int, CustomerType> { [0] = new CustomerType("123") };
            var expectation = new Dictionary<int, CustomerType> { [0] = new DerivedCustomerType("123") };

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().NotThrow("because it should ignore the properties of the derived type");
        }

        [Fact]
        public void When_injecting_a_null_config_it_should_throw()
        {
            // Arrange
            var actual = new Dictionary<int, CustomerType>();
            var expectation = new Dictionary<int, CustomerType>();

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expectation, config: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("config");
        }

        [Fact]
        public void
            When_asserting_equivalence_of_generic_dictionaries_and_configured_to_use_runtime_properties_it_should_respect_the_runtime_type()
        {
            // Arrange
            var actual = new Dictionary<int, CustomerType> { [0] = new CustomerType("123") };
            var expectation = new Dictionary<int, CustomerType> { [0] = new DerivedCustomerType("123") };

            // Act
            Action act =
                () =>
                    actual.Should().BeEquivalentTo(expectation, opts => opts
                        .RespectingRuntimeTypes()
                        .ComparingByMembers<CustomerType>()
                    );

            // Assert
            act.Should().Throw<XunitException>("the runtime types have different properties");
        }

        [Fact]
        public void
            When_asserting_equivalence_of_generic_dictionaries_and_the_expectation_key_type_is_assignable_from_the_subjects_it_should_fail_if_incompatible()
        {
            // Arrange
            var actual = new Dictionary<object, string> { [new object()] = "hello" };
            var expected = new Dictionary<string, string> { ["greeting"] = "hello" };

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected actual to contain key \"greeting\"*");
        }

        [Fact]
        public void When_the_subjects_key_type_is_compatible_with_the_expected_key_type_it_should_match()
        {
            // Arrange
            var dictionary1 = new Dictionary<object, string> { ["greeting"] = "hello" };
            var dictionary2 = new Dictionary<string, string> { ["greeting"] = "hello" };

            // Act
            Action act = () => dictionary1.Should().BeEquivalentTo(dictionary2);

            // Assert
            act.Should().NotThrow("the keys are still strings");
        }

        [Fact]
        public void When_the_subjects_key_type_is_not_compatible_with_the_expected_key_type_it_should_throw()
        {
            // Arrange
            var actual = new Dictionary<int, string> { [1234] = "hello" };
            var expectation = new Dictionary<string, string> { ["greeting"] = "hello" };

            // Act
            Action act = () => expectation.Should().BeEquivalentTo(actual);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*expectation*keys*Int32*compatible types*IDictionary`2[System.String,System.String]*");
        }

        [Fact]
        public void
            When_asserting_equivalence_of_generic_dictionaries_the_type_information_should_be_preserved_for_other_equivalency_steps()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var dictionary1 = new Dictionary<Guid, IEnumerable<string>> { [userId] = new List<string> { "Admin", "Special" } };
            var dictionary2 = new Dictionary<Guid, IEnumerable<string>> { [userId] = new List<string> { "Admin", "Other" } };

            // Act
            Action act = () => dictionary1.Should().BeEquivalentTo(dictionary2);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_equivalence_of_non_generic_dictionaries_the_lack_of_type_information_should_be_preserved_for_other_equivalency_steps()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var dictionary1 = new NonGenericDictionary { [userId] = new List<string> { "Admin", "Special" } };
            var dictionary2 = new NonGenericDictionary { [userId] = new List<string> { "Admin", "Other" } };

            // Act
            Action act = () => dictionary1.Should().BeEquivalentTo(dictionary2);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Other*Special*");
        }

        [Fact]
        public void When_asserting_the_equivalence_of_generic_dictionaries_it_should_respect_the_declared_type()
        {
            // Arrange
            var actual = new Dictionary<int, CustomerType>
            {
                [0] = new DerivedCustomerType("123")
            };

            var expectation = new Dictionary<int, CustomerType> { [0] = new CustomerType("123") };

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().NotThrow("the objects are equivalent according to the members on the declared type");
        }

        [Fact]
        public void When_the_both_properties_are_null_it_should_not_throw()
        {
            // Arrange
            var expected = new ClassWithMemberDictionary
            {
                Dictionary = null
            };

            var subject = new ClassWithMemberDictionary
            {
                Dictionary = null
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void
            When_the_dictionary_values_are_handled_by_the_enumerable_equivalency_step_the_type_information_should_be_preserved()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var actual = new UserRolesLookupElement();
            actual.Add(userId, "Admin", "Special");

            var expected = new UserRolesLookupElement();
            expected.Add(userId, "Admin", "Other");

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*Roles[*][1]*Other*Special*");
        }

        [Fact]
        public void When_the_other_dictionary_does_not_contain_enough_items_it_should_throw
            ()
        {
            // Arrange
            var expected = new
            {
                Customers = new Dictionary<string, string>
                {
                    ["Key1"] = "Value1",
                    ["Key2"] = "Value2"
                }
            };

            var subject = new
            {
                Customers = new Dictionary<string, string>
                {
                    ["Key1"] = "Value1"
                }
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*Customers*dictionary*2 item(s)*but*misses*");
        }

        [Fact]
        public void When_the_other_property_is_not_a_dictionary_it_should_throw()
        {
            // Arrange
            var subject = new
            {
                Customers = "I am a string"
            };

            var expected = new
            {
                Customers = new Dictionary<string, string>
                {
                    ["Key2"] = "Value2",
                    ["Key1"] = "Value1"
                }
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*Customers*Dictionary`2[System.String,System.String], but found*String*");
        }

        [Fact]
        public void When_the_other_property_is_null_it_should_throw()
        {
            // Arrange
            var subject = new ClassWithMemberDictionary
            {
                Dictionary = new Dictionary<string, string>
                {
                    ["Key2"] = "Value2",
                    ["Key1"] = "Value1"
                }
            };

            var expected = new ClassWithMemberDictionary
            {
                Dictionary = null
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*member*Dictionary to be <null>, but found *{*}*");
        }

        [Fact]
        public void When_subject_dictionary_asserted_to_be_equivalent_have_less_elements_fails_describing_missing_keys()
        {
            // Arrange
            var dictionary1 = new Dictionary<string, string>
            {
                ["greeting"] = "hello"
            };
            var dictionary2 = new Dictionary<string, string>
            {
                ["greeting"] = "hello",
                ["farewell"] = "goodbye"
            };

            // Act
            Action action = () => dictionary1.Should().BeEquivalentTo(dictionary2);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected dictionary1 to be a dictionary with 2 item(s), but it misses key(s) {\"farewell\"}*");
        }

        [Fact]
        public void When_subject_dictionary_with_class_keys_asserted_to_be_equivalent_have_less_elements_other_dictionary_derived_class_keys_fails_describing_missing_keys()
        {
            // Arrange
            var dictionary1 = new Dictionary<SomeBaseKeyClass, string>
            {
                [new SomeDerivedKeyClass(1)] = "hello"
            };

            var dictionary2 = new Dictionary<SomeDerivedKeyClass, string>
            {
                [new SomeDerivedKeyClass(1)] = "hello",
                [new SomeDerivedKeyClass(2)] = "hello"
            };

            // Act
            Action action = () => dictionary1.Should().BeEquivalentTo(dictionary2);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected*to be a dictionary with 2 item(s), but*misses key(s) {BaseKey 2}*");
        }

        [Fact]
        public void When_subject_dictionary_asserted_to_be_equivalent_have_more_elements_fails_describing_additional_keys()
        {
            // Arrange
            var dictionary1 = new Dictionary<string, string>
            {
                ["greeting"] = "hello"
            };
            var dictionary2 = new Dictionary<string, string>
            {
                ["greeting"] = "hello",
                ["farewell"] = "goodbye"
            };

            // Act
            Action action = () => dictionary2.Should().BeEquivalentTo(dictionary1);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected dictionary2 to be a dictionary with 1 item(s), but*additional key(s) {\"farewell\"}*");
        }

        [Fact]
        public void When_subject_dictionary_with_class_keys_asserted_to_be_equivalent_and_other_dictionary_derived_class_keys_fails_because_of_types_incompatibility()
        {
            // Arrange
            var dictionary1 = new Dictionary<SomeBaseKeyClass, string>
            {
                [new SomeDerivedKeyClass(1)] = "hello"
            };

            var dictionary2 = new Dictionary<SomeDerivedKeyClass, string>
            {
                [new SomeDerivedKeyClass(1)] = "hello",
                [new SomeDerivedKeyClass(2)] = "hello"
            };

            // Act
            Action action = () => dictionary2.Should().BeEquivalentTo(dictionary1);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*the expectation is not keyed with any compatible types*");
        }

        [Fact]
        public void When_subject_dictionary_asserted_to_be_equivalent_have_less_elements_but_some_missing_and_some_additional_elements_fails_describing_missing_and_additional_keys()
        {
            // Arrange
            var dictionary1 = new Dictionary<string, string>
            {
                ["GREETING"] = "hello"
            };
            var dictionary2 = new Dictionary<string, string>
            {
                ["greeting"] = "hello",
                ["farewell"] = "goodbye"
            };

            // Act
            Action action = () => dictionary1.Should().BeEquivalentTo(dictionary2);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected*to be a dictionary with 2 item(s), but*misses key(s)*{\"greeting\", \"farewell\"}*additional key(s) {\"GREETING\"}*");
        }

        [Fact]
        public void When_subject_dictionary_asserted_to_be_equivalent_have_more_elements_but_some_missing_and_some_additional_elements_fails_describing_missing_and_additional_keys()
        {
            // Arrange
            var dictionary1 = new Dictionary<string, string>
            {
                ["GREETING"] = "hello"
            };
            var dictionary2 = new Dictionary<string, string>
            {
                ["greeting"] = "hello",
                ["farewell"] = "goodbye"
            };

            // Act
            Action action = () => dictionary2.Should().BeEquivalentTo(dictionary1);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected*to be a dictionary with 1 item(s), but*misses key(s) {\"GREETING\"}*additional key(s) {\"greeting\", \"farewell\"}*");
        }

        [Fact]
        public void When_two_equivalent_dictionaries_are_compared_directly_as_if_it_is_a_collection_it_should_succeed()
        {
            // Arrange
            var result = new Dictionary<string, int?>
            {
                ["C"] = null,
                ["B"] = 0,
                ["A"] = 0
            };

            // Act
            Action act = () => result.Should().BeEquivalentTo(new Dictionary<string, int?>
            {
                ["A"] = 0,
                ["B"] = 0,
                ["C"] = null
            });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_two_equivalent_dictionaries_are_compared_directly_it_should_succeed()
        {
            // Arrange
            var result = new Dictionary<string, int>
            {
                ["C"] = 0,
                ["B"] = 0,
                ["A"] = 0
            };

            // Act
            Action act = () => result.Should().BeEquivalentTo(new Dictionary<string, int>
            {
                ["A"] = 0,
                ["B"] = 0,
                ["C"] = 0
            });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_two_nested_dictionaries_contain_null_values_it_should_not_crash()
        {
            // Arrange
            var projection = new
            {
                ReferencedEquipment = new Dictionary<int, string>
                {
                    [1] = null
                }
            };

            var persistedProjection = new
            {
                ReferencedEquipment = new Dictionary<int, string>
                {
                    [1] = null
                }
            };

            // Act
            Action act = () => persistedProjection.Should().BeEquivalentTo(projection);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_two_nested_dictionaries_do_not_match_it_should_throw()
        {
            // Arrange
            var projection = new
            {
                ReferencedEquipment = new Dictionary<int, string>
                {
                    [1] = "Bla1"
                }
            };

            var persistedProjection = new
            {
                ReferencedEquipment = new Dictionary<int, string>
                {
                    [1] = "Bla2"
                }
            };

            // Act
            Action act = () => persistedProjection.Should().BeEquivalentTo(projection);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*ReferencedEquipment[1]*Bla1*Bla2*2*index 3*");
        }

        [Fact]
        public void When_a_nested_dictionary_value_doesnt_match_it_should_throw()
        {
            // Arrange
            const string json = @"{
                ""NestedDictionary"": {
                    ""StringProperty"": ""string"",
                    ""IntProperty"": 123
                }
            }";

            var expectedResult = new Dictionary<string, object>
            {
                ["NestedDictionary"] = new Dictionary<string, object>
                {
                    ["StringProperty"] = "string",
                    ["IntProperty"] = 123
                }
            };

            // Act
            var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Action act = () => result.Should().BeEquivalentTo(expectedResult);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*String*JValue*");
        }

        [Fact]
        public void When_a_custom_rule_is_applied_on_a_dictionary_it_should_apply_it_on_the_values()
        {
            // Arrange
            var dictOne = new Dictionary<string, double>
            {
                { "a", 1.2345 },
                { "b", 2.4567 },
                { "c", 5.6789 },
                { "s", 3.333 }
            };

            var dictTwo = new Dictionary<string, double>
            {
                { "a", 1.2348 },
                { "b", 2.4561 },
                { "c", 5.679 },
                { "s", 3.333 }
            };

            // Act / Assert
            dictOne.Should().BeEquivalentTo(dictTwo, options => options
                .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.1))
                .WhenTypeIs<double>()
            );
        }
    }
}
