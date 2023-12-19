using FluentAssertionsAsync.Execution;
using Xunit;

namespace FluentAssertionsAsync.Specs.Execution;

public class FallbackTestFrameworkTests
{
    [Fact]
    public void The_fallback_test_framework_is_available()
    {
        var sut = new FallbackTestFramework();

        sut.IsAvailable.Should().BeTrue();
    }

    [Fact]
    public void Throwing_with_messages_throws_the_exception()
    {
        var sut = new FallbackTestFramework();

        sut.Invoking(x => x.Throw("test message")).Should().ThrowExactly<AssertionFailedException>()
            .WithMessage("test message");
    }
}
