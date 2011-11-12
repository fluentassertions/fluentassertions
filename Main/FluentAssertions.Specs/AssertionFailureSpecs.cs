using System;

using FluentAssertions.Assertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class AssertionFailureSpecs
    {
        [TestMethod]
        public void When_reason_starts_with_because_it_should_not_do_anything()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assertions = new AssertionsTestSubClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                assertions.AssertFail("because {0} should always fail.", typeof (AssertionsTestSubClass).Name);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected it to fail because AssertionsTestSubClass should always fail.");
        }

        [TestMethod]
        public void When_reason_includes_no_because_it_should_be_added()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assertions = new AssertionsTestSubClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                assertions.AssertFail("{0} should always fail.", typeof(AssertionsTestSubClass).Name);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected it to fail because AssertionsTestSubClass should always fail.");
        }

        internal class AssertionsTestSubClass : ReferenceTypeAssertions<object, AssertionsTestSubClass>
        {
            public void AssertFail(string reason, params object [] reasonArgs)
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected it to fail{reason}");
            }
        }
    }
}