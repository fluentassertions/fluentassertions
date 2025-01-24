using System.Text;

namespace FluentAssertions.CallerIdentification;

/// <summary>
/// Tries to determine if the statement ends with ".Should(" or ".Should.", and assumes
/// the preceding identifier is the actual identifier.
/// </summary>
internal class ShouldCallParsingStrategy : IParsingStrategy
{
    private const string ExpectedPhrase = ".Should";

    public ParsingState Parse(char symbol, StringBuilder statement)
    {
        if (IsLongEnough(statement) && EndsWithExpectedPhrase(statement) && EndsWithInvocation(statement))
        {
            // Remove the ".Should." or ".Should(" part from the statement, so we keep the actual identifier
            statement.Remove(statement.Length - (ExpectedPhrase.Length + 1), ExpectedPhrase.Length + 1);
            return ParsingState.CandidateFound;
        }

        return ParsingState.InProgress;
    }

    private static bool IsLongEnough(StringBuilder statement) => statement.Length >= ExpectedPhrase.Length + 1;

    private static bool EndsWithExpectedPhrase(StringBuilder statement)
    {
        // Start from the index on the character just before the last ( or .
        var rightIndexInStatement = statement.Length - 2;

        var rightIndexInExpectedPhrase = ExpectedPhrase.Length - 1;

        // Do a reverse comparison to see if the statement ends with ".Should"
        for (var i = 0; i < ExpectedPhrase.Length; i++)
        {
            if (statement[rightIndexInStatement - i] != ExpectedPhrase[rightIndexInExpectedPhrase - i])
            {
                return false;
            }
        }

        return true;
    }

    private static bool EndsWithInvocation(StringBuilder statement) => statement[^1] is '(' or '.';

    public bool IsWaitingForContextEnd()
    {
        return false;
    }

    public void NotifyEndOfLineReached()
    {
    }
}
