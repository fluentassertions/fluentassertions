namespace FluentAssertions.Execution
{
    internal interface IVerificationStrategy
    {
        bool HasFailures { get; }

        void HandleFailure(string message);

        Verifier GetCurrentVerifier();
    }
}