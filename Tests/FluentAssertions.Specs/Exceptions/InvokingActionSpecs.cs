using System;
using Xunit;

namespace FluentAssertions.Specs.Exceptions
{
    public class InvokingActionSpecs
    {
        [Fact]
        public void When_invoking_an_action_on_a_null_subject_it_should_throw()
        {
            // Arrange
            Does someClass = null;

            // Act
            Action act = () => someClass.Invoking(d => d.Do());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("subject");
        }

        [Fact]
        public void When_invoking_an_action_with_null_it_should_throw()
        {
            // Arrange
            Does someClass = Does.NotThrow();

            // Act
            Action act = () => someClass.Invoking(null).Should().NotThrow();

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("action");
        }
    }
}
