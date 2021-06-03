using System.Text;

namespace FluentAssertions.CallerIdentification
{
    public class AddNonEmptySymbolHandler : IHandler
    {
        private readonly StringBuilder statement;

        public AddNonEmptySymbolHandler(StringBuilder statement) => this.statement = statement;

        public HandlerResult Handle(char symbol)
        {
            if (!char.IsWhiteSpace(symbol))
            {
                statement.Append(symbol);
            }

            return HandlerResult.Handled;
        }
    }
}
