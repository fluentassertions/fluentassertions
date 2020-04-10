using System;
using System.Diagnostics;
using FluentAssertions.Common;

namespace FluentAssertions.Specialized
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="Action"/> yields the expected result.
    /// </summary>
    [DebuggerNonUserCode]
    public class ActionAssertions : DelegateAssertions<Action, ActionAssertions>
    {
        public ActionAssertions(Action subject, IExtractExceptions extractor) : this(subject, extractor, new Clock())
        {
        }

        public ActionAssertions(Action subject, IExtractExceptions extractor, IClock clock) : base(subject, extractor, clock)
        {
        }

        protected override void InvokeSubject()
        {
            Subject();
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Identifier => "action";
    }
}
