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

            if (arr.Length <= 0)
            {
                return "{empty}";
            }

            var sb = new StringBuilder();

            var indecies = Enumerable.Range(0, arr.Rank).Select(dimention => arr.GetLowerBound(dimention)).ToArray();

            bool IsFirstIteration(int index, int dimention)
            {
                return index == arr.GetLowerBound(dimention);
            }

            bool IsMostInnerLoop(int index)
            {
                return index == arr.Rank - 1;
            }

            bool IsLastIteration(int index, int dimention)
            {
                return index >= arr.GetUpperBound(dimention);
            }

            bool IsNotLastIteration(int index, int dimention)
            {
                return !IsLastIteration(index, dimention);
            }

            var currentLoopIndex = 0;
            var enumerator = arr.GetEnumerator();

            // Emulate n-ary loop
            while (currentLoopIndex >= 0)
            {
                var loopValue = indecies[currentLoopIndex];

                if (IsFirstIteration(loopValue, currentLoopIndex))
                {
                    sb.Append("{");
                }

                if (IsMostInnerLoop(currentLoopIndex))
                {
                    enumerator.MoveNext();
                    sb.Append(formatChild(string.Join("-", indecies), enumerator.Current));
                    if (IsNotLastIteration(loopValue, currentLoopIndex))
                        sb.Append(", ");
                    ++indecies[currentLoopIndex];       // Increment loop variable
                }
                else
                {
                    ++currentLoopIndex;
                    continue;
                }

                while (IsLastIteration(loopValue, currentLoopIndex))
                {
                    sb.Append("}");
                    indecies[currentLoopIndex] = arr.GetLowerBound(currentLoopIndex);       // Reset current loop's variable to start value
                    --currentLoopIndex;                 // Move to outer loop

                    if (currentLoopIndex < 0)
                        break;

                    // update loopValue and loopMaxValue
                    loopValue = indecies[currentLoopIndex];
                    if (IsNotLastIteration(loopValue, currentLoopIndex))
                        sb.Append(", ");
                    ++indecies[currentLoopIndex];       // Increment outer's loop variable
                }
            }

            return sb.ToString();
        }
    }
}
