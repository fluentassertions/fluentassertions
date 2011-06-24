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
        public bool? Subject { get; private set; }

        public AndConstraint<BooleanAssertions> BeFalse()
        {
            return BeFalse(String.Empty);
        }

        public AndConstraint<BooleanAssertions> BeFalse(string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.Value)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", false, Subject);

            return new AndConstraint<BooleanAssertions>(this);
        }

        public AndConstraint<BooleanAssertions> BeTrue()
        {
            return BeTrue(String.Empty);
        }

        public AndConstraint<BooleanAssertions> BeTrue(string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", true, Subject);

            return new AndConstraint<BooleanAssertions>(this);
        }

        public AndConstraint<BooleanAssertions> Be(bool expected)
        {
            return Be(expected, String.Empty);
        }

        public AndConstraint<BooleanAssertions> Be(bool expected, string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.Equals(expected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<BooleanAssertions>(this);
        }
    }
}