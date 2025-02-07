using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentAssertions.CallerIdentification;

internal class StatementParser
{
    private readonly StringBuilder statement;
    private readonly List<IParsingStrategy> parsingStrategies;
    private readonly List<string> candidates = new();
    private ParsingState state = ParsingState.InProgress;

    internal StatementParser()
    {
        statement = new StringBuilder();

        parsingStrategies =
        [
            new QuotesParsingStrategy(),
            new MultiLineCommentParsingStrategy(),
            new SingleLineCommentParsingStrategy(),
            new SemicolonParsingStrategy(),
            new ShouldCallParsingStrategy(),
            new WhichParsingStrategy(),
            new AwaitParsingStrategy(),
            new AddNonEmptySymbolParsingStrategy()
        ];
    }

    /// <summary>
    /// Gets the identifiers preceding a Should or Which clause as extracted from lines of code passed to <see cref="Append"/>
    /// </summary>
    public string[] Identifiers => candidates.ToArray();

    public void Append(string symbols)
    {
        using var symbolEnumerator = symbols.GetEnumerator();

        while (symbolEnumerator.MoveNext() && state != ParsingState.Completed)
        {
            // The logic ensures that parsing does not continue with irrelevant strategies when a strategy is currently in the middle
            // of a multi-symbol context (e.g., waiting for "_/" to match the beginning "/_"). In such cases, it skips over strategies
            // that aren't waiting for the "end of context" while allowing the active (waiting) strategy to resume processing.
            IEnumerable<IParsingStrategy> activeParsers = parsingStrategies;
            if (parsingStrategies.Exists(s => s.IsWaitingForContextEnd()))
            {
                activeParsers = parsingStrategies.SkipWhile(parsingStrategy => !parsingStrategy.IsWaitingForContextEnd());
            }

            state = ParsingState.InProgress;

            foreach (IParsingStrategy parser in activeParsers)
            {
                state = parser.Parse(symbolEnumerator.Current, statement);
                if (state == ParsingState.CandidateFound)
                {
                    candidates.Add(statement.ToString());
                }

                if (state != ParsingState.InProgress)
                {
                    break;
                }
            }
        }

        if (IsDone())
        {
            return;
        }

        parsingStrategies.ForEach(strategy => strategy.NotifyEndOfLineReached());
    }

    public bool IsDone() => state == ParsingState.Completed;
}
