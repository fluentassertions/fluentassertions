using System;

using FluentAssertions.Common;
using FluentAssertions.Formatting;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class ReferenceTypeAssertionsSpecs
    {
        [TestMethod]
        public void When_object_satisfies_predicate_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var someObject = new object();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            someObject.Should().Match(o => (o != null));
        }

        [TestMethod]
        public void When_typed_object_satisfies_predicate_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var someObject = new SomeDto
            {
                Name = "Dennis Doomen",
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20)
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            someObject.Should().Match<SomeDto>(o => o.Age > 0);
        }

        [TestMethod]
        public void When_object_does_not_match_the_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var someObject = new object();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => someObject.Should().Match(o => o == null, "it is not initialized yet");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().Where(e => e.Message.EndsWith(
                "to match (o == null) because it is not initialized yet."));
        }

        [TestMethod]
        public void When_a_typed_object_does_not_match_the_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var someObject = new SomeDto
            {
                Name = "Dennis Doomen",
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20)
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => someObject.Should().Match((SomeDto d) => d.Name.Length == 0, "it is not initialized yet");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected " + Formatter.ToString(someObject) + " to match (d.Name.Length == 0) because it is not initialized yet.");
        }

        [TestMethod]
        public void When_object_is_matched_against_a_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var someObject = new object();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => someObject.Should().Match(null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot match an object against a <null> predicate.");
        }

        #region Structure Reporting

        [TestMethod]
        public void When_an_assertion_on_two_objects_fails_it_should_show_the_properties_of_the_class()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new SomeDto
            {
                Age = 37,
                Birthdate = 20.September(1973),
                Name = "Dennis"
            };

            var other = new SomeDto
            {
                Age = 2,
                Birthdate = 22.February(2009),
                Name = "Teddie"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().Be(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected object to be \r\n\r\nFluentAssertions.Specs.SomeDto\r\n{\r\n   Age = 2\r\n   Birthdate = <2009-02-22>\r\n" +
                    "   Name = \"Teddie\"\r\n}, but found \r\n\r\nFluentAssertions.Specs.SomeDto\r\n{\r\n   Age = 37\r\n" +
                        "   Birthdate = <1973-09-20>\r\n   Name = \"Dennis\"\r\n}.");
        }

        [TestMethod]
        public void When_an_assertion_on_two_objects_fails_and_they_implement_tostring_it_should_show_their_string_representation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object subject = 3;
            object other = 4;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().Be(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected object to be 4, but found 3.");
        }

        [TestMethod]
        public void When_an_assertion_on_two_unknown_objects_fails_it_should_report_the_type_name()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new object();
            var other = new object();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().Be(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assertt
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(string.Format(
                "Expected object to be System.Object (HashCode={0}), but found System.Object (HashCode={1}).",
                other.GetHashCode(), subject.GetHashCode()));
        }

        #endregion
    }

    internal class SomeDto
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime Birthdate { get; set; }
    }
}