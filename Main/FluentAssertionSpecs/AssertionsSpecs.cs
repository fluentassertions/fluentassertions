using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class AssertionsSpecs
    {
        [TestMethod]
        public void When_reason_starts_with_because_it_should_not_do_anything()
        {
            var assertions = new AssertionsTestSubClass();

            assertions
                .ShouldThrow(x => x.AssertFail("because {0} should always fail.", typeof(AssertionsTestSubClass).Name))
                .Exception<SpecificationMismatchException>()
                .WithMessage("Expected it to fail because AssertionsTestSubClass should always fail.");
        }        
        
        [TestMethod]
        public void When_reason_includes_no_because_it_should_be_added()
        {
            var assertions = new AssertionsTestSubClass();

            assertions
                .ShouldThrow(x => x.AssertFail("{0} should always fail.", typeof(AssertionsTestSubClass).Name))
                .Exception<SpecificationMismatchException>()
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
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected <System.Object> to match (o = null) because it is not initialized yet.");
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
            act.ShouldThrow().Exception<NullReferenceException>().WithMessage(
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
}