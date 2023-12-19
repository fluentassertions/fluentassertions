using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Common;
using Xunit;

namespace FluentAssertionsAsync.Specs.Common;

public class ConfigurationSpecs
{
    [Fact]
    public void Value_formatter_detection_mode_is_disabled_with_empty_store()
    {
        // Arrange
        var store = new DummyConfigurationStore(new Dictionary<string, string>());
        var sut = new Configuration(store);

        // Act / Assert
        sut.ValueFormatterDetectionMode.Should().Be(ValueFormatterDetectionMode.Disabled);
    }

    [Fact]
    public void Value_formatter_detection_mode_is_specific_with_given_value_formatters_assembly()
    {
        // Arrange
        var store = new DummyConfigurationStore(new Dictionary<string, string>
        {
            { "valueFormattersAssembly", "foo" }
        });

        var sut = new Configuration(store);

        // Act / Assert
        sut.ValueFormatterDetectionMode.Should().Be(ValueFormatterDetectionMode.Specific);
    }

    [Fact]
    public void Value_formatter_detection_mode_can_be_specified_in_configuration_store()
    {
        // Arrange
        var store = new DummyConfigurationStore(new Dictionary<string, string>
        {
            { "valueFormatters", nameof(ValueFormatterDetectionMode.Scan) }
        });

        var sut = new Configuration(store);

        // Act / Assert
        sut.ValueFormatterDetectionMode.Should().Be(ValueFormatterDetectionMode.Scan);
    }

    [Fact]
    public void Value_formatter_detection_mode_throws_when_configured_incorrectly()
    {
        // Arrange
        var store = new DummyConfigurationStore(new Dictionary<string, string>
        {
            { "valueFormatters", "foo" }
        });

        var sut = new Configuration(store);

        // Act
        var act = () => sut.ValueFormatterDetectionMode;

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    private class DummyConfigurationStore : IConfigurationStore
    {
        private readonly Dictionary<string, string> settings;

        public DummyConfigurationStore(Dictionary<string, string> settings)
        {
            this.settings = settings;
        }

        public string GetSetting(string name)
            => settings.TryGetValue(name, out var value) ? value : null;
    }
}
