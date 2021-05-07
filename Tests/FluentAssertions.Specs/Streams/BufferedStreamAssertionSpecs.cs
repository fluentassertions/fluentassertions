#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1
using System;
using System.IO;
using Xunit;
using Xunit.Sdk;
#endif

namespace FluentAssertions.Specs.Streams
{
    public class BufferedStreamAssertionSpecs
    {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1

        #region HaveBufferSize / NotHaveBufferSize

        [Fact]
        public void When_asserting_a_stream_should_have_buffer_size_with_the_same_value_it_should_succeed()
        {
            // Arrange
            using var stream = new BufferedStream(new MemoryStream(), 10);

            // Act
            Action act = () =>
                stream.Should().HaveBufferSize(10);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_stream_should_have_buffer_size_with_a_different_value_it_should_throw()
        {
            // Arrange
            using var stream = new BufferedStream(new MemoryStream(), 1);

            // Act
            Action act = () =>
                stream.Should().HaveBufferSize(10);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the stream buffer size to be 10, but it was 1.");
        }

        [Fact]
        public void When_asserting_null_stream_should_have_buffer_size_should_throw()
        {
            // Arrange
            BufferedStream stream = null;

            // Act
            Action act = () =>
                stream.Should().HaveBufferSize(10);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the stream buffer size to be 10, but found a <null> BufferedStream.");
        }

        [Fact]
        public void When_asserting_a_stream_should_not_have_buffer_size_with_a_different_value_it_should_succeed()
        {
            // Arrange
            using var stream = new BufferedStream(new MemoryStream(), 1);

            // Act
            Action act = () =>
                stream.Should().NotHaveBufferSize(10);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_stream_should_not_have_buffer_size_with_the_same_value_it_should_throw()
        {
            // Arrange
            using var stream = new BufferedStream(new MemoryStream(), 10);

            // Act
            Action act = () =>
                stream.Should().NotHaveBufferSize(10);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the stream buffer size not to be 10, but it was 10.");
        }

        [Fact]
        public void When_asserting_null_stream_not_should_have_buffer_size_should_throw()
        {
            // Arrange
            BufferedStream stream = null;

            // Act
            Action act = () =>
                stream.Should().NotHaveBufferSize(10);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the stream buffer size not to be 10, but found a <null> BufferedStream.");
        }

        #endregion
#endif
    }
}
