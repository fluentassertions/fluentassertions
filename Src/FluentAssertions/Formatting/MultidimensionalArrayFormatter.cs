using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    public class MultidimensionalArrayFormatter : IValueFormatter
    {
        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return value is Array arr && arr.Rank >= 2;
        }

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            var arr = (Array)value;

            if (arr.Length == 0)
            {
                return "{empty}";
            }

            var sb = new StringBuilder();

            int[] dimensionIndices = Enumerable.Range(0, arr.Rank).Select(dimension => arr.GetLowerBound(dimension)).ToArray();

            int currentLoopIndex = 0;
            IEnumerator enumerator = arr.GetEnumerator();

            // Emulate n-ary loop
            while (currentLoopIndex >= 0)
            {
                int currentDimensionIndex = dimensionIndices[currentLoopIndex];

                if (IsFirstIteration(arr, currentDimensionIndex, currentLoopIndex))
                {
                    sb.Append('{');
                }

                if (IsInnerMostLoop(arr, currentLoopIndex))
                {
                    enumerator.MoveNext();
                    sb.Append(formatChild(string.Join("-", dimensionIndices), enumerator.Current));
                    if (!IsLastIteration(arr, currentDimensionIndex, currentLoopIndex))
                        sb.Append(", ");

                    ++dimensionIndices[currentLoopIndex];
                }
                else
                {
                    ++currentLoopIndex;
                    continue;
                }

                while (IsLastIteration(arr, currentDimensionIndex, currentLoopIndex))
                {
                    sb.Append('}');
                    // Reset current loop's variable to start value ...and move to outer loop
                    dimensionIndices[currentLoopIndex] = arr.GetLowerBound(currentLoopIndex);
                    --currentLoopIndex;

                    if (currentLoopIndex < 0)
                        break;

                    currentDimensionIndex = dimensionIndices[currentLoopIndex];
                    if (!IsLastIteration(arr, currentDimensionIndex, currentLoopIndex))
                        sb.Append(", ");

                    ++dimensionIndices[currentLoopIndex];
                }
            }

            return sb.ToString();
        }

        private bool IsFirstIteration(Array arr, int index, int dimension)
        {
            return index == arr.GetLowerBound(dimension);
        }

        private bool IsInnerMostLoop(Array arr, int index)
        {
            return index == arr.Rank - 1;
        }

        private bool IsLastIteration(Array arr, int index, int dimension)
        {
            return index >= arr.GetUpperBound(dimension);
        }
    }
}
