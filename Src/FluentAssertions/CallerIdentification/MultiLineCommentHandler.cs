using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class MultiLineCommentHandler : IHandler
    {
        private readonly StringBuilder statement;
        private char? previousChar;
        private bool isCommentContext;

        internal MultiLineCommentHandler(StringBuilder statement) => this.statement = statement;

        public HandlerResult Handle(char symbol)
        {
            var result = HandlerResult.InProgress;

            if (isCommentContext)
            {
                result = HandlerResult.Handled;
                if (symbol == '/' && previousChar == '*')
                {
                    isCommentContext = false;
                }
            }
            else if (symbol == '*' && previousChar == '/')
            {
                result = HandlerResult.Handled;
                statement.Remove(statement.Length - 1, 1);
                isCommentContext = true;
            }

            previousChar = symbol;
            return result;
        }
    }
}
