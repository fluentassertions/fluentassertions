using System;
using Xunit;

namespace FluentAssertions.Specs.Exceptions
{
    public class InvokingFunctionSpecs
    {
        [Fact]
        public void When_invoking_a_function_on_a_null_subject_it_should_throw()
        {
            // Arrange
            Does someClass = null;

            // Act
            Action act = () => someClass.Invoking(d => d.ToString());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("subject");
        }

        [Fact]
        public void When_invoking_a_function_with_null_it_should_throw()
        {
            // Arrange
            object someClass = new();

            // Act
            Action act = () => someClass.Invoking((Func<object, string>)null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("action");
        }
    }
}
