using System;

namespace FluentAssertions
{
    public class FloatAssertions : NumericAssertions<float>
    {
        public FloatAssertions(float? value) : base(value)
        {
        }

        /// <summary>
        /// Asserts a floating point value approximates another value as close as possible.
        /// </summary>
        /// <param name="expectedValue">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of which the two values may differ.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        public AndConstraint<FloatAssertions> BeApproximately(float expectedValue, float precision, string reason,
            params object[] reasonArgs)
        {
            float actualDifference = Math.Abs(expectedValue - Subject.Value);

            if (actualDifference > precision)
            {
                Verification.Fail(
                    "Expected value <" + Subject + "> to approximate <" + expectedValue +
                        "> +/- {0}{2}, but it differed by {1}.",
                    precision, actualDifference, reason, reasonArgs);
            }

            return new AndConstraint<FloatAssertions>(this);
        }
    }
}