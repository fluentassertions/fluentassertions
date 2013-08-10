using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Net40.Specs
{
    [TestClass]
    public class AggregateExceptionAssertionSpecs
    {
        [TestMethod]
        public void When_the_expected_exception_is_wrapped_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var exception = new AggregateException(
                new InvalidOperationException("Ignored"),
                new AssertFailedException("Background"));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => { throw exception; };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage("Background");
        }

        [TestMethod]
        public void When_the_expected_exception_was_not_thrown_it_should_report_the_actual_exceptions()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Action throwingOperation = () =>
            {
                throw new AggregateException(
                    new InvalidOperationException("You can't do this"),
                    new NullReferenceException("Found a null"));
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => throwingOperation
                .ShouldThrow<ArgumentException>()
                .WithMessage("Something I expected");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("*InvalidOperation*You can't do this*")
                .WithMessage("*NullReferenceException*Found a null*");
        }

        [TestMethod]
        public void When_no_exception_was_expected_it_should_report_the_actual_exceptions()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Action throwingOperation = () =>
            {
                throw new AggregateException(
                    new InvalidOperationException("You can't do this"),
                    new NullReferenceException("Found a null"));
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => throwingOperation.ShouldNotThrow();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("*InvalidOperation*You can't do this*")
                .WithMessage("*NullReferenceException*Found a null*");
        }
    }
}