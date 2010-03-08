using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class StringAssertions : Assertions<string, StringAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        internal StringAssertions(string value)
        {
            ActualValue = value;
        }

        public AndConstraint<StringAssertions> Equal(string expected)
        {
            return Equal(expected, String.Empty);
        }

        public AndConstraint<StringAssertions> Equal(string expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue == expected),
                "Expected <{0}>{2}, but found <{1}>.", expected, ActualValue, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Case insensitive comparison
        /// </summary>
        public AndConstraint<StringAssertions> BeEquivalentTo(string expected)
        {
            return BeEquivalentTo(expected, String.Empty);
        }

        /// <summary>
        /// Case insensitive comparison
        /// </summary>
        public AndConstraint<StringAssertions> BeEquivalentTo(string expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (String.Compare(ActualValue, expected, StringComparison.CurrentCultureIgnoreCase) == 0),
                "Expected string equivalent to <{0}>{2}, but found <{1}>.", expected, ActualValue, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> NotEqual(string expected)
        {
            return NotEqual(expected, String.Empty);
        }

        public AndConstraint<StringAssertions> NotEqual(string expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue != expected),
                "Did not expect <{0}>{2}.", expected, ActualValue, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> StartWith(string expected)
        {
            return StartWith(expected, String.Empty);
        }

        public AndConstraint<StringAssertions> StartWith(string expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.StartsWith(expected),
                "Expected string starting with <{0}>{2}, but found <{1}>.", expected, ActualValue, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> StartWithEquivalent(string expected)
        {
            return StartWithEquivalent(expected, String.Empty);
        }

        public AndConstraint<StringAssertions> StartWithEquivalent(string expected, string reason,
            params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.StartsWith(expected, StringComparison.CurrentCultureIgnoreCase),
                "Expected string starting with equivalent of <{0}>{2}, but found <{1}>.", expected, ActualValue, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> EndWith(string expected)
        {
            return EndWith(expected, String.Empty);
        }

        public AndConstraint<StringAssertions> EndWith(string expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.EndsWith(expected),
                "Expected string ending with <{0}>{2}, but found <{1}>.", expected, ActualValue, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> EndWithEquivalent(string expected)
        {
            return EndWithEquivalent(expected, String.Empty);
        }

        public AndConstraint<StringAssertions> EndWithEquivalent(string expected, string reason,
            params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.EndsWith(expected, StringComparison.CurrentCultureIgnoreCase),
                "Expected string ending with equivalent of <{0}>{2}, but found <{1}>.", expected, ActualValue, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> Contain(string expected)
        {
            return Contain(expected, String.Empty);
        }

        public AndConstraint<StringAssertions> Contain(string expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.Contains(expected),
                "Expected string containing <{0}>{2}, but found <{1}>.", expected, ActualValue, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> BeEmpty()
        {
            return BeEmpty(String.Empty);
        }

        public AndConstraint<StringAssertions> BeEmpty(string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Length == 0),
                "Expected empty string{2}, but found <{1}>.", null, ActualValue, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> NotBeEmpty()
        {
            return NotBeEmpty(String.Empty);
        }

        public AndConstraint<StringAssertions> NotBeEmpty(string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Length > 0),
                "Did not expect empty string{2}.", null, ActualValue, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> HaveLength(int expected)
        {
            return HaveLength(expected, String.Empty);
        }

        public AndConstraint<StringAssertions> HaveLength(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Length == expected),
                "Expected string with length <{0}>{2}, but found string <{1}>.", expected, ActualValue, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }
    }
}