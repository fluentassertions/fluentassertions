using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentAssertions.Formatting;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class TimeSpanFormatterSpecs
    {
        [TestMethod]
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
               new { Input = TimeSpan.FromMilliseconds(10), Output = "0.010s" }
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
            var result = formatter.ToString(timeSpan);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be(expectedOutput);
        }
    }
}
