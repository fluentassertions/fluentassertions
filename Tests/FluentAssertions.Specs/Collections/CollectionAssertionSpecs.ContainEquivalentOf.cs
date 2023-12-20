using System;
using System.Threading.Tasks;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The [Not]ContainEquivalentOfAsync specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class ContainEquivalentOfAsync
    {
        [Fact]
        public async Task When_collection_contains_object_equal_of_another_it_should_succeed()
        {
            // Arrange
            var item = new Customer { Name = "John" };
            var collection = new[] { new Customer { Name = "Jane" }, item };

            // Act / Assert
            await collection.Should().ContainEquivalentOfAsync(item);
        }

        [Fact]
        public async Task When_collection_contains_object_equivalent_of_another_it_should_succeed()
        {
            // Arrange
            var collection = new[] { new Customer { Name = "Jane" }, new Customer { Name = "John" } };
            var item = new Customer { Name = "John" };

            // Act / Assert
            await collection.Should().ContainEquivalentOfAsync(item);
        }

        [Fact]
        public async Task When_character_collection_does_contain_equivalent_it_should_succeed()
        {
            // Arrange
            char[] collection = "abc123ab".ToCharArray();
            char item = 'c';

            // Act / Assert
            await collection.Should().ContainEquivalentOfAsync(item);
        }

        [Fact]
        public async Task When_string_collection_does_contain_same_string_with_other_case_it_should_throw()
        {
            // Arrange
            string[] collection = { "a", "b", "c" };
            string item = "C";

            // Act
            Func<Task> act = () => collection.Should().ContainEquivalentOfAsync(item);

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("Expected collection {\"a\", \"b\", \"c\"} to contain equivalent of \"C\".*");
        }

        [Fact]
        public async Task When_string_collection_does_contain_same_string_it_should_throw_with_a_useful_message()
        {
            // Arrange
            string[] collection = { "a" };
            string item = "b";

            // Act
            Func<Task> act = () => collection.Should().ContainEquivalentOfAsync(item, "because we want to test the failure {0}", "message");

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Fact]
        public async Task When_collection_does_not_contain_object_equivalent_of_another_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };
            int item = 4;

            // Act
            Func<Task> act = () => collection.Should().ContainEquivalentOfAsync(item);

            // Act / Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage("Expected collection {1, 2, 3} to contain equivalent of 4.*");
        }

        [Fact]
        public async Task When_asserting_collection_to_contain_equivalent_but_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] collection = null;
            int expectation = 1;

            // Act
            Func<Task> act = async () =>
            {
                using var _ = new AssertionScope();
                await collection.Should().ContainEquivalentOfAsync(expectation, "because we want to test the behaviour with a null subject");
            };

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected collection to contain equivalent of 1 because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public async Task When_collection_contains_equivalent_null_object_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 3, (int?)null };
            int? item = null;

            // Act
            Func<Task> act = () => collection.Should().ContainEquivalentOfAsync(item);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_collection_does_not_contain_equivalent_null_object_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };
            int? item = null;

            // Act
            Func<Task> act = () => collection.Should().ContainEquivalentOfAsync(item);

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage("Expected collection {1, 2, 3} to contain equivalent of <null>.*");
        }

        [Fact]
        public async Task When_empty_collection_does_not_contain_equivalent_it_should_throw()
        {
            // Arrange
            var collection = new int[0];
            int item = 1;

            // Act
            Func<Task> act = () => collection.Should().ContainEquivalentOfAsync(item);

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage("Expected collection {empty} to contain equivalent of 1.*");
        }

        [Fact]
        public async Task When_collection_does_not_contain_equivalent_because_of_second_property_it_should_throw()
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
            Func<Task> act = () => subject.Should().ContainEquivalentOfAsync(item);

            // Assert
            await act.Should().ThrowAsync<XunitException>();
        }

        [Fact]
        public async Task When_collection_does_contain_equivalent_by_including_single_property_it_should_not_throw()
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
            await collection.Should().ContainEquivalentOfAsync(item, options => options.Including(x => x.Name));
        }

        [Fact]
        public async Task Tracing_should_be_included_in_the_assertion_output()
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
            Func<Task> act = () => collection.Should().ContainEquivalentOfAsync(item, options => options.WithTracing());

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage("*Equivalency was proven*");
        }

        [Fact]
        public async Task When_injecting_a_null_config_to_ContainEquivalentOf_it_should_throw()
        {
            // Arrange
            int[] collection = null;
            object item = null;

            // Act
            Func<Task> act = () => collection.Should().ContainEquivalentOfAsync(item, config: null);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("config");
        }

        [Fact]
        public async Task When_collection_contains_object_equivalent_of_boxed_object_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };
            object boxedValue = 2;

            // Act / Assert
            await collection.Should().ContainEquivalentOfAsync(boxedValue);
        }
    }

    public class NotContainEquivalentOfAsync
    {
        [Fact]
        public async Task When_collection_contains_object_equal_to_another_it_should_throw()
        {
            // Arrange
            var item = 1;
            var collection = new[] { 0, 1 };

            // Act
            Func<Task> act = () => collection.Should().NotContainEquivalentOfAsync(item, "because we want to test the failure {0}", "message");

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected collection {0, 1} not to contain*because we want to test the failure message, " +
                "but found one at index 1.*With configuration*");
        }

        [Fact]
        public async Task When_collection_contains_several_objects_equal_to_another_it_should_throw()
        {
            // Arrange
            var item = 1;
            var collection = new[] { 0, 1, 1 };

            // Act
            Func<Task> act = () => collection.Should().NotContainEquivalentOfAsync(item, "because we want to test the failure {0}", "message");

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected collection {0, 1, 1} not to contain*because we want to test the failure message, " +
                "but found several at indices {1, 2}.*With configuration*");
        }

        [Fact]
        public async Task When_asserting_collection_to_not_to_contain_equivalent_but_collection_is_null_it_should_throw()
        {
            // Arrange
            var item = 1;
            int[] collection = null;

            // Act
            Func<Task> act = () => collection.Should().NotContainEquivalentOfAsync(item);

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage("Expected collection*not to contain*but collection is <null>.");
        }

        [Fact]
        public async Task When_injecting_a_null_config_to_NotContainEquivalentOf_it_should_throw()
        {
            // Arrange
            int[] collection = null;
            object item = null;

            // Act
            Func<Task> act = () => collection.Should().NotContainEquivalentOfAsync(item, config: null);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("config");
        }

        [Fact]
        public async Task When_asserting_empty_collection_to_not_contain_equivalent_it_should_succeed()
        {
            // Arrange
            var collection = new int[0];
            int item = 4;

            // Act / Assert
            await collection.Should().NotContainEquivalentOfAsync(item);
        }

        [Fact]
        public async Task When_asserting_a_null_collection_to_not_contain_equivalent_of__then_it_should_fail()
        {
            // Arrange
            int[] collection = null;

            // Act
            Func<Task> act = async () =>
            {
                using var _ = new AssertionScope();
                await collection.Should().NotContainEquivalentOfAsync(1, config => config, "we want to test the failure {0}", "message");
            };

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("Expected collection not to contain *failure message*, but collection is <null>.");
        }

        [Fact]
        public async Task When_collection_does_not_contain_object_equivalent_of_unexpected_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };
            int item = 4;

            // Act / Assert
            await collection.Should().NotContainEquivalentOfAsync(item);
        }

        [Fact]
        public async Task When_asserting_collection_to_not_contain_equivalent_it_should_respect_config()
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
            Func<Task> act = () => collection.Should().NotContainEquivalentOfAsync(item, options => options.Excluding(x => x.Age));

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage("*Exclude*Age*");
        }

        [Fact]
        public async Task When_asserting_collection_to_not_contain_equivalent_it_should_allow_combining_inside_assertion_scope()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };
            int another = 3;

            // Act
            Func<Task> act = async () =>
            {
                using (new AssertionScope())
                {
                    (await collection.Should().NotContainEquivalentOfAsync(another, "because we want to test {0}", "first message"))
                        .And
                        .HaveCount(4, "because we want to test {0}", "second message");
                }
            };

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage("Expected collection*not to contain*first message*but*.\n" +
                "Expected*4 item(s)*because*second message*but*.");
        }
    }
}
