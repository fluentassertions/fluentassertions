using System.Text;

namespace FluentAssertionsAsync.CallerIdentification;

internal class MultiLineCommentParsingStrategy : IParsingStrategy
{
    private bool isCommentContext;
    private char? commentContextPreviousChar;

    public ParsingState Parse(char symbol, StringBuilder statement)
    {
        if (isCommentContext)
        {
            var isEndOfMultilineComment = symbol is '/' && commentContextPreviousChar is '*';

            if (isEndOfMultilineComment)
            {
                isCommentContext = false;
                commentContextPreviousChar = null;
            }
            else
            {
                commentContextPreviousChar = symbol;
            }

            return ParsingState.GoToNextSymbol;
        }

        var isStartOfMultilineComment = symbol is '*' && statement is [.., '/'];

        if (isStartOfMultilineComment)
        {
            statement.Remove(statement.Length - 1, 1);
            isCommentContext = true;
            return ParsingState.GoToNextSymbol;
        }

        return ParsingState.InProgress;
    }

    public bool IsWaitingForContextEnd()
    {
        return isCommentContext;
    }

    public void NotifyEndOfLineReached()
    {
    }
}
