using System.Collections.Generic;
using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class CallerStatementBuilder
    {
        private readonly StringBuilder statement;
        private readonly IEnumerable<IHandler> handlers;
        private HandlerResult result = HandlerResult.InProgress;

        internal CallerStatementBuilder()
        {
            statement = new StringBuilder();
            handlers = new IHandler[]
            {
                new QuotesHandler(statement),
                new SingleLineCommentHandler(),
                new MultiLineCommentHandler(statement),
                new SemicolonHandler(statement),
                new ShouldCallHandler(statement),
                new AddNonEmptySymbolHandler(statement)
            };
        }

        internal void Append(string symbols)
        {
            using var symbolEnumerator = symbols.GetEnumerator();
            while (symbolEnumerator.MoveNext() && result != HandlerResult.Done)
            {
                result = HandlerResult.InProgress;
                using var handlerEnumerator = handlers.GetEnumerator();
                while (handlerEnumerator.MoveNext() && result == HandlerResult.InProgress)
                {
                    result = handlerEnumerator.Current.Handle(symbolEnumerator.Current);
                }
            }
        }

        internal bool IsDone() => result == HandlerResult.Done;

        public override string ToString() => statement.ToString();
    }
}
