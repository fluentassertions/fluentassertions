#if NET6_0_OR_GREATER
using System;
using System.IO;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Streams;

public class BufferedStreamAssertionSpecs
{
    public class HaveBufferSize
    {
        [Fact]
        public void When_a_stream_has_the_expected_buffer_size_it_should_succeed()
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
        public void When_a_stream_has_an_unexpected_buffer_size_should_fail()
        {
            // Arrange
            using var stream = new BufferedStream(new MemoryStream(), 1);

            // Act
            Action act = () =>
                stream.Should().HaveBufferSize(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the buffer size of stream to be 10 *failure message*, but it was 1.");
        }

        [Fact]
        public void When_null_have_buffer_size_should_fail()
        {
            // Arrange
            BufferedStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().HaveBufferSize(10, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the buffer size of stream to be 10 *failure message*, but found a <null> reference.");
        }
    }

    public class NotHaveBufferSize
    {
        [Fact]
        public void When_a_stream_does_not_have_an_unexpected_buffer_size_it_should_succeed()
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
        public void When_a_stream_does_have_the_unexpected_buffer_size_it_should_fail()
        {
            // Arrange
            using var stream = new BufferedStream(new MemoryStream(), 10);

            // Act
            Action act = () =>
                stream.Should().NotHaveBufferSize(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the buffer size of stream not to be 10 *failure message*, but it was.");
        }

        [Fact]
        public void When_null_not_have_buffer_size_should_fail()
        {
            // Arrange
            BufferedStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().NotHaveBufferSize(10, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the buffer size of stream not to be 10 *failure message*, but found a <null> reference.");
        }
    }
}
#endif
