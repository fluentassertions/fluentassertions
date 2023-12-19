using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertionsAsync.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Equivalency.Specs;

public class CollectionSpecs
{
    public interface IInterface
    {
        int InterfaceProperty { get; set; }
    }

    public class MyClass : IInterface
    {
        public int InterfaceProperty { get; set; }

        public int ClassProperty { get; set; }
    }

    public class SubDummy
    {
        public SubDummy(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public override bool Equals(object obj)
        {
            return obj is SubDummy subDummy
                && Id == subDummy.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class TestDummy
    {
        public TestDummy(SubDummy sd)
        {
            Sd = sd;
        }

        public SubDummy Sd { get; }
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
            ((ICollection)inner).CopyTo(array, index);
        }

        public int Count => inner.Count;

        public object SyncRoot => ((ICollection)inner).SyncRoot;

        public bool IsSynchronized => ((ICollection)inner).IsSynchronized;
    }

    private class EnumerableOfStringAndObject : IEnumerable<object>, IEnumerable<string>
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
            yield return string.Empty;
        }
    }

    public class MyObject
    {
        public string MyString { get; set; }

        public ClassIdentifiedById Child { get; set; }
    }

    public class ClassIdentifiedById
    {
        public int Id { get; set; }

        public string MyChildString { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ClassIdentifiedById other && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    private class SelectPropertiesSelectionRule : IMemberSelectionRule
    {
        public bool OverridesStandardIncludeRules => throw new NotImplementedException();

        public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
            MemberSelectionContext context)
        {
            return context.Type.GetProperties().Select(pi => new Property(pi, currentNode));
        }

        bool IMemberSelectionRule.IncludesMembers => OverridesStandardIncludeRules;
    }

    private class SelectNoMembersSelectionRule : IMemberSelectionRule
    {
        public bool OverridesStandardIncludeRules => true;

        public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
            MemberSelectionContext context)
        {
            return Enumerable.Empty<IMember>();
        }

        bool IMemberSelectionRule.IncludesMembers => OverridesStandardIncludeRules;
    }

    public class UserRolesLookupElement
    {
        private readonly Dictionary<Guid, List<string>> innerRoles = new();

        public virtual Dictionary<Guid, IEnumerable<string>> Roles
            => innerRoles.ToDictionary(x => x.Key, y => y.Value.Select(z => z));

        public void Add(Guid userId, params string[] roles)
        {
            innerRoles[userId] = roles.ToList();
        }
    }

    public class ExceptionThrowingClass
    {
#pragma warning disable CA1065 // this is for testing purposes.
        public object ExceptionThrowingProperty => throw new NotImplementedException();
#pragma warning restore CA1065
    }

    [Fact]
    public async Task When_the_expectation_is_an_array_of_interface_type_it_should_respect_declared_types()
    {
        // Arrange
        var actual = new IInterface[]
        {
            new MyClass { InterfaceProperty = 1, ClassProperty = 42 }
        };

        var expected = new IInterface[]
        {
            new MyClass { InterfaceProperty = 1, ClassProperty = 1337 }
        };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().NotThrowAsync<XunitException>("it should respect the declared types on IInterface");
    }

