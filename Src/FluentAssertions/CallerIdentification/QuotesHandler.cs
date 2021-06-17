using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class QuotesHandler : IHandler
    {
        private readonly StringBuilder statement;
        private char isQuoteEscapeSymbol = '\\';
        private bool isQuoteContext;
        private char? previousChar;

        internal QuotesHandler(StringBuilder statement) => this.statement = statement;

        public HandlerResult Handle(char symbol)
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
                    if (IsVerbatim())
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
            return isQuoteContext ? HandlerResult.Handled : HandlerResult.InProgress;
        }

        private bool IsVerbatim()
        {
            return previousChar == '@'
               || (statement.Length > 1
                   && statement[statement.Length - 1] == '$'
                   && statement[statement.Length - 2] == '@');
        }
    }
}
