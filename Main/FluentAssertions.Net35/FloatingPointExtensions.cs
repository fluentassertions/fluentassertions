using System;

using FluentAssertions.Assertions;

namespace FluentAssertions
{
    /// <summary>
    /// Contains a number of extension methods for floating point <see cref="NumericAssertions{T}"/>.
    /// </summary>
    public static class FloatingPointExtensions
    {
        /// <summary>
        /// Asserts that the floating point value is exactly the same as the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expected">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <remarks>
        /// Beware that floating point math is not exact. Simple values like 0.2 cannot be precisely represented
        /// using binary floating point numbers, and the limited precision of floating point numbers means that
        /// slight changes in the order of operations can change the result. Different compilers and CPU architectures store
        /// temporary results at different precisions, so results will differ depending on the details of your
        /// environment. If you do a calculation and then compare the results against some expected value it is highly
        /// unlikely that you will get exactly the result you intended.<br />
        /// Source: http://www.cygnus-software.com/papers/comparingfloats/comparingfloats.htm. <br />
        /// It might be better to use <see cref="BeApproximately(FluentAssertions.Assertions.NumericAssertions{float},float,float)"/>
        /// </remarks>
        public static AndConstraint<NumericAssertions<float>> Be(this NumericAssertions<float> parent,
            float expected)
        {
            return Be(parent, expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the floating point value is exactly the same as the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expected">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        /// <remarks>
        /// Beware that floating point math is not exact. Simple values like 0.2 cannot be precisely represented
        /// using binary floating point numbers, and the limited precision of floating point numbers means that
        /// slight changes in the order of operations can change the result. Different compilers and CPU architectures store
        /// temporary results at different precisions, so results will differ depending on the details of your
        /// environment. If you do a calculation and then compare the results against some expected value it is highly
        /// unlikely that you will get exactly the result you intended.<br />
        /// Source: http://www.cygnus-software.com/papers/comparingfloats/comparingfloats.htm. <br />
        /// It might be better to use <see cref="BeApproximately(FluentAssertions.Assertions.NumericAssertions{float},float,float)"/>
        /// </remarks>
        public static AndConstraint<NumericAssertions<float>> Be(this NumericAssertions<float> parent,
            float expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(ReferenceEquals(parent.Subject, expected) || (parent.Subject.CompareTo(expected) == 0))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected value {0} to be exactly {1}{reason}.", parent.Subject, expected);

            return new AndConstraint<NumericAssertions<float>>(parent);
        }

        /// <summary>
        /// Asserts that the floating point value is exactly the same as the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expected">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <remarks>
        /// Beware that floating point math is not exact. Simple values like 0.2 cannot be precisely represented
        /// using binary floating point numbers, and the limited precision of floating point numbers means that
        /// slight changes in the order of operations can change the result. Different compilers and CPU architectures store
        /// temporary results at different precisions, so results will differ depending on the details of your
        /// environment. If you do a calculation and then compare the results against some expected value it is highly
        /// unlikely that you will get exactly the result you intended.<br />
        /// Source: http://www.cygnus-software.com/papers/comparingfloats/comparingfloats.htm. <br />
        /// It might be better to use <see cref="BeApproximately(FluentAssertions.Assertions.NumericAssertions{System.Nullable{float}},float,float)"/>
        /// </remarks>
        public static AndConstraint<NumericAssertions<float?>> Be(this NumericAssertions<float?> parent,
            float expected)
        {
            return Be(parent, expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the floating point value is exactly the same as the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expected">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        /// <remarks>
        /// Beware that floating point math is not exact. Simple values like 0.2 cannot be precisely represented
        /// using binary floating point numbers, and the limited precision of floating point numbers means that
        /// slight changes in the order of operations can change the result. Different compilers and CPU architectures store
        /// temporary results at different precisions, so results will differ depending on the details of your
        /// environment. If you do a calculation and then compare the results against some expected value it is highly
        /// unlikely that you will get exactly the result you intended.<br />
        /// Source: http://www.cygnus-software.com/papers/comparingfloats/comparingfloats.htm. <br />
        /// It might be better to use <see cref="BeApproximately(FluentAssertions.Assertions.NumericAssertions{System.Nullable{float}},float,float)"/>
        /// </remarks>
        public static AndConstraint<NumericAssertions<float?>> Be(this NumericAssertions<float?> parent,
            float expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(parent.Subject != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected value to be exactly {0}{reason}, but it was <null>.", expected);

            var nonNullableAssertions = new NumericAssertions<float>(expected);
            nonNullableAssertions.Be(expected, reason, reasonArgs);

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

            var nonNullableAssertions = new NumericAssertions<float>(expectedValue);
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
        /// Asserts that the double value is exactly the same as the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expected">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <remarks>
        /// Beware that floating point math is not exact. Simple values like 0.2 cannot be precisely represented
        /// using binary floating point numbers, and the limited precision of floating point numbers means that
        /// slight changes in the order of operations can change the result. Different compilers and CPU architectures store
        /// temporary results at different precisions, so results will differ depending on the details of your
        /// environment. If you do a calculation and then compare the results against some expected value it is highly
        /// unlikely that you will get exactly the result you intended.<br />
        /// Source: http://www.cygnus-software.com/papers/comparingfloats/comparingfloats.htm. <br />
        /// It might be better to use <see cref="BeApproximately(FluentAssertions.Assertions.NumericAssertions{double},double,double)"/>
        /// </remarks>
        public static AndConstraint<NumericAssertions<double>> Be(this NumericAssertions<double> parent,
            double expected)
        {
            return Be(parent, expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the double value is exactly the same as the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expected">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        /// <remarks>
        /// Beware that floating point math is not exact. Simple values like 0.2 cannot be precisely represented
        /// using binary floating point numbers, and the limited precision of floating point numbers means that
        /// slight changes in the order of operations can change the result. Different compilers and CPU architectures store
        /// temporary results at different precisions, so results will differ depending on the details of your
        /// environment. If you do a calculation and then compare the results against some expected value it is highly
        /// unlikely that you will get exactly the result you intended.<br />
        /// Source: http://www.cygnus-software.com/papers/comparingfloats/comparingfloats.htm. <br />
        /// It might be better to use <see cref="BeApproximately(FluentAssertions.Assertions.NumericAssertions{double},double,double)"/>
        /// </remarks>
        public static AndConstraint<NumericAssertions<double>> Be(this NumericAssertions<double> parent,
            double expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(ReferenceEquals(parent.Subject, expected) || (parent.Subject.CompareTo(expected) == 0))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected value {0} to be exactly {1}{reason}.", parent.Subject, expected);

            return new AndConstraint<NumericAssertions<double>>(parent);
        }

        /// <summary>
        /// Asserts that the double value is exactly the same as the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expected">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <remarks>
        /// Beware that floating point math is not exact. Simple values like 0.2 cannot be precisely represented
        /// using binary floating point numbers, and the limited precision of floating point numbers means that
        /// slight changes in the order of operations can change the result. Different compilers and CPU architectures store
        /// temporary results at different precisions, so results will differ depending on the details of your
        /// environment. If you do a calculation and then compare the results against some expected value it is highly
        /// unlikely that you will get exactly the result you intended.<br />
        /// Source: http://www.cygnus-software.com/papers/comparingfloats/comparingfloats.htm. <br />
        /// It might be better to use <see cref="BeApproximately(FluentAssertions.Assertions.NumericAssertions{System.Nullable{double}},double,double)"/>
        /// </remarks>
        public static AndConstraint<NumericAssertions<double?>> Be(this NumericAssertions<double?> parent,
            double expected)
        {
            return Be(parent, expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the double value is exactly the same as the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="parent">The <see cref="NumericAssertions{T}"/> object that is being extended.</param>
        /// <param name="expected">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        /// <remarks>
        /// Beware that floating point math is not exact. Simple values like 0.2 cannot be precisely represented
        /// using binary floating point numbers, and the limited precision of floating point numbers means that
        /// slight changes in the order of operations can change the result. Different compilers and CPU architectures store
        /// temporary results at different precisions, so results will differ depending on the details of your
        /// environment. If you do a calculation and then compare the results against some expected value it is highly
        /// unlikely that you will get exactly the result you intended.<br />
        /// Source: http://www.cygnus-software.com/papers/comparingfloats/comparingfloats.htm. <br />
        /// It might be better to use <see cref="BeApproximately(FluentAssertions.Assertions.NumericAssertions{System.Nullable{double}},double,double)"/>
        /// </remarks>
        public static AndConstraint<NumericAssertions<double?>> Be(this NumericAssertions<double?> parent,
            double expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(parent.Subject != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected value to be exactly {0}{reason}, but it was <null>.", expected);

            var nonNullableAssertions = new NumericAssertions<double>(expected);
            nonNullableAssertions.Be(expected, reason, reasonArgs);

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

            var nonNullableAssertions = new NumericAssertions<double>(expectedValue);
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