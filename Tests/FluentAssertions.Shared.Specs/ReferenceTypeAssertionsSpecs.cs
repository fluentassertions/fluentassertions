using System;

using FluentAssertions.Common;
using FluentAssertions.Formatting;

#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class ReferenceTypeAssertionsSpecs
    {
        [TestMethod]
        public void When_the_same_objects_are_expected_to_be_the_same_it_should_not_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new ClassWithCustomEqualMethod(1);
            var referenceToSubject = subject;

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Arrange
            //-------------------------------------------------------------------------------------------------------------------
            subject.Should().BeSameAs(referenceToSubject);
        }

        [TestMethod]
        public void When_two_different_objects_are_expected_to_be_the_same_it_should_fail_with_a_clear_explanation()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Name = "John Doe"
            };

            var otherObject = new
            {
                UserName = "JohnDoe"
            };

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeSameAs(otherObject, "they are {0} {1}", "the", "same");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected object to refer to \r\n{ UserName = JohnDoe } because " +
                    "they are the same, but found \r\n{ Name = John Doe }.");
        }

        [TestMethod]
        public void When_two_different_objects_are_expected_not_to_be_the_same_it_should_not_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var someObject = new ClassWithCustomEqualMethod(1);
            var notSameObject = new ClassWithCustomEqualMethod(1);

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            someObject.Should().NotBeSameAs(notSameObject);
        }

        [TestMethod]
        public void When_two_equal_object_are_expected_not_to_be_the_same_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var someObject = new ClassWithCustomEqualMethod(1);
            ClassWithCustomEqualMethod sameObject = someObject;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => someObject.Should().NotBeSameAs(sameObject, "they are {0} {1}", "the", "same");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect reference to object \r\nClassWithCustomEqualMethod(1) because they are the same.");
        }

        [TestMethod]
        public void When_object_is_of_the_expected_type_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            string aString = "blah";

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => aString.Should().BeOfType(typeof(string));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }        
        
        [TestMethod]
        public void When_object_is_not_of_the_expected_type_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            string aString = "blah";

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => aString.Should().BeOfType(typeof(Int32));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type to be System.Int32, but found System.String.");
        }

        [TestMethod]
        public void When_object_is_of_the_expected_type_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            string aString = "blah";

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => aString.Should().NotBeOfType(typeof(string));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type to not be System.String, but it is.");
        }        
        
        [TestMethod]
        public void When_object_is_not_of_the_expected_type_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            string aString = "blah";

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => aString.Should().NotBeOfType(typeof(Int32));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

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

    public class SomeDto
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime Birthdate { get; set; }
    }

    internal class ClassWithCustomEqualMethod
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:System.Object" /> class.
        /// </summary>
        public ClassWithCustomEqualMethod(int key)
        {
            Key = key;
        }

        private int Key { get; set; }

        private bool Equals(ClassWithCustomEqualMethod other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.Key == Key;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof(ClassWithCustomEqualMethod))
            {
                return false;
            }
            return Equals((ClassWithCustomEqualMethod)obj);
        }

        public override int GetHashCode()
        {
            return Key;
        }

        public static bool operator ==(ClassWithCustomEqualMethod left, ClassWithCustomEqualMethod right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ClassWithCustomEqualMethod left, ClassWithCustomEqualMethod right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///   Returns a <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   A <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("ClassWithCustomEqualMethod({0})", Key);
        }
    }

}