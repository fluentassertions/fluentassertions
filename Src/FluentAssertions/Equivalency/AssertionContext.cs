namespace FluentAssertions.Equivalency
{
    internal class AssertionContext<TSubject> : IAssertionContext<TSubject>
    {
        private AssertionContext(INode currentNode, TSubject subject, TSubject expectation, string because,
                                object[] becauseArgs)
        {
            SelectedNode = currentNode;
            Subject = subject;
            Expectation = expectation;
            Because = because;
            BecauseArgs = becauseArgs;
        }

        public INode SelectedNode { get; }

        public TSubject Subject { get; }

        public TSubject Expectation { get; }

        public string Because { get; set; }

        public object[] BecauseArgs { get; set; }

        internal static AssertionContext<TSubject> CreateFromEquivalencyValidationContext(IEquivalencyValidationContext context)
        {
            TSubject expectation = (context.Expectation is not null) ? (TSubject)context.Expectation : default;

            return new AssertionContext<TSubject>(
                context.CurrentNode,
                (TSubject)context.Subject,
                expectation,
                context.Reason.FormattedMessage,
                context.Reason.Arguments);
        }
    }
}
