using System.Text;

namespace FluentAssertions.CallerIdentification
{
    internal class AddNonEmptySymbolParsingStrategy : IParsingStrategy
    {
        private Mode mode = Mode.RemoveAllWhitespace;
        private char? precedingSymbol;

        public ParsingState Parse(char symbol, StringBuilder statement)
        {
            if (!char.IsWhiteSpace(symbol))
            {
                statement.Append(symbol);
                mode = Mode.RemoveSuperfluousWhitespace;
            }
            else if (mode is Mode.RemoveSuperfluousWhitespace)
            {
                if (precedingSymbol.HasValue && !char.IsWhiteSpace(precedingSymbol.Value))
                {
                    statement.Append(symbol);
                }
            }
            else
            {
                // skip the rest
            }

            precedingSymbol = symbol;

            return ParsingState.GoToNextSymbol;
        }

        public bool IsWaitingForContextEnd()
        {
            return false;
        }

        public void NotifyEndOfLineReached()
        {
            // Assume all new lines start with whitespace
            mode = Mode.RemoveAllWhitespace;
        }

        private enum Mode
        {
            /// <summary>
            /// Remove all whitespace until we find a non-whitespace character
            /// </summary>
            RemoveAllWhitespace,

            /// <summary>
            /// Only keep one whitespace character if more than one follow each other.
            /// </summary>
            RemoveSuperfluousWhitespace,
        }
    }
}
