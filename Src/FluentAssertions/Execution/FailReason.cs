namespace FluentAssertions.Execution
{
    public class FailReason
    {
        public FailReason(string message, params object[] args)
        {
            Message = message;
            Args = args;
        }
        public string Message { get; }
        public object[] Args { get; }
    }
}
