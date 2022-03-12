using System;
using FluentAssertions.Extensions;
using Xunit;

#if NET6_0_OR_GREATER
namespace FluentAssertions.Specs.Extensions
{
    public class DateOnlyConversionExtensionSpecs
    {
        [Fact]
        public void When_specifying_a_time_before_another_dateonly_it_should_return_the_correct_time()
        {
            // Act
            DateOnly dateOnly = new(2011, 07, 13);

            DateOnly twoDaysAgo = 2.Days().Before(dateOnly);

            // Assert
            twoDaysAgo.Should().Be(new DateOnly(2011, 7, 11));
        }

        [Fact]
        public void When_specifying_a_time_after_another_dateonly_it_should_return_the_correct_time()
        {
            // Act
            DateOnly dateOnly = new(2011, 07, 13);

            DateOnly twoDaysAgo = 2.Days().After(dateOnly);

            // Assert
            twoDaysAgo.Should().Be(new DateOnly(2011, 7, 15));
        }
    }
}
#endif
