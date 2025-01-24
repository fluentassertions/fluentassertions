using System.Text;

namespace FluentAssertions.CallerIdentification;

internal class SemicolonParsingStrategy : IParsingStrategy
{
    public ParsingState Parse(char symbol, StringBuilder statement)
    {
        if (symbol is ';')
        {
            statement.Clear();
            return ParsingState.Completed;
        }

        return ParsingState.InProgress;
    }

    public bool IsWaitingForContextEnd()
    {
        return false;
    }

    public void NotifyEndOfLineReached()
    {
    }
}
