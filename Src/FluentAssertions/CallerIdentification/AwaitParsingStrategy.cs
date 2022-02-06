using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class AwaitParsingStrategy : IParsingStrategy
    {
        private const string KeywordToSkip = "await ";

        public ParsingState Parse(char symbol, StringBuilder statement)
        {
            if (IsLongEnoughToContainOurKeyword(statement) && EndsWithOurKeyword(statement))
            {
                statement.Remove(statement.Length - KeywordToSkip.Length, KeywordToSkip.Length);
            }

            return ParsingState.InProgress;
        }

        private static bool EndsWithOurKeyword(StringBuilder statement)
        {
            int leftIndex = statement.Length - 1;
            int rightIndex = KeywordToSkip.Length - 1;

            for (int offset = 0; offset < KeywordToSkip.Length; offset++)
            {
                if (statement[leftIndex - offset] != KeywordToSkip[rightIndex - offset])
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsLongEnoughToContainOurKeyword(StringBuilder statement)
        {
            return statement.Length >= KeywordToSkip.Length;
        }

        public bool IsWaitingForContextEnd()
        {
            return false;
        }

        public void NotifyEndOfLineReached()
        {
        }
    }
}
