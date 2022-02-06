using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class MultiLineCommentParsingStrategy : IParsingStrategy
    {
        private bool isCommentContext;
        private char? commentContextPreviousChar;

        public ParsingState Parse(char symbol, StringBuilder statement)
        {
            if (isCommentContext)
            {
                bool isEndOfMultilineComment = symbol == '/' && commentContextPreviousChar == '*';

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

            bool isStartOfMultilineComment = symbol == '*' && statement.Length > 0 && statement[statement.Length - 1] == '/';

            if (isStartOfMultilineComment)
            {
                statement.Remove(statement.Length - 1, length: 1);
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
}
