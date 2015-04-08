using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class CollectionEquivalencySpecs
    {
        #region Include & Exclude

        [TestMethod]
        public void When_a_deeply_nested_property_of_a_collection_with_an_invalid_value_is_excluded_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Text = "Root",
                Level = new
                {
                    Text = "Level1",
                    Level = new
                    {
                        Text = "Level2"
                    },
                    Collection = new[]
                    {
                        new {Number = 1, Text = "Text"},
                        new {Number = 2, Text = "Actual"}
                    }
                }
            };

            var expected = new
            {
                Text = "Root",
                Level = new
                {
                    Text = "Level1",
                    Level = new
                    {
                        Text = "Level2"
                    },
                    Collection = new[]
                    {
                        new {Number = 1, Text = "Text"},
                        new {Number = 2, Text = "Expected"}
                    }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expected, options => options.
                Excluding(x => x.Level.Collection[1].Number).
                Excluding(x => x.Level.Collection[1].Text));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_specific_property_is_included_it_should_ignore_the_rest_of_the_properties()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var result = new[] {new {A = "aaa", B = "bbb"}};

            var expected = new
            {
                A = "aaa",
                B = "ccc"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => result.ShouldAllBeEquivalentTo(new[] {expected}, options => options.Including(x => x.A));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region Non-Generic Collections

        [TestMethod]
        public void When_asserting_equivalence_of_collections_it_should_respect_the_declared_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            ICollection collection1 = new NonGenericCollection(new[] {new Car()});
            ICollection collection2 = new NonGenericCollection(new[] {new Customer()});

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.ShouldBeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow("the declared type is object");
        }

        [TestMethod]
        public void
            When_asserting_equivalence_of_collections_and_configured_to_use_runtime_properties_it_should_respect_the_runtime_type
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            ICollection collection1 = new NonGenericCollection(new[] {new Car()});
            ICollection collection2 = new NonGenericCollection(new[] {new Customer()});

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                    collection1.ShouldBeEquivalentTo(collection2,
                        opts => opts.RespectingRuntimeTypes());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>("the types have different properties");
        }

        private class NonGenericCollection : ICollection
        {
            private readonly IList<object> inner;

            public NonGenericCollection(IList<object> inner)
            {
                this.inner = inner;
            }

            public IEnumerator GetEnumerator()
            {
                foreach (var @object in inner)
                {
                    yield return @object;
                }
            }

            public void CopyTo(Array array, int index)
            {
                ((ICollection) inner).CopyTo(array, index);
            }

            public int Count
            {
                get { return inner.Count; }
            }

            public object SyncRoot
            {
                get { return ((ICollection) inner).SyncRoot; }
            }

            public bool IsSynchronized
            {
                get { return ((ICollection) inner).IsSynchronized; }
            }
        }

        #endregion

        #region Generics

        [TestMethod]
        public void When_a_type_implements_multiple_IEnumerable_interfaces_it_should_fail_descriptively()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var enumerable1 = new EnumerableOfStringAndObject();
            var enumerable2 = new EnumerableOfStringAndObject();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => enumerable1.ShouldBeEquivalentTo(enumerable2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("*System.String*System.Object*cannot determine*");
        }

        [TestMethod]
        public void When_asserting_equivalence_of_generic_collections_it_should_respect_the_declared_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection1 = new Collection<CustomerType> {new DerivedCustomerType("123")};
            var collection2 = new Collection<CustomerType> {new CustomerType("123")};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.ShouldBeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow("the objects are equivalent according to the members on the declared type");
        }

        [TestMethod]
        public void
            When_asserting_equivalence_of_generic_collections_and_configured_to_use_runtime_type_information_it_should_respect_the_runtime_type
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection1 = new Collection<CustomerType> {new DerivedCustomerType("123")};
            var collection2 = new Collection<CustomerType> {new CustomerType("123")};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                    collection1.ShouldBeEquivalentTo(collection2,
                        opts => opts.RespectingRuntimeTypes());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>("the runtime types have different properties");
        }

        [TestMethod]
        public void
            When_a_strongly_typed_collection_is_declared_as_an_untyped_collection_is_should_respect_the_declared_type
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            ICollection collection1 = new List<Car> {new Car()};
            ICollection collection2 = new List<Customer> {new Customer()};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.ShouldBeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow("the declared type is object");
        }

        [TestMethod]
        public void
            When_a_strongly_typed_collection_is_declared_as_an_untyped_collection_and_runtime_checking_is_configured_is_should_use_the_runtime_type
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            ICollection collection1 = new List<Car> {new Car()};
            ICollection collection2 = new List<Customer> {new Customer()};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => collection1.ShouldBeEquivalentTo(collection2, opts => opts.RespectingRuntimeTypes());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>("the items have different runtime types");
        }

        [TestMethod]
        public void
            When_an_object_implements_multiple_IEnumerable_interfaces_but_the_declared_type_is_assignable_to_only_one_it_should_respect_the_declared_type
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new EnumerableOfStringAndObject();
            IEnumerable<string> collection2 = new EnumerableOfStringAndObject();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.ShouldBeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow("the declared type is assignable to only one IEnumerable interface");
        }

        [TestMethod]
        public void
            When_a_object_implements_multiple_IEnumerable_interfaces_but_the_declared_type_is_assignable_to_only_one_and_runtime_checking_is_configured_it_should_fail
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new EnumerableOfStringAndObject();
            IEnumerable<string> collection2 = new EnumerableOfStringAndObject();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => collection1.ShouldBeEquivalentTo(collection2, opts => opts.RespectingRuntimeTypes());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>("the runtime type is assignable to two IEnumerable interfaces")
                .WithMessage("*cannot determine which one*");
        }

        private class EnumerableOfStringAndObject : IEnumerable<string>, IEnumerable<object>
        {
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            IEnumerator<object> IEnumerable<object>.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<string> GetEnumerator()
            {
                yield return String.Empty;
            }
        }

        #endregion

        #region Collection Equivalence

        [TestMethod]
        public void When_two_unordered_lists_are_structurally_equivalent_and_order_is_strict_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new[]
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            var expectation = new Collection<Customer>
            {
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                },
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldAllBeEquivalentTo(expectation, options => options.WithStrictOrdering());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected item[0].Name*Jane*John*item[1].Name*John*Jane*");
        }

        [TestMethod]
        public void When_an_unordered_collection_must_be_strict_using_an_expression_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new[]
            {
                new
                {
                    Name = "John",
                    UnorderedCollection = new[] {1, 2, 3, 4, 5}
                },
                new
                {
                    Name = "Jane",
                    UnorderedCollection = new int[0]
                }
            };

            var expectation = new[]
            {
                new
                {
                    Name = "John",
                    UnorderedCollection = new[] {5, 4, 3, 2, 1}
                },
                new
                {
                    Name = "Jane",
                    UnorderedCollection = new int[0]
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action =
                () =>
                    subject.ShouldAllBeEquivalentTo(expectation,
                        options => options
                            .WithStrictOrderingFor(
                                s => s.UnorderedCollection));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "*Expected item[0].UnorderedCollection*5 item(s)*0*");
        }

        [TestMethod]
        public void
            When_an_unordered_collection_must_be_strict_using_a_predicate_it_should_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new[]
            {
                new
                {
                    Name = "John",
                    UnorderedCollection = new[] {1, 2, 3, 4, 5}
                },
                new
                {
                    Name = "Jane",
                    UnorderedCollection = new int[0]
                }
            };

            var expectation = new[]
            {
                new
                {
                    Name = "John",
                    UnorderedCollection = new[] {5, 4, 3, 2, 1}
                },
                new
                {
                    Name = "Jane",
                    UnorderedCollection = new int[0]
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action =
                () =>
                    subject.ShouldAllBeEquivalentTo(expectation,
                        options => options
                            .WithStrictOrderingFor(
                                s =>
                                    s.SelectedMemberPath.Contains(
                                        "UnorderedCollection")));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "*Expected item[0].UnorderedCollection*5 item(s)*0*");
        }

        [TestMethod]
        public void When_two_lists_only_differ_in_excluded_properties_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            var expectation = new List<CustomerDto>
            {
                new CustomerDto
                {
                    Name = "John",
                    Age = 27
                },
                new CustomerDto
                {
                    Name = "Jane",
                    Age = 30
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action =
                () =>
                    subject.ShouldAllBeEquivalentTo(expectation,
                        options => options
                            .ExcludingMissingMembers()
                            .Excluding(c => c.Age));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        private class MyObject
        {
            public string MyString { get; set; }
            public MyChildObject Child { get; set; }
        }

        private class MyChildObject
        {
            public int Id { get; set; }
            
            public string MyChildString { get; set; }

            public override bool Equals(object obj)
            {
                return (obj is MyChildObject) && (((MyChildObject)obj).Id == this.Id);
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }

        [TestMethod]
        public void When_nested_objects_are_excluded_from_collections_it_should_use_simple_equality_semantics()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            MyObject actual = new MyObject
            {
                MyString = "identical string",
                Child = new MyChildObject
                {
                    Id = 1, 
                    MyChildString = "identical string"
                }
            };

            MyObject expectation = new MyObject
            {
                MyString = "identical string",
                Child = new MyChildObject
                {
                    Id = 1,
                    MyChildString = "DIFFERENT STRING"
                }
            };

            IList<MyObject> actualList = new List<MyObject> { actual };
            IList<MyObject> expectationList = new List<MyObject> { expectation };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => actualList.ShouldAllBeEquivalentTo(expectationList, opt => opt.ExcludingNestedObjects());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void
            When_two_collections_have_properties_of_the_contained_items_excluded_but_still_differ_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var list1 = new[] {new KeyValuePair<int, int>(1, 123)};
            var list2 = new[] {new KeyValuePair<int, int>(2, 321)};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => list1.ShouldAllBeEquivalentTo(list2, config => config.Excluding(ctx => ctx.Key));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage("Expected item[0].Value to be 321, but found 123.*");
        }

        [TestMethod]
        public void
            When_ShouldAllBeEquivalentTo_has_selection_rules_configured_they_should_be_evaluated_from_right_to_left()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var list1 = new[] {new {Value = 3}};
            var list2 = new[] {new {Value = 2}};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => list1.ShouldAllBeEquivalentTo(list2, config =>
            {
                config.WithoutSelectionRules();
                config.Using(new SelectNoMembersSelectionRule());
                config.Using(new SelectPropertiesSelectionRule());
                return config;
            });

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage("*to be 2, but found 3*");
        }

        private class SelectPropertiesSelectionRule : IMemberSelectionRule
        {
            public IEnumerable<SelectedMemberInfo> SelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers,
                ISubjectInfo context, IEquivalencyAssertionOptions config)
            {
                return context.CompileTimeType.GetNonPrivateProperties().Select(SelectedMemberInfo.Create);
            }

            public bool OverridesStandardIncludeRules
            {
                get { throw new NotImplementedException(); }
            }

            bool IMemberSelectionRule.IncludesMembers
            {
                get { return OverridesStandardIncludeRules; }
            }
        }

        private class SelectNoMembersSelectionRule : IMemberSelectionRule
        {
            public bool OverridesStandardIncludeRules
            {
                get { return true; }
            }

            public IEnumerable<SelectedMemberInfo> SelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers,
                ISubjectInfo context, IEquivalencyAssertionOptions config)
            {
                return Enumerable.Empty<SelectedMemberInfo>();
            }

            bool IMemberSelectionRule.IncludesMembers
            {
                get { return OverridesStandardIncludeRules; }
            }
        }

        [TestMethod]
        public void
            When_two_collections_have_nested_members_of_the_contained_equivalent_but_not_equal_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var list1 = new[]
            {
                new
                {
                    Nested = new ClassWithOnlyAProperty
                    {
                        Value = 1
                    }
                }
            };

            var list2 = new[]
            {
                new
                {
                    Nested = new
                    {
                        Value = 1
                    }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => list1.ShouldAllBeEquivalentTo(list2, opts => opts);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_ShouldAllBeEquivalentTo_utilizes_custom_assertion_rules_the_rules_should_be_respected()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new[]
            {
                new
                {
                    Value =
                        new Customer
                        {
                            Name = "John",
                            Age = 27,
                            Id = 1
                        }
                },
                new
                {
                    Value =
                        new Customer
                        {
                            Name = "Jane",
                            Age = 24,
                            Id = 2
                        }
                }
            };

            var expectation = new[]
            {
                new
                {
                    Value =
                        new CustomerDto
                        {
                            Name = "John",
                            Age = 27
                        }
                },
                new
                {
                    Value =
                        new CustomerDto
                        {
                            Name = "Jane",
                            Age = 30
                        }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                    subject.ShouldAllBeEquivalentTo(
                        expectation,
                        opts =>
                            opts.Using<Customer>(
                                ctx =>
                                {
                                    throw new Exception(
                                        "Interestingly, Using cannot cross types so this is never hit");
                                })
                                .WhenTypeIs<Customer>());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(String.Format("*to be a {0}, but found a {1}*", typeof (CustomerDto), typeof (Customer)));
        }

        [TestMethod]
        public void
            When_two_ordered_lists_are_structurally_equivalent_it_should_succeed
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action =
                () => subject.ShouldAllBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void
            When_two_unordered_lists_are_structurally_equivalent_it_should_succeed
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new[]
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            var expectation = new Collection<Customer>
            {
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                },
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action =
                () => subject.ShouldAllBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void
            When_two_lists_dont_contain_the_same_structural_equal_objects_it_should_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 30,
                    Id = 2
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action =
                () => subject.ShouldAllBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected*item[1].Age*30*24*");
        }

        [TestMethod]
        public void
            When_a_byte_array_does_not_match_strictly_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new byte[] {1, 2, 3, 4, 5, 6};

            var expectation = new byte[] {6, 5, 4, 3, 2, 1};

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected*item[0]*6*1*");
        }

        [TestMethod]
        public void
            When_no_collection_item_matches_it_should_report_the_closest_match
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 30,
                    Id = 2
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "Jane",
                    Age = 30,
                    Id = 2
                },
                new Customer
                {
                    Name = "John",
                    Age = 28,
                    Id = 1
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action =
                () => subject.ShouldAllBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected*item[1].Age*28*27*");
        }

        [TestMethod]
        public void
            When_the_subject_contains_same_number_of_items_but_subject_contains_duplicates_it_should_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action =
                () => subject.ShouldAllBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected item[1].Name to be \"Jane\", but \"John\" differs near*");
        }

        [TestMethod]
        public void
            When_the_subject_contains_more_items_than_expected_it_should_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action =
                () => subject.ShouldAllBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected subject to be a collection with 1 item(s), but found 2*");
        }

        [TestMethod]
        public void
            When_the_subject_contains_less_items_than_expected_it_should_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action =
                () => subject.ShouldAllBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "*subject to be a collection with 2 item(s), but found 1*");
        }

        [TestMethod]
        public void
            When_the_subject_contains_same_number_of_items_but_expectation_contains_duplicates_it_should_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action =
                () => subject.ShouldAllBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected item[1].Name to be \"John\", but \"Jane\" differs near*");
        }

        [TestMethod]
        public void
            When_the_subject_contains_same_number_of_items_and_both_contain_duplicates_it_should_succeed
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                },
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action =
                () => subject.ShouldAllBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_collection_is_compared_to_a_non_collection_it_should_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldAllBeEquivalentTo("hello");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected subject to be \"hello\", but found {empty}*");
        }

        #endregion

        #region Cyclic References

        [TestMethod]
        public void
            When_the_root_object_is_referenced_from_an_object_in_a_nested_collection_it_should_treat_it_as_a_cyclic_reference
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var company1 = new MyCompany {Name = "Company"};
            var user1 = new MyUser {Name = "User", Company = company1};
            company1.Users = new List<MyUser> {user1};
            var logo1 = new MyCompanyLogo {Url = "blank", Company = company1, CreatedBy = user1};
            company1.Logo = logo1;

            var company2 = new MyCompany {Name = "Company"};
            var user2 = new MyUser {Name = "User", Company = company2};
            company2.Users = new List<MyUser> {user2};
            var logo2 = new MyCompanyLogo {Url = "blank", Company = company2, CreatedBy = user2};
            company2.Logo = logo2;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => company1.ShouldBeEquivalentTo(company2, o => o.IgnoringCyclicReferences());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void
            When_a_collection_contains_a_reference_to_an_object_that_is_also_in_its_parent_it_should_not_be_treated_as_a_cyclic_reference
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var logbook = new BasicEquivalencySpecs.LogbookCode("SomeKey");

            var logbookEntry = new BasicEquivalencySpecs.LogbookEntryProjection
            {
                Logbook = logbook,
                LogbookRelations = new[]
                {
                    new BasicEquivalencySpecs.LogbookRelation
                    {
                        Logbook = logbook
                    }
                }
            };

            var equivalentLogbookEntry = new BasicEquivalencySpecs.LogbookEntryProjection
            {
                Logbook = logbook,
                LogbookRelations = new[]
                {
                    new BasicEquivalencySpecs.LogbookRelation
                    {
                        Logbook = logbook
                    }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                    logbookEntry.ShouldBeEquivalentTo(equivalentLogbookEntry);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region Nested Enumerables

        [TestMethod]
        public void
            When_a_collection_property_contains_objects_with_matching_properties_in_any_order_it_should_not_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new
            {
                Customers = new[]
                {
                    new Customer
                    {
                        Age = 32,
                        Birthdate = 31.July(1978),
                        Name = "Jane"
                    },
                    new Customer
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "John"
                    }
                }
            };

            var subject = new
            {
                Customers = new[]
                {
                    new CustomerDto
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "John"
                    },
                    new CustomerDto
                    {
                        Age = 32,
                        Birthdate = 31.July(1978),
                        Name = "Jane"
                    }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void
            When_a_collection_property_contains_objects_with_mismatching_properties_it_should_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new
            {
                Customers = new[]
                {
                    new Customer
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "John"
                    }
                }
            };

            var subject = new
            {
                Customers = new[]
                {
                    new CustomerDto
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "Jane"
                    }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("*Customers[0].Name*John*Jane*");
        }

        [TestMethod]
        public void
            When_a_collection_property_was_expected_but_the_property_is_not_a_collection_it_should_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Customers = "Jane, John"
            };

            var expected = new
            {
                Customers = new[]
                {
                    new Customer
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "John"
                    }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "*member Customers to be*Customer[]*, but*System.String*");
        }

        [TestMethod]
        public void
            When_a_collection_contains_more_items_than_expected_it_should_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new
            {
                Customers = new[]
                {
                    new Customer
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "John"
                    }
                }
            };

            var subject = new
            {
                Customers = new[]
                {
                    new CustomerDto
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "Jane"
                    },
                    new CustomerDto
                    {
                        Age = 24,
                        Birthdate = 21.September(1973),
                        Name = "John"
                    }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "*member Customers to be a collection with 1 item(s), but found 2*");
        }

        [TestMethod]
        public void
            When_a_collection_contains_less_items_than_expected_it_should_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new
            {
                Customers = new[]
                {
                    new Customer
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "John"
                    },
                    new Customer
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "Jane"
                    }
                }
            };

            var subject = new
            {
                Customers = new[]
                {
                    new CustomerDto
                    {
                        Age = 24,
                        Birthdate = 21.September(1973),
                        Name = "John"
                    }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "*member Customers to be a collection with 2 item(s), but found 1*");
        }

        [TestMethod]
        public void
            When_a_complex_object_graph_with_collections_matches_expectations_it_should_not_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Bytes = new byte[]
                {
                    1, 2, 3, 4
                },
                Object = new
                {
                    A = 1,
                    B = 2
                }
            };

            var expected = new
            {
                Bytes = new byte[]
                {
                    1, 2, 3, 4
                },
                Object = new
                {
                    A = 1,
                    B = 2
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region (Nested) Dictionaries

        [TestMethod]
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
            Action act = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void
            When_the_other_property_is_not_a_dictionary_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new
            {
                Customers = "I am a string"
            };

            var subject = new
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
            Action act = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Member*Customers*dictionary*non-dictionary*");
        }

        [TestMethod]
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
            Action act = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected*Customers*dictionary*2 item(s)*but*1 item(s)*");
        }

        [TestMethod]
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
            Action act = () => result.ShouldBeEquivalentTo(new Dictionary<string, int>
            {
                {"A", 0},
                {"B", 0},
                {"C", 0}
            });

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void
            When_two_equivalent_dictionaries_are_compared_directly_as_if_it_is_a_collection_it_should_succeed()
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
            Action act = () => result.ShouldAllBeEquivalentTo(new Dictionary<string, int?>
            {
                {"A", 0},
                {"B", 0},
                {"C", null}
            });

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
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
            Action act = () => persistedProjection.ShouldBeEquivalentTo(projection);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected*ReferencedEquipment[1]*Bla1*Bla2*2*index 3*");
        }

        [TestMethod]
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
            Action act = () => persistedProjection.ShouldBeEquivalentTo(projection);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_two_nested_dictionaries_contain_null_values_it_should_not_crash2()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Guid userId = Guid.NewGuid();

            var actual = new UserRolesLookupElement();
            actual.Add(userId, "Admin", "Special");

            var expected = new UserRolesLookupElement();
            expected.Add(userId, "Admin", "Other");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => actual.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected*Roles[*][1]*Other*Special*");
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

        #endregion
    }
}