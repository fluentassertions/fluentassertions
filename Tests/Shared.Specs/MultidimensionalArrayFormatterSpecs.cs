using System;
using System.Collections.Generic;
using FluentAssertions.Formatting;
using Xunit;

namespace FluentAssertions.Specs
{
    [Collection("FormatterSpecs")]
    public class MultidimensionalArrayFormatterSpecs
    {
        [Theory]
        [MemberData(nameof(MultiDimensionalArrayData))]
        public void When_formatting_a_multi_dimensional_array_it_should_show_structure(object value, string expected)
        {
            // Arrange

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Match(expected);
        }

        public static IEnumerable<object[]> MultiDimensionalArrayData =>
            new List<object[]>
            {
                new object[] { new int[0, 0], "{empty}" },
                new object[] { new int[,] {{1, 2}, {3, 4}}, "{{1, 2}, {3, 4}}" },
                new object[] { new int[,,] {{{1, 2, 3}, {4, 5, 6}}, {{7, 8, 9}, {10, 11, 12}}}, "{{{1, 2, 3}, {4, 5, 6}}, {{7, 8, 9}, {10, 11, 12}}}" },
            };

        [Fact]
        public void When_formatting_a_multi_dimensional_array_with_bounds_it_should_show_structure()
        {
            // Arrange
            var lengthsArray = new int[] { 2, 3, 4 };
            var boundsArray = new int[] { 1, 5, 7 };
            var value = Array.CreateInstance(typeof(string), lengthsArray, boundsArray);
            for (int i = value.GetLowerBound(0); i <= value.GetUpperBound(0); i++)
            {
                for (int j = value.GetLowerBound(1); j <= value.GetUpperBound(1); j++)
                {
                    for (int k = value.GetLowerBound(2); k <= value.GetUpperBound(2); k++)
                    {
                        var indices = new int[] { i, j, k };
                        value.SetValue($"{i}-{j}-{k}", indices);
                    }
                }
            }

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Match("{{{'1-5-7', '1-5-8', '1-5-9', '1-5-10'}, {'1-6-7', '1-6-8', '1-6-9', '1-6-10'}, {'1-7-7', '1-7-8', '1-7-9', '1-7-10'}}, {{'2-5-7', '2-5-8', '2-5-9', '2-5-10'}, {'2-6-7', '2-6-8', '2-6-9', '2-6-10'}, {'2-7-7', '2-7-8', '2-7-9', '2-7-10'}}}".Replace("'", "\""));
        }
    }
}
