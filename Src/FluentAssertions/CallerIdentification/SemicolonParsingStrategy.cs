using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class SemicolonParsingStrategy : IParsingStrategy
    {
        public ParsingState Parse(char symbol, StringBuilder statement)
        {
            if (symbol == ';')
            {
                statement.Clear();
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
}
