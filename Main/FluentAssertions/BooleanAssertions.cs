using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class BooleanAssertions : Assertions<bool?, BooleanAssertions>
    {
        protected BooleanAssertions(bool? value)
        {
            ActualValue = value;
        }

        internal BooleanAssertions(bool value)
        {
            ActualValue = value;
        }

        public AndConstraint<BooleanAssertions> BeFalse()
        {
            return BeFalse(String.Empty);
        }

        public AndConstraint<BooleanAssertions> BeFalse(string reason, params object[] reasonParameters)
        {
            VerifyThat(!ActualValue.Value, "Expected <{0}>{2}, but found <{1}>.", false, ActualValue, reason, reasonParameters);

            return new AndConstraint<BooleanAssertions>(this);
        }

        public AndConstraint<BooleanAssertions> BeTrue()
        {
            return BeTrue(String.Empty);
        }

        public AndConstraint<BooleanAssertions> BeTrue(string reason, params object[] reasonParameters)
        {
            VerifyThat(ActualValue.Value, "Expected <{0}>{2}, but found <{1}>.", true, ActualValue, reason, reasonParameters);

            return new AndConstraint<BooleanAssertions>(this);
        }

        public AndConstraint<BooleanAssertions> Equal(bool expected)
        {
            return Equal(expected, String.Empty);
        }

        public AndConstraint<BooleanAssertions> Equal(bool expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.Value.Equals(expected), "Expected <{0}>{2}, but found <{1}>.", expected, ActualValue,
                reason, reasonParameters);

            return new AndConstraint<BooleanAssertions>(this);
        }
    }
}