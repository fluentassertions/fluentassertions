using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    [DebuggerNonUserCode]
    public class BooleanAssertions
    {
        protected internal BooleanAssertions(bool? value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public bool? Subject
        {
            get; private set;
        }

        public AndConstraint<BooleanAssertions> BeFalse()
        {
            return BeFalse(String.Empty);
        }

        public AndConstraint<BooleanAssertions> BeFalse(string reason, params object[] reasonParameters)
        {
            Execute.Verify(!Subject.Value, "Expected {0}{2}, but found {1}.", false, Subject, reason, reasonParameters);

            return new AndConstraint<BooleanAssertions>(this);
        }

        public AndConstraint<BooleanAssertions> BeTrue()
        {
            return BeTrue(String.Empty);
        }

        public AndConstraint<BooleanAssertions> BeTrue(string reason, params object[] reasonParameters)
        {
            Execute.Verify(Subject.Value, "Expected {0}{2}, but found {1}.", true, Subject, reason, reasonParameters);

            return new AndConstraint<BooleanAssertions>(this);
        }

        public AndConstraint<BooleanAssertions> Be(bool expected)
        {
            return Be(expected, String.Empty);
        }

        public AndConstraint<BooleanAssertions> Be(bool expected, string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => Subject.Value.Equals(expected), "Expected {0}{2}, but found {1}.", expected, Subject,
                reason, reasonParameters);

            return new AndConstraint<BooleanAssertions>(this);
        }
    }
}