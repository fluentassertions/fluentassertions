using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="TimeSpan"/> is in the expected state.
    /// </summary>
    /// <remarks>
    /// You can use the <see cref="TimeSpanConversionExtensions"/> for a more fluent way of specifying a <see cref="TimeSpan"/>.
    /// </remarks>
    [DebuggerNonUserCode]
    public class NullableSimpleTimeSpanAssertions : SimpleTimeSpanAssertions
    {
        protected internal NullableSimpleTimeSpanAssertions(TimeSpan? value)
            : base(value)
        {
        }

        public AndConstraint<NullableSimpleTimeSpanAssertions> HaveValue()
        {
            return HaveValue(String.Empty);
        }

        public AndConstraint<NullableSimpleTimeSpanAssertions> HaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value{reason}.");

            return new AndConstraint<NullableSimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<NullableSimpleTimeSpanAssertions> NotHaveValue()
        {
            return NotHaveValue(String.Empty);
        }

        public AndConstraint<NullableSimpleTimeSpanAssertions> NotHaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect a value{reason}, but found {0}.", Subject);

            return new AndConstraint<NullableSimpleTimeSpanAssertions>(this);
        }
    }
}