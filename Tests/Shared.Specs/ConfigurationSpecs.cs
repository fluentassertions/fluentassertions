using System;
using System.Threading.Tasks;
using FluentAssertions.Common;
using Xunit;

namespace FluentAssertions.Specs
{
    // Due to tests that call Configuration.Current
    [CollectionDefinition("ConfigurationSpecs", DisableParallelization = true)]
    public class ConfigurationSpecsDefinition { }

    [Collection("ConfigurationSpecs")]
    public class ConfigurationSpecs
    {
        [Fact]
        public void When_concurrently_accessing_current_Configuration_no_exception_should_be_thrown()
        {
            // Act
            Action act = () => Parallel.For(
                0,
                10000,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = 8
                },
                e =>
                {
                    Configuration.Current.ValueFormatterAssembly = string.Empty;
                    var mode = Configuration.Current.ValueFormatterDetectionMode;
                }
            );

            // Assert
            act.Should().NotThrow();
        }
    }
}
