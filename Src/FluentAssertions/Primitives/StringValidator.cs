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

        private const int HumanReadableLength = 8;

        protected string Subject { get; }

        protected string Expected { get; }

        protected IAssertionScope Assertion { get; set; }

        #endregion

        protected StringValidator(string subject, string expected, string because, object[] becauseArgs)
        {
            Assertion = Execute.Assertion.BecauseOf(because, becauseArgs);

            Subject = subject;
            Expected = expected;
        }

        public void Validate()
        {
            if ((Expected is not null) || (Subject is not null))
            {
                if (ValidateAgainstNulls())
                {
                    if (IsLongOrMultiline(Expected) || IsLongOrMultiline(Subject))
                    {
                        Assertion = Assertion.UsingLineBreaks;
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
            if ((Expected is null) != (Subject is null))
            {
                Assertion.FailWith(ExpectationDescription + "{0}{reason}, but found {1}.", Expected, Subject);
                return false;
            }

            return true;
        }

        private static bool IsLongOrMultiline(string value)
        {
            return (value.Length > HumanReadableLength) || value.Contains(Environment.NewLine, StringComparison.Ordinal);
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
