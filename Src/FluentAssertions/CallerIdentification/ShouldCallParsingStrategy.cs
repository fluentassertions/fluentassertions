using System.Text;

namespace FluentAssertionsAsync.CallerIdentification;

internal class ShouldCallParsingStrategy : IParsingStrategy
{
    private const string ShouldCall = ".Should";

    public ParsingState Parse(char symbol, StringBuilder statement)
    {
        if (statement.Length >= ShouldCall.Length + 1)
        {
            var leftIndex = statement.Length - 2;
            var rightIndex = ShouldCall.Length - 1;

            for (var i = 0; i < ShouldCall.Length; i++)
            {
                if (statement[leftIndex - i] != ShouldCall[rightIndex - i])
                {
                    return ParsingState.InProgress;
                }
            }

            if (statement[^1] is not ('(' or '.'))
            {
                return ParsingState.InProgress;
            }

            statement.Remove(statement.Length - (ShouldCall.Length + 1), ShouldCall.Length + 1);
            return ParsingState.Done;
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
