namespace FluentAssertions.CallerIdentification
{
    public class SingleLineCommentHandler : IHandler
    {
        private char? previousChar;
        private bool isCommentContext;

        public HandlerResult Handle(char symbol)
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
            return isCommentContext ? HandlerResult.Handled : HandlerResult.InProgress;
        }
    }
}
