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

    }
}
