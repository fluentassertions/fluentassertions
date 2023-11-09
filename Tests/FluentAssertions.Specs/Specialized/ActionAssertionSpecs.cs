using System;
using Xunit;

namespace FluentAssertions.Specs.Specialized;

public class ActionAssertionSpecs
{
    public class NotThrow
    {
        [Fact]
        public void Allow_additional_assertions_on_return_value()
        {
            // Arrange
            Action subject = () => { };

            // Act / Assert
            subject.Should().NotThrow()
                .And.NotBeNull();
        }
    }
}
