namespace FluentAssertions.Frameworks
{
    internal interface ITestFramework
    {
        void Throw(string message);
        bool IsAvailable { get; }
    }
}