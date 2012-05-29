using System;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of extension methods for floating point <see cref="NumericAssertions{T}"/>.
    /// </summary>
    public static class FloatingPointExtensions
    {
        /// <summary>
        /// Asserts a floating point value approximates another value as close as possible.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expectedValue">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of which the two values may differ.
        /// </param>
        public static AndConstraint<NumericAssertions<float?>> BeApproximately(this NumericAssertions<float?> parent,
            float expectedValue, float precision)
        {
            return BeApproximately(parent, expectedValue, precision, string.Empty);
        }

        /// <summary>
        /// Asserts a floating point value approximates another value as close as possible.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
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
        public static AndConstraint<NumericAssertions<float?>> BeApproximately(this NumericAssertions<float?> parent,
            float expectedValue, float precision, string reason,
            params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(parent.Subject != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected value to approximate {0} +/- {1}{reason}, but it was <null>.", expectedValue, precision);

            var nonNullableAssertions = new NumericAssertions<float>((float) parent.Subject);
            nonNullableAssertions.BeApproximately(expectedValue, precision, reason, reasonArgs);

            return new AndConstraint<NumericAssertions<float?>>(parent);
        }

        /// <summary>
        /// Asserts a floating point value approximates another value as close as possible.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expectedValue">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of which the two values may differ.
        /// </param>
        public static AndConstraint<NumericAssertions<float>> BeApproximately(this NumericAssertions<float> parent,
            float expectedValue, float precision)
        {
            return BeApproximately(parent, expectedValue, precision, string.Empty);
        }

        /// <summary>
        /// Asserts a floating point value approximates another value as close as possible.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
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
        public static AndConstraint<NumericAssertions<float>> BeApproximately(this NumericAssertions<float> parent,
            float expectedValue, float precision, string reason,
            params object [] reasonArgs)
        {
            float actualDifference = Math.Abs(expectedValue - (float) parent.Subject);

            if (actualDifference > precision)
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected value {0} to approximate {1} +/- {2}{reason}, but it differed by {3}.",
                        parent.Subject, expectedValue, precision, actualDifference);
            }

            return new AndConstraint<NumericAssertions<float>>(parent);
        }

        /// <summary>
        /// Asserts a floating point value approximates another value as close as possible.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expectedValue">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of which the two values may differ.
        /// </param>
        public static AndConstraint<NumericAssertions<double?>> BeApproximately(this NumericAssertions<double?> parent,
            double expectedValue, double precision)
        {
            return BeApproximately(parent, expectedValue, precision, string.Empty);
        }

        /// <summary>
        /// Asserts a floating point value approximates another value as close as possible.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
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
        public static AndConstraint<NumericAssertions<double?>> BeApproximately(this NumericAssertions<double?> parent,
            double expectedValue, double precision, string reason,
            params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(parent.Subject != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected value to approximate {0} +/- {1}{reason}, but it was <null>.", expectedValue, precision);

            var nonNullableAssertions = new NumericAssertions<double>((double) parent.Subject);
            BeApproximately(nonNullableAssertions, expectedValue, precision, reason, reasonArgs);

            return new AndConstraint<NumericAssertions<double?>>(parent);
        }

        /// <summary>
        /// Asserts a floating point value approximates another value as close as possible.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expectedValue">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of which the two values may differ.
        /// </param>
        public static AndConstraint<NumericAssertions<double>> BeApproximately(this NumericAssertions<double> parent,
            double expectedValue, double precision)
        {
            return BeApproximately(parent, expectedValue, precision, string.Empty);
        }

        /// <summary>
        /// Asserts a floating point value approximates another value as close as possible.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
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
        public static AndConstraint<NumericAssertions<double>> BeApproximately(this NumericAssertions<double> parent,
            double expectedValue, double precision, string reason,
            params object [] reasonArgs)
        {
            double actualDifference = Math.Abs(expectedValue - (double) parent.Subject);

            Execute.Verification
                .ForCondition(actualDifference <= precision)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected value {0} to approximate {1} +/- {2}{reason}, but it differed by {3}.",
                    parent.Subject, expectedValue, precision, actualDifference);

            return new AndConstraint<NumericAssertions<double>>(parent);
        }
    }
}