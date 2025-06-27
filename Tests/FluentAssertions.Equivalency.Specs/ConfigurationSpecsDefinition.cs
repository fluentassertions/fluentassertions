using Xunit;

namespace FluentAssertions.Equivalency.Specs;

// Due to tests that call the static AssertionConfiguration or AssertionEngine, we need to disable parallelization
[CollectionDefinition("ConfigurationSpecs", DisableParallelization = true)]
public class ConfigurationSpecsDefinition;
