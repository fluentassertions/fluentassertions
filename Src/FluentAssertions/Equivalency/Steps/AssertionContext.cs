namespace FluentAssertions.Equivalency.Steps
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

        internal static AssertionContext<TSubject> CreateFrom(Comparands comparands, IEquivalencyValidationContext context)
        {
            return new(
                context.CurrentNode,
                (TSubject)comparands.Subject,
                (TSubject)comparands.Expectation,
                context.Reason.FormattedMessage,
                context.Reason.Arguments);
        }
    }
}
