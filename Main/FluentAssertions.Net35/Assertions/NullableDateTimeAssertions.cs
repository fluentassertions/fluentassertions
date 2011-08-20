using System;
using System.Diagnostics;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="DateTime"/> is in the expected state.
    /// </summary>
    /// <remarks>
    /// You can use the <see cref="FluentDateTimeExtensions"/> for a more fluent way of specifying a <see cref="DateTime"/>.
    /// </remarks>
    [DebuggerNonUserCode]
    public class NullableDateTimeAssertions : DateTimeAssertions
    {
        protected internal NullableDateTimeAssertions(DateTime? expected)
            : base(expected)
        {
        }

        public AndConstraint<NullableDateTimeAssertions> HaveValue()
        {
            return HaveValue(string.Empty);
        }

        public AndConstraint<NullableDateTimeAssertions> HaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected variable to have a value{reason}, but found {0}", Subject);

            return new AndConstraint<NullableDateTimeAssertions>(this);
        }

        public AndConstraint<NullableDateTimeAssertions> NotHaveValue()
        {
            return NotHaveValue(string.Empty);
        }

        public AndConstraint<NullableDateTimeAssertions> NotHaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect variable to have a value{reason}, but found {0}", Subject);
            
            return new AndConstraint<NullableDateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> Be(DateTime? expected)
        {
            return Be(expected, string.Empty);
        }

        public AndConstraint<DateTimeAssertions> Be(DateTime? expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}", expected, Subject);

            return new AndConstraint<DateTimeAssertions>(this);
        }
    }
}