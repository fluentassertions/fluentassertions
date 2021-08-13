using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class CallerStatementBuilder
    {
        private readonly StringBuilder statement;
        private readonly List<IParsingStrategy> priorityOrderedParsingStrategies;
        private ParsingState parsingState = ParsingState.InProgress;

        internal CallerStatementBuilder()
        {
            statement = new StringBuilder();
            priorityOrderedParsingStrategies = new List<IParsingStrategy>
            {
                new QuotesParsingStrategy(),
                new MultiLineCommentParsingStrategy(),
                new SingleLineCommentParsingStrategy(),
                new SemicolonParsingStrategy(),
                new ShouldCallParsingStrategy(),
                new AwaitParsingStrategy(),
                new AddNonEmptySymbolParsingStrategy()
            };
        }

        internal void Append(string symbols)
        {
            using var symbolEnumerator = symbols.GetEnumerator();
            while (symbolEnumerator.MoveNext() && parsingState != ParsingState.Done)
            {
                var hasParsingStrategyWaitingForEndContext = priorityOrderedParsingStrategies
                    .Any(s => s.IsWaitingForContextEnd());

                parsingState = ParsingState.InProgress;
                foreach (var parsingStrategy in
                    priorityOrderedParsingStrategies
                        .SkipWhile(parsingStrategy =>
                            hasParsingStrategyWaitingForEndContext
                            && !parsingStrategy.IsWaitingForContextEnd()))
                {
                    parsingState = parsingStrategy.Parse(symbolEnumerator.Current, statement);
                    if (parsingState != ParsingState.InProgress)
                    {
                        break;
                    }
                }
            }

            if (IsDone())
            {
                return;
            }

            priorityOrderedParsingStrategies
                .ForEach(strategy => strategy.NotifyEndOfLineReached());
        }

        internal bool IsDone() => parsingState == ParsingState.Done;

        public override string ToString() => statement.ToString();
    }
}
