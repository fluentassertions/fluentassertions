using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class AssertionsSpecs
    {
        [TestMethod]
        public void When_reason_starts_with_because_it_should_not_do_anything()
        {
            var assertions = new AssertionsTestSubClass();

            assertions
                .Invoking(x => x.AssertFail("because {0} should always fail.", typeof(AssertionsTestSubClass).Name))
                .ShouldThrow<SpecificationMismatchException>()
                .WithMessage("Expected it to fail because AssertionsTestSubClass should always fail.");
        }        
        
        [TestMethod]
        public void When_reason_includes_no_because_it_should_be_added()
        {
            var assertions = new AssertionsTestSubClass();

            assertions
                .Invoking(x => x.AssertFail("{0} should always fail.", typeof(AssertionsTestSubClass).Name))
                .ShouldThrow<SpecificationMismatchException>()
                .WithMessage("Expected it to fail because AssertionsTestSubClass should always fail.");
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
            act.ShouldThrow<SpecificationMismatchException>().WithMessage(
                "Expected <System.Object> to match (o == null) because it is not initialized yet.");
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
            Action act = () => someObject.Should().Match<SomeDto>(d => d.Name.Length == 0, "it is not initialized yet");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<SpecificationMismatchException>().WithMessage(
                "Expected <FluentAssertions.Specs.SomeDto> to match (d.Name.Length == 0) because it is not initialized yet.");
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

        internal class AssertionsTestSubClass : Assertions<object,AssertionsTestSubClass>
        {
            public void AssertFail(string reason, params object[] reasonParameters)
            {
                VerifyThat(false, "Expected it to fail{2}", null, null, reason, reasonParameters);
            }
        }
    }

    internal class SomeDto
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime Birthdate { get; set; }
    }
}