using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class MultiLineCommentParsingStrategy : IParsingStrategy
    {
        private char? previousChar;
        private bool isCommentContext;

        public ParsingState Parse(char symbol, StringBuilder statement)
        {
            var result = ParsingState.InProgress;

            if (isCommentContext)
            {
                result = ParsingState.GoToNextSymbol;
                if (symbol == '/' && previousChar == '*')
                {
                    isCommentContext = false;
                }
            }
            else if (symbol == '*' && previousChar == '/')
            {
                result = ParsingState.GoToNextSymbol;
                statement.Remove(statement.Length - 1, 1);
                isCommentContext = true;
            }

            previousChar = symbol;
            return result;
        }
    }
}
