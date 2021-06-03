using System.Text;

namespace FluentAssertions.CallerIdentification
{
    public class QuotesHandler : IHandler
    {
        private readonly StringBuilder statement;
        private char isQuoteEscapeSymbol = '\\';
        private bool isQuoteContext;
        private char? previousChar;

        public QuotesHandler(StringBuilder statement) => this.statement = statement;

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
                    if (IsAtEscaped())
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

        private bool IsAtEscaped()
        {
            if (previousChar == '@')
            {
                return true;
            }

            if (statement.Length > 1)
            {
                var idx = statement.Length - 1;
                return statement[idx--] == '$'
                    && statement[idx] == '@';
            }

            return false;
        }
    }
}
