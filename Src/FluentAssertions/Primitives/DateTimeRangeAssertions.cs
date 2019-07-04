using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions.Execution;
using FluentAssertions.Localization;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that two <see cref="DateTime"/> objects differ in the expected way.
    /// </summary>
    /// <remarks>
    /// You can use the <see cref="FluentAssertions.Extensions.FluentDateTimeExtensions"/> and
    /// <see cref="FluentAssertions.Extensions.FluentTimeSpanExtensions"/> for a more fluent
    /// way of specifying a <see cref="DateTime"/> or a <see cref="TimeSpan"/>.
    /// </remarks>
    [DebuggerNonUserCode]
    public class DateTimeRangeAssertions
    {
        #region Private Definitions

        private readonly TimeSpanCondition condition;
        private readonly DateTimeAssertions parentAssertions;
        private readonly TimeSpanPredicate predicate;

        private readonly Dictionary<TimeSpanCondition, TimeSpanPredicate> predicates = new Dictionary
            <TimeSpanCondition, TimeSpanPredicate>
        {
            [TimeSpanCondition.MoreThan] = new TimeSpanPredicate((ts1, ts2) => ts1 > ts2),
            [TimeSpanCondition.AtLeast] = new TimeSpanPredicate((ts1, ts2) => ts1 >= ts2),
            [TimeSpanCondition.Exactly] = new TimeSpanPredicate((ts1, ts2) => ts1 == ts2),
            [TimeSpanCondition.Within] = new TimeSpanPredicate((ts1, ts2) => ts1 <= ts2),
            [TimeSpanCondition.LessThan] = new TimeSpanPredicate((ts1, ts2) => ts1 < ts2)
        };

        private readonly DateTime? subject;
        private readonly TimeSpan timeSpan;

        #endregion

        protected internal DateTimeRangeAssertions(DateTimeAssertions parentAssertions, DateTime? subject,
            TimeSpanCondition condition,
            TimeSpan timeSpan)
        {
            this.parentAssertions = parentAssertions;
            this.subject = subject;
            this.condition = condition;
            this.timeSpan = timeSpan;

            predicate = predicates[condition];
        }

        /// <summary>
        /// Asserts that a <see cref="DateTime"/> occurs a specified amount of time before another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="target">
        /// The <see cref="DateTime"/> to compare the subject with.
        /// </param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<DateTimeAssertions> Before(DateTime target, string because = "",
            params object[] becauseArgs)
        {
            bool success = Execute.Assertion
                .ForCondition(subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(GetBeforeStringFormatMessage() + Resources.DateTime_CommaButFoundANullDateTime,
                    subject, timeSpan, target);

            if (success)
            {
                TimeSpan actual = target - subject.Value;

                if (!predicate.IsMatchedBy(actual, timeSpan))
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith(GetBeforeStringFormatMessage() + Resources.Common_CommaButItDiffersX3Format,
                            subject, timeSpan, target, actual);
                }
            }

            return new AndConstraint<DateTimeAssertions>(parentAssertions);
        }

        /// <summary>
        /// Asserts that a <see cref="DateTime"/> occurs a specified amount of time after another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="target">
        /// The <see cref="DateTime"/> to compare the subject with.
        /// </param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<DateTimeAssertions> After(DateTime target, string because = "",
            params object[] becauseArgs)
        {
            bool success = Execute.Assertion
                .ForCondition(subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(GetAfterStringFormatMessage() + Resources.DateTime_CommaButFoundANullDateTime,
                    subject, timeSpan, target);

            if (success)
            {
                TimeSpan actual = subject.Value - target;

                if (!predicate.IsMatchedBy(actual, timeSpan))
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith(GetAfterStringFormatMessage() + Resources.Common_CommaButItDiffersX3Format,
                            subject, timeSpan, target, actual);
                }
            }

            return new AndConstraint<DateTimeAssertions>(parentAssertions);
        }

        private string GetBeforeStringFormatMessage()
        {
            switch (condition)
            {
                case TimeSpanCondition.AtLeast:
                    return Resources.DateTime_ExpectedDateAndOrTimeX0ToBeAtLeastX1BeforeX2Format;

                case TimeSpanCondition.MoreThan:
                    return Resources.DateTime_ExpectedDateAndOrTimeX0ToBeMoreThanX1BeforeX2Format;

                case TimeSpanCondition.Exactly:
                    return Resources.DateTime_ExpectedDateAndOrTimeX0ToBeExactlyX1BeforeX2Format;

                case TimeSpanCondition.Within:
                    return Resources.DateTime_ExpectedDateAndOrTimeX0ToBeWithinX1BeforeX2Format;

                case TimeSpanCondition.LessThan:
                    return Resources.DateTime_ExpectedDateAndOrTimeX0ToBeLessThanX1BeforeX2Format;

                default:
                    throw new InvalidOperationException(); // TODO: Amaury - check exception type + add message
            }
        }

        private string GetAfterStringFormatMessage()
        {
            switch (condition)
            {
                case TimeSpanCondition.AtLeast:
                    return Resources.DateTime_ExpectedDateAndOrTimeX0ToBeAtLeastX1AfterX2Format;

                case TimeSpanCondition.MoreThan:
                    return Resources.DateTime_ExpectedDateAndOrTimeX0ToBeMoreThanX1AfterX2Format;

                case TimeSpanCondition.Exactly:
                    return Resources.DateTime_ExpectedDateAndOrTimeX0ToBeExactlyX1AfterX2Format;

                case TimeSpanCondition.Within:
                    return Resources.DateTime_ExpectedDateAndOrTimeX0ToBeWithinX1AfterX2Format;

                case TimeSpanCondition.LessThan:
                    return Resources.DateTime_ExpectedDateAndOrTimeX0ToBeLessThanX1AfterX2Format;

                default:
                    throw new InvalidOperationException(); // TODO: Amaury - check exception type + add message
            }
        }
    }
}
