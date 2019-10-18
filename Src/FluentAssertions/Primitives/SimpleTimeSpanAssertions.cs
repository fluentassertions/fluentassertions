using System;
using System.Diagnostics;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="TimeSpan"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class SimpleTimeSpanAssertions
    {
        public SimpleTimeSpanAssertions(TimeSpan? value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public TimeSpan? Subject
        {
            get;
            private set;
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is greater than zero.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BePositive(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:time} to be positive{reason}, ")
                .ForCondition(Subject.HasValue)
                .FailWith("but found <null>.")
                .Then
                .ForCondition(Subject.Value.CompareTo(TimeSpan.Zero) > 0)
                .FailWith("but found {0}.", Subject.Value)
                .Then
                .ClearExpectation();

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is less than zero.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeNegative(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:time} to be negative{reason}, ")
                .ForCondition(Subject.HasValue)
                .FailWith("but found <null>.")
                .Then
                .ForCondition(Subject.Value.CompareTo(TimeSpan.Zero) < 0)
                .FailWith("but found {0}.", Subject.Value)
                .Then
                .ClearExpectation();

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is equal to the
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The expected time difference</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> Be(TimeSpan expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {0}{reason}, ", expected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found <null>.")
                .Then
                .ForCondition(Subject.Value.CompareTo(expected) == 0)
                .FailWith("but found {0}.", Subject.Value)
                .Then
                .ClearExpectation();

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is not equal to the
        /// specified <paramref name="unexpected"/> time.
        /// </summary>
        /// <param name="unexpected">The unexpected time difference</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> NotBe(TimeSpan unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue || Subject.Value.CompareTo(unexpected) != 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect {0}{reason}.", unexpected);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is less than the
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeLessThan(TimeSpan expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:time} to be less than {0}{reason}, ", expected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found <null>.")
                .Then
                .ForCondition(Subject.Value.CompareTo(expected) < 0)
                .FailWith("but found {0}.", Subject.Value)
                .Then
                .ClearExpectation();

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is less than or equal to the
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeLessOrEqualTo(TimeSpan expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:time} to be less or equal to {0}{reason}, ", expected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found <null>.")
                .Then
                .ForCondition(Subject.Value.CompareTo(expected) <= 0)
                .FailWith("but found {0}.", Subject.Value)
                .Then
                .ClearExpectation();

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is greater than the
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterThan(TimeSpan expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:time} to be greater than {0}{reason}, ", expected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found <null>.")
                .Then
                .ForCondition(Subject.Value.CompareTo(expected) > 0)
                .FailWith("but found {0}.", Subject.Value)
                .Then
                .ClearExpectation();

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is greater than or equal to the
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterOrEqualTo(TimeSpan expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:time} to be greater or equal to {0}{reason}, ", expected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found <null>.")
                .Then
                .ForCondition(Subject.Value.CompareTo(expected) >= 0)
                .FailWith("but found {0}.", Subject.Value)
                .Then
                .ClearExpectation();

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="TimeSpan"/> is within the specified number of milliseconds (default = 20 ms)
        /// from the specified <paramref name="nearbyTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="Be"/>.
        /// </remarks>
        /// <param name="nearbyTime">
        /// The expected time to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of milliseconds which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeCloseTo(TimeSpan nearbyTime, int precision = 20, string because = "",
            params object[] becauseArgs)
        {
            return BeCloseTo(nearbyTime, TimeSpan.FromMilliseconds(precision), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="TimeSpan"/> is within the specified time
        /// from the specified <paramref name="nearbyTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="Be"/>.
        /// </remarks>
        /// <param name="nearbyTime">
        /// The expected time to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of time which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeCloseTo(TimeSpan nearbyTime, TimeSpan precision, string because = "",
            params object[] becauseArgs)
        {
            TimeSpan minimumValue = nearbyTime - precision;
            TimeSpan maximumValue = nearbyTime + precision;

            Execute.Assertion
                .ForCondition(Subject.HasValue && (Subject.Value >= minimumValue) && (Subject.Value <= maximumValue))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:time} to be within {0} from {1}{reason}, but found {2}.",
                    precision,
                    nearbyTime, Subject ?? default(TimeSpan?));

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="TimeSpan"/> is not within the specified number of milliseconds (default = 20 ms)
        /// from the specified <paramref name="distantTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="NotBe"/>.
        /// </remarks>
        /// <param name="distantTime">
        /// The time to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of milliseconds which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> NotBeCloseTo(TimeSpan distantTime, int precision = 20, string because = "",
            params object[] becauseArgs)
        {
            return NotBeCloseTo(distantTime, TimeSpan.FromMilliseconds(precision), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="TimeSpan"/> is not within the specified time
        /// from the specified <paramref name="distantTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="NotBe"/>.
        /// </remarks>
        /// <param name="distantTime">
        /// The time to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of time which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> NotBeCloseTo(TimeSpan distantTime, TimeSpan precision, string because = "",
            params object[] becauseArgs)
        {
            TimeSpan minimumValue = distantTime - precision;
            TimeSpan maximumValue = distantTime + precision;

            Execute.Assertion
                .ForCondition(Subject.HasValue && !((Subject.Value >= minimumValue) && (Subject.Value <= maximumValue)))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:time} to not be within {0} from {1}{reason}, but found {2}.",
                    precision,
                    distantTime, Subject ?? default(TimeSpan?));

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }
    }
}
