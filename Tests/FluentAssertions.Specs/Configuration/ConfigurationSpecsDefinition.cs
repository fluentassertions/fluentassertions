using Xunit;

namespace FluentAssertions.Specs.Configuration;

// Due to tests that call the static AssertionConfiguration or AssertionEngine, we need to disable parallelization
[CollectionDefinition("ConfigurationSpecs", DisableParallelization = true)]
public class ConfigurationSpecsDefinition;
