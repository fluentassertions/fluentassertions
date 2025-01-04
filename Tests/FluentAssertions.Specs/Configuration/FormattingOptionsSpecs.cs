using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Configuration;

[Collection("ConfigurationSpecs")]
public sealed class FormattingOptionsSpecs : IDisposable
{
    [Fact]
    public void When_global_formatting_settings_are_modified()
    {
        AssertionConfiguration.Current.Formatting.UseLineBreaks = true;
        AssertionConfiguration.Current.Formatting.MaxDepth = 123;
        AssertionConfiguration.Current.Formatting.MaxLines = 33;

        AssertionScope.Current.FormattingOptions.UseLineBreaks.Should().BeTrue();
        AssertionScope.Current.FormattingOptions.MaxDepth.Should().Be(123);
        AssertionScope.Current.FormattingOptions.MaxLines.Should().Be(33);
    }

    [Fact]
    public void When_a_custom_formatter_exists_in_any_loaded_assembly_it_should_override_the_default_formatters()
    {
        // Arrange
        AssertionConfiguration.Current.Formatting.ValueFormatterDetectionMode = ValueFormatterDetectionMode.Scan;

        var subject = new SomeClassWithCustomFormatter
        {
            Property = "SomeValue"
        };

        // Act
        string result = Formatter.ToString(subject);

        // Assert
        result.Should().Be("Property = SomeValue", "it should use my custom formatter");
    }

    [Fact]
    public void When_a_base_class_has_a_custom_formatter_it_should_override_the_default_formatters()
    {
        // Arrange
        AssertionConfiguration.Current.Formatting.ValueFormatterDetectionMode = ValueFormatterDetectionMode.Scan;

        var subject = new SomeClassInheritedFromClassWithCustomFormatterLvl1
        {
            Property = "SomeValue"
        };

        // Act
        string result = Formatter.ToString(subject);

        // Assert
        result.Should().Be("Property = SomeValue", "it should use my custom formatter");
    }

    [Fact]
    public void When_there_are_multiple_custom_formatters_it_should_select_a_more_specific_one()
    {
        // Arrange
        AssertionConfiguration.Current.Formatting.ValueFormatterDetectionMode = ValueFormatterDetectionMode.Scan;

        var subject = new SomeClassInheritedFromClassWithCustomFormatterLvl2
        {
            Property = "SomeValue"
        };

        // Act
        string result = Formatter.ToString(subject);

        // Assert
        result.Should().Be("Property is SomeValue", "it should use my custom formatter");
    }

    [Fact]
    public void When_a_base_class_has_multiple_custom_formatters_it_should_work_the_same_as_for_the_base_class()
    {
        // Arrange
        AssertionConfiguration.Current.Formatting.ValueFormatterDetectionMode = ValueFormatterDetectionMode.Scan;

        var subject = new SomeClassInheritedFromClassWithCustomFormatterLvl3
        {
            Property = "SomeValue"
        };

        // Act
        string result = Formatter.ToString(subject);

        // Assert
        result.Should().Be("Property is SomeValue", "it should use my custom formatter");
    }

    [Fact]
    public void When_no_custom_formatter_exists_in_the_specified_assembly_it_should_use_the_default()
    {
        // Arrange
        AssertionConfiguration.Current.Formatting.ValueFormatterAssembly = "FluentAssertions";

        var subject = new SomeClassWithCustomFormatter
        {
            Property = "SomeValue"
        };

        // Act
        string result = Formatter.ToString(subject);

        // Assert
        result.Should().Be(subject.ToString());
    }

    [Fact]
    public void When_formatter_scanning_is_disabled_it_should_use_the_default_formatters()
    {
        // Arrange
        AssertionConfiguration.Current.Formatting.ValueFormatterDetectionMode = ValueFormatterDetectionMode.Disabled;

        var subject = new SomeClassWithCustomFormatter
        {
            Property = "SomeValue"
        };

        // Act
        string result = Formatter.ToString(subject);

        // Assert
        result.Should().Be(subject.ToString());
    }

    [Fact]
    public void When_no_formatter_scanning_is_configured_it_should_use_the_default_formatters()
    {
        // Arrange
        AssertionConfiguration.Current.Formatting.ValueFormatterDetectionMode = ValueFormatterDetectionMode.Disabled;

        var subject = new SomeClassWithCustomFormatter
        {
            Property = "SomeValue"
        };

        // Act
        string result = Formatter.ToString(subject);

        // Assert
        result.Should().Be(subject.ToString());
    }

    public class SomeClassWithCustomFormatter
    {
        public string Property { get; set; }

        public override string ToString()
        {
            return "The value of my property is " + Property;
        }
    }

    public class SomeOtherClassWithCustomFormatter
    {
        public string Property { get; set; }

        public override string ToString()
        {
            return "The value of my property is " + Property;
        }
    }

    public class SomeClassInheritedFromClassWithCustomFormatterLvl1 : SomeClassWithCustomFormatter;

    public class SomeClassInheritedFromClassWithCustomFormatterLvl2 : SomeClassInheritedFromClassWithCustomFormatterLvl1;

    public class SomeClassInheritedFromClassWithCustomFormatterLvl3 : SomeClassInheritedFromClassWithCustomFormatterLvl2;

    public static class CustomFormatter
    {
        [ValueFormatter]
        public static int Bar(SomeClassWithCustomFormatter _)
        {
            return -1;
        }

        [ValueFormatter]
        public static void Foo(SomeClassWithCustomFormatter value, FormattedObjectGraph output)
        {
            output.AddFragment("Property = " + value.Property);
        }

        [ValueFormatter]
        [SuppressMessage("ReSharper", "CA1801")]
        public static void Foo(SomeOtherClassWithCustomFormatter _, FormattedObjectGraph output)
        {
            throw new XunitException("Should never be called");
        }

        [ValueFormatter]
        public static void Foo(SomeClassInheritedFromClassWithCustomFormatterLvl2 value, FormattedObjectGraph output)
        {
            output.AddFragment("Property is " + value.Property);
        }

        [ValueFormatter]
        public static void Foo2(SomeClassInheritedFromClassWithCustomFormatterLvl2 value, FormattedObjectGraph output)
        {
            output.AddFragment("Property is " + value.Property);
        }
    }

    public void Dispose()
    {
        AssertionEngine.ResetToDefaults();
    }
}
