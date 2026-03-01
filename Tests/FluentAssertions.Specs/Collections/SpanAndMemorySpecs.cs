#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

public class SpanAndMemorySpecs
{
    public class GeneralPurposeCollections
    {
        [Fact]
        public void A_span_supports_all_collection_assertions()
        {
            // Arrange
            Span<int> span = new[] { 1, 2, 3 };

            // Act / Assert
            span.Should().HaveCount(3)
                .And.Contain(2)
                .And.NotContain(4)
                .And.StartWith(1)
                .And.EndWith(3)
                .And.BeInAscendingOrder();
        }

        [Fact]
        public void A_readonly_span_supports_all_collection_assertions()
        {
            // Arrange
            ReadOnlySpan<int> span = [1, 2, 3];

            // Act / Assert
            span.Should().HaveCount(3)
                .And.Contain(2)
                .And.NotContain(4)
                .And.StartWith(1)
                .And.EndWith(3)
                .And.BeInAscendingOrder();
        }

        [Fact]
        public void A_memory_object_supports_all_collection_assertions()
        {
            // Arrange
            Memory<int> memory = new[] { 1, 2, 3 };

            // Act / Assert
            memory.Should().HaveCount(3)
                .And.Contain(2)
                .And.NotContain(4)
                .And.StartWith(1)
                .And.EndWith(3)
                .And.BeInAscendingOrder();
        }

        [Fact]
        public void A_readonly_memory_object_supports_all_collection_assertions()
        {
            // Arrange
            ReadOnlyMemory<int> memory = new[] { 1, 2, 3 };

            // Act / Assert
            memory.Should().HaveCount(3)
                .And.Contain(2)
                .And.NotContain(4)
                .And.StartWith(1)
                .And.EndWith(3)
                .And.BeInAscendingOrder();
        }

        [Fact]
        public void A_readonly_span_of_strings_supports_string_collection_assertions()
        {
            // Arrange
            string[] array = ["one", "two", "three"];
            ReadOnlySpan<string> span = new(array);

            // Act / Assert
            span.Should().HaveCount(3)
                .And.Contain("two")
                .And.NotContain("four")
                .And.StartWith("one")
                .And.EndWith("three")
                .And.ContainMatch("o*");
        }

        [Fact]
        public void An_empty_span_supports_assertions()
        {
            // Arrange
            Span<int> span = [];

            // Act / Assert
            span.Should().BeEmpty()
                .And.HaveCount(0)
                .And.NotContain(1);
        }

        [Fact]
        public void An_empty_readonly_span_supports_assertions()
        {
            // Arrange
            ReadOnlySpan<int> span = [];

            // Act / Assert
            span.Should().BeEmpty()
                .And.HaveCount(0)
                .And.NotContain(1);
        }

        [Fact]
        public void An_empty_memory_supports_assertions()
        {
            // Arrange
            Memory<int> memory = Array.Empty<int>();

            // Act / Assert
            memory.Should().BeEmpty()
                .And.HaveCount(0)
                .And.NotContain(1);
        }

        [Fact]
        public void An_empty_readonly_memory_supports_assertions()
        {
            // Arrange
            ReadOnlyMemory<int> memory = Array.Empty<int>();

            // Act / Assert
            memory.Should().BeEmpty()
                .And.HaveCount(0)
                .And.NotContain(1);
        }

        [Fact]
        public void A_span_of_strings_supports_string_collection_assertions()
        {
            // Arrange
            Span<string> span = ["one", "two", "three"];

            // Act / Assert
            span.Should().HaveCount(3)
                .And.Contain("two")
                .And.NotContain("four")
                .And.ContainMatch("t*")
                .And.OnlyContain(s => s.Length >= 3);
        }

        [Fact]
        public void Chaining_on_a_span_is_supported()
        {
            // Arrange
            Span<int> span = new[] { 1, 2, 3 };

            // Act / Assert
            span.Should().HaveCount(3).And.Contain(2);
        }
    }

    public class Equality
    {
        [Fact]
        public void A_span_of_integers_can_be_asserted_for_equality()
        {
            // Arrange
            Span<int> span = [1, 2, 3];

            // Act / Assert
            span.Should().Equal(1, 2, 3);
            span.Should().BeEqualTo([1, 2, 3]);
        }

