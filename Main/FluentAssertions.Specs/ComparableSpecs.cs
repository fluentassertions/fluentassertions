using System;

using FluentAssertions.Assertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class ComparableSpecs
    {
        #region Be / Not Be

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
            subject.Should().Be(other);
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
            Action act = () => subject.Should().Be(other, "they have the same property values");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected*Hi*because they have the same property values, but found*Hello*.", ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_two_equal_objects_should_not_be_equal_it_should_throw()
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
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().NotBe(other, "they represent different things");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Did not expect object to be equal to*Hello*because they represent different things.", ComparisonMode.Wildcard);
        }
        
        [TestMethod]
        public void When_two_unequal_objects_should_not_be_equal_it_should_not_throw()
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
            Action act = () => subject.Should().NotBe(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region Be Less Than

        [TestMethod]
        public void When_subject_is_less_than_another_subject_and_that_is_expected_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableClass
            {
                Value = "City"
            };

            var other = new ComparableClass
            {
                Value = "World"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeLessThan(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_subject_is_not_less_than_another_subject_but_that_is_expected_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableClass
            {
                Value = "World"
            };

            var other = new ComparableClass
            {
                Value = "City"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeLessThan(other, "a city is smaller than the world");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected object*World*to be less than*City*because a city is smaller than the world.", ComparisonMode.Wildcard);
        }

        #endregion
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