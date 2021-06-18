using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class AddNonEmptySymbolParsingStrategy : IParsingStrategy
    {
        public ParsingState Parse(char symbol, StringBuilder statement)
        {
            if (!char.IsWhiteSpace(symbol))
            {
                statement.Append(symbol);
            }

            return ParsingState.GoToNextSymbol;
        }
    }
}
