using System.Collections.Generic;
using System.Text;

namespace FluentAssertions
{
    internal class CallerStatementBuilder
    {
        private readonly StringBuilder statement;
        private readonly IEnumerable<IHandler> handlers;

        public CallerStatementBuilder()
        {
            statement = new StringBuilder();
            handlers = new IHandler[]
            {
                new QuotesHandler(statement),
                new SemicolonHandler(statement),
                new ShouldCallHandler(statement),
                new AddNonEmptySymbolHandler(statement)
            };
        }

        public Result Append(string symbols)
        {
            Result result = Result.InProgress;
            using var symbolEnumerator = symbols.GetEnumerator();
            while (symbolEnumerator.MoveNext() && result != Result.Done)
            {
                result = Result.InProgress;
                using var handlerEnumerator = handlers.GetEnumerator();
                while (handlerEnumerator.MoveNext() && result == Result.InProgress)
                {
                    result = handlerEnumerator.Current.Handle(symbolEnumerator.Current);
                }
            }

            return result;
        }

        public override string ToString() => statement.ToString();

        public enum Result
        {
            InProgress,
            Handled,
            Done
        }

        private interface IHandler
        {
            Result Handle(char symbol);
        }

        private class QuotesHandler : IHandler
        {
            private readonly StringBuilder statement;
            private char isQuoteEscapeSymbol = '\\';
            private bool isQuoteContext;
            private char? previousChar;

            public QuotesHandler(StringBuilder statement) => this.statement = statement;

            public Result Handle(char symbol)
            {
                if (symbol == '"')
                {
                    if (isQuoteContext)
                    {
                        if (previousChar != isQuoteEscapeSymbol)
                        {
                            isQuoteContext = false;
                            isQuoteEscapeSymbol = '\\';
                        }
                    }
                    else
                    {
                        isQuoteContext = true;
                        if (IsAtEscaped())
                        {
                            isQuoteEscapeSymbol = '"';
                        }
                    }
                }

                if (isQuoteContext)
                {
                    statement.Append(symbol);
                }

                previousChar = symbol;
                return isQuoteContext ? Result.Handled : Result.InProgress;
            }

            private bool IsAtEscaped()
            {
                if (previousChar == '@')
                {
                    return true;
                }

                if (statement.Length > 1)
                {
                    var idx = statement.Length - 1;
                    return statement[idx--] == '$'
                        && statement[idx] == '@';
                }

                return false;
            }
        }

        private class SemicolonHandler : IHandler
        {
            private readonly StringBuilder statement;

            public SemicolonHandler(StringBuilder statement) => this.statement = statement;

            public Result Handle(char symbol)
            {
                if (symbol == ';')
                {
                    statement.Clear();
                    return Result.Done;
                }

                return Result.InProgress;
            }
        }

        private class ShouldCallHandler : IHandler
        {
            private const string ShouldCall = ".Should";
            private readonly StringBuilder statement;

            public ShouldCallHandler(StringBuilder statement) => this.statement = statement;

            public Result Handle(char symbol)
            {
                if (statement.Length >= ShouldCall.Length)
                {
                    var ldx = statement.Length - 1;
                    var rdx = ShouldCall.Length - 1;

                    for (var i = 0; i < ShouldCall.Length; i++)
                    {
                        if (statement[ldx - i] != ShouldCall[rdx - i])
                        {
                            return Result.InProgress;
                        }
                    }

                    statement.Remove(statement.Length - ShouldCall.Length, ShouldCall.Length);
                    return Result.Done;
                }

                return Result.InProgress;
            }
        }

        private class AddNonEmptySymbolHandler : IHandler
        {
            private readonly StringBuilder statement;

            public AddNonEmptySymbolHandler(StringBuilder statement) => this.statement = statement;

            public Result Handle(char symbol)
            {
                if (!char.IsWhiteSpace(symbol))
                {
                    statement.Append(symbol);
                }

                return Result.Handled;
            }
        }
    }
}
