using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;
using FluentAssertions.Specs.Equivalency;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections
{
    /// <content>
    /// The AllSatisfy specs.
    /// </content>
    public partial class CollectionAssertionSpecs
    {
        #region All Satisfy

        [Fact]
        public void When_collection_asserting_against_null_inspector_it_should_throw_with_clear_explanation()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2 };

            // Act
            Action act = () => collection.Should().AllSatisfy(null);

            // Assert
            act.Should()
               .Throw<ArgumentException>()
               .WithMessage("Cannot verify against a <null> inspector*");
        }

        [Fact]
        public void When_collection_which_is_asserting_against_inspector_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().AllSatisfy(x => x.Should().Be(1), "because we want to test the failure {0}", "message");
            };

            // Assert
            act.Should()
               .Throw<XunitException>()
               .WithMessage(
                   "Expected collection to contain only items satisfying the inspector because we want to test the failure message, but collection is <null>.");
        }

        [Fact]
        public void When_collection_asserting_against_inspector_is_empty_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<int>();

            // Act
            Action act = ()
                => collection.Should().AllSatisfy(x => x.Should().Be(1), "because we want to test the failure {0}", "message");

            // Assert
            act.Should()
               .Throw<XunitException>()
               .WithMessage(
                   "Expected collection to contain only items satisfying the inspector because we want to test the failure message, but collection is empty.");
        }

        [Fact]
        public void When_collection_asserting_with_all_items_satisfying_inspector_it_should_succeed()
        {
            // Arrange
            var collection = new[] { new Customer { Age = 21, Name = "John" }, new Customer { Age = 21, Name = "Jane" } };

            // Act / Assert
            collection.Should().AllSatisfy(x => x.Age.Should().Be(21));
        }

        [Fact]
        public void When_collection_asserting_with_any_item_that_does_not_satisfy_inspector_it_should_throw()
        {
            // Arrange
            var customers = new[]
            {
                new CustomerWithItems { Age = 21, Items = new[] { 1, 2 } },
                new CustomerWithItems { Age = 22, Items = new[] { 3 } }
            };

            // Act
            Action act = () => customers.Should()
                                        .AllSatisfy(
                                            customer =>
                                            {
                                                customer.Age.Should().BeLessThan(21);
                                                customer.Items.Should()
                                                        .AllSatisfy(item => item.Should().Be(3));
                                            },
                                            "because we want to test {0}",
                                            "nested assertions");

            // Assert
            act.Should()
               .Throw<XunitException>()
               .WithMessage(
                   @"Expected customers to contain only items satisfying the inspector because we want to test nested assertions:
*At index 0:
*Expected customer.Age to be less than 21, but found 21
*Expected customer.Items to contain only items satisfying the inspector:
*At index 0:
*Expected item to be 3, but found 1
*At index 1:
*Expected item to be 3, but found 2
*At index 1:
*Expected customer.Age to be less than 21, but found 22"
               );
        }

        [Fact]
        public void When_collection_asserting_with_inspector_message_that_is_not_reformatable_it_should_not_throw()
        {
            // Arrange
            byte[][] subject = { new byte[] { 1 } };

            // Act
            Action act = () => subject.Should().AllSatisfy(e => e.Should().BeEquivalentTo(new byte[] { 2, 3, 4 }));

            // Assert
            act.Should().NotThrow<FormatException>();
        }

        #endregion
    }
}
