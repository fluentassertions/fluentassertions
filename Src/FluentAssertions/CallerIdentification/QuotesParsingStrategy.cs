using System.Linq;
using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class QuotesParsingStrategy : IParsingStrategy
    {
        private char isQuoteEscapeSymbol = '\\';
        private bool isQuoteContext;
        private char? previousChar;

        public ParsingState Parse(char symbol, StringBuilder statement)
        {
            if (symbol == '"')
            {
                if (isQuoteContext)
                {
                    if (previousChar != isQuoteEscapeSymbol)
                    {
                        isQuoteContext = false;
                        isQuoteEscapeSymbol = '\\';
                        previousChar = null;
                        statement.Append(symbol);
                        return ParsingState.GoToNextSymbol;
                    }
                }
                else
                {
                    isQuoteContext = true;
                    if (IsVerbatim(statement))
                    {
                        isQuoteEscapeSymbol = '"';
                    }
                }
            }

            if (isQuoteContext)
            {
                statement.Append(symbol);
            }

            previousChar = symbol;
            return isQuoteContext ? ParsingState.GoToNextSymbol : ParsingState.InProgress;
        }

        public bool IsWaitingForContextEnd()
        {
            return isQuoteContext;
        }

        public void NotifyEndOfLineReached()
        {
        }

        private bool IsVerbatim(StringBuilder statement)
        {
            return
                statement.Length >= 1
                &&
                    new[]
                    {
                        "$@",
                        "@$",
                    }
                    .Any(verbatimStringOpener =>
                        previousChar == verbatimStringOpener[1]
                        && statement[statement.Length - 1] == verbatimStringOpener[1]
                        && statement[statement.Length - 2] == verbatimStringOpener[0]);
        }
    }
}
