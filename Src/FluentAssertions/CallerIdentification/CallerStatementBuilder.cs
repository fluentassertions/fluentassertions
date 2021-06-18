using System.Collections.Generic;
using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class CallerStatementBuilder
    {
        private readonly StringBuilder statement;
        private readonly IEnumerable<IParsingStrategy> priorityOrderedParsingStrategies;
        private ParsingState parsingState = ParsingState.InProgress;

        internal CallerStatementBuilder()
        {
            statement = new StringBuilder();
            priorityOrderedParsingStrategies = new IParsingStrategy[]
            {
                new QuotesParsingStrategy(),
                new SingleLineCommentParsingStrategy(),
                new MultiLineCommentParsingStrategy(),
                new SemicolonParsingStrategy(),
                new ShouldCallParsingStrategy(),
                new AddNonEmptySymbolParsingStrategy()
            };
        }

        internal void Append(string symbols)
        {
            using var symbolEnumerator = symbols.GetEnumerator();
            while (symbolEnumerator.MoveNext() && parsingState != ParsingState.Done)
            {
                parsingState = ParsingState.InProgress;
                using var handlerEnumerator = priorityOrderedParsingStrategies.GetEnumerator();
                while (handlerEnumerator.MoveNext() && parsingState == ParsingState.InProgress)
                {
                    parsingState = handlerEnumerator.Current.Parse(symbolEnumerator.Current, statement);
                }
            }
        }

        internal bool IsDone() => parsingState == ParsingState.Done;

        public override string ToString() => statement.ToString();
    }
}
