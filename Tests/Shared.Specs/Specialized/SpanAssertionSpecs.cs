using System;
using Xunit;

namespace FluentAssertions.Specs
{
#if NETSTANDARD2_1||NETCOREAPP3_0

    public class SpanAssertionSpecs
    {
        [Fact]
        public void X()
        {
            // Arrange
            ReadOnlySpan<string> span = new ReadOnlySpan<string>(new[] { "a", "b", "c" });

            // Act
            span = span.Slice(1);

            // Assert
            span.Should().NotContain("a");
        }
    }

#endif
}
