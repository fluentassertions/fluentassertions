using System;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class AssertionFailureSpecs
    {
        private static readonly string AssertionsTestSubClassName = typeof(AssertionsTestSubClass).Name;

        [Fact]
        public void When_reason_starts_with_because_it_should_not_do_anything()
        {
            // Arrange
            var assertions = new AssertionsTestSubClass();

            // Act
            Action action = () =>
                assertions.AssertFail("because {0} should always fail.", AssertionsTestSubClassName);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected it to fail because AssertionsTestSubClass should always fail.");
        }

        [Fact]
        public void When_reason_does_not_start_with_because_it_should_be_added()
        {
            // Arrange
            var assertions = new AssertionsTestSubClass();

            // Act
            Action action = () =>
                assertions.AssertFail("{0} should always fail.", AssertionsTestSubClassName);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected it to fail because AssertionsTestSubClass should always fail.");
        }

        [Fact]
        public void When_reason_starts_with_because_but_is_prefixed_with_blanks_it_should_not_do_anything()
        {
            // Arrange
            var assertions = new AssertionsTestSubClass();

            // Act
            Action action = () =>
                assertions.AssertFail("\r\nbecause {0} should always fail.", AssertionsTestSubClassName);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected it to fail\r\nbecause AssertionsTestSubClass should always fail.");
        }

        [Fact]
        public void When_reason_does_not_start_with_because_but_is_prefixed_with_blanks_it_should_add_because_after_the_blanks()
        {
            // Arrange
            var assertions = new AssertionsTestSubClass();

            // Act
            Action action = () =>
                assertions.AssertFail("\r\n{0} should always fail.", AssertionsTestSubClassName);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected it to fail\r\nbecause AssertionsTestSubClass should always fail.");
        }

        internal class AssertionsTestSubClass : ReferenceTypeAssertions<object, AssertionsTestSubClass>
        {
            public void AssertFail(string because, params object[] becauseArgs)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected it to fail{reason}");
            }

            protected override string Identifier
            {
                get { return "test"; }
            }

            public AssertionsTestSubClass() : base(null)
            {
            }
        }
    }
}
