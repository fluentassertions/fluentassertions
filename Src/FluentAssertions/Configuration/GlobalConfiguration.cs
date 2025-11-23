namespace FluentAssertions.Configuration;

public class GlobalConfiguration
{
    /// <summary>
    /// Provides access to the formatting defaults for all assertions.
    /// </summary>
    public GlobalFormattingOptions Formatting { get; set; } = new();

    /// <summary>
    /// Provides access to the defaults used by the structural equivalency assertions.
    /// </summary>
    public GlobalEquivalencyOptions Equivalency { get; set; } = new();

    /// <summary>
    /// Sets a specific test framework to be used by FluentAssertions when throwing assertion exceptions.
    /// </summary>
    /// <remarks>
    /// If set to <see langword="null"/>, the test framework will be automatically detected by scanning the appdomain.
    /// </remarks>
    public TestFramework? TestFramework
    {
        get;
        set
        {
            field = value;
            AssertionEngine.TestFramework = null;
        }
    }
}
