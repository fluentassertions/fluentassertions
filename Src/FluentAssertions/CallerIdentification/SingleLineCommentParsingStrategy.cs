using System.Text;

namespace FluentAssertionsAsync.CallerIdentification;

internal class SingleLineCommentParsingStrategy : IParsingStrategy
{
    private bool isCommentContext;

    public ParsingState Parse(char symbol, StringBuilder statement)
    {
        if (isCommentContext)
        {
            return ParsingState.GoToNextSymbol;
        }

        var doesSymbolStartComment = symbol is '/' && statement is [.., '/'];

        if (!doesSymbolStartComment)
        {
            return ParsingState.InProgress;
        }

        isCommentContext = true;
        statement.Remove(statement.Length - 1, 1);
        return ParsingState.GoToNextSymbol;
    }

    public bool IsWaitingForContextEnd()
    {
        return isCommentContext;
    }

    public void NotifyEndOfLineReached()
    {
        if (isCommentContext)
        {
            isCommentContext = false;
        }
    }
}
