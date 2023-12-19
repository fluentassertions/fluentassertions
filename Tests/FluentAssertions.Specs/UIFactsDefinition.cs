using Xunit;

namespace FluentAssertionsAsync.Specs;

// Try to stabilize UIFact tests
[CollectionDefinition("UIFacts", DisableParallelization = true)]
public class UIFactsDefinition
{
}
