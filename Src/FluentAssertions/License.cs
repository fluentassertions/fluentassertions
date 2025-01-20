namespace FluentAssertions;

/// <summary>
/// Provides access to the licensing state of Fluent Assertions
/// </summary>
public static class License
{
    /// <summary>
    /// Can be used to accept the license and suppress the soft warning about the license requirements for commercial use
    /// </summary>
    public static bool Accepted { get; set; }
}