        [Fact]
        public void A_readonly_span_of_integers_can_be_asserted_for_equality()
        {
            // Arrange
            ReadOnlySpan<int> span = [1, 2, 3];

            // Act / Assert
            span.Should().Equal(1, 2, 3);
            span.Should().BeEqualTo([1, 2, 3]);
        }

        [Fact]
        public void A_span_be_equal_to_accepts_span()
        {
            // Arrange
            Span<int> span = [1, 2, 3];
            Span<int> expected = [1, 2, 3];

            // Act / Assert
            span.Should().BeEqualTo(expected);
        }

        [Fact]
        public void A_span_be_equal_to_accepts_readonly_span()
        {
            // Arrange
            Span<int> span = [1, 2, 3];
            ReadOnlySpan<int> expected = [1, 2, 3];

            // Act / Assert
            span.Should().BeEqualTo(expected);
        }

        [Fact]
        public void A_readonly_span_be_equal_to_accepts_readonly_span()
        {
            // Arrange
            ReadOnlySpan<int> span = [1, 2, 3];
            ReadOnlySpan<int> expected = [1, 2, 3];

            // Act / Assert
            span.Should().BeEqualTo(expected);
        }

        [Fact]
        public void A_span_of_integers_be_equal_to_should_throw_when_values_differ()
        {
            // Arrange
            Span<int> span = [1, 2, 3];

            try
            {
                // Act
                span.Should().BeEqualTo([1, 2, 4], "because {0}", "reasons");
            }
            catch (XunitException e)
            {
                // Assert
                e.Message.Should().Be(
                    "Expected span to be equal to {1, 2, 4} because reasons, but {1, 2, 3} differs at index 2.");
                return;
            }

            throw new XunitException("This point should not be reached.");
        }

        [Fact]
        public void A_readonly_span_of_integers_be_equal_to_should_throw_when_values_differ()
        {
            // Arrange
            ReadOnlySpan<int> span = [1, 2, 3];

            try
            {
                // Act
                span.Should().BeEqualTo([1, 2, 4]);
            }
            catch (XunitException e)
            {
                // Assert
                e.Message.Should().Be(
                    "Expected span to be equal to {1, 2, 4}, but {1, 2, 3} differs at index 2.");
                return;
            }

            throw new XunitException("This point should not be reached.");
        }
    }

    public class Strings
    {
        [Fact]
        public void A_span_of_chars_can_be_asserted_using_a_string()
        {
            // Arrange
            Span<char> span = ['a', 'b', 'c'];

            // Act / Assert
            span.Should().Be("abc");
        }

        [Fact]
        public void A_readonly_span_of_chars_can_be_asserted_using_a_string()
        {
            // Arrange
            ReadOnlySpan<char> span = ['a', 'b', 'c'];

            // Act / Assert
            span.Should().Be("abc");
        }

        [Fact]
        public void A_span_of_chars_be_should_throw_when_values_differ()
        {
            // Arrange
            Span<char> span = ['a', 'b', 'c'];

            try
            {
                // Act
                span.Should().Be("abd", "because {0}", "reasons");
            }
            catch (XunitException e)
            {
                // Assert
                e.Message.Should().Match("Expected span to be \"abd\" because reasons, but*");
                return;
            }

            throw new XunitException("This point should not be reached.");
        }

        [Fact]
        public void A_readonly_span_of_chars_be_should_throw_when_values_differ()
        {
            // Arrange
            ReadOnlySpan<char> span = ['a', 'b', 'c'];

            try
            {
                // Act
                span.Should().Be("abd");
            }
            catch (XunitException e)
            {
                // Assert
                e.Message.Should().Match("Expected span to be \"abd\", but*");
                return;
            }

            throw new XunitException("This point should not be reached.");
        }

        [Fact]
        public void A_span_of_chars_be_should_throw_when_expected_is_null()
        {
            // Arrange
            Span<char> span = ['a', 'b', 'c'];

            try
            {
                // Act
                span.Should().Be(null!);
            }
            catch (ArgumentNullException e)
            {
                // Assert
                e.ParamName.Should().Be("expected");
                return;
            }

            throw new XunitException("This point should not be reached.");
        }
    }
}

#endif
