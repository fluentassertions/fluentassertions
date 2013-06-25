using System;

using System.Reflection;

using FluentAssertions.Execution;
using FluentAssertions.Primitives;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class AssertionFailureSpecs
    {
#if !WINRT
        private static readonly string AssertionsTestSubClassName = typeof (AssertionsTestSubClass).Name;
#else
        private static readonly string AssertionsTestSubClassName = typeof(AssertionsTestSubClass).GetTypeInfo().Name;
#endif

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
                assertions.AssertFail("because {0} should always fail.", AssertionsTestSubClassName);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected it to fail because AssertionsTestSubClass should always fail.");
        }

        [TestMethod]
        public void When_reason_does_not_start_with_because_it_should_be_added()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assertions = new AssertionsTestSubClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                assertions.AssertFail("{0} should always fail.", AssertionsTestSubClassName);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected it to fail because AssertionsTestSubClass should always fail.");
        }

        [TestMethod]
        public void When_reason_starts_with_because_but_is_prefixed_with_blanks_it_should_not_do_anything()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assertions = new AssertionsTestSubClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                assertions.AssertFail("\r\nbecause {0} should always fail.", AssertionsTestSubClassName);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected it to fail\r\nbecause AssertionsTestSubClass should always fail.");
        }

        [TestMethod]
        public void When_reason_does_not_start_with_because_but_is_prefixed_with_blanks_it_should_add_because_after_the_blanks()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assertions = new AssertionsTestSubClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                assertions.AssertFail("\r\n{0} should always fail.", AssertionsTestSubClassName);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected it to fail\r\nbecause AssertionsTestSubClass should always fail.");
        }

        internal class AssertionsTestSubClass : ReferenceTypeAssertions<object, AssertionsTestSubClass>
        {
            public void AssertFail(string reason, params object [] reasonArgs)
            {
                Execute.Assertion
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected it to fail{reason}");
            }

            protected override string Context
            {
                get { return "test"; }
            }
        }
    }
}