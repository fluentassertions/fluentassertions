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

        private bool IsVerbatim(StringBuilder statement)
        {
            return previousChar == '@'
                   || (statement.Length > 1
                           && statement[statement.Length - 1] == '$'
                           && statement[statement.Length - 2] == '@');
        }
    }
}