    [Fact]
    public async Task When_the_expectation_has_fewer_dimensions_than_a_multi_dimensional_subject_it_should_fail()
    {
        // Arrange
        object objectA = new();
        object objectB = new();

        var actual = new[] { new[] { objectA, objectB } };
        var expected = actual[0];

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*be a collection with 2 item(s)*contains 1 item(s) less than*",
                "adding a `params object[]` overload cannot distinguish 'an array of objects' from 'an element which is an array of objects'");
    }

    [Fact]
    public async Task When_the_expectation_is_an_array_of_anonymous_types_it_should_respect_runtime_types()
    {
        // Arrange
        var actual = new[] { new { A = 1, B = 2 }, new { A = 1, B = 2 } };
        var expected = new object[] { new { A = 1 }, new { B = 2 } };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_a_byte_array_does_not_match_strictly_it_should_throw()
    {
        // Arrange
        var subject = new byte[] { 1, 2, 3, 4, 5, 6 };

        var expectation = new byte[] { 6, 5, 4, 3, 2, 1 };

        // Act
        Func<Task> action = async () => await subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected*subject[0]*6*1*");
    }

    [Fact]
    public async Task When_a_byte_array_does_not_match_strictly_and_order_is_not_strict_it_should_throw()
    {
        // Arrange
        var subject = new byte[] { 1, 2, 3, 4, 5, 6 };

        var expectation = new byte[] { 6, 5, 4, 3, 2, 1 };

        // Act
        Func<Task> action = async () => await subject.Should().BeEquivalentToAsync(expectation, options => options.WithoutStrictOrdering());

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected*subject[0]*6*1*");
    }

    [Fact]
    public async Task
        When_a_collection_property_is_a_byte_array_which_does_not_match_strictly_and_order_is_not_strict_it_should_throw()
    {
        // Arrange
        var subject = new { bytes = new byte[] { 1, 2, 3, 4, 5, 6 } };

        var expectation = new { bytes = new byte[] { 6, 5, 4, 3, 2, 1 } };

        // Act
        Func<Task> action = async () => await subject.Should().BeEquivalentToAsync(expectation, options => options.WithoutStrictOrdering());

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected *bytes[0]*6*1*");
    }

    [Fact]
    public async Task When_a_collection_does_not_match_it_should_include_items_in_message()
    {
        // Arrange
        var subject = new[] { 1, 2 };

        var expectation = new[] { 3, 2, 1 };

        // Act
        Func<Task> action = async () => await subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected*but*{1, 2}*1 item(s) less than*{3, 2, 1}*");
    }

    [Fact]
    public async Task When_collection_of_same_count_does_not_match_it_should_include_at_most_10_items_in_message()
    {
        // Arrange
        const int commonLength = 11;

        // Subjects contains different values, because we want to distinguish them in the assertion message
        var subject = new int[commonLength] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var expectation = Enumerable.Repeat(20, commonLength).ToArray();

        // Act
        Func<Task> action = async () => await subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        (await action.Should().ThrowAsync<XunitException>()).Which
            .Message.Should().Contain("[9]").And.NotContain("[10]");
    }

    [Fact]
    public async Task When_a_nullable_collection_does_not_match_it_should_throw()
    {
        // Arrange
        var subject = new
        {
            Values = (ImmutableArray<int>?)ImmutableArray.Create(1, 2, 3)
        };

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(new
        {
            Values = (ImmutableArray<int>?)ImmutableArray.Create(1, 2, 4)
        });

        // Assert
        await act.Should().ThrowAsync<XunitException>().WithMessage("Expected*Values[2]*to be 4, but found 3*");
    }

    [Fact]
    public async Task When_object_with_async_enumerable_are_different_it_should_throw()
    {
        // Arrange
        AsyncEnumerableDto dto = new()
        {
            Foo = AsyncEnumerable.Range(2, 1)
        };

        // Act
        Func<Task> act = async () => await dto.Should().BeEquivalentToAsync(new AsyncEnumerableDto
        {
            Foo = Enumerable.Range(1, 2).ToAsyncEnumerable()
        });

        // Assert
        await act.Should().ThrowAsync<XunitException>().WithMessage("Expected property dto.Foo to be a collection with 2 item(s)*contains 1 item(s) less than*");
    }

    [Fact]
    public async Task When_subject_and_expectation_with_async_enumerable_are_different_it_should_throw()
    {
        // Arrange
        IAsyncEnumerable<int> range = AsyncEnumerable.Range(1, 3);

        // Act
        Func<Task> act = async () => await range
            .Should().BeEquivalentToAsync(AsyncEnumerable.Range(1, 5));

        // Assert
        await act.Should().ThrowAsync<XunitException>().WithMessage("Expected range to be a collection with 5 item(s)*contains 2 item(s) less than*");
    }

    [Fact]
    public async Task
        When_a_collection_contains_a_reference_to_an_object_that_is_also_in_its_parent_it_should_not_be_treated_as_a_cyclic_reference()
    {
        // Arrange
        var logbook = new LogbookCode("SomeKey");

        var logbookEntry = new LogbookEntryProjection
        {
            Logbook = logbook,
            LogbookRelations = new[] { new LogbookRelation { Logbook = logbook } }
        };

        var equivalentLogbookEntry = new LogbookEntryProjection
        {
            Logbook = logbook,
            LogbookRelations = new[] { new LogbookRelation { Logbook = logbook } }
        };

        // Act
        Func<Task> act = async
            () => await
                logbookEntry.Should().BeEquivalentToAsync(equivalentLogbookEntry);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_a_collection_contains_less_items_than_expected_it_should_throw()
    {
        // Arrange
        var expected = new
        {
            Customers = new[]
            {
                new Customer { Age = 38, Birthdate = 20.September(1973), Name = "John" },
                new Customer { Age = 38, Birthdate = 20.September(1973), Name = "Jane" }
            }
        };

        var subject = new
        {
            Customers = new[] { new CustomerDto { Age = 24, Birthdate = 21.September(1973), Name = "John" } }
        };

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "*Customers*to be a collection with 2 item(s), but*contains 1 item(s) less than*");
    }

    [Fact]
    public async Task When_a_collection_contains_more_items_than_expected_it_should_throw()
    {
        // Arrange
        var expected = new { Customers = new[] { new Customer { Age = 38, Birthdate = 20.September(1973), Name = "John" } } };

        var subject = new
        {
            Customers = new[]
            {
                new CustomerDto { Age = 38, Birthdate = 20.September(1973), Name = "Jane" },
                new CustomerDto { Age = 24, Birthdate = 21.September(1973), Name = "John" }
            }
        };

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "*Customers*to be a collection with 1 item(s), but*contains 1 item(s) more than*");
    }

    [Fact]
    public async Task When_a_collection_property_contains_objects_with_matching_properties_in_any_order_it_should_not_throw()
    {
        // Arrange
        var expected = new
        {
            Customers = new[]
            {
                new Customer { Age = 32, Birthdate = 31.July(1978), Name = "Jane" },
                new Customer { Age = 38, Birthdate = 20.September(1973), Name = "John" }
            }
        };

        var subject = new
        {
            Customers = new[]
            {
                new CustomerDto { Age = 38, Birthdate = 20.September(1973), Name = "John" },
                new CustomerDto { Age = 32, Birthdate = 31.July(1978), Name = "Jane" }
            }
        };

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(expected, o => o.ExcludingMissingMembers());

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_two_deeply_nested_collections_are_equivalent_while_ignoring_the_order_it_should_not_throw()
    {
        // Arrange
        var items = new[] { new int[0], new[] { 42 } };

        // Act / Assert
        await items.Should().BeEquivalentToAsync(
            new[] { new[] { 42 }, new int[0] }
        );
    }

    [Fact]
    public async Task When_a_collection_property_contains_objects_with_mismatching_properties_it_should_throw()
    {
        // Arrange
        var expected = new { Customers = new[] { new Customer { Age = 38, Birthdate = 20.September(1973), Name = "John" } } };

        var subject = new
        {
            Customers = new[] { new CustomerDto { Age = 38, Birthdate = 20.September(1973), Name = "Jane" } }
        };

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*Customers[0].Name*John*Jane*");
    }

    [Fact]
    public async Task When_the_subject_is_a_non_generic_collection_it_should_still_work()
    {
        // Arrange
        object item = new();
        object[] array = { item };
        IList readOnlyList = ArrayList.ReadOnly(array);

        // Act / Assert
        await readOnlyList.Should().BeEquivalentToAsync(array);
    }

    [Fact]
    public async Task When_a_collection_property_was_expected_but_the_property_is_not_a_collection_it_should_throw()
    {
        // Arrange
        var subject = new { Customers = "Jane, John" };

        var expected = new { Customers = new[] { new Customer { Age = 38, Birthdate = 20.September(1973), Name = "John" } } };

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected*Customers*collection*String*");
    }

    [Fact]
    public async Task When_a_complex_object_graph_with_collections_matches_expectations_it_should_not_throw()
    {
        // Arrange
        var subject = new { Bytes = new byte[] { 1, 2, 3, 4 }, Object = new { A = 1, B = 2 } };

        var expected = new { Bytes = new byte[] { 1, 2, 3, 4 }, Object = new { A = 1, B = 2 } };

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_a_deeply_nested_property_of_a_collection_with_an_invalid_value_is_excluded_it_should_not_throw()
    {
        // Arrange
        var subject = new
        {
            Text = "Root",
            Level = new
            {
                Text = "Level1",
                Level = new { Text = "Level2" },
                Collection = new[] { new { Number = 1, Text = "Text" }, new { Number = 2, Text = "Actual" } }
            }
        };

        var expected = new
        {
            Text = "Root",
            Level = new
            {
                Text = "Level1",
                Level = new { Text = "Level2" },
                Collection = new[] { new { Number = 1, Text = "Text" }, new { Number = 2, Text = "Expected" } }
            }
        };

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(expected,
            options => options.Excluding(x => x.Level.Collection[1].Number).Excluding(x => x.Level.Collection[1].Text));

        // Assert
        await act.Should().NotThrowAsync();
    }

    public class For
    {
        [Fact]
        public async Task When_property_in_collection_is_excluded_it_should_not_throw()
        {
            // Arrange
            var subject = new
            {
                Level = new
                {
                    Collection = new[]
                    {
                        new
                        {
                            Number = 1,
                            Text = "Text"
                        },
                        new
                        {
                            Number = 2,
                            Text = "Actual"
                        }
                    }
                }
            };

            var expected = new
            {
                Level = new
                {
                    Collection = new[]
                    {
                        new
                        {
                            Number = 1,
                            Text = "Text"
                        },
                        new
                        {
                            Number = 3,
                            Text = "Actual"
                        }
                    }
                }
            };

            // Act / Assert
            await subject.Should().BeEquivalentToAsync(expected,
                options => options
                    .For(x => x.Level.Collection)
                    .Exclude(x => x.Number));
        }

        [Fact]
        public async Task When_property_in_collection_is_excluded_it_should_not_throw_if_root_is_a_collection()
        {
            // Arrange
            var subject = new
            {
                Level = new
                {
                    Collection = new[]
                    {
                        new
                        {
                            Number = 1,
                            Text = "Text"
                        },
                        new
                        {
                            Number = 2,
                            Text = "Actual"
                        }
                    }
                }
            };

            var expected = new
            {
                Level = new
                {
                    Collection = new[]
                    {
                        new
                        {
                            Number = 1,
                            Text = "Text"
                        },
                        new
                        {
                            Number = 3,
                            Text = "Actual"
                        }
                    }
                }
            };

            // Act / Assert
            await new[] { subject }.Should().BeEquivalentToAsync(new[] { expected },
                options => options
                    .For(x => x.Level.Collection)
                    .Exclude(x => x.Number));
        }

        [Fact]
        public async Task When_object_with_async_enumerable_are_equivalent_it_should_not_throw()
        {
            // Arrange
            AsyncEnumerableDto dto = new()
            {
                Foo = AsyncEnumerable.Range(1, 2)
            };

            // Act / Assert
            await dto.Should().BeEquivalentToAsync(new AsyncEnumerableDto
            {
                Foo = Enumerable.Range(1, 2).ToAsyncEnumerable()
            });
        }

        [Fact]
        public async Task When_subject_and_expectation_of_async_enumerable_are_equivalent_it_should_not_throw()
        {
            // Arrange / Act / Assert
            await AsyncEnumerable.Range(1, 5)
                .Should().BeEquivalentToAsync(AsyncEnumerable.Range(1, 5));
        }

        [Fact]
        public async Task When_collection_in_collection_is_excluded_it_should_not_throw()
        {
            // Arrange
            var subject = new
            {
                Level = new
                {
                    Collection = new[]
                    {
                        new
                        {
                            Number = 1,
                            NextCollection = new[]
                            {
                                new
                                {
                                    Text = "Text"
                                }
                            }
                        },
                        new
                        {
                            Number = 2,
                            NextCollection = new[]
                            {
                                new
                                {
                                    Text = "Actual"
                                }
                            }
                        }
                    }
                }
            };

            var expected = new
            {
                Level = new
                {
                    Collection = new[]
                    {
                        new
                        {
                            Number = 1,
                            NextCollection = new[]
                            {
                                new
                                {
                                    Text = "Text"
                                }
                            }
                        },
                        new
                        {
                            Number = 2,
                            NextCollection = new[]
                            {
                                new
                                {
                                    Text = "Expected"
                                }
                            }
                        }
                    }
                }
            };

            // Act / Assert
            await subject.Should().BeEquivalentToAsync(expected,
                options => options
                    .For(x => x.Level.Collection)
                    .Exclude(x => x.NextCollection));
        }

        [Fact]
        public async Task When_property_in_collection_in_collection_is_excluded_it_should_not_throw()
        {
            // Arrange
            var subject = new
            {
                Text = "Text",
                Level = new
                {
                    Collection = new[]
                    {
                        new
                        {
                            Number = 1,
                            NextCollection = new[]
                            {
                                new
                                {
                                    Text = "Text"
                                }
                            }
                        },
                        new
                        {
                            Number = 2,
                            NextCollection = new[]
                            {
                                new
                                {
                                    Text = "Actual"
                                }
                            }
                        }
                    }
                }
            };

            var expected = new
            {
                Text = "Text",
                Level = new
                {
                    Collection = new[]
                    {
                        new
                        {
                            Number = 1,
                            NextCollection = new[]
                            {
                                new
                                {
                                    Text = "Text"
                                }
                            }
                        },
                        new
                        {
                            Number = 2,
                            NextCollection = new[]
                            {
                                new
                                {
                                    Text = "Expected"
                                }
                            }
                        }
                    }
                }
            };

            // Act / Assert
            await subject.Should().BeEquivalentToAsync(expected,
                options => options
                    .For(x => x.Level.Collection)
                    .For(x => x.NextCollection)
                    .Exclude(x => x.Text)
            );
        }

        [Fact]
        public async Task When_property_in_object_in_collection_is_excluded_it_should_not_throw()
        {
            // Arrange
            var subject = new
            {
                Collection = new[]
                {
                    new
                    {
                        Level = new
                        {
                            Text = "Actual"
                        }
                    }
                }
            };

            var expected = new
            {
                Collection = new[]
                {
                    new
                    {
                        Level = new
                        {
                            Text = "Expected"
                        }
                    }
                }
            };

            // Act / Assert
            await subject.Should().BeEquivalentToAsync(expected,
                options => options
                    .For(x => x.Collection)
                    .Exclude(x => x.Level.Text)
            );
        }

        [Fact]
        public async Task When_property_in_object_in_collection_in_object_in_collection_is_excluded_it_should_not_throw()
        {
            // Arrange
            var subject = new
            {
                Collection = new[]
                {
                    new
                    {
                        Level = new
                        {
                            Collection = new[]
                            {
                                new
                                {
                                    Level = new
                                    {
                                        Text = "Actual"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var expected = new
            {
                Collection = new[]
                {
                    new
                    {
                        Level = new
                        {
                            Collection = new[]
                            {
                                new
                                {
                                    Level = new
                                    {
                                        Text = "Expected"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act / Assert
            await subject.Should().BeEquivalentToAsync(expected,
                options => options
                    .For(x => x.Collection)
                    .For(x => x.Level.Collection)
                    .Exclude(x => x.Level)
            );
        }

        [Fact]
        public async Task A_nested_exclusion_can_be_followed_by_a_root_level_exclusion()
        {
            // Arrange
            var subject = new
            {
                Text = "Actual",
                Level = new
                {
                    Collection = new[]
                    {
                        new
                        {
                            Number = 1,
                            Text = "Text"
                        },
                        new
                        {
                            Number = 2,
                            Text = "Actual"
                        }
                    }
                }
            };

            var expected = new
            {
                Text = "Expected",
                Level = new
                {
                    Collection = new[]
                    {
                        new
                        {
                            Number = 1,
                            Text = "Text"
                        },
                        new
                        {
                            Number = 2,
                            Text = "Expected"
                        }
                    }
                }
            };

            // Act / Assert
            await subject.Should().BeEquivalentToAsync(expected,
                options => options
                    .For(x => x.Level.Collection).Exclude(x => x.Text)
                    .Excluding(x => x.Text));
        }

        [Fact]
        public async Task A_nested_exclusion_can_be_preceded_by_a_root_level_exclusion()
        {
            // Arrange
            var subject = new
            {
                Text = "Actual",
                Level = new
                {
                    Collection = new[]
                    {
                        new
                        {
                            Number = 1,
                            Text = "Text"
                        },
                        new
                        {
                            Number = 2,
                            Text = "Actual"
                        }
                    }
                }
            };

            var expected = new
            {
                Text = "Expected",
                Level = new
                {
                    Collection = new[]
                    {
                        new
                        {
                            Number = 1,
                            Text = "Text"
                        },
                        new
                        {
                            Number = 2,
                            Text = "Expected"
                        }
                    }
                }
            };

            // Act / Assert
            await subject.Should().BeEquivalentToAsync(expected,
                options => options
                    .Excluding(x => x.Text)
                    .For(x => x.Level.Collection).Exclude(x => x.Text));
        }

        [Fact]
        public async Task A_nested_exclusion_can_be_followed_by_a_nested_exclusion()
        {
            // Arrange
            var subject = new
            {
                Text = "Actual",
                Level = new
                {
                    Collection = new[]
                    {
                        new
                        {
                            Number = 1,
                            Text = "Text"
                        },
                        new
                        {
                            Number = 2,
                            Text = "Actual"
                        }
                    }
                }
            };

            var expected = new
            {
                Text = "Actual",
                Level = new
                {
                    Collection = new[]
                    {
                        new
                        {
                            Number = 1,
                            Text = "Text"
                        },
                        new
                        {
                            Number = 3,
                            Text = "Expected"
                        }
                    }
                }
            };

            // Act / Assert
            await subject.Should().BeEquivalentToAsync(expected,
                options => options
                    .For(x => x.Level.Collection).Exclude(x => x.Text)
                    .For(x => x.Level.Collection).Exclude(x => x.Number));
        }
    }

    [Fact]
    public async Task When_a_dictionary_property_is_detected_it_should_ignore_the_order_of_the_pairs()
    {
        // Arrange
        var expected = new { Customers = new Dictionary<string, string> { ["Key2"] = "Value2", ["Key1"] = "Value1" } };

        var subject = new { Customers = new Dictionary<string, string> { ["Key1"] = "Value1", ["Key2"] = "Value2" } };

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_injecting_a_null_config_it_should_throw()
    {
        // Arrange
        IEnumerable<string> collection1 = new EnumerableOfStringAndObject();
        IEnumerable<string> collection2 = new EnumerableOfStringAndObject();

        // Act
        Func<Task> act = async () => await collection1.Should().BeEquivalentToAsync(collection2, config: null);

        // Assert
        await act.Should().ThrowExactlyAsync<ArgumentNullException>()
            .WithParameterName("config");
    }

    [Fact]
    public async Task
        When_a_object_implements_multiple_IEnumerable_interfaces_but_the_declared_type_is_assignable_to_only_one_and_runtime_checking_is_configured_it_should_fail()
    {
        // Arrange
        IEnumerable<string> collection1 = new EnumerableOfStringAndObject();
        IEnumerable<string> collection2 = new EnumerableOfStringAndObject();

        // Act
        Func<Task> act = async
            () => await collection1.Should().BeEquivalentToAsync(collection2, opts => opts.RespectingRuntimeTypes());

        // Assert
        await act.Should().ThrowAsync<XunitException>("the runtime type is assignable to two IEnumerable interfaces")
            .WithMessage("*cannot determine which one*");
    }

    [Fact]
    public async Task When_a_specific_property_is_included_it_should_ignore_the_rest_of_the_properties()
    {
        // Arrange
        var result = new[] { new { A = "aaa", B = "bbb" } };

        var expected = new { A = "aaa", B = "ccc" };

        // Act
        Func<Task> act = () => result.Should().BeEquivalentToAsync(new[] { expected }, options => options.Including(x => x.A));

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task
        When_a_strongly_typed_collection_is_declared_as_an_untyped_collection_and_runtime_checking_is_configured_is_should_use_the_runtime_type()
    {
        // Arrange
        ICollection collection1 = new List<Car> { new() };
        ICollection collection2 = new List<Customer> { new() };

        // Act
        Func<Task> act =
            () => collection1.Should().BeEquivalentToAsync(collection2, opts => opts.RespectingRuntimeTypes());

        // Assert
        await act.Should().ThrowAsync<XunitException>("the items have different runtime types");
    }

    [Fact]
    public async Task When_all_strings_in_the_collection_are_equal_to_the_expected_string_it_should_succeed()
    {
        // Arrange
        var subject = new List<string> { "one", "one", "one" };

        // Act
        Func<Task> action = () => subject.Should().AllBeAsync("one");

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_all_strings_in_the_collection_are_equal_to_the_expected_string_it_should_allow_chaining()
    {
        // Arrange
        var subject = new List<string> { "one", "one", "one" };

        // Act
        Func<Task> action = async () => (await subject.Should().AllBeAsync("one")).And.HaveCount(3);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_some_string_in_the_collection_is_not_equal_to_the_expected_string_it_should_throw()
    {
        // Arrange
        var subject = new[] { "one", "two", "six" };

        // Act
        Func<Task> action = () => subject.Should().AllBeAsync("one");

        // Assert
        await action.Should().ThrowAsync<XunitException>().WithMessage(
            "Expected subject[1]*to be \"one\", but \"two\" differs near \"two\" (index 0).*" +
            "Expected subject[2]*to be \"one\", but \"six\" differs near \"six\" (index 0).*");
    }

    [Fact]
    public async Task When_some_string_in_the_collection_is_in_different_case_than_expected_string_it_should_throw()
    {
        // Arrange
        var subject = new[] { "one", "One", "ONE" };

        // Act
        Func<Task> action = () => subject.Should().AllBeAsync("one");

        // Assert
        await action.Should().ThrowAsync<XunitException>().WithMessage(
            "Expected subject[1]*to be \"one\", but \"One\" differs near \"One\" (index 0).*" +
            "Expected subject[2]*to be \"one\", but \"ONE\" differs near \"ONE\" (index 0).*");
    }

    [Fact]
    public async Task When_more_than_10_strings_in_the_collection_are_not_equal_to_expected_string_only_10_are_reported()
    {
        // Arrange
        var subject = Enumerable.Repeat("two", 11);

        // Act
        Func<Task> action = () => subject.Should().AllBeAsync("one");

        // Assert
        (await action.Should().ThrowAsync<XunitException>()).Which
            .Message.Should().Contain("subject[9] to be \"one\", but \"two\" differs near \"two\" (index 0)")
            .And.NotContain("subject[10]");
    }

    [Fact]
    public void
        When_some_strings_in_the_collection_are_not_equal_to_expected_string_for_huge_table_execution_time_should_still_be_short()
    {
        // Arrange
        const int N = 100000;
        var subject = new List<string>(N) { "one" };

        for (int i = 1; i < N; i++)
        {
            subject.Add("two");
        }

        // Act
        Func<Task> action = async () =>
        {
            try
            {
                await subject.Should().AllBeAsync("one");
            }
            catch
            {
                // ignored, we only care about execution time
            }
        };

        // Assert
        action.ExecutionTime().Should().BeLessThan(1.Seconds());
    }

    [Fact]
    public async Task When_all_subject_items_are_equivalent_to_expectation_object_it_should_succeed()
    {
        // Arrange
        var subject = new List<SomeDto>
        {
            new() { Name = "someDto", Age = 1 },
            new() { Name = "someDto", Age = 1 },
            new() { Name = "someDto", Age = 1 }
        };

        // Act
        Func<Task> action = () => subject.Should().AllBeEquivalentToAsync(new
        {
            Name = "someDto",
            Age = 1,
            Birthdate = default(DateTime)
        });

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_all_subject_items_are_equivalent_to_expectation_object_it_should_allow_chaining()
    {
        // Arrange
        var subject = new List<SomeDto>
        {
            new() { Name = "someDto", Age = 1 },
            new() { Name = "someDto", Age = 1 },
            new() { Name = "someDto", Age = 1 }
        };

        // Act
        Func<Task> action = async () =>
        {
            var expectation = new
            {
                Name = "someDto",
                Age = 1,
                Birthdate = default(DateTime)
            };

            (await subject.Should().AllBeEquivalentToAsync(expectation)).And.HaveCount(3);
        };

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_some_subject_items_are_not_equivalent_to_expectation_object_it_should_throw()
    {
        // Arrange
        var subject = new[] { 1, 2, 3 };

        // Act
        Func<Task> action = () => subject.Should().AllBeEquivalentToAsync(1);

        // Assert
        await action.Should().ThrowAsync<XunitException>().WithMessage(
            "Expected subject[1]*to be 1, but found 2.*Expected subject[2]*to be 1, but found 3*");
    }

    [Fact]
    public async Task When_more_than_10_subjects_items_are_not_equivalent_to_expectation_only_10_are_reported()
    {
        // Arrange
        var subject = Enumerable.Repeat(2, 11);

        // Act
        Func<Task> action = () => subject.Should().AllBeEquivalentToAsync(1);

        // Assert
        (await action.Should().ThrowAsync<XunitException>()).Which
            .Message.Should().Contain("subject[9] to be 1, but found 2")
            .And.NotContain("item[10]");
    }

    [Fact]
    public void
        When_some_subject_items_are_not_equivalent_to_expectation_for_huge_table_execution_time_should_still_be_short()
    {
        // Arrange
        const int N = 100000;
        var subject = new List<int>(N) { 1 };

        for (int i = 1; i < N; i++)
        {
            subject.Add(2);
        }

        // Act
        Func<Task> action = async () =>
        {
            try
            {
                await subject.Should().AllBeEquivalentToAsync(1);
            }
            catch
            {
                // ignored, we only care about execution time
            }
        };

        // Assert
        action.ExecutionTime().Should().BeLessThan(1.Seconds());
    }

    [Fact]
    public async Task
        When_an_object_implements_multiple_IEnumerable_interfaces_but_the_declared_type_is_assignable_to_only_one_it_should_respect_the_declared_type()
    {
        // Arrange
        IEnumerable<string> collection1 = new EnumerableOfStringAndObject();
        IEnumerable<string> collection2 = new EnumerableOfStringAndObject();

        // Act
        Func<Task> act = () => collection1.Should().BeEquivalentToAsync(collection2);

        // Assert
        await act.Should().NotThrowAsync("the declared type is assignable to only one IEnumerable interface");
    }

    [Fact]
    public async Task When_an_unordered_collection_must_be_strict_using_a_predicate_it_should_throw()
    {
        // Arrange
        var subject = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 1, 2, 3, 4, 5 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        var expectation = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 5, 4, 3, 2, 1 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        // Act
        Func<Task> action = async () => await subject.Should().BeEquivalentToAsync(expectation, options =>
            options.WithStrictOrderingFor(s => s.Path.Contains("UnorderedCollection")));

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("*Expected*[0].UnorderedCollection*5 item(s)*empty collection*");
    }

    [Fact]
    public async Task
        When_an_unordered_collection_must_be_strict_using_a_predicate_and_order_was_reset_to_not_strict_it_should_not_throw()
    {
        // Arrange
        var subject = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 1, 2, 3, 4, 5 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        var expectation = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 5, 4, 3, 2, 1 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        // Act
        Func<Task> action = async () => await subject.Should().BeEquivalentToAsync(expectation, options =>
            options
                .WithStrictOrderingFor(s => s.Path.Contains("UnorderedCollection"))
                .WithoutStrictOrdering());

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_an_unordered_collection_must_be_strict_using_an_expression_it_should_throw()
    {
        // Arrange
        var subject = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 1, 2, 3, 4, 5 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        var expectation = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 5, 4, 3, 2, 1 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        // Act
        Func<Task> action = async () =>
                await subject.Should().BeEquivalentToAsync(expectation,
                    options => options
                        .WithStrictOrderingFor(
                            s => s.UnorderedCollection));

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "*Expected*[0].UnorderedCollection*5 item(s)*empty collection*");
    }

    [Fact]
    public async Task Can_force_strict_ordering_based_on_the_parent_type_of_an_unordered_collection()
    {
        // Arrange
        var subject = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 1, 2, 3, 4, 5 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        var expectation = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 5, 4, 3, 2, 1 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        // Act
        Func<Task> action = async () => await subject.Should().BeEquivalentToAsync(expectation, options => options
            .WithStrictOrderingFor(oi => oi.ParentType == expectation[0].GetType()));

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "*Expected*[0].UnorderedCollection*5 item(s)*empty collection*");
    }

    [Fact]
    public async Task
        When_an_unordered_collection_must_be_strict_using_an_expression_and_order_is_reset_to_not_strict_it_should_not_throw()
    {
        // Arrange
        var subject = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 1, 2, 3, 4, 5 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        var expectation = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 5, 4, 3, 2, 1 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        // Act
        Func<Task> action = async () =>
                await subject.Should().BeEquivalentToAsync(expectation,
                    options => options
                        .WithStrictOrderingFor(s => s.UnorderedCollection)
                        .WithoutStrictOrdering());

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_an_unordered_collection_must_not_be_strict_using_a_predicate_it_should_not_throw()
    {
        // Arrange
        var subject = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 1, 2, 3, 4, 5 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        var expectation = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 5, 4, 3, 2, 1 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        // Act
        Func<Task> action = async () => await subject.Should().BeEquivalentToAsync(expectation, options => options
            .WithStrictOrdering()
            .WithoutStrictOrderingFor(s => s.Path.Contains("UnorderedCollection")));

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_an_unordered_collection_must_not_be_strict_using_an_expression_it_should_not_throw()
    {
        // Arrange
        var subject = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 1, 2, 3, 4, 5 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        var expectation = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 5, 4, 3, 2, 1 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        // Act
        Func<Task> action = async () => await subject.Should().BeEquivalentToAsync(expectation, options => options
            .WithStrictOrdering()
            .WithoutStrictOrderingFor(x => x.UnorderedCollection));

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task
        When_an_unordered_collection_must_not_be_strict_using_a_predicate_and_order_was_reset_to_strict_it_should_throw()
    {
        // Arrange
        var subject = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 1, 2 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        var expectation = new[]
        {
            new { Name = "John", UnorderedCollection = new[] { 2, 1 } },
            new { Name = "Jane", UnorderedCollection = new int[0] }
        };

        // Act
        Func<Task> action = async () => await subject.Should().BeEquivalentToAsync(expectation, options => options
            .WithStrictOrdering()
            .WithoutStrictOrderingFor(s => s.Path.Contains("UnorderedCollection"))
            .WithStrictOrdering());

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "*Expected*subject[0].UnorderedCollection[0]*to be 2, but found 1.*Expected subject[0].UnorderedCollection[1]*to be 1, but found 2*");
    }

    [Fact]
    public async Task When_an_unordered_collection_must_not_be_strict_using_an_expression_and_collection_is_not_equal_it_should_throw()
    {
        // Arrange
        var subject = new
        {
            UnorderedCollection = new[] { 1 }
        };

        var expectation = new
        {
            UnorderedCollection = new[] { 2 }
        };

        // Act
        Func<Task> action = async () => await subject.Should().BeEquivalentToAsync(expectation, options => options
            .WithStrictOrdering()
            .WithoutStrictOrderingFor(x => x.UnorderedCollection));

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("*not strict*");
    }

    [Fact]
    public async Task
        When_asserting_equivalence_of_collections_and_configured_to_use_runtime_properties_it_should_respect_the_runtime_type()
    {
        // Arrange
        ICollection collection1 = new NonGenericCollection(new[] { new Customer() });
        ICollection collection2 = new NonGenericCollection(new[] { new Car() });

        // Act
        Func<Task> act =
            () =>
                collection1.Should().BeEquivalentToAsync(collection2,
                    opts => opts.RespectingRuntimeTypes());

        // Assert
        await act.Should().ThrowAsync<XunitException>("the types have different properties");
    }

    [Fact]
    public async Task When_asserting_equivalence_of_generic_collections_it_should_respect_the_declared_type()
    {
        // Arrange
        var collection1 = new Collection<CustomerType> { new DerivedCustomerType("123") };
        var collection2 = new Collection<CustomerType> { new("123") };

        // Act
        Func<Task> act = () => collection1.Should().BeEquivalentToAsync(collection2);

        // Assert
        await act.Should().NotThrowAsync("the objects are equivalent according to the members on the declared type");
    }

    [Fact]
    public async Task When_asserting_equivalence_of_non_generic_collections_it_should_respect_the_runtime_type()
    {
        // Arrange
        ICollection subject = new NonGenericCollection(new[] { new Customer() });
        ICollection expectation = new NonGenericCollection(new[] { new Car() });

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*Wheels*not have*VehicleId*not have*");
    }

    [Fact]
    public async Task When_custom_assertion_rules_are_utilized_the_rules_should_be_respected()
    {
        // Arrange
        var subject = new[]
        {
            new { Value = new Customer { Name = "John", Age = 31, Id = 1 } },
            new { Value = new Customer { Name = "Jane", Age = 24, Id = 2 } }
        };

        var expectation = new[]
        {
            new { Value = new CustomerDto { Name = "John", Age = 30 } },
            new { Value = new CustomerDto { Name = "Jane", Age = 24 } }
        };

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(expectation, opts => opts
            .Using<int>(ctx => ctx.Subject.Should().BeInRange(ctx.Expectation - 1, ctx.Expectation + 1))
            .WhenTypeIs<int>()
        );

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_expectation_is_null_enumerable_it_should_throw()
    {
        // Arrange
        var subject = Enumerable.Empty<object>();

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync((IEnumerable<object>)null);

        // Assert
        await act.Should().ThrowAsync<XunitException>().WithMessage(
            "Expected subject*to be <null>, but found*");
    }

    [Fact]
    public async Task When_nested_objects_are_excluded_from_collections_it_should_use_simple_equality_semantics()
    {
        // Arrange
        var actual = new MyObject
        {
            MyString = "identical string",
            Child = new ClassIdentifiedById { Id = 1, MyChildString = "identical string" }
        };

        var expectation = new MyObject
        {
            MyString = "identical string",
            Child = new ClassIdentifiedById { Id = 1, MyChildString = "DIFFERENT STRING" }
        };

        IList<MyObject> actualList = new List<MyObject> { actual };
        IList<MyObject> expectationList = new List<MyObject> { expectation };

        // Act
        Func<Task> act = () => actualList.Should().BeEquivalentToAsync(expectationList, opt => opt.ExcludingNestedObjects());

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_no_collection_item_matches_it_should_report_the_closest_match()
    {
        // Arrange
        var subject = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "Jane", Age = 30, Id = 2 }
        };

        var expectation = new List<Customer>
        {
            new() { Name = "Jane", Age = 30, Id = 2 },
            new() { Name = "John", Age = 28, Id = 1 }
        };

        // Act
        Func<Task> action = async
            () => await subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected*[1].Age*28*27*");
    }

    [Fact]
    public async Task When_only_a_deeply_nested_property_is_included_it_should_exclude_the_other_properties()
    {
        // Arrange
        var actualObjects = new[]
        {
            new { SubObject = new { Property1 = "John", Property2 = "John" } },
            new { SubObject = new { Property1 = "John", Property2 = "John" } }
        };

        var expectedObjects = new[]
        {
            new { SubObject = new { Property1 = "John", Property2 = "John" } },
            new { SubObject = new { Property1 = "John", Property2 = "Jane" } }
        };

        // Act
        Func<Task> act = () => actualObjects.Should().BeEquivalentToAsync(expectedObjects, options =>
            options.Including(order => order.SubObject.Property1));

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_selection_rules_are_configured_they_should_be_evaluated_from_last_to_first()
    {
        // Arrange
        var list1 = new[] { new { Value = 3 } };
        var list2 = new[] { new { Value = 2 } };

        // Act
        Func<Task> act = () => list1.Should().BeEquivalentToAsync(list2, config =>
        {
            config.WithoutSelectionRules();
            config.Using(new SelectNoMembersSelectionRule());
            config.Using(new SelectPropertiesSelectionRule());
            return config;
        });

        // Assert
        await act.Should().ThrowAsync<XunitException>().WithMessage("*to be 2, but found 3*");
    }

    [Fact]
    public async Task When_injecting_a_null_config_to_generic_overload_it_should_throw()
    {
        // Arrange
        var list1 = new[] { new { Value = 3 } };
        var list2 = new[] { new { Value = 2 } };

        // Act
        Func<Task> act = () => list1.Should().BeEquivalentToAsync(list2, config: null);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("config");
    }

    [Fact]
    public async Task When_subject_and_expectation_are_null_enumerable_it_should_succeed()
    {
        // Arrange
        IEnumerable<object> subject = null;

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync((IEnumerable<int>)null);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_string_collection_subject_is_empty_and_expectation_is_object_succeed()
    {
        // Arrange
        var subject = new List<string>();

        // Act
        Func<Task> action = () => subject.Should().AllBeAsync("one");

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_injecting_a_null_config_to_AllBe_for_string_collection_it_should_throw()
    {
        // Arrange
        var subject = new List<string>();

        // Act
        Func<Task> action = () => subject.Should().AllBeAsync("one", config: null);

        // Assert
        await action.Should().ThrowExactlyAsync<ArgumentNullException>()
            .WithParameterName("config");
    }

    [Fact]
    public async Task When_all_string_subject_items_are_equal_to_expectation_object_with_a_config_it_should_succeed()
    {
        // Arrange
        var subject = new List<string> { "one", "one" };

        // Act
        Func<Task> action = () => subject.Should().AllBeAsync("one", opt => opt);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_not_all_string_subject_items_are_equal_to_expectation_object_with_a_config_it_should_fail()
    {
        // Arrange
        var subject = new List<string> { "one", "two" };

        // Act
        Func<Task> action = () => subject.Should().AllBeAsync("one", opt => opt, "we want to test the failure {0}", "message");

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("*we want to test the failure message*");
    }

    [Fact]
    public async Task When_all_string_subject_items_are_equal_to_expectation_object_with_a_config_it_should_allow_chaining()
    {
        // Arrange
        var subject = new List<string> { "one", "one" };

        // Act
        Func<Task> action = async () => (await subject.Should().AllBeAsync("one", opt => opt))
            .And.HaveCount(2);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_subject_is_empty_and_expectation_is_object_succeed()
    {
        // Arrange
        var subject = new List<char>();

        // Act
        Func<Task> action = () => subject.Should().AllBeEquivalentToAsync('g');

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_injecting_a_null_config_to_AllBeEquivalentTo_it_should_throw()
    {
        // Arrange
        var subject = new List<char>();

        // Act
        Func<Task> action = () => subject.Should().AllBeEquivalentToAsync('g', config: null);

        // Assert
        await action.Should().ThrowExactlyAsync<ArgumentNullException>()
            .WithParameterName("config");
    }

    [Fact]
    public async Task When_all_subject_items_are_equivalent_to_expectation_object_with_a_config_it_should_succeed()
    {
        // Arrange
        var subject = new List<char> { 'g', 'g' };

        // Act
        Func<Task> action = () => subject.Should().AllBeEquivalentToAsync('g', opt => opt);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_not_all_subject_items_are_equivalent_to_expectation_object_with_a_config_it_should_fail()
    {
        // Arrange
        var subject = new List<char> { 'g', 'a' };

        // Act
        Func<Task> action = () =>
            subject.Should().AllBeEquivalentToAsync('g', opt => opt, "we want to test the failure {0}", "message");

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("*we want to test the failure message*");
    }

    [Fact]
    public async Task When_all_subject_items_are_equivalent_to_expectation_object_with_a_config_it_should_allow_chaining()
    {
        // Arrange
        var subject = new List<char> { 'g', 'g' };

        // Act
        Func<Task> action = async () => (await subject.Should().AllBeEquivalentToAsync('g', opt => opt))
            .And.HaveCount(2);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_subject_is_null_and_expectation_is_enumerable_it_should_throw()
    {
        // Arrange
        IEnumerable<object> subject = null;

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(Enumerable.Empty<object>());

        // Assert
        await act.Should().ThrowAsync<XunitException>().WithMessage(
            "Expected subject*not to be <null>*");
    }

    [Fact]
    public async Task When_the_expectation_is_null_it_should_throw()
    {
        // Arrange
        var actual = new[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync<object>(null);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected actual*to be <null>*{{1, 2, 3}, {4, 5, 6}}*");
    }

    [Fact]
    public async Task When_a_multi_dimensional_array_is_compared_to_null_it_should_throw()
    {
        // Arrange
        Array actual = null;

        var expectation = new[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Cannot compare a multi-dimensional array to <null>*");
    }

    [Fact]
    public async Task When_a_multi_dimensional_array_is_compared_to_a_non_array_it_should_throw()
    {
        // Arrange
        var actual = new object();

        var expectation = new[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Cannot compare a multi-dimensional array to something else*");
    }

    [Fact]
    public async Task When_the_length_of_the_2nd_dimension_differs_between_the_arrays_it_should_throw()
    {
        // Arrange
        var actual = new[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        var expectation = new[,] { { 1, 2, 3 } };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected dimension 0 to contain 1 item(s), but found 2*");
    }

    [Fact]
    public async Task When_the_length_of_the_first_dimension_differs_between_the_arrays_it_should_throw()
    {
        // Arrange
        var actual = new[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        var expectation = new[,]
        {
            { 1, 2 },
            { 4, 5 }
        };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected dimension 1 to contain 2 item(s), but found 3*");
    }

    [Fact]
    public async Task When_the_number_of_dimensions_of_the_arrays_are_not_the_same_it_should_throw()
    {
        // Arrange
#pragma warning disable format // VS and Rider disagree on how to format a multidimensional array initializer
        var actual = new[,,]
        {
            {
                { 1 },
                { 2 },
                { 3 }
            },
            {
                { 4 },
                { 5 },
                { 6 }
            }
        };
#pragma warning restore format

        var expectation = new[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected actual*2 dimension(s)*but it has 3*");
    }

    [Fact]
    public async Task When_the_other_dictionary_does_not_contain_enough_items_it_should_throw()
    {
        // Arrange
        var expected = new { Customers = new Dictionary<string, string> { ["Key1"] = "Value1", ["Key2"] = "Value2" } };

        var subject = new { Customers = new Dictionary<string, string> { ["Key1"] = "Value1" } };

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().ThrowAsync<XunitException>().WithMessage(
            "Expected*Customers*dictionary*2 item(s)*but*misses*Key2*");
    }

    [Fact]
    public async Task When_the_other_property_is_not_a_dictionary_it_should_throw()
    {
        // Arrange
        var expected = new { Customers = "I am a string" };

        var subject = new { Customers = new Dictionary<string, string> { ["Key2"] = "Value2", ["Key1"] = "Value1" } };

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*property*Customers*String*found*Dictionary*");
    }

    [Fact]
    public async Task
        When_the_root_object_is_referenced_from_an_object_in_a_nested_collection_it_should_treat_it_as_a_cyclic_reference()
    {
        // Arrange
        var company1 = new MyCompany { Name = "Company" };
        var user1 = new MyUser { Name = "User", Company = company1 };
        company1.Users = new List<MyUser> { user1 };
        var logo1 = new MyCompanyLogo { Url = "blank", Company = company1, CreatedBy = user1 };
        company1.Logo = logo1;

        var company2 = new MyCompany { Name = "Company" };
        var user2 = new MyUser { Name = "User", Company = company2 };
        company2.Users = new List<MyUser> { user2 };
        var logo2 = new MyCompanyLogo { Url = "blank", Company = company2, CreatedBy = user2 };
        company2.Logo = logo2;

        // Act
        Func<Task> action = () => company1.Should().BeEquivalentToAsync(company2, o => o.IgnoringCyclicReferences());

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_the_subject_contains_less_items_than_expected_it_should_throw()
    {
        // Arrange
        var subject = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 }
        };

        var expectation = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "Jane", Age = 24, Id = 2 }
        };

        // Act
        Func<Task> action = async
            () => await subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "*subject*to be a collection with 2 item(s), but*contains 1 item(s) less than*");
    }

    [Fact]
    public async Task When_the_subject_contains_more_items_than_expected_it_should_throw()
    {
        // Arrange
        var subject = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "Jane", Age = 24, Id = 2 }
        };

        var expectation = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 }
        };

        // Act
        Func<Task> action = async
            () => await subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected subject*to be a collection with 1 item(s), but*contains 1 item(s) more than*");
    }

    [Fact]
    public async Task When_the_subject_contains_same_number_of_items_and_both_contain_duplicates_it_should_succeed()
    {
        // Arrange
        var subject = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "Jane", Age = 24, Id = 2 }
        };

        var expectation = new List<Customer>
        {
            new() { Name = "Jane", Age = 24, Id = 2 },
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "John", Age = 27, Id = 1 }
        };

        // Act
        Func<Task> action = async
            () => await subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_the_subject_contains_same_number_of_items_but_expectation_contains_duplicates_it_should_throw()
    {
        // Arrange
        var subject = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "Jane", Age = 24, Id = 2 }
        };

        var expectation = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "John", Age = 27, Id = 1 }
        };

        // Act
        Func<Task> action = async
            () => await subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected property subject[1].Name*to be \"John\", but \"Jane\" differs near*");
    }

    [Fact]
    public async Task When_the_subject_contains_same_number_of_items_but_subject_contains_duplicates_it_should_throw()
    {
        // Arrange
        var subject = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "John", Age = 27, Id = 1 }
        };

        var expectation = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "Jane", Age = 24, Id = 2 }
        };

        // Act
        Func<Task> action = async
            () => await subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected property subject[1].Name*to be \"Jane\", but \"John\" differs near*");
    }

    [Fact]
    public async Task
        When_two_collections_have_nested_members_of_the_contained_equivalent_but_not_equal_it_should_not_throw()
    {
        // Arrange
        var list1 = new[] { new { Nested = new ClassWithOnlyAProperty { Value = 1 } } };

        var list2 = new[] { new { Nested = new { Value = 1 } } };

        // Act
        Func<Task> act = () => list1.Should().BeEquivalentToAsync(list2, opts => opts);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task
        When_two_collections_have_properties_of_the_contained_items_excluded_but_still_differ_it_should_throw()
    {
        // Arrange
        var list1 = new[] { new KeyValuePair<int, int>(1, 123) };
        var list2 = new[] { new KeyValuePair<int, int>(2, 321) };

        // Act
        Func<Task> act = () => list1.Should().BeEquivalentToAsync(list2, config => config
            .Excluding(ctx => ctx.Key)
            .ComparingByMembers<KeyValuePair<int, int>>());

        // Assert
        await act.Should().ThrowAsync<XunitException>().WithMessage("Expected property list1[0].Value*to be 321, but found 123.*");
    }

    [Fact]
    public async Task
        When_two_equivalent_dictionaries_are_compared_directly_as_if_it_is_a_collection_it_should_succeed()
    {
        // Arrange
        var result = new Dictionary<string, int?> { ["C"] = null, ["B"] = 0, ["A"] = 0 };

        // Act
        Func<Task> act = () => result.Should().BeEquivalentToAsync(new Dictionary<string, int?>
        {
            ["A"] = 0,
            ["B"] = 0,
            ["C"] = null
        });

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_two_equivalent_dictionaries_are_compared_directly_it_should_succeed()
    {
        // Arrange
        var result = new Dictionary<string, int> { ["C"] = 0, ["B"] = 0, ["A"] = 0 };

        // Act
        Func<Task> act = () => result.Should().BeEquivalentToAsync(new Dictionary<string, int> { ["A"] = 0, ["B"] = 0, ["C"] = 0 });

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_two_lists_dont_contain_the_same_structural_equal_objects_it_should_throw()
    {
        // Arrange
        var subject = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "Jane", Age = 24, Id = 2 }
        };

        var expectation = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "Jane", Age = 30, Id = 2 }
        };

        // Act
        Func<Task> action = async
            () => await subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected*subject[1].Age*30*24*");
    }

    [Fact]
    public async Task When_two_lists_only_differ_in_excluded_properties_it_should_not_throw()
    {
        // Arrange
        var subject = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "Jane", Age = 24, Id = 2 }
        };

        var expectation = new List<CustomerDto>
        {
            new() { Name = "John", Age = 27 },
            new() { Name = "Jane", Age = 30 }
        };

        // Act
        Func<Task> action = async () =>
                await subject.Should().BeEquivalentToAsync(expectation,
                    options => options
                        .ExcludingMissingMembers()
                        .Excluding(c => c.Age));

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_two_multi_dimensional_arrays_are_equivalent_it_should_not_throw()
    {
        // Arrange
        var subject = new[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        var expectation = new[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        // Act
        Func<Task> act = async () => await subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_two_multi_dimensional_arrays_are_not_equivalent_it_should_throw()
    {
        // Arrange
        var actual = new[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        var expectation = new[,]
        {
            { 1, 2, 4 },
            { 4, -5, 6 }
        };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*actual[0,2]*4*3*actual[1,1]*-5*5*");
    }

    [Fact]
    public async Task When_two_multi_dimensional_arrays_have_empty_dimensions_they_should_be_equivalent()
    {
        // Arrange
        Array actual = new long[,] { { } };

        Array expectation = new long[,] { { } };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_two_multi_dimensional_arrays_are_empty_they_should_be_equivalent()
    {
        // Arrange
        Array actual = new long[0, 1] { };

        Array expectation = new long[0, 1] { };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_two_nested_dictionaries_contain_null_values_it_should_not_crash()
    {
        // Arrange
        var projection = new { ReferencedEquipment = new Dictionary<int, string> { [1] = null } };

        var persistedProjection = new { ReferencedEquipment = new Dictionary<int, string> { [1] = null } };

        // Act
        Func<Task> act = () => persistedProjection.Should().BeEquivalentToAsync(projection);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_two_nested_dictionaries_contain_null_values_it_should_not_crash2()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var actual = new UserRolesLookupElement();
        actual.Add(userId, "Admin", "Special");

        var expected = new UserRolesLookupElement();
        expected.Add(userId, "Admin", "Other");

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected*Roles[*][1]*Other*Special*");
    }

    [Fact]
    public async Task When_two_nested_dictionaries_do_not_match_it_should_throw()
    {
        // Arrange
        var projection = new { ReferencedEquipment = new Dictionary<int, string> { [1] = "Bla1" } };

        var persistedProjection = new { ReferencedEquipment = new Dictionary<int, string> { [1] = "Bla2" } };

        // Act
        Func<Task> act = () => persistedProjection.Should().BeEquivalentToAsync(projection);

        // Assert
        await act.Should().ThrowAsync<XunitException>().WithMessage(
            "Expected*ReferencedEquipment[1]*Bla1*Bla2*2*index 3*");
    }

    [Fact]
    public async Task When_two_ordered_lists_are_structurally_equivalent_it_should_succeed()
    {
        // Arrange
        var subject = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "Jane", Age = 24, Id = 2 }
        };

        var expectation = new List<Customer>
        {
            new() { Name = "John", Age = 27, Id = 1 },
            new() { Name = "Jane", Age = 24, Id = 2 }
        };

        // Act
        Func<Task> action = async
            () => await subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_two_unordered_lists_are_structurally_equivalent_and_order_is_strict_it_should_fail()
    {
        // Arrange
        var subject = new[]
        {
            new Customer { Name = "John", Age = 27, Id = 1 }, new Customer { Name = "Jane", Age = 24, Id = 2 }
        };

        var expectation = new Collection<Customer>
        {
            new() { Name = "Jane", Age = 24, Id = 2 },
            new() { Name = "John", Age = 27, Id = 1 }
        };

        // Act
        Func<Task> action = async () => await subject.Should().BeEquivalentToAsync(expectation, options => options.WithStrictOrdering());

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected property subject[0].Name*Jane*John*subject[1].Name*John*Jane*");
    }

    [Fact]
    public async Task When_two_unordered_lists_are_structurally_equivalent_and_order_was_reset_to_strict_it_should_fail()
    {
        // Arrange
        var subject = new[]
        {
            new Customer { Name = "John", Age = 27, Id = 1 }, new Customer { Name = "Jane", Age = 24, Id = 2 }
        };

        var expectation = new Collection<Customer>
        {
            new() { Name = "Jane", Age = 24, Id = 2 },
            new() { Name = "John", Age = 27, Id = 1 }
        };

        // Act
        Func<Task> action = async () => await subject.Should().BeEquivalentToAsync(
            expectation,
            options => options
                .WithStrictOrdering()
                .WithoutStrictOrdering()
                .WithStrictOrdering());

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected property subject[0].Name*Jane*John*subject[1].Name*John*Jane*");
    }

    [Fact]
    public async Task When_two_unordered_lists_are_structurally_equivalent_and_order_was_reset_to_not_strict_it_should_succeed()
    {
        // Arrange
        var subject = new[]
        {
            new Customer { Name = "John", Age = 27, Id = 1 }, new Customer { Name = "Jane", Age = 24, Id = 2 }
        };

        var expectation = new Collection<Customer>
        {
            new() { Name = "Jane", Age = 24, Id = 2 },
            new() { Name = "John", Age = 27, Id = 1 }
        };

        // Act
        Func<Task> action = async
            () => await subject.Should().BeEquivalentToAsync(expectation, x => x.WithStrictOrdering().WithoutStrictOrdering());

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_two_unordered_lists_are_structurally_equivalent_it_should_succeed()
    {
        // Arrange
        var subject = new[]
        {
            new Customer { Name = "John", Age = 27, Id = 1 }, new Customer { Name = "Jane", Age = 24, Id = 2 }
        };

        var expectation = new Collection<Customer>
        {
            new() { Name = "Jane", Age = 24, Id = 2 },
            new() { Name = "John", Age = 27, Id = 1 }
        };

        // Act
        Func<Task> action = async
            () => await subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_two_unordered_lists_contain_empty_different_objects_it_should_throw()
    {
        // Arrange
        var actual = new object[] { new() };
        var expected = new object[] { new() };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task When_two_unordered_lists_contain_null_in_subject_it_should_throw()
    {
        // Arrange
        var actual = new object[] { null };
        var expected = new object[] { new() };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().ThrowAsync<XunitException>();
    }

    [Fact]
    public async Task When_two_unordered_lists_contain_null_in_expectation_it_should_throw()
    {
        // Arrange
        var actual = new object[] { new() };
        var expected = new object[] { null };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().ThrowAsync<XunitException>();
    }

    [Theory]
    [MemberData(nameof(ArrayTestData))]
    public async Task When_two_unordered_lists_contain_empty_objects_they_should_still_be_structurally_equivalent<TActual,
        TExpected>(TActual[] actual, TExpected[] expected)
    {
        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_an_exception_is_thrown_during_data_access_the_stack_trace_contains_the_original_site()
    {
        // Arrange
        var genericCollectionA = new List<ExceptionThrowingClass> { new() };

        var genericCollectionB = new List<ExceptionThrowingClass> { new() };

        var expectedTargetSite = typeof(ExceptionThrowingClass)
            .GetProperty(nameof(ExceptionThrowingClass.ExceptionThrowingProperty)).GetMethod;

        // Act
        Func<Task> act = () => genericCollectionA.Should().BeEquivalentToAsync(genericCollectionB);

        // Assert
        (await act.Should().ThrowAsync<NotImplementedException>()).And.TargetSite.Should().Be(expectedTargetSite);
    }

    public static IEnumerable<object[]> ArrayTestData()
    {
        var arrays = new object[]
        {
            new int?[] { null, 1 }, new int?[] { 1, null }, new object[] { null, 1 }, new object[] { 1, null }
        };

        return
            from x in arrays
            from y in arrays
            select new[] { x, y };
    }

    [Fact]
    public void Comparing_lots_of_complex_objects_should_still_be_fast()
    {
        // Arrange
        ClassWithLotsOfProperties GetObject(int i)
        {
            return new ClassWithLotsOfProperties
            {
#pragma warning disable CA1305
                Id = i.ToString(),
                Value1 = i.ToString(),
                Value2 = i.ToString(),
                Value3 = i.ToString(),
                Value4 = i.ToString(),
                Value5 = i.ToString(),
                Value6 = i.ToString(),
                Value7 = i.ToString(),
                Value8 = i.ToString(),
                Value9 = i.ToString(),
                Value10 = i.ToString(),
                Value11 = i.ToString(),
                Value12 = i.ToString(),
#pragma warning restore CA1305
            };
        }

        var actual = new List<ClassWithLotsOfProperties>();
        var expectation = new List<ClassWithLotsOfProperties>();

        var maxAmount = 100;

        for (var i = 0; i < maxAmount; i++)
        {
            actual.Add(GetObject(i));
            expectation.Add(GetObject(maxAmount - 1 - i));
        }

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expectation);

        // Assert
        act.ExecutionTime().Should().BeLessThan(20.Seconds());
    }

    private class ClassWithLotsOfProperties
    {
        public string Id { get; set; }

        public string Value1 { get; set; }

        public string Value2 { get; set; }

        public string Value3 { get; set; }

        public string Value4 { get; set; }

        public string Value5 { get; set; }

        public string Value6 { get; set; }

        public string Value7 { get; set; }

        public string Value8 { get; set; }

        public string Value9 { get; set; }

        public string Value10 { get; set; }

        public string Value11 { get; set; }

        public string Value12 { get; set; }
    }

    private class LogbookEntryProjection
    {
        public virtual LogbookCode Logbook { get; set; }

        public virtual ICollection<LogbookRelation> LogbookRelations { get; set; }
    }

    private class LogbookRelation
    {
        public virtual LogbookCode Logbook { get; set; }
    }

    private class LogbookCode
    {
        public LogbookCode(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }

    private class AsyncEnumerableDto
    {
        public IAsyncEnumerable<int> Foo { get; init; }
    }
}
