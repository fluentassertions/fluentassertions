using System;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections
{
    /// <content>
    /// The [Not]ContainEquivalentOf specs.
    /// </content>
    public partial class CollectionAssertionSpecs
    {
        public class ContainEquivalentOf
        {
            [Fact]
            public void When_collection_contains_object_equal_of_another_it_should_succeed()
            {
                // Arrange
                var item = new Customer { Name = "John" };
                var collection = new[] { new Customer { Name = "Jane" }, item };

                // Act / Assert
                collection.Should().ContainEquivalentOf(item);
            }

            [Fact]
            public void When_collection_contains_object_equivalent_of_another_it_should_succeed()
            {
                // Arrange
                var collection = new[] { new Customer { Name = "Jane" }, new Customer { Name = "John" } };
                var item = new Customer { Name = "John" };

                // Act / Assert
                collection.Should().ContainEquivalentOf(item);
            }

            [Fact]
            public void When_character_collection_does_contain_equivalent_it_should_succeed()
            {
                // Arrange
                char[] collection = "abc123ab".ToCharArray();
                char item = 'c';

                // Act / Assert
                collection.Should().ContainEquivalentOf(item);
            }

            [Fact]
            public void When_string_collection_does_contain_same_string_with_other_case_it_should_throw()
            {
                // Arrange
                string[] collection = new[] { "a", "b", "c" };
                string item = "C";

                // Act
                Action act = () => collection.Should().ContainEquivalentOf(item);

                // Assert
                act.Should().Throw<XunitException>().WithMessage("Expected collection {\"a\", \"b\", \"c\"} to contain equivalent of \"C\".*");
            }

            [Fact]
            public void When_string_collection_does_contain_same_string_it_should_throw_with_a_useful_message()
            {
                // Arrange
                string[] collection = new[] { "a" };
                string item = "b";

                // Act
                Action act = () => collection.Should().ContainEquivalentOf(item, "because we want to test the failure {0}", "message");

                // Assert
                act.Should().Throw<XunitException>()
                    .WithMessage("*because we want to test the failure message*");
            }

            [Fact]
            public void When_collection_does_not_contain_object_equivalent_of_another_it_should_throw()
            {
                // Arrange
                var collection = new[] { 1, 2, 3 };
                int item = 4;

                // Act
                Action act = () => collection.Should().ContainEquivalentOf(item);

                // Act / Assert
                act.Should().Throw<XunitException>().WithMessage("Expected collection {1, 2, 3} to contain equivalent of 4.*");
            }

            [Fact]
            public void When_asserting_collection_to_contain_equivalent_but_collection_is_null_it_should_throw()
            {
                // Arrange
                int[] collection = null;
                int expectation = 1;

                // Act
                Action act = () =>
                {
                    using var _ = new AssertionScope();
                    collection.Should().ContainEquivalentOf(expectation, "because we want to test the behaviour with a null subject");
                };

                // Assert
                act.Should().Throw<XunitException>().WithMessage(
                    "Expected collection to contain equivalent of 1 because we want to test the behaviour with a null subject, but found <null>.");
            }

            [Fact]
            public void When_collection_contains_equivalent_null_object_it_should_succeed()
            {
                // Arrange
                var collection = new[] { 1, 2, 3, (int?)null };
                int? item = null;

                // Act
                Action act = () => collection.Should().ContainEquivalentOf(item);

                // Assert
                act.Should().NotThrow();
            }

            [Fact]
            public void When_collection_does_not_contain_equivalent_null_object_it_should_throw()
            {
                // Arrange
                var collection = new[] { 1, 2, 3 };
                int? item = null;

                // Act
                Action act = () => collection.Should().ContainEquivalentOf(item);

                // Assert
                act.Should().Throw<XunitException>().WithMessage("Expected collection {1, 2, 3} to contain equivalent of <null>.*");
            }

            [Fact]
            public void When_empty_collection_does_not_contain_equivalent_it_should_throw()
            {
                // Arrange
                var collection = new int[0];
                int item = 1;

                // Act
                Action act = () => collection.Should().ContainEquivalentOf(item);

                // Assert
                act.Should().Throw<XunitException>().WithMessage("Expected collection {empty} to contain equivalent of 1.*");
            }

            [Fact]
            public void When_collection_does_not_contain_equivalent_because_of_second_property_it_should_throw()
            {
                // Arrange
                var subject = new[]
                {
                    new Customer
                    {
                        Name = "John",
                        Age = 18
                    },
                    new Customer
                    {
                        Name = "Jane",
                        Age = 18
                    }
                };
                var item = new Customer { Name = "John", Age = 20 };

                // Act
                Action act = () => subject.Should().ContainEquivalentOf(item);

                // Assert
                act.Should().Throw<XunitException>();
            }

            [Fact]
            public void When_collection_does_contain_equivalent_by_including_single_property_it_should_not_throw()
            {
                // Arrange
                var collection = new[]
                {
                    new Customer
                    {
                        Name = "John",
                        Age = 18
                    },
                    new Customer
                    {
                        Name = "Jane",
                        Age = 18
                    }
                };
                var item = new Customer { Name = "John", Age = 20 };

                // Act / Assert
                collection.Should().ContainEquivalentOf(item, options => options.Including(x => x.Name));
            }

            [Fact]
            public void Tracing_should_be_included_in_the_assertion_output()
            {
                // Arrange
                var collection = new[]
                {
                    new Customer
                    {
                        Name = "John",
                        Age = 18
                    },
                    new Customer
                    {
                        Name = "Jane",
                        Age = 18
                    }
                };

                var item = new Customer { Name = "John", Age = 21 };

                // Act
                Action act = () => collection.Should().ContainEquivalentOf(item, options => options.WithTracing());

                // Assert
                act.Should().Throw<XunitException>().WithMessage("*Equivalency was proven*");
            }

            [Fact]
            public void When_injecting_a_null_config_to_ContainEquivalentOf_it_should_throw()
            {
                // Arrange
                int[] collection = null;
                object item = null;

                // Act
                Action act = () => collection.Should().ContainEquivalentOf(item, config: null);

                // Assert
                act.Should().ThrowExactly<ArgumentNullException>()
                    .WithParameterName("config");
            }

            [Fact]
            public void When_collection_contains_object_equivalent_of_boxed_object_it_should_succeed()
            {
                // Arrange
                var collection = new[] { 1, 2, 3 };
                object boxedValue = 2;

                // Act / Assert
                collection.Should().ContainEquivalentOf(boxedValue);
            }
        }

        public class NotContainEquivalentOf
        {
            [Fact]
            public void When_collection_contains_object_equal_to_another_it_should_throw()
            {
                // Arrange
                var item = 1;
                var collection = new[] { 0, 1 };

                // Act
                Action act = () => collection.Should().NotContainEquivalentOf(item, "because we want to test the failure {0}", "message");

                // Assert
                act.Should().Throw<XunitException>().WithMessage("Expected collection {0, 1} not to contain*because we want to test the failure message, " +
                                                                 "but found one at index 1.*With configuration*");
            }

            [Fact]
            public void When_collection_contains_several_objects_equal_to_another_it_should_throw()
            {
                // Arrange
                var item = 1;
                var collection = new[] { 0, 1, 1 };

                // Act
                Action act = () => collection.Should().NotContainEquivalentOf(item, "because we want to test the failure {0}", "message");

                // Assert
                act.Should().Throw<XunitException>().WithMessage("Expected collection {0, 1, 1} not to contain*because we want to test the failure message, " +
                                                                 "but found several at indices {1, 2}.*With configuration*");
            }

            [Fact]
            public void When_asserting_collection_to_not_to_contain_equivalent_but_collection_is_null_it_should_throw()
            {
                // Arrange
                var item = 1;
                int[] collection = null;

                // Act
                Action act = () => collection.Should().NotContainEquivalentOf(item);

                // Assert
                act.Should().Throw<XunitException>().WithMessage("Expected collection*not to contain*but collection is <null>.");
            }

            [Fact]
            public void When_injecting_a_null_config_to_NotContainEquivalentOf_it_should_throw()
            {
                // Arrange
                int[] collection = null;
                object item = null;

                // Act
                Action act = () => collection.Should().NotContainEquivalentOf(item, config: null);

                // Assert
                act.Should().ThrowExactly<ArgumentNullException>()
                    .WithParameterName("config");
            }

            [Fact]
            public void When_asserting_empty_collection_to_not_contain_equivalent_it_should_succeed()
            {
                // Arrange
                var collection = new int[0];
                int item = 4;

                // Act / Assert
                collection.Should().NotContainEquivalentOf(item);
            }

            [Fact]
            public void When_asserting_a_null_collection_to_not_contain_equivalent_of__then_it_should_fail()
            {
                // Arrange
                int[] collection = null;

                // Act
                Action act = () =>
                {
                    using var _ = new AssertionScope();
                    collection.Should().NotContainEquivalentOf(1, config => config, "we want to test the failure {0}", "message");
                };

                // Assert
                act.Should().Throw<XunitException>()
                    .WithMessage("Expected collection not to contain *failure message*, but collection is <null>.");
            }

            [Fact]
            public void When_collection_does_not_contain_object_equivalent_of_unexpected_it_should_succeed()
            {
                // Arrange
                var collection = new[] { 1, 2, 3 };
                int item = 4;

                // Act / Assert
                collection.Should().NotContainEquivalentOf(item);
            }

            [Fact]
            public void When_asserting_collection_to_not_contain_equivalent_it_should_respect_config()
            {
                // Arrange
                var collection = new[]
                {
                    new Customer
                    {
                        Name = "John",
                        Age = 18
                    },
                    new Customer
                    {
                        Name = "Jane",
                        Age = 18
                    }
                };
                var item = new Customer { Name = "John", Age = 20 };

                // Act
                Action act = () => collection.Should().NotContainEquivalentOf(item, options => options.Excluding(x => x.Age));

                // Assert
                act.Should().Throw<XunitException>().WithMessage("*Exclude*Age*");
            }

            [Fact]
            public void When_asserting_collection_to_not_contain_equivalent_it_should_allow_combining_inside_assertion_scope()
            {
                // Arrange
                var collection = new[] { 1, 2, 3 };
                int another = 3;

                // Act
                Action act = () =>
                {
                    using (new AssertionScope())
                    {
                        collection.Should().NotContainEquivalentOf(another, "because we want to test {0}", "first message")
                            .And
                            .HaveCount(4, "because we want to test {0}", "second message");
                    }
                };

                // Assert
                act.Should().Throw<XunitException>().WithMessage("Expected collection*not to contain*first message*but*.\n" +
                                                                 "Expected*4 item(s)*because*second message*but*.");
            }
        }
    }
}
