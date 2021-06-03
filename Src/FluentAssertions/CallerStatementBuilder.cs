using System.Collections.Generic;
using System.Text;
using FluentAssertions.CallerIdentification;

namespace FluentAssertions
{
    internal class CallerStatementBuilder
    {
        private readonly StringBuilder statement;
        private readonly IEnumerable<IHandler> handlers;
        private HandlerResult result = HandlerResult.InProgress;

        public CallerStatementBuilder()
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

        public void Append(string symbols)
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

        public bool IsDone() => result == HandlerResult.Done;

        public override string ToString() => statement.ToString();
    }
}
