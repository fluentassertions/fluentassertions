using System;

namespace FluentAssertions
{
    public static class FloatingPointExtensions
    {
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
        public static AndConstraint<NumericAssertions<float>> BeApproximately(this NumericAssertions<float> parent, float expectedValue, float precision)
        {
            return BeApproximately(parent, expectedValue, precision, "");
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
        public static AndConstraint<NumericAssertions<float>> BeApproximately(this NumericAssertions<float> parent, float expectedValue, float precision, string reason,
            params object[] reasonArgs)
        {
            float actualDifference = Math.Abs(expectedValue - (float) parent.Subject);

            if (actualDifference > precision)
            {
                Verification.Fail(
                    "Expected value <" + parent.Subject + "> to approximate <" + expectedValue +
                        "> +/- {0}{2}, but it differed by {1}.",
                    precision, actualDifference, reason, reasonArgs);
            }

            return new AndConstraint<NumericAssertions<float>>(parent);
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
        public static AndConstraint<NumericAssertions<double>> BeApproximately(this NumericAssertions<double> parent, double expectedValue, double precision)
        {
            return BeApproximately(parent, expectedValue, precision, "");
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
        public static AndConstraint<NumericAssertions<double>> BeApproximately(this NumericAssertions<double> parent, double expectedValue, double precision, string reason,
            params object[] reasonArgs)
        {
            double actualDifference = Math.Abs(expectedValue - (double) parent.Subject);

            if (actualDifference > precision)
            {
                Verification.Fail(
                    "Expected value <" + parent.Subject + "> to approximate <" + expectedValue +
                        "> +/- {0}{2}, but it differed by {1}.",
                    precision, actualDifference, reason, reasonArgs);
            }

            return new AndConstraint<NumericAssertions<double>>(parent);
        }
    }
}