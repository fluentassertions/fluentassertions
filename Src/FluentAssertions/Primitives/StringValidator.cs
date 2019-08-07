using System;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Dedicated class for comparing two strings and generating consistent error messages.
    /// </summary>
    internal abstract class StringValidator
    {
        #region Private Definition

        protected readonly string subject;
        protected readonly string expected;
        protected IAssertionScope assertion;
        private const int HumanReadableLength = 8;

        #endregion

        protected StringValidator(string subject, string expected, string because, object[] becauseArgs)
        {
            assertion = Execute.Assertion.BecauseOf(because, becauseArgs);

            this.subject = subject;
            this.expected = expected;
        }

        public void Validate()
        {
            if ((expected != null) || (subject != null))
            {
                if (ValidateAgainstNulls())
                {
                    if (IsLongOrMultiline(expected) || IsLongOrMultiline(subject))
                    {
                        assertion = assertion.UsingLineBreaks;
                    }

                    if (ValidateAgainstSuperfluousWhitespace())
                    {
                        if (ValidateAgainstLengthDifferences())
                        {
                            ValidateAgainstMismatch();
                        }
                    }
                }
            }
        }

        private bool ValidateAgainstNulls()
        {
            if ((expected is null) ^ (subject is null))
            {
                assertion.FailWith(ExpectationDescription + "{0}{reason}, but found {1}.", expected, subject);
                return false;
            }

            return true;
        }

        private static bool IsLongOrMultiline(string value)
        {
            return (value.Length > HumanReadableLength) || value.Contains(Environment.NewLine);
        }

        protected virtual bool ValidateAgainstSuperfluousWhitespace()
        {
            return true;
        }

        protected virtual bool ValidateAgainstLengthDifferences()
        {
            return true;
        }

        protected abstract void ValidateAgainstMismatch();

        protected abstract string ExpectationDescription { get; }
    }
}
