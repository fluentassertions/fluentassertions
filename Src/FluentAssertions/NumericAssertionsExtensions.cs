using System;
using FluentAssertions.Execution;
using FluentAssertions.Numeric;

namespace FluentAssertions
{
    /// <summary>
    /// Contains a number of extension methods for floating point <see cref="NumericAssertions{T}"/>.
    /// </summary>
    public static class NumericAssertionsExtensions
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
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public static AndConstraint<NullableNumericAssertions<float>> BeApproximately(this NullableNumericAssertions<float> parent,
            float expectedValue, float precision, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(parent.Subject != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to approximate {0} +/- {1}{reason}, but it was <null>.", expectedValue, precision);

            var nonNullableAssertions = new NumericAssertions<float>((float)parent.Subject);
            nonNullableAssertions.BeApproximately(expectedValue, precision, because, becauseArgs);

            return new AndConstraint<NullableNumericAssertions<float>>(parent);
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
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public static AndConstraint<NumericAssertions<float>> BeApproximately(this NumericAssertions<float> parent,
            float expectedValue, float precision, string because = "",
            params object[] becauseArgs)
        {
            float actualDifference = Math.Abs(expectedValue - (float)parent.Subject);

            Execute.Assertion
                .ForCondition(actualDifference <= precision)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to approximate {1} +/- {2}{reason}, but {0} differed by {3}.",
                    parent.Subject, expectedValue, precision, actualDifference);

            return new AndConstraint<NumericAssertions<float>>(parent);
        }

        /// <summary>
        /// Asserts a double value approximates another value as close as possible.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expectedValue">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public static AndConstraint<NullableNumericAssertions<double>> BeApproximately(this NullableNumericAssertions<double> parent,
            double expectedValue, double precision, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(parent.Subject != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to approximate {0} +/- {1}{reason}, but it was <null>.", expectedValue, precision);

            var nonNullableAssertions = new NumericAssertions<double>((double)parent.Subject);
            BeApproximately(nonNullableAssertions, expectedValue, precision, because, becauseArgs);

            return new AndConstraint<NullableNumericAssertions<double>>(parent);
        }

        /// <summary>
        /// Asserts a double value approximates another value as close as possible.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expectedValue">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public static AndConstraint<NumericAssertions<double>> BeApproximately(this NumericAssertions<double> parent,
            double expectedValue, double precision, string because = "",
            params object[] becauseArgs)
        {
            double actualDifference = Math.Abs(expectedValue - (double)parent.Subject);

            Execute.Assertion
                .ForCondition(actualDifference <= precision)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to approximate {1} +/- {2}{reason}, but {0} differed by {3}.",
                    parent.Subject, expectedValue, precision, actualDifference);

            return new AndConstraint<NumericAssertions<double>>(parent);
        }

        /// <summary>
        /// Asserts a decimal value approximates another value as close as possible.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expectedValue">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public static AndConstraint<NullableNumericAssertions<decimal>> BeApproximately(this NullableNumericAssertions<decimal> parent,
            decimal expectedValue, decimal precision, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(parent.Subject != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to approximate {0} +/- {1}{reason}, but it was <null>.", expectedValue, precision);

            var nonNullableAssertions = new NumericAssertions<decimal>((decimal)parent.Subject);
            BeApproximately(nonNullableAssertions, expectedValue, precision, because, becauseArgs);

            return new AndConstraint<NullableNumericAssertions<decimal>>(parent);
        }

        /// <summary>
        /// Asserts a decimal value approximates another value as close as possible.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expectedValue">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public static AndConstraint<NumericAssertions<decimal>> BeApproximately(this NumericAssertions<decimal> parent,
            decimal expectedValue, decimal precision, string because = "",
            params object[] becauseArgs)
        {
            decimal actualDifference = Math.Abs(expectedValue - (decimal)parent.Subject);

            Execute.Assertion
                .ForCondition(actualDifference <= precision)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to approximate {1} +/- {2}{reason}, but {0} differed by {3}.",
                    parent.Subject, expectedValue, precision, actualDifference);

            return new AndConstraint<NumericAssertions<decimal>>(parent);
        }

        /// <summary>
        /// Asserts a floating point value does not approximate another value by a given amount.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="unexpectedValue">
        /// The unexpected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The minimum exclusive amount of which the two values should differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public static AndConstraint<NullableNumericAssertions<float>> NotBeApproximately(this NullableNumericAssertions<float> parent,
            float unexpectedValue, float precision, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(parent.Subject != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to not approximate {0} +/- {1}{reason}, but it was <null>.", unexpectedValue, precision);

            var nonNullableAssertions = new NumericAssertions<float>((float)parent.Subject);
            nonNullableAssertions.NotBeApproximately(unexpectedValue, precision, because, becauseArgs);

            return new AndConstraint<NullableNumericAssertions<float>>(parent);
        }

        /// <summary>
        /// Asserts a floating point value does not approximate another value by a given amount.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="unexpectedValue">
        /// The unexpected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The minimum exclusive amount of which the two values should differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public static AndConstraint<NumericAssertions<float>> NotBeApproximately(this NumericAssertions<float> parent,
            float unexpectedValue, float precision, string because = "",
            params object[] becauseArgs)
        {
            float actualDifference = Math.Abs(unexpectedValue - (float)parent.Subject);

            Execute.Assertion
                .ForCondition(actualDifference > precision)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to not approximate {1} +/- {2}{reason}, but {0} only differed by {3}.",
                    parent.Subject, unexpectedValue, precision, actualDifference);

            return new AndConstraint<NumericAssertions<float>>(parent);
        }

        /// <summary>
        /// Asserts a double value does not approximate another value by a given amount.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="unexpectedValue">
        /// The unexpected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The minimum exclusive amount of which the two values should differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public static AndConstraint<NullableNumericAssertions<double>> NotBeApproximately(this NullableNumericAssertions<double> parent,
            double unexpectedValue, double precision, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(parent.Subject != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to not approximate {0} +/- {1}{reason}, but it was <null>.", unexpectedValue, precision);

            var nonNullableAssertions = new NumericAssertions<double>((double)parent.Subject);
            NotBeApproximately(nonNullableAssertions, unexpectedValue, precision, because, becauseArgs);

            return new AndConstraint<NullableNumericAssertions<double>>(parent);
        }

        /// <summary>
        /// Asserts a double value does not approximate another value by a given amount.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="unexpectedValue">
        /// The unexpected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The minimum exclusive amount of which the two values should differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public static AndConstraint<NumericAssertions<double>> NotBeApproximately(this NumericAssertions<double> parent,
            double unexpectedValue, double precision, string because = "",
            params object[] becauseArgs)
        {
            double actualDifference = Math.Abs(unexpectedValue - (double)parent.Subject);

            Execute.Assertion
                .ForCondition(actualDifference > precision)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to not approximate {1} +/- {2}{reason}, but {0} only differed by {3}.",
                    parent.Subject, unexpectedValue, precision, actualDifference);

            return new AndConstraint<NumericAssertions<double>>(parent);
        }

        /// <summary>
        /// Asserts a decimal value does not approximate another value by a given amount.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="unexpectedValue">
        /// The unexpected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The minimum exclusive amount of which the two values should differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public static AndConstraint<NullableNumericAssertions<decimal>> NotBeApproximately(this NullableNumericAssertions<decimal> parent,
            decimal unexpectedValue, decimal precision, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(parent.Subject != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to not approximate {0} +/- {1}{reason}, but it was <null>.", unexpectedValue, precision);

            var nonNullableAssertions = new NumericAssertions<decimal>((decimal)parent.Subject);
            NotBeApproximately(nonNullableAssertions, unexpectedValue, precision, because, becauseArgs);

            return new AndConstraint<NullableNumericAssertions<decimal>>(parent);
        }

        /// <summary>
        /// Asserts a decimal value does not approximate another value by a given amount.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="unexpectedValue">
        /// The unexpected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The minimum exclusive amount of which the two values should differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public static AndConstraint<NumericAssertions<decimal>> NotBeApproximately(this NumericAssertions<decimal> parent,
            decimal unexpectedValue, decimal precision, string because = "",
            params object[] becauseArgs)
        {
            decimal actualDifference = Math.Abs(unexpectedValue - (decimal)parent.Subject);

            Execute.Assertion
                .ForCondition(actualDifference > precision)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to not approximate {1} +/- {2}{reason}, but {0} only differed by {3}.",
                    parent.Subject, unexpectedValue, precision, actualDifference);

            return new AndConstraint<NumericAssertions<decimal>>(parent);
        }
    }
}
