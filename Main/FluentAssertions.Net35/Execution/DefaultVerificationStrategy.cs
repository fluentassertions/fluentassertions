namespace FluentAssertions.Execution
{
    internal class DefaultVerificationStrategy : IVerificationStrategy
    {
        public bool HasFailures
        {
            get { return false; }
        }

        public void HandleFailure(string message)
        {
            AssertionHelper.Throw(message);
        }

        public Verifier GetCurrentVerifier()
        {
            return new Verifier();
        }
    }
}