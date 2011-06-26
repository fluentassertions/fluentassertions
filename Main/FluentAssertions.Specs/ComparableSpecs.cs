using System;

using FluentAssertions.Assertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class ComparableSpecs
    {
        [TestMethod]
        public void When_two_instances_are_equal_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableClass
            {
                Value = "Hello"
            };

            var other = new ComparableClass
            {
                Value = "Hello"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.Should().BeEqualTo(other);
        }

        [TestMethod]
        public void When_two_instances_are_not_equal_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableClass
            {
                Value = "Hello"
            };

            var other = new ComparableClass
            {
                Value = "Hi"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeEqualTo(other, "they have the same property values");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected*Hi*because they have the same property values, but found*Hello*.", ComparisonMode.Wildcard);
        }
    }

    internal class ComparableClass : IComparable<ComparableClass>
    {
        public string Value { get; set; }

        public int CompareTo(ComparableClass other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}