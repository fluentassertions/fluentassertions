using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class SemicolonHandler : IHandler
    {
        private readonly StringBuilder statement;

        internal SemicolonHandler(StringBuilder statement) => this.statement = statement;

        public HandlerResult Handle(char symbol)
        {
            if (symbol == ';')
            {
                statement.Clear();
                return HandlerResult.Done;
            }

            return HandlerResult.InProgress;
        }
    }
}
