using System;
using System.Diagnostics;
using System.Globalization;

namespace FluentAssertions
{
    public static partial class CustomAssertionExtensions
    {
        #region Nested type: StringAssertions

        [DebuggerNonUserCode]
        public class StringAssertions : Assertions
        {
            private readonly string actualValue;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:System.Object"/> class.
            /// </summary>
            internal StringAssertions(string value)
            {
                actualValue = value;
            }

            public AndConstraint<StringAssertions> Equal(string expected)
            {
                return Equal(expected, string.Empty);
            }

            public AndConstraint<StringAssertions> Equal(string expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => (actualValue == expected),
                           "Expected <{0}>{2}, but found <{1}>.", expected, actualValue, reason, reasonParameters);

                return new AndConstraint<StringAssertions>(this);
            }

            /// <summary>
            /// Case insensitive comparison
            /// </summary>
            public AndConstraint<StringAssertions> BeEquivalentTo(string expected)
            {
                return BeEquivalentTo(expected, string.Empty);
            }

            /// <summary>
            /// Case insensitive comparison
            /// </summary>
            public AndConstraint<StringAssertions> BeEquivalentTo(string expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => (String.Compare(actualValue, expected, true, CultureInfo.CurrentCulture) == 0),
                           "Expected string equivalent to <{0}>{2}, but found <{1}>.", expected, actualValue, reason, reasonParameters);

                return new AndConstraint<StringAssertions>(this);
            }

            public AndConstraint<StringAssertions> NotEqual(string expected)
            {
                return NotEqual(expected, string.Empty);
            }

            public AndConstraint<StringAssertions> NotEqual(string expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => (actualValue != expected),
                           "Did not expect <{0}>{2}.", expected, actualValue, reason, reasonParameters);

                return new AndConstraint<StringAssertions>(this);
            }

            public AndConstraint<StringAssertions> StartWith(string expected)
            {
                return StartWith(expected, string.Empty);
            }

            public AndConstraint<StringAssertions> StartWith(string expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => actualValue.StartsWith(expected),
                           "Expected string starting with <{0}>{2}, but found <{1}>.", expected, actualValue, reason, reasonParameters);

                return new AndConstraint<StringAssertions>(this);
            }

            public AndConstraint<StringAssertions> StartWithEquivalent(string expected)
            {
                return StartWithEquivalent(expected, string.Empty);
            }

            public AndConstraint<StringAssertions> StartWithEquivalent(string expected, string reason,
                                                                       params object[] reasonParameters)
            {
                AssertThat(() => actualValue.StartsWith(expected, StringComparison.CurrentCultureIgnoreCase),
                           "Expected string starting with equivalent of <{0}>{2}, but found <{1}>.", expected, actualValue, reason,
                           reasonParameters);

                return new AndConstraint<StringAssertions>(this);
            }

            public AndConstraint<StringAssertions> EndWith(string expected)
            {
                return EndWith(expected, string.Empty);
            }

            public AndConstraint<StringAssertions> EndWith(string expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => actualValue.EndsWith(expected),
                           "Expected string ending with <{0}>{2}, but found <{1}>.", expected, actualValue, reason, reasonParameters);

                return new AndConstraint<StringAssertions>(this);
            }

            public AndConstraint<StringAssertions> EndWithEquivalent(string expected)
            {
                return EndWithEquivalent(expected, string.Empty);
            }

            public AndConstraint<StringAssertions> EndWithEquivalent(string expected, string reason,
                                                                     params object[] reasonParameters)
            {
                AssertThat(() => actualValue.EndsWith(expected, StringComparison.CurrentCultureIgnoreCase),
                           "Expected string ending with equivalent of <{0}>{2}, but found <{1}>.", expected, actualValue, reason,
                           reasonParameters);

                return new AndConstraint<StringAssertions>(this);
            }

            public AndConstraint<StringAssertions> Contain(string expected)
            {
                return Contain(expected, string.Empty);
            }

            public AndConstraint<StringAssertions> Contain(string expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => actualValue.Contains(expected),
                           "Expected string containing <{0}>{2}, but found <{1}>.", expected, actualValue, reason,
                           reasonParameters);

                return new AndConstraint<StringAssertions>(this);
            }

            public AndConstraint<StringAssertions> BeEmpty()
            {
                return BeEmpty(string.Empty);
            }

            public AndConstraint<StringAssertions> BeEmpty(string reason, params object[] reasonParameters)
            {
                AssertThat(() => (actualValue.Length == 0),
                           "Expected empty string{2}, but found <{1}>.", null, actualValue, reason,
                           reasonParameters);

                return new AndConstraint<StringAssertions>(this);
            }

            public AndConstraint<StringAssertions> NotBeEmpty()
            {
                return NotBeEmpty(string.Empty);
            }

            public AndConstraint<StringAssertions> NotBeEmpty(string reason, params object[] reasonParameters)
            {
                AssertThat(() => (actualValue.Length > 0),
                           "Did not expect empty string{2}.", null, actualValue, reason, reasonParameters);

                return new AndConstraint<StringAssertions>(this);
            }

            public AndConstraint<StringAssertions> HaveLength(int expected)
            {
                return HaveLength(expected, string.Empty);
            }

            public AndConstraint<StringAssertions> HaveLength(int expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => (actualValue.Length == expected),
                           "Expected string with length <{0}>{2}, but found string <{1}>.", expected, actualValue, reason, reasonParameters);

                return new AndConstraint<StringAssertions>(this);
            }
        }

        #endregion
    }
}