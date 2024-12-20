namespace FluentAssertions.Configuration;

public class GlobalConfiguration
{
    public GlobalFormattingOptions Formatting { get; set; }

    public GlobalEquivalencyOptions Equivalency { get; set; }

    public string TestFrameworkName { get; set; }
}
