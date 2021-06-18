using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class SingleLineCommentParsingStrategy : IParsingStrategy
    {
        private char? previousChar;
        private bool isCommentContext;

        public ParsingState Parse(char symbol, StringBuilder unused)
        {
            if (!isCommentContext)
            {
                if (symbol == '/' && previousChar == '/')
                {
                    isCommentContext = true;
                }
            }
            else if (symbol == '\n')
            {
                isCommentContext = false;
            }

            previousChar = symbol;
            return isCommentContext ? ParsingState.GoToNextSymbol : ParsingState.InProgress;
        }
    }
}
