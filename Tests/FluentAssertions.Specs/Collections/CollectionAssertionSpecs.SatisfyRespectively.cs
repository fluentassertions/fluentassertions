using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The SatisfyRespectively specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class SatisfyRespectively
    {
        [Fact]
        public void When_collection_asserting_against_null_inspectors_it_should_throw_with_clear_explanation()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2 };

            // Act
            Action act = () => collection.Should().SatisfyRespectively(null);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify against a <null> collection of inspectors*");
        }

        [Fact]
        public void When_collection_asserting_against_empty_inspectors_it_should_throw_with_clear_explanation()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2 };

            // Act
            Action act = () => collection.Should().SatisfyRespectively();

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify against an empty collection of inspectors*");
        }

        [Fact]
        public void When_collection_which_is_asserting_against_inspectors_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();

                collection.Should().SatisfyRespectively(
                    new Action<int>[] { x => x.Should().Be(1) }, "because we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to satisfy all inspectors because we want to test the failure message, but collection is <null>.");
        }

        [Fact]
        public void When_collection_which_is_asserting_against_inspectors_is_empty_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<int>();

            // Act
            Action act = () => collection.Should().SatisfyRespectively(new Action<int>[] { x => x.Should().Be(1) },
                "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to satisfy all inspectors because we want to test the failure message, but collection is empty.");
        }

        [Fact]
        public void When_asserting_collection_satisfies_all_inspectors_it_should_succeed()
        {
            // Arrange
            var collection = new[] { new Customer { Age = 21, Name = "John" }, new Customer { Age = 22, Name = "Jane" } };

            // Act / Assert
            collection.Should().SatisfyRespectively(
                value =>
                {
                    value.Age.Should().Be(21);
                    value.Name.Should().Be("John");
                },
                value =>
                {
                    value.Age.Should().Be(22);
                    value.Name.Should().Be("Jane");
                });
        }

        [Fact]
        public void When_asserting_collection_does_not_satisfy_any_inspector_it_should_throw()
        {
            // Arrange
            var customers = new[]
            {
                new CustomerWithItems { Age = 21, Items = new[] { 1, 2 } },
                new CustomerWithItems { Age = 22, Items = new[] { 3 } }
            };

            // Act
            Action act = () => customers.Should().SatisfyRespectively(
                new Action<CustomerWithItems>[]
                {
                    customer =>
                    {
                        customer.Age.Should().BeLessThan(21);

                        customer.Items.Should().SatisfyRespectively(
                            item => item.Should().Be(2),
                            item => item.Should().Be(1));
                    },
                    customer =>
                    {
                        customer.Age.Should().BeLessThan(22);
                        customer.Items.Should().SatisfyRespectively(item => item.Should().Be(2));
                    }
                }, "because we want to test {0}", "nested assertions");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                @"Expected customers to satisfy all inspectors because we want to test nested assertions, but some inspectors are not satisfied:
*At index 0:
*Expected customer.Age to be less than 21, but found 21
*Expected customer.Items to satisfy all inspectors, but some inspectors are not satisfied:
*At index 0:
*Expected item to be 2, but found 1
*At index 1:
*Expected item to be 1, but found 2
*At index 1:
*Expected customer.Age to be less than 22, but found 22
*Expected customer.Items to satisfy all inspectors, but some inspectors are not satisfied:
*At index 0:
*Expected item to be 2, but found 3"
            );
        }

        [Fact]
        public async Task When_inspector_message_is_not_reformatable_it_should_not_throw()
        {
            // Arrange
            byte[][] subject = { new byte[] { 1 } };

            // Act
            Func<Task> act = () => subject.Should().SatisfyRespectivelyAsync(async e => await e.Should().BeEquivalentToAsync(new byte[] { 2, 3, 4 }));

            // Assert
            await act.Should().NotThrowAsync<FormatException>();
        }

        [Fact]
        public void When_inspectors_count_does_not_equal_asserting_collection_length_it_should_throw_with_a_useful_message()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().SatisfyRespectively(
                new Action<int>[] { value => value.Should().Be(1), value => value.Should().Be(2) },
                "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain exactly 2 items*we want to test the failure message*, but it contains 3 items");
        }

        [Fact]
        public void When_inspectors_count_does_not_equal_asserting_collection_length_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var collection = new int[0];

            // Act
            Action act = () => collection.Should().SatisfyRespectively(
                new Action<int>[] { value => value.Should().Be(1), }, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*because we want to test the failure*");
        }
    }

    private class Customer
    {
        private string PrivateProperty { get; }

        protected string ProtectedProperty { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime Birthdate { get; set; }

        public long Id { get; set; }

        public void SetProtected(string value)
        {
            ProtectedProperty = value;
        }

        public Customer()
        {
        }

        public Customer(string privateProperty)
        {
            PrivateProperty = privateProperty;
        }
    }

    private class CustomerWithItems : Customer
    {
        public int[] Items { get; set; }
    }
}
