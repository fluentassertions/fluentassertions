using System.Diagnostics;

namespace FluentAssertions
{
    public static partial class CustomAssertionExtensions
    {
        #region Nested type: BooleanAssertions

        [DebuggerNonUserCode]
        public class BooleanAssertions : Assertions
        {
            protected readonly bool? actualValue;

            protected BooleanAssertions(bool? value)
            {
                actualValue = value;
            }

            internal BooleanAssertions(bool value)
            {
                actualValue = value;
            }

            public AndConstraint<BooleanAssertions> BeFalse()
            {
                return BeFalse(string.Empty);
            }

            public AndConstraint<BooleanAssertions> BeFalse(string reason, params object[] reasonParameters)
            {
                AssertThat(!actualValue.Value, "Expected <{0}>{2}, but found <{1}>.", false, actualValue, reason, reasonParameters);

                return new AndConstraint<BooleanAssertions>(this);
            }

            public AndConstraint<BooleanAssertions> BeTrue()
            {
                return BeTrue(string.Empty);
            }

            public AndConstraint<BooleanAssertions> BeTrue(string reason, params object[] reasonParameters)
            {
                AssertThat(actualValue.Value, "Expected <{0}>{2}, but found <{1}>.", true, actualValue, reason, reasonParameters);

                return new AndConstraint<BooleanAssertions>(this);
            }

            public AndConstraint<BooleanAssertions> Equal(bool expected)
            {
                return Equal(expected, string.Empty);
            }

            public AndConstraint<BooleanAssertions> Equal(bool expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => actualValue.Value.Equals(expected), "Expected <{0}>{2}, but found <{1}>.", expected, actualValue,
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
                AssertThat(actualValue.HasValue, "Expected a value{2}.", null, actualValue, reason, reasonParameters);

                return new AndConstraint<NullableBooleanAssertions>(this);
            }

            public AndConstraint<NullableBooleanAssertions> NotHaveValue()
            {
                return NotHaveValue(string.Empty);
            }

            public AndConstraint<NullableBooleanAssertions> NotHaveValue(string reason, params object[] reasonParameters)
            {
                AssertThat(!actualValue.HasValue, "Did not expect a value{2}, but found <{1}>.", null, actualValue, reason,
                           reasonParameters);

                return new AndConstraint<NullableBooleanAssertions>(this);
            }
        }

        #endregion
    }
}