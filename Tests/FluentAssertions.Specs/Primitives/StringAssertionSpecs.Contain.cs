using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The [Not]Contain specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class Contain
    {
        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_string_contains_the_expected_string_it_should_not_throw()
        {
            // Arrange
            string actual = "ABCDEF";
            string expectedSubstring = "BCD";

            // Act / Assert
            actual.Should().Contain(expectedSubstring);
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_string_does_not_contain_an_expected_string_it_should_throw()
        {
            // Act
            Action act = () => "ABCDEF".Should().Contain("XYZ", "that is {0}", "required");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string \"ABCDEF\" to contain \"XYZ\" because that is required.");
        }

        [Fact]
        public void When_containment_is_asserted_against_null_it_should_throw()
        {
            // Act
            Action act = () => "a".Should().Contain(null);

            // Assert
            act
                .Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert string containment against <null>.*")
                .WithParameterName("expected");
        }

        [Fact]
        public void When_containment_is_asserted_against_an_empty_string_it_should_throw()
        {
            // Act
            Action act = () => "a".Should().Contain("");

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot assert string containment against an empty string.*")
                .WithParameterName("expected");
        }

        [Fact]
        public void When_string_containment_is_asserted_and_actual_value_is_null_then_it_should_throw()
        {
            // Act
            string someString = null;
            Action act = () => someString.Should().Contain("XYZ", "that is {0}", "required");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString <null> to contain \"XYZ\" because that is required.");
        }

        public class ContainExactly
        {
            [Fact]
            public void
                When_string_containment_once_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_throw()
            {
                // Arrange
                string actual = "ABCDEF";
                string expectedSubstring = "XYS";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, Exactly.Once(), "that is {0}", "required");

                // Assert
                act.Should().Throw<XunitException>()
                    .WithMessage(
                        "Expected * \"ABCDEF\" to contain \"XYS\" exactly 1 time because that is required, but found it 0 times.");
            }

            [Fact]
            public void When_containment_once_is_asserted_against_null_it_should_throw_earlier()
            {
                // Arrange
                string actual = "a";
                string expectedSubstring = null;

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, Exactly.Once());

                // Assert
                act
                    .Should().Throw<ArgumentNullException>()
                    .WithMessage("Cannot assert string containment against <null>.*");
            }

            [Fact]
            public void When_string_containment_once_is_asserted_and_actual_value_is_null_then_it_should_throw()
            {
                // Arrange
                string actual = null;
                string expectedSubstring = "XYZ";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, Exactly.Once());

                // Assert
                act.Should().Throw<XunitException>()
                    .WithMessage("Expected * <null> to contain \"XYZ\" exactly 1 time, but found it 0 times.");
            }

            [Fact]
            public void When_string_containment_exactly_is_asserted_and_expected_value_is_negative_it_should_throw()
            {
                // Arrange
                string actual = "ABCDEBCDF";
                string expectedSubstring = "BCD";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, Exactly.Times(-1));

                // Assert
                act.Should().Throw<ArgumentOutOfRangeException>()
                    .WithMessage("Expected count cannot be negative.*");
            }

            [Fact]
            public void
                When_string_containment_exactly_is_asserted_and_actual_value_contains_the_expected_string_exactly_expected_times_it_should_not_throw()
            {
                // Arrange
                string actual = "ABCDEBCDF";
                string expectedSubstring = "BCD";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, Exactly.Times(2));

                // Assert
                act.Should().NotThrow();
            }

            [Fact]
            public void
                When_string_containment_exactly_is_asserted_and_actual_value_contains_the_expected_string_but_not_exactly_expected_times_it_should_throw()
            {
                // Arrange
                string actual = "ABCDEBCDF";
                string expectedSubstring = "BCD";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, Exactly.Times(3));

                // Assert
                act.Should().Throw<XunitException>()
                    .WithMessage("Expected * \"ABCDEBCDF\" to contain \"BCD\" exactly 3 times, but found it 2 times.");
            }
        }

        public class ContainAtLeast
        {
            [Fact]
            public void
                When_string_containment_at_least_is_asserted_and_actual_value_contains_the_expected_string_at_least_expected_times_it_should_not_throw()
            {
                // Arrange
                string actual = "ABCDEBCDF";
                string expectedSubstring = "BCD";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, AtLeast.Times(2));

                // Assert
                act.Should().NotThrow();
            }

            [Fact]
            public void
                When_string_containment_at_least_is_asserted_and_actual_value_contains_the_expected_string_but_not_at_least_expected_times_it_should_throw()
            {
                // Arrange
                string actual = "ABCDEBCDF";
                string expectedSubstring = "BCD";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, AtLeast.Times(3));

                // Assert
                act.Should().Throw<XunitException>()
                    .WithMessage("Expected * \"ABCDEBCDF\" to contain \"BCD\" at least 3 times, but found it 2 times.");
            }

            [Fact]
            public void
                When_string_containment_at_least_once_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_throw_earlier()
            {
                // Arrange
                string actual = "ABCDEF";
                string expectedSubstring = "XYS";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, AtLeast.Once());

                // Assert
                act.Should().Throw<XunitException>()
                    .WithMessage("Expected * \"ABCDEF\" to contain \"XYS\" at least 1 time, but found it 0 times.");
            }

            [Fact]
            public void When_string_containment_at_least_once_is_asserted_and_actual_value_is_null_then_it_should_throw()
            {
                // Arrange
                string actual = null;
                string expectedSubstring = "XYZ";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, AtLeast.Once());

                // Assert
                act.Should().Throw<XunitException>()
                    .WithMessage("Expected * <null> to contain \"XYZ\" at least 1 time, but found it 0 times.");
            }
        }

        public class ContainMoreThan
        {
            [Fact]
            public void
                When_string_containment_more_than_is_asserted_and_actual_value_contains_the_expected_string_more_than_expected_times_it_should_not_throw()
            {
                // Arrange
                string actual = "ABCDEBCDF";
                string expectedSubstring = "BCD";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, MoreThan.Times(1));

                // Assert
                act.Should().NotThrow();
            }

            [Fact]
            public void
                When_string_containment_more_than_is_asserted_and_actual_value_contains_the_expected_string_but_not_more_than_expected_times_it_should_throw()
            {
                // Arrange
                string actual = "ABCDEBCDF";
                string expectedSubstring = "BCD";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, MoreThan.Times(2));

                // Assert
                act.Should().Throw<XunitException>()
                    .WithMessage("Expected * \"ABCDEBCDF\" to contain \"BCD\" more than 2 times, but found it 2 times.");
            }

            [Fact]
            public void
                When_string_containment_more_than_once_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_throw()
            {
                // Arrange
                string actual = "ABCDEF";
                string expectedSubstring = "XYS";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, MoreThan.Once());

                // Assert
                act.Should().Throw<XunitException>()
                    .WithMessage("Expected * \"ABCDEF\" to contain \"XYS\" more than 1 time, but found it 0 times.");
            }

            [Fact]
            public void When_string_containment_more_than_once_is_asserted_and_actual_value_is_null_then_it_should_throw()
            {
                // Arrange
                string actual = null;
                string expectedSubstring = "XYZ";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, MoreThan.Once());

                // Assert
                act.Should().Throw<XunitException>()
                    .WithMessage("Expected * <null> to contain \"XYZ\" more than 1 time, but found it 0 times.");
            }
        }

        public class ContainAtMost
        {
            [Fact]
            public void
                When_string_containment_at_most_is_asserted_and_actual_value_contains_the_expected_string_at_most_expected_times_it_should_not_throw()
            {
                // Arrange
                string actual = "ABCDEBCDF";
                string expectedSubstring = "BCD";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, AtMost.Times(2));

                // Assert
                act.Should().NotThrow();
            }

            [Fact]
            public void
                When_string_containment_at_most_is_asserted_and_actual_value_contains_the_expected_string_but_not_at_most_expected_times_it_should_throw()
            {
                // Arrange
                string actual = "ABCDEBCDF";
                string expectedSubstring = "BCD";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, AtMost.Times(1));

                // Assert
                act.Should().Throw<XunitException>()
                    .WithMessage("Expected * \"ABCDEBCDF\" to contain \"BCD\" at most 1 time, but found it 2 times.");
            }

            [Fact]
            public void
                When_string_containment_at_most_once_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_not_throw()
            {
                // Arrange
                string actual = "ABCDEF";
                string expectedSubstring = "XYS";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, AtMost.Once());

                // Assert
                act.Should().NotThrow();
            }

            [Fact]
            public void When_string_containment_at_most_once_is_asserted_and_actual_value_is_null_then_it_should_not_throw()
            {
                // Arrange
                string actual = null;
                string expectedSubstring = "XYZ";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, AtMost.Once());

                // Assert
                act.Should().NotThrow();
            }
        }

        public class ContainsLessThan
        {
            [Fact]
            public void
                When_string_containment_less_than_is_asserted_and_actual_value_contains_the_expected_string_less_than_expected_times_it_should_not_throw()
            {
                // Arrange
                string actual = "ABCDEBCDF";
                string expectedSubstring = "BCD";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, LessThan.Times(3));

                // Assert
                act.Should().NotThrow();
            }

            [Fact]
            public void
                When_string_containment_less_than_is_asserted_and_actual_value_contains_the_expected_string_but_not_less_than_expected_times_it_should_throw()
            {
                // Arrange
                string actual = "ABCDEBCDF";
                string expectedSubstring = "BCD";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, LessThan.Times(2));

                // Assert
                act.Should().Throw<XunitException>()
                    .WithMessage("Expected * \"ABCDEBCDF\" to contain \"BCD\" less than 2 times, but found it 2 times.");
            }

            [Fact]
            public void
                When_string_containment_less_than_twice_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_not_throw()
            {
                // Arrange
                string actual = "ABCDEF";
                string expectedSubstring = "XYS";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, LessThan.Twice());

                // Assert
                act.Should().NotThrow();
            }

            [Fact]
            public void When_string_containment_less_than_once_is_asserted_and_actual_value_is_null_then_it_should_not_throw()
            {
                // Arrange
                string actual = null;
                string expectedSubstring = "XYZ";

                // Act
                Action act = () => actual.Should().Contain(expectedSubstring, LessThan.Twice());

                // Assert
                act.Should().NotThrow();
            }
        }
    }

    public class NotContain
    {
        [Fact]
        public void When_string_does_not_contain_the_unexpected_string_it_should_succeed()
        {
            // Act
            Action act = () => "a".Should().NotContain("A");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_string_contains_unexpected_fragment_it_should_throw()
        {
            // Act
            Action act = () => "abcd".Should().NotContain("bc", "it was not expected {0}", "today");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect string \"abcd\" to contain \"bc\" because it was not expected today.");
        }

        [Fact]
        public void When_exclusion_is_asserted_against_null_it_should_throw()
        {
            // Act
            Action act = () => "a".Should().NotContain(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert string containment against <null>.*")
                .WithParameterName("unexpected");
        }

        [Fact]
        public void When_exclusion_is_asserted_against_an_empty_string_it_should_throw()
        {
            // Act
            Action act = () => "a".Should().NotContain("");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Cannot assert string containment against an empty string.*")
                .WithParameterName("unexpected");
        }
    }
}
