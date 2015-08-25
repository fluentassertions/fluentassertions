using System.Reflection;

namespace FluentAssertions.Equivalency
{
    internal class AssertionContext<TSubject> : IAssertionContext<TSubject>
    {
        public AssertionContext(SelectedMemberInfo subjectProperty, TSubject subject, TSubject expectation, string because,
                                object[] reasonArgs)
        {
            SubjectProperty = subjectProperty;
            Subject = subject;
            Expectation = expectation;
            Reason = because;
            ReasonArgs = reasonArgs;
        }

        public SelectedMemberInfo SubjectProperty { get; private set; }
        public TSubject Subject { get; private set; }
        public TSubject Expectation { get; private set; }
        public string Reason { get; set; }
        public object[] ReasonArgs { get; set; }

        internal static AssertionContext<TSubject> CreateFromEquivalencyValidationContext(IEquivalencyValidationContext context)
        {
            var expectation = (context.Expectation != null) ? (TSubject)context.Expectation : default(TSubject);

            var assertionContext = new AssertionContext<TSubject>(
                context.SelectedMemberInfo,
                (TSubject)context.Subject,
                expectation,
                context.Reason,
                context.ReasonArgs);
            return assertionContext;
        }
    }
}