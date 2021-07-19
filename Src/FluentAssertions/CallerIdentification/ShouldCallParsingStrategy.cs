using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class ShouldCallParsingStrategy : IParsingStrategy
    {
        private const string ShouldCall = ".Should(";

        public ParsingState Parse(char symbol, StringBuilder statement)
        {
            if (statement.Length >= ShouldCall.Length)
            {
                var leftIndex = statement.Length - 1;
                var rightIndex = ShouldCall.Length - 1;

                for (var i = 0; i < ShouldCall.Length; i++)
                {
                    if (statement[leftIndex - i] != ShouldCall[rightIndex - i])
                    {
                        return ParsingState.InProgress;
                    }
                }

                statement.Remove(statement.Length - ShouldCall.Length, ShouldCall.Length);
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
