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

            var indecies = Enumerable.Range(0, arr.Rank).Select(dimention => arr.GetLowerBound(dimention)).ToArray();

            var currentLoopIndex = 0;
            var enumerator = arr.GetEnumerator();

            // Emulate n-ary loop
            while (currentLoopIndex >= 0)
            {
                var loopValue = indecies[currentLoopIndex];

                if (IsFirstIteration(arr, loopValue, currentLoopIndex))
                {
                    sb.Append("{");
                }

                if (IsMostInnerLoop(arr, currentLoopIndex))
                {
                    enumerator.MoveNext();
                    sb.Append(formatChild(string.Join("-", indecies), enumerator.Current));
                    if (IsNotLastIteration(arr, loopValue, currentLoopIndex))
                        sb.Append(", ");
                    ++indecies[currentLoopIndex];       // Increment loop variable
                }
                else
                {
                    ++currentLoopIndex;
                    continue;
                }

                while (IsLastIteration(arr, loopValue, currentLoopIndex))
                {
                    sb.Append("}");
                    indecies[currentLoopIndex] = arr.GetLowerBound(currentLoopIndex);       // Reset current loop's variable to start value
                    --currentLoopIndex;                 // Move to outer loop

                    if (currentLoopIndex < 0)
                        break;

                    // update loopValue and loopMaxValue
                    loopValue = indecies[currentLoopIndex];
                    if (IsNotLastIteration(arr, loopValue, currentLoopIndex))
                        sb.Append(", ");
                    ++indecies[currentLoopIndex];       // Increment outer's loop variable
                }
            }

            return sb.ToString();
        }

        private bool IsFirstIteration(Array arr, int index, int dimention)
        {
            return index == arr.GetLowerBound(dimention);
        }

        private bool IsMostInnerLoop(Array arr, int index)
        {
            return index == arr.Rank - 1;
        }

        private bool IsLastIteration(Array arr, int index, int dimention)
        {
            return index >= arr.GetUpperBound(dimention);
        }

        private bool IsNotLastIteration(Array arr, int index, int dimention)
        {
            return !IsLastIteration(arr, index, dimention);
        }
    }
}
