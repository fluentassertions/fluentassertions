using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace XUnit3Core.Specs;

public class FrameworkSpecs
{
    [Fact]
    public void When_xunit3_without_xunit_assert_is_used_it_should_throw_IAssertionException_for_assertion_failures()
    {
        // Act
        Action act = () => 0.Should().Be(1);

        // Assert
        Exception exception = act.Should().Throw<Exception>().Which;
        exception.GetType().GetInterfaces().Select(e => e.Name).Should().Contain("IAssertionException");
    }
}
