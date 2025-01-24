using System.Text;

namespace FluentAssertions.CallerIdentification;

/// <summary>
/// Tries to find the <c>.Which.</c> construct and assumes everything preceding it has become irrelevant
/// for the chained assertion.
/// </summary>
internal class WhichParsingStrategy : IParsingStrategy
{
    private const string ExpectedPhrase = ".Which.";

    public ParsingState Parse(char symbol, StringBuilder statement)
    {
        if (IsLongEnough(statement) && EndsWithExpectedPhrase(statement))
        {
            // Remove everything we collected up to know and assume everything following the
            // .Which. property is a new assertion
            statement.Clear();
        }

        return ParsingState.InProgress;
    }

    private static bool IsLongEnough(StringBuilder statement) => statement.Length >= ExpectedPhrase.Length;

    private static bool EndsWithExpectedPhrase(StringBuilder statement)
    {
        // Start from the index of the last character
        var rightIndexInStatement = statement.Length - 1;

        var rightIndexInExpectedPhrase = ExpectedPhrase.Length - 1;

        // Do a reverse comparison to see if the statement ends with ".Which."
        for (var i = 0; i < ExpectedPhrase.Length; i++)
        {
            if (statement[rightIndexInStatement - i] != ExpectedPhrase[rightIndexInExpectedPhrase - i])
            {
                return false;
            }
        }

        return true;
    }

    public bool IsWaitingForContextEnd()
    {
        return false;
    }

    public void NotifyEndOfLineReached()
    {
    }
}
