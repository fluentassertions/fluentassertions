using System.Text;

namespace FluentAssertions.CallerIdentification
{
    public class ShouldCallHandler : IHandler
    {
        private const string ShouldCall = ".Should";
        private readonly StringBuilder statement;

        public ShouldCallHandler(StringBuilder statement) => this.statement = statement;

        public HandlerResult Handle(char symbol)
        {
            if (statement.Length >= ShouldCall.Length)
            {
                var ldx = statement.Length - 1;
                var rdx = ShouldCall.Length - 1;

                for (var i = 0; i < ShouldCall.Length; i++)
                {
                    if (statement[ldx - i] != ShouldCall[rdx - i])
                    {
                        return HandlerResult.InProgress;
                    }
                }

                statement.Remove(statement.Length - ShouldCall.Length, ShouldCall.Length);
                return HandlerResult.Done;
            }

            return HandlerResult.InProgress;
        }
    }
}
