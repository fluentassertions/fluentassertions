using System;
using System.Threading.Tasks;
using FluentAssertions.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs;

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
            __ =>
            {
                Configuration.Current.ValueFormatterAssembly = string.Empty;
                _ = Configuration.Current.ValueFormatterDetectionMode;
            }
        );

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Tracing_must_be_safe_when_executed_concurrently()
    {
        try
        {
            // Arrange
            AssertionOptions.AssertEquivalencyUsing(e => e.WithTracing());

            Parallel.For(1, 10_000, (_, _) =>
            {
                try
                {
                    new { A = "a" }.Should().BeEquivalentTo(new { A = "b" });
                }
                catch (XunitException)
                {
                }
            });
        }
        finally
        {
            AssertionOptions.AssertEquivalencyUsing(_ => new());
        }
    }
}

// Due to tests that call Configuration.Current
[CollectionDefinition("ConfigurationSpecs", DisableParallelization = true)]
public class ConfigurationSpecsDefinition;
