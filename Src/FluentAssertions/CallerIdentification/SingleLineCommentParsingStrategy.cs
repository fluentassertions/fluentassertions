using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class SingleLineCommentParsingStrategy : IParsingStrategy
    {
        private bool isCommentContext;

        public ParsingState Parse(char symbol, StringBuilder statement)
        {
            if (!isCommentContext)
            {
                if (symbol == '/' && statement.Length > 0 && statement[statement.Length - 1] == '/')
                {
                    isCommentContext = true;
                    statement.Remove(statement.Length - 1, 1);
                }
            }
            else if (symbol == '\n')
            {
                isCommentContext = false;
            }

            return isCommentContext ? ParsingState.GoToNextSymbol : ParsingState.InProgress;
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
}
