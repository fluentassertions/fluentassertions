using System;
using System.IO;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Streams;

public class StreamAssertionSpecs
{
    public class BeWritable
    {
        [Fact]
        public void When_having_a_writable_stream_be_writable_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Writable = true };

            // Act
            Action act = () =>
                stream.Should().BeWritable();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_having_a_non_writable_stream_be_writable_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Writable = false };

            // Act
            Action act = () =>
                stream.Should().BeWritable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be writable *failure message*, but it was not.");
        }

        [Fact]
        public void When_null_be_writable_should_fail()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().BeWritable("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be writable *failure message*, but found a <null> reference.");
        }
    }

    public class NotBeWritable
    {
        [Fact]
        public void When_having_a_non_writable_stream_be_not_writable_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Writable = false };

            // Act
            Action act = () =>
                stream.Should().NotBeWritable();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_having_a_writable_stream_be_not_writable_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Writable = true };

            // Act
            Action act = () =>
                stream.Should().NotBeWritable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be writable *failure message*, but it was.");
        }

        [Fact]
        public void When_null_not_be_writable_should_fail()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().NotBeWritable("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be writable *failure message*, but found a <null> reference.");
        }
    }

    public class BeSeekable
    {
        [Fact]
        public void When_having_a_seekable_stream_be_seekable_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true };

            // Act
            Action act = () =>
                stream.Should().BeSeekable();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_having_a_non_seekable_stream_be_seekable_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Seekable = false };

            // Act
            Action act = () =>
                stream.Should().BeSeekable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be seekable *failure message*, but it was not.");
        }

        [Fact]
        public void When_null_be_seekable_should_fail()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().BeSeekable("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be seekable *failure message*, but found a <null> reference.");
        }
    }

    public class NotBeSeekable
    {
        [Fact]
        public void When_having_a_non_seekable_stream_be_not_seekable_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Seekable = false };

            // Act
            Action act = () =>
                stream.Should().NotBeSeekable();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_having_a_seekable_stream_be_not_seekable_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true };

            // Act
            Action act = () =>
                stream.Should().NotBeSeekable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be seekable *failure message*, but it was.");
        }

        [Fact]
        public void When_null_not_be_seekable_should_fail()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().NotBeSeekable("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be seekable *failure message*, but found a <null> reference.");
        }
    }

    public class BeReadable
    {
        [Fact]
        public void When_having_a_readable_stream_be_readable_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Readable = true };

            // Act
            Action act = () =>
                stream.Should().BeReadable();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_having_a_non_readable_stream_be_readable_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Readable = false };

            // Act
            Action act = () =>
                stream.Should().BeReadable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be readable *failure message*, but it was not.");
        }

        [Fact]
        public void When_null_be_readable_should_fail()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().BeReadable("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be readable *failure message*, but found a <null> reference.");
        }
    }

    public class NotBeReadable
    {
        [Fact]
        public void When_having_a_non_readable_stream_be_not_readable_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Readable = false };

            // Act
            Action act = () =>
                stream.Should().NotBeReadable();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_having_a_readable_stream_be_not_readable_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Readable = true };

            // Act
            Action act = () =>
                stream.Should().NotBeReadable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be readable *failure message*, but it was.");
        }

        [Fact]
        public void When_null_not_be_readable_should_fail()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().NotBeReadable("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be readable *failure message*, but found a <null> reference.");
        }
    }

    public class HavePosition
    {
        [Fact]
        public void When_a_stream_has_the_expected_position_it_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true, Position = 10 };

            // Act
            Action act = () =>
                stream.Should().HavePosition(10);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_stream_has_the_unexpected_position_it_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true, Position = 1 };

            // Act
            Action act = () =>
                stream.Should().HavePosition(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the position of stream to be 10* *failure message*, but it was 1*.");
        }

        [Fact]
        public void When_null_have_position_should_fail()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().HavePosition(10, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the position of stream to be 10* *failure message*, but found a <null> reference.");
        }

        [Theory]
        [MemberData(nameof(GetPositionExceptions), MemberType = typeof(StreamAssertionSpecs))]
        public void When_a_throwing_stream_should_have_a_position_it_should_fail(Exception exception)
        {
            // Arrange
            using var stream = new ExceptingStream(exception);

            // Act
            Action act = () =>
                stream.Should().HavePosition(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the position of stream to be 10* *failure message*, " +
                    "but it failed with*GetPositionExceptionMessage*");
        }
    }

    public class NotHavePosition
    {
        [Fact]
        public void When_a_stream_does_not_have_an_unexpected_position_it_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true, Position = 1 };

            // Act
            Action act = () =>
                stream.Should().NotHavePosition(10);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_stream_does_have_the_unexpected_position_it_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true, Position = 10 };

            // Act
            Action act = () =>
                stream.Should().NotHavePosition(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the position of stream not to be 10* *failure message*, but it was.");
        }

        [Fact]
        public void When_null_not_have_position_should_fail()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().NotHavePosition(10, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the position of stream not to be 10* *failure message*, but found a <null> reference.");
        }

        [Theory]
        [MemberData(nameof(GetPositionExceptions), MemberType = typeof(StreamAssertionSpecs))]
        public void When_a_throwing_stream_should_not_have_a_position_it_should_fail(Exception exception)
        {
            // Arrange
            using var stream = new ExceptingStream(exception);

            // Act
            Action act = () =>
                stream.Should().NotHavePosition(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the position of stream not to be 10* *failure message*, " +
                    "but it failed with*GetPositionExceptionMessage*");
        }
    }

    public static TheoryData<Exception> GetPositionExceptions => new()
    {
        // https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.position#exceptions
        new IOException("GetPositionExceptionMessage"),
        new NotSupportedException("GetPositionExceptionMessage"),
        new ObjectDisposedException("GetPositionExceptionMessage")
    };

    public class HaveLength
    {
        [Fact]
        public void When_a_stream_has_the_expected_length_it_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true, WithLength = 10 };

            // Act
            Action act = () =>
                stream.Should().HaveLength(10);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_stream_has_an_unexpected_length_it_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true, WithLength = 1 };

            // Act
            Action act = () =>
                stream.Should().HaveLength(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the length of stream to be 10* *failure message*, but it was 1*.");
        }

        [Fact]
        public void When_null_have_length_should_fail()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().HaveLength(10, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the length of stream to be 10* *failure message*, but found a <null> reference.");
        }

        [Theory]
        [MemberData(nameof(GetLengthExceptions), MemberType = typeof(StreamAssertionSpecs))]
        public void When_a_throwing_stream_should_have_a_length_it_should_fail(Exception exception)
        {
            // Arrange
            using var stream = new ExceptingStream(exception);

            // Act
            Action act = () =>
                stream.Should().HaveLength(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the length of stream to be 10* *failure message*, " +
                    "but it failed with*GetLengthExceptionMessage*");
        }
    }

    public class NotHaveLength
    {
        [Fact]
        public void When_a_stream_does_not_have_an_unexpected_length_it_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true, WithLength = 1 };

            // Act
            Action act = () =>
                stream.Should().NotHaveLength(10);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_stream_does_have_the_unexpected_length_it_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true, WithLength = 10 };

            // Act
            Action act = () =>
                stream.Should().NotHaveLength(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the length of stream not to be 10* *failure message*, but it was.");
        }

        [Fact]
        public void When_null_not_have_length_should_fail()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().NotHaveLength(10, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the length of stream not to be 10* *failure message*, but found a <null> reference.");
        }

        [Theory]
        [MemberData(nameof(GetLengthExceptions), MemberType = typeof(StreamAssertionSpecs))]
        public void When_a_throwing_stream_should_not_have_a_length_it_should_fail(Exception exception)
        {
            // Arrange
            using var stream = new ExceptingStream(exception);

            // Act
            Action act = () =>
                stream.Should().NotHaveLength(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the length of stream not to be 10* *failure message*, " +
                    "but it failed with*GetLengthExceptionMessage*");
        }
    }

    public static TheoryData<Exception> GetLengthExceptions => new()
    {
        // https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.length#exceptions
        new IOException("GetLengthExceptionMessage"),
        new NotSupportedException("GetLengthExceptionMessage"),
        new ObjectDisposedException("GetLengthExceptionMessage")
    };

    public class BeReadOnly
    {
        [Fact]
        public void When_having_a_readonly_stream_be_read_only_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Readable = true, Writable = false };

            // Act
            Action act = () =>
                stream.Should().BeReadOnly();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_having_a_writable_stream_be_read_only_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Readable = true, Writable = true };

            // Act
            Action act = () =>
                stream.Should().BeReadOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be read-only *failure message*, but it was writable or not readable.");
        }

        [Fact]
        public void When_having_a_non_readable_stream_be_read_only_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Readable = false, Writable = false };

            // Act
            Action act = () =>
                stream.Should().BeReadOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be read-only *failure message*, but it was writable or not readable.");
        }

        [Fact]
        public void When_null_be_read_only_should_fail()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().BeReadOnly("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be read-only *failure message*, but found a <null> reference.");
        }
    }

    public class NotBeReadOnly
    {
        [Fact]
        public void When_having_a_non_readable_stream_be_not_read_only_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Readable = false, Writable = false };

            // Act
            Action act = () =>
                stream.Should().NotBeReadOnly();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_having_a_writable_stream_be_not_read_only_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Readable = true, Writable = true };

            // Act
            Action act = () =>
                stream.Should().NotBeReadOnly();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_having_a_readonly_stream_be_not_read_only_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Readable = true, Writable = false };

            // Act
            Action act = () =>
                stream.Should().NotBeReadOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be read-only *failure message*, but it was.");
        }

        [Fact]
        public void When_null_not_be_read_only_should_fail()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().NotBeReadOnly("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be read-only *failure message*, but found a <null> reference.");
        }
    }

    public class BeWriteOnly
    {
        [Fact]
        public void When_having_a_writeonly_stream_be_write_only_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Readable = false, Writable = true };

            // Act
            Action act = () =>
                stream.Should().BeWriteOnly();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_having_a_readable_stream_be_write_only_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Readable = true, Writable = true };

            // Act
            Action act = () =>
                stream.Should().BeWriteOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be write-only *failure message*, but it was readable or not writable.");
        }

        [Fact]
        public void When_having_a_non_writable_stream_be_write_only_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Readable = false, Writable = false };

            // Act
            Action act = () =>
                stream.Should().BeWriteOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be write-only *failure message*, but it was readable or not writable.");
        }

        [Fact]
        public void When_null_be_write_only_should_fail()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().BeWriteOnly("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be write-only *failure message*, but found a <null> reference.");
        }
    }

    public class NotBeWriteOnly
    {
        [Fact]
        public void When_having_a_non_writable_stream_be_not_write_only_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Readable = false, Writable = false };

            // Act
            Action act = () =>
                stream.Should().NotBeWriteOnly();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_having_a_readable_stream_be_not_write_only_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Readable = true, Writable = true };

            // Act
            Action act = () =>
                stream.Should().NotBeWriteOnly();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_having_a_writeonly_stream_be_not_write_only_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Readable = false, Writable = true };

            // Act
            Action act = () =>
                stream.Should().NotBeWriteOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be write-only *failure message*, but it was.");
        }

        [Fact]
        public void When_null_not_be_write_only_should_fail()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                stream.Should().NotBeWriteOnly("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be write-only *failure message*, but found a <null> reference.");
        }
    }
}

internal class ExceptingStream : Stream
{
    private readonly Exception exception;

    public ExceptingStream(Exception exception)
    {
        this.exception = exception;
    }

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => true;

    public override long Length => throw exception;

    public override long Position
    {
        get => throw exception;
        set => throw new NotImplementedException();
    }

    public override void Flush() => throw new NotImplementedException();

    public override int Read(byte[] buffer, int offset, int count) => throw new NotImplementedException();

    public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

    public override void SetLength(long value) => throw new NotImplementedException();

    public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();
}

internal class TestStream : Stream
{
    public bool Readable { private get; set; }

    public bool Seekable { private get; set; }

    public bool Writable { private get; set; }

    public long WithLength { private get; set; }

    public override bool CanRead => Readable;

    public override bool CanSeek => Seekable;

    public override bool CanWrite => Writable;

    public override long Length => WithLength;

    public override long Position { get; set; }

    public override void Flush() => throw new NotImplementedException();

    public override int Read(byte[] buffer, int offset, int count) => throw new NotImplementedException();

    public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

    public override void SetLength(long value) => throw new NotImplementedException();

    public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();
}
