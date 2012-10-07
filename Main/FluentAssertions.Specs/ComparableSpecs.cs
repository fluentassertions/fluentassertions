using System;

using FluentAssertions.Primitives;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
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
            var subject = new ComparableOfString("Hello");
            var other = new ComparableOfString("Hello");

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
            var subject = new ComparableOfString("Hello");
            var other = new ComparableOfString("Hi");

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
            var subject = new ComparableOfString("Hello");
            var other = new ComparableOfString("Hello");

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
                    "Did not expect object to be equal to*Hello*because they represent different things.",
                    ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_two_unequal_objects_should_not_be_equal_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableOfString("Hello");
            var other = new ComparableOfString("Hi");

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

        #region BeNull / NotBeNull

        [TestMethod]
        public void When_assertion_an_instance_to_be_null_and_it_is_null_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            ComparableOfString subject = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => 
                subject.Should().BeNull();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_assertion_an_instance_to_be_null_and_it_is_not_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableOfString("");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => 
                subject.Should().BeNull();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Did not expect a value*, but found *.", ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_assertion_an_instance_not_to_be_null_and_it_is_not_null_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableOfString("");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                subject.Should().NotBeNull();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_assertion_an_instance_not_to_be_null_and_it_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            ComparableOfString subject = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                subject.Should().NotBeNull();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected a value*, but found *.", ComparisonMode.Wildcard);
        }

        #endregion

        #region BeInRange

        [TestMethod]
        public void When_assertion_an_instance_to_be_in_a_certain_range_and_it_is_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableOfInt(1);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                subject.Should().BeInRange(new ComparableOfInt(1), new ComparableOfInt(2));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_assertion_an_instance_to_be_in_a_certain_range_but_it_is_not_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableOfInt(3);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                subject.Should().BeInRange(new ComparableOfInt(1), new ComparableOfInt(2));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected object to be between*and*, but found *.", ComparisonMode.Wildcard);
        }

        #endregion

        #region Be Less Than

        [TestMethod]
        public void When_subject_is_less_than_another_subject_and_that_is_expected_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableOfString("City");
            var other = new ComparableOfString("World");

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
            var subject = new ComparableOfString("World");
            var other = new ComparableOfString("City");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeLessThan(other, "a city is smaller than the world");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected object*World*to be less than*City*because a city is smaller than the world.",
                    ComparisonMode.Wildcard);
        }

        #endregion

        #region Be Less Or Equal To

        [TestMethod]
        public void When_subject_is_greater_than_another_subject_and_that_is_not_expected_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableOfString("World");
            var other = new ComparableOfString("City");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeLessOrEqualTo(other, "we want to order them");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected object*World*to be less or equal to*City*because we want to order them.",
                    ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_subject_is_equal_to_another_subject_and_that_is_expected_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableOfString("World");
            var other = new ComparableOfString("World");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeLessOrEqualTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_subject_is_less_than_another_subject_and_less_or_equal_is_expected_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableOfString("City");
            var other = new ComparableOfString("World");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeLessOrEqualTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region Be Greater Than

        [TestMethod]
        public void When_subject_is_greater_than_another_subject_and_that_is_expected_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableOfString("efg");
            var other = new ComparableOfString("abc");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeGreaterThan(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_subject_is_not_greater_than_another_subject_but_that_is_expected_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableOfString("abc");
            var other = new ComparableOfString("def");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeGreaterThan(other, "'a' is smaller then 'e'");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected object*abc*to be greater than*def*because 'a' is smaller then 'e'.",
                    ComparisonMode.Wildcard);
        }

        #endregion

        #region Be Greater Or Equal To

        [TestMethod]
        public void When_subject_is_less_than_another_subject_and_that_is_not_expected_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableOfString("abc");
            var other = new ComparableOfString("def");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeGreaterOrEqualTo(other, "'d' is bigger then 'a'");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected object*abc*to be greater or equal to*def*because 'd' is bigger then 'a'.",
                    ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_subject_is_equal_to_another_subject_and_that_is_equal_or_greater_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableOfString("def");
            var other = new ComparableOfString("def");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeGreaterOrEqualTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_subject_is_greater_than_another_subject_and_greater_or_equal_is_expected_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ComparableOfString("xyz");
            var other = new ComparableOfString("abc");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeGreaterOrEqualTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion
    }

    public class ComparableOfString : IComparable<ComparableOfString>
    {
        public string Value { get; set; }

        public ComparableOfString(string value)
        {
            Value = value;
        }

        public int CompareTo(ComparableOfString other)
        {
            return Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public class ComparableOfInt : IComparable<ComparableOfInt>
    {
        public int Value { get; set; }

        public ComparableOfInt(int value)
        {
            Value = value;
        }

        public int CompareTo(ComparableOfInt other)
        {
            return Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

}