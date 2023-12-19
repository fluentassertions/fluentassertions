using System.Text;

namespace FluentAssertionsAsync.CallerIdentification;

internal class QuotesParsingStrategy : IParsingStrategy
{
    private char isQuoteEscapeSymbol = '\\';
    private bool isQuoteContext;
    private char? previousChar;

    public ParsingState Parse(char symbol, StringBuilder statement)
    {
        if (symbol is '"')
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
        return (previousChar is '@' && statement is [.., '$', '@'])
            || (previousChar is '$' && statement is [.., '@', '$']);
    }
}
