using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentAssertions.Formatting;
using Xunit;

namespace FluentAssertions.Specs
{
    
    public class TimeSpanFormatterSpecs
    {
        [Fact]
        public void When_rendering_empty_time_span_to_string_it_should_return_a_literal()
        {
            When_rendering_a_time_span_it_should_include_the_non_zero_parts_of(new TimeSpan(), "default");
        }

        [Fact]
        public void When_rendering_time_spans_to_string_it_should_only_include_the_nonzero_parts()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var testCases = new[]
            {
               new { Input = TimeSpan.FromDays(2), Output = "2d" },
               new { Input = new TimeSpan(0, 3, 20, 10, 58), Output = "3h, 20m and 10.058s" },
               new { Input = new TimeSpan(2, 0, 0, 12, 0), Output = "2d and 12s" },
               new { Input = TimeSpan.FromSeconds(90), Output = "1m and 30s" },
               new { Input = TimeSpan.FromMilliseconds(10), Output = "0.010s" },
               new { Input = TimeSpan.MinValue, Output = "min time span" },
               new { Input = TimeSpan.MaxValue, Output = "max time span" },
               new { Input = TimeSpan.FromTicks(1), Output = "0.1us" },
               new { Input = TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond - 1), Output = "999.9us" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            foreach (var testCase in testCases)
            {
                When_rendering_a_time_span_it_should_include_the_non_zero_parts_of(testCase.Input, testCase.Output);
            }
        }

        [Fact]
        public void When_rendering_negative_time_spans_to_string_it_should_only_include_the_nonzero_parts()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var testCases = new[]
            {
               new { Input = TimeSpan.FromDays(2).Negate(), Output = "-2d" },
               new { Input = new TimeSpan(0, 3, 20, 10, 58).Negate(), Output = "-3h, 20m and 10.058s" },
               new { Input = new TimeSpan(2, 0, 0, 12, 0).Negate(), Output = "-2d and 12s" },
               new { Input = TimeSpan.FromSeconds(90).Negate(), Output = "-1m and 30s" },
               new { Input = TimeSpan.FromMilliseconds(10).Negate(), Output = "-0.010s" },
               new { Input = TimeSpan.FromTicks(1).Negate(), Output = "-0.1us" },
               new { Input = TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond - 1).Negate(), Output = "-999.9us" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            foreach (var testCase in testCases)
            {
                When_rendering_a_time_span_it_should_include_the_non_zero_parts_of(testCase.Input, testCase.Output);
            }
        }

        private static void When_rendering_a_time_span_it_should_include_the_non_zero_parts_of(TimeSpan timeSpan, string expectedOutput)
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------            
            var formatter = new TimeSpanValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var result = formatter.ToString(timeSpan, useLineBreaks: false);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be(expectedOutput);
        }
    }
}
