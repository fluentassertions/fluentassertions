using System;
using System.IO;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Streams
{
    public class StreamAssertionSpecs
    {
        #region BeWritable / NotBeWritable

        [Fact]
        public void When_asserting_a_writeable_stream_is_writable_it_should_succeed()
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
        public void When_asserting_a_non_writeable_stream_is_writable_it_should_fail()
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
        public void When_asserting_a_null_stream_should_be_writable_should_throw()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
                stream.Should().BeWritable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be writable *failure message*, but found a <null> Stream.");
        }

        [Fact]
        public void When_asserting_a_non_writeable_stream_is_not_writable_it_should_succeed()
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
        public void When_asserting_a_writeable_stream_is_not_writable_it_should_fail()
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
        public void When_asserting_a_null_stream_should_not_be_writable_should_throw()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
                stream.Should().NotBeWritable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be writable *failure message*, but found a <null> Stream.");
        }

        #endregion

        #region BeSeekable / NotBeSeekable

        [Fact]
        public void When_asserting_a_seekable_stream_is_seekable_it_should_succeed()
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
        public void When_asserting_a_non_seekable_stream_is_seekable_it_should_fail()
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
        public void When_asserting_a_null_stream_should_be_seekable_should_throw()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
                stream.Should().BeSeekable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be seekable *failure message*, but found a <null> Stream.");
        }

        [Fact]
        public void When_asserting_a_non_seekable_stream_is_not_seekable_it_should_succeed()
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
        public void When_asserting_a_seekable_stream_is_not_seekable_it_should_fail()
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
        public void When_asserting_a_null_stream_should_not_be_seekable_should_throw()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
                stream.Should().NotBeSeekable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be seekable *failure message*, but found a <null> Stream.");
        }

        #endregion

        #region BeReadable / NotBeReadable

        [Fact]
        public void When_asserting_a_readable_stream_is_readable_it_should_succeed()
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
        public void When_asserting_a_non_readable_stream_is_readable_it_should_fail()
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
        public void When_asserting_a_null_stream_should_be_readable_should_throw()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
                stream.Should().BeReadable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be readable *failure message*, but found a <null> Stream.");
        }

        [Fact]
        public void When_asserting_a_non_readable_stream_is_not_readable_it_should_succeed()
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
        public void When_asserting_a_readable_stream_is_not_readable_it_should_fail()
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
        public void When_asserting_a_null_stream_should_not_be_readable_should_throw()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
                stream.Should().NotBeReadable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be readable *failure message*, but found a <null> Stream.");
        }

        #endregion

        #region HavePosition / NotHavePosition

        [Fact]
        public void When_asserting_a_stream_should_have_position_with_the_same_value_it_should_succeed()
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
        public void When_asserting_a_stream_should_have_position_with_a_different_value_it_should_throw()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true, Position = 1 };

            // Act
            Action act = () =>
                stream.Should().HavePosition(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the position of stream to be 10 *failure message*, but it was 1*.");
        }

        [Fact]
        public void When_asserting_a_non_seekable_stream_should_have_position_should_throw()
        {
            // Arrange
            using var stream = new TestStream { Seekable = false };

            // Act
            Action act = () =>
                stream.Should().HavePosition(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the position of stream to be 10 *failure message*, but found a non-seekable stream.");
        }

        [Fact]
        public void When_asserting_a_null_stream_should_have_position_should_throw()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
                stream.Should().HavePosition(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the position of stream to be 10 *failure message*, but found a <null> Stream.");
        }

        [Fact]
        public void When_asserting_a_stream_should_not_have_position_with_a_different_it_should_succeed()
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
        public void When_asserting_a_stream_should_not_have_position_with_the_same_value_it_should_throw()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true, Position = 10 };

            // Act
            Action act = () =>
                stream.Should().NotHavePosition(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the position of stream not to be 10 *failure message*, but it was.");
        }

        [Fact]
        public void When_asserting_a_non_seekable_stream_should_not_have_position_should_throw()
        {
            // Arrange
            using var stream = new TestStream { Seekable = false };

            // Act
            Action act = () =>
                stream.Should().NotHavePosition(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the position of stream not to be 10 *failure message*, but found a non-seekable stream.");
        }

        [Fact]
        public void When_asserting_a_null_stream_should_not_have_position_should_throw()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
                stream.Should().NotHavePosition(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the position of stream not to be 10 *failure message*, but found a <null> Stream.");
        }

        #endregion

        #region HaveLength / NotHaveLength

        [Fact]
        public void When_asserting_a_stream_should_have_length_with_the_same_value_it_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true, WithLentgh = 10 };

            // Act
            Action act = () =>
                stream.Should().HaveLength(10);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_stream_should_have_length_with_a_different_value_it_should_throw()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true, WithLentgh = 1 };

            // Act
            Action act = () =>
                stream.Should().HaveLength(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the length of stream to be 10 *failure message*, but it was 1*.");
        }

        [Fact]
        public void When_asserting_a_non_seekable_stream_should_have_length_should_throw()
        {
            // Arrange
            using var stream = new TestStream { Seekable = false };

            // Act
            Action act = () =>
                stream.Should().HaveLength(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the length of stream to be 10 *failure message*, but found a non-seekable stream.");
        }

        [Fact]
        public void When_asserting_a_null_stream_should_have_length_should_throw()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
                stream.Should().HaveLength(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the length of stream to be 10 *failure message*, but found a <null> Stream.");
        }

        [Fact]
        public void When_asserting_a_stream_should_not_have_length_with_a_different_it_should_succeed()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true, WithLentgh = 1 };

            // Act
            Action act = () =>
                stream.Should().NotHaveLength(10);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_stream_should_not_have_length_with_the_same_value_it_should_throw()
        {
            // Arrange
            using var stream = new TestStream { Seekable = true, WithLentgh = 10 };

            // Act
            Action act = () =>
                stream.Should().NotHaveLength(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the length of stream not to be 10 *failure message*, but it was.");
        }

        [Fact]
        public void When_asserting_a_non_seekable_stream_should_not_have_length_should_throw()
        {
            // Arrange
            using var stream = new TestStream { Seekable = false };

            // Act
            Action act = () =>
                stream.Should().NotHaveLength(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the length of stream not to be 10 *failure message*, but found a non-seekable stream.");
        }

        [Fact]
        public void When_asserting_a_null_stream_should_not_have_length_should_throw()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
                stream.Should().NotHaveLength(10, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the length of stream not to be 10 *failure message*, but found a <null> Stream.");
        }

        #endregion

        #region BeReadOnly / NotBeReadOnly

        [Fact]
        public void When_asserting_a_readonly_stream_is_readonly_it_should_succeed()
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
        public void When_asserting_a_writable_stream_is_readonly_it_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Readable = true, Writable = true };

            // Act
            Action act = () =>
                stream.Should().BeReadOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be read only *failure message*, but it was writable or not readable.");
        }

        [Fact]
        public void When_asserting_a_non_readable_stream_is_readonly_it_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Readable = false, Writable = false };

            // Act
            Action act = () =>
                stream.Should().BeReadOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be read only *failure message*, but it was writable or not readable.");
        }

        [Fact]
        public void When_asserting_a_null_stream_should_be_readonly_should_throw()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
                stream.Should().BeReadOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be read only *failure message*, but found a <null> Stream.");
        }

        [Fact]
        public void When_asserting_a_non_readable_stream_is_not_readonly_it_should_succeed()
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
        public void When_asserting_a_writable_stream_is_not_readonly_it_should_succeed()
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
        public void When_asserting_a_readonly_stream_is_not_readonly_it_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Readable = true, Writable = false };

            // Act
            Action act = () =>
                stream.Should().NotBeReadOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be read only *failure message*, but it was.");
        }

        [Fact]
        public void When_asserting_a_null_stream_should_not_be_readonly_should_throw()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
                stream.Should().NotBeReadOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be read only *failure message*, but found a <null> Stream.");
        }

        #endregion

        #region BeWriteOnly / NotBeWriteOnly

        [Fact]
        public void When_asserting_a_writeonly_stream_is_writeonly_it_should_succeed()
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
        public void When_asserting_a_readable_stream_is_writeonly_it_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Readable = true, Writable = true };

            // Act
            Action act = () =>
                stream.Should().BeWriteOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be write only *failure message*, but it was readable or not writable.");
        }

        [Fact]
        public void When_asserting_a_non_wrtieable_stream_is_writeonly_it_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Readable = false, Writable = false };

            // Act
            Action act = () =>
                stream.Should().BeWriteOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be write only *failure message*, but it was readable or not writable.");
        }

        [Fact]
        public void When_asserting_a_null_stream_should_be_writeonly_should_throw()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
                stream.Should().BeWriteOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream to be write only *failure message*, but found a <null> Stream.");
        }

        [Fact]
        public void When_asserting_a_non_writable_stream_is_not_writeonly_it_should_succeed()
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
        public void When_asserting_a_readable_stream_is_not_writeonly_it_should_succeed()
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
        public void When_asserting_a_writeonly_stream_is_not_writeonly_it_should_fail()
        {
            // Arrange
            using var stream = new TestStream { Readable = false, Writable = true };

            // Act
            Action act = () =>
                stream.Should().NotBeWriteOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be write only *failure message*, but it was.");
        }

        [Fact]
        public void When_asserting_a_null_stream_should_not_be_writeonly_should_throw()
        {
            // Arrange
            TestStream stream = null;

            // Act
            Action act = () =>
                stream.Should().NotBeWriteOnly("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected stream not to be write only *failure message*, but found a <null> Stream.");
        }

        #endregion
    }

    internal class TestStream : Stream
    {
        public bool Readable { private get; set; }

        public bool Seekable { private get; set; }

        public bool Writable { private get; set; }

        public long WithLentgh { private get; set; }

        public override bool CanRead => Readable;

        public override bool CanSeek => Seekable;

        public override bool CanWrite => Writable;

        public override long Length => WithLentgh;

        public override long Position { get; set; }

        public override void Flush() => throw new NotImplementedException();

        public override int Read(byte[] buffer, int offset, int count) => throw new NotImplementedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

        public override void SetLength(long value) => throw new NotImplementedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();
    }
}
