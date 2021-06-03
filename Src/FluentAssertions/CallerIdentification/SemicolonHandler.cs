using System.Text;

namespace FluentAssertions.CallerIdentification
{
    public class SemicolonHandler : IHandler
    {
        private readonly StringBuilder statement;

        public SemicolonHandler(StringBuilder statement) => this.statement = statement;

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
