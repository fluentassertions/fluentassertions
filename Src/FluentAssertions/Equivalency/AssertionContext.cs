namespace FluentAssertions.Equivalency
{
    internal class AssertionContext<TSubject> : IAssertionContext<TSubject>
    {
        public AssertionContext(SelectedMemberInfo subjectProperty, TSubject subject, TSubject expectation, string because,
                                object[] becauseArgs)
        {
            SubjectProperty = subjectProperty;
            Subject = subject;
            Expectation = expectation;
            Because = because;
            BecauseArgs = becauseArgs;
        }

        public SelectedMemberInfo SubjectProperty { get; private set; }

        public TSubject Subject { get; private set; }

        public TSubject Expectation { get; private set; }

        public string Because { get; set; }

        public object[] BecauseArgs { get; set; }

        internal static AssertionContext<TSubject> CreateFromEquivalencyValidationContext(IEquivalencyValidationContext context)
        {
            TSubject expectation = (context.Expectation != null) ? (TSubject)context.Expectation : default;

            var assertionContext = new AssertionContext<TSubject>(
                context.SelectedMemberInfo,
                (TSubject)context.Subject,
                expectation,
                context.Because,
                context.BecauseArgs);
            return assertionContext;
        }
    }
}
