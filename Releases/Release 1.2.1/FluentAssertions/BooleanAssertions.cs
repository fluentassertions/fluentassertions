using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class BooleanAssertions : Assertions<bool?, BooleanAssertions>
    {
        protected BooleanAssertions(bool? value)
        {
            Subject = value;
        }

        internal BooleanAssertions(bool value)
        {
            Subject = value;
        }

        public AndConstraint<BooleanAssertions> BeFalse()
        {
            return BeFalse(String.Empty);
        }

        public AndConstraint<BooleanAssertions> BeFalse(string reason, params object[] reasonParameters)
        {
            VerifyThat(!Subject.Value, "Expected {0}{2}, but found {1}.", false, Subject, reason, reasonParameters);

            return new AndConstraint<BooleanAssertions>(this);
        }

        public AndConstraint<BooleanAssertions> BeTrue()
        {
            return BeTrue(String.Empty);
        }

        public AndConstraint<BooleanAssertions> BeTrue(string reason, params object[] reasonParameters)
        {
            VerifyThat(Subject.Value, "Expected {0}{2}, but found {1}.", true, Subject, reason, reasonParameters);

            return new AndConstraint<BooleanAssertions>(this);
        }

        public AndConstraint<BooleanAssertions> Be(bool expected)
        {
            return Be(expected, String.Empty);
        }

        public AndConstraint<BooleanAssertions> Be(bool expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => Subject.Value.Equals(expected), "Expected {0}{2}, but found {1}.", expected, Subject,
                reason, reasonParameters);

            return new AndConstraint<BooleanAssertions>(this);
        }
    }
}