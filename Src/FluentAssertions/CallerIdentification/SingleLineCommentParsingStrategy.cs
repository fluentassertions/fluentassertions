using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class SingleLineCommentParsingStrategy : IParsingStrategy
    {
        private bool isCommentContext;

        public ParsingState Parse(char symbol, StringBuilder statement)
        {
            if (isCommentContext)
            {
                return ParsingState.GoToNextSymbol;
            }

            bool doesSymbolStartComment = symbol == '/' && statement.Length > 0 && statement[statement.Length - 1] == '/';

            if (!doesSymbolStartComment)
            {
                return ParsingState.InProgress;
            }

            isCommentContext = true;
            statement.Remove(statement.Length - 1, length: 1);
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
}
