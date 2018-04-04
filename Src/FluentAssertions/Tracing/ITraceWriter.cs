using System;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Defines the contract for an object that is used by Fluent Assertions to provide tracing in the equivalency API
    /// </summary>
    public interface ITraceWriter
    {
        void AddSingle(string trace);

        IDisposable AddBlock(string trace);

        string ToString();
    }
}
