using FluentAssertions.Common;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FluentAssertions.Specs
{
    public class ConfigurationSpecs
    {
        [Fact]
        public void When_concurrently_accessing_current_Configuration_no_exception_should_be_thrown()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
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

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }
    }
}
