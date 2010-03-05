using System.Diagnostics;

namespace FluentAssertions
{
    public static partial class FluentAssertionExtensions
    {
        #region Nested type: BooleanAssertions

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
                return BeFalse(string.Empty);
            }

            public AndConstraint<BooleanAssertions> BeFalse(string reason, params object[] reasonParameters)
            {
                VerifyThat(!ActualValue.Value, "Expected <{0}>{2}, but found <{1}>.", false, ActualValue, reason, reasonParameters);

                return new AndConstraint<BooleanAssertions>(this);
            }

            public AndConstraint<BooleanAssertions> BeTrue()
            {
                return BeTrue(string.Empty);
            }

            public AndConstraint<BooleanAssertions> BeTrue(string reason, params object[] reasonParameters)
            {
                VerifyThat(ActualValue.Value, "Expected <{0}>{2}, but found <{1}>.", true, ActualValue, reason, reasonParameters);

                return new AndConstraint<BooleanAssertions>(this);
            }

            public AndConstraint<BooleanAssertions> Equal(bool expected)
            {
                return Equal(expected, string.Empty);
            }

            public AndConstraint<BooleanAssertions> Equal(bool expected, string reason, params object[] reasonParameters)
            {
                VerifyThat(() => ActualValue.Value.Equals(expected), "Expected <{0}>{2}, but found <{1}>.", expected, ActualValue,
                           reason, reasonParameters);

                return new AndConstraint<BooleanAssertions>(this);
            }
        }

        #endregion

        #region Nested type: NullableBooleanAssertions

        [DebuggerNonUserCode]
        public class NullableBooleanAssertions : BooleanAssertions
        {
            internal NullableBooleanAssertions(bool? value)
                : base(value)
            {
            }

            public AndConstraint<NullableBooleanAssertions> HaveValue()
            {
                return HaveValue(string.Empty);
            }

            public AndConstraint<NullableBooleanAssertions> HaveValue(string reason, params object[] reasonParameters)
            {
                VerifyThat(ActualValue.HasValue, "Expected a value{2}.", null, ActualValue, reason, reasonParameters);

                return new AndConstraint<NullableBooleanAssertions>(this);
            }

            public AndConstraint<NullableBooleanAssertions> NotHaveValue()
            {
                return NotHaveValue(string.Empty);
            }

            public AndConstraint<NullableBooleanAssertions> NotHaveValue(string reason, params object[] reasonParameters)
            {
                VerifyThat(!ActualValue.HasValue, "Did not expect a value{2}, but found <{1}>.", null, ActualValue, reason,
                           reasonParameters);

                return new AndConstraint<NullableBooleanAssertions>(this);
            }
        }

        #endregion
    }
}