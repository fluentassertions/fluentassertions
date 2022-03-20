using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

[SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1512:Single-line comments should not be followed by blank line")]
public class TypeEqualitySpecs
{
    // TODO: nested run-time type doesn't match
    // TODO: nested compile-time type doesn't match
    // TODO: top-level doesn't match, but strict typing is only enabled for a nested type that doesn't match
    // TODO: top-level doesn't match, but strict typing is only enabled for a nested type that does match
    // TODO: strict typing for top-level type, but not for the rest and the run-time type of a nested type mismatches
    // TODO: global strict typing is enabled, but disabled for a particular test case
    // TODO: global strict typing is enabled for a particular type, but disabled for a particular test case

    [Fact]
    public void Throws_when_top_level_types_are_expected_to_match()
    {
        // Arrange
        var subject = new FooWithNestedClass();
        
        var expectation = new BarWithNested();

        // Act 
        Action act = () => subject.Should().BeEquivalentTo(expectation, x => x.WithStrictTyping());
        
        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expect subject to be of type BarWithNested, but found FooWithNestedClass");
    }

    [Fact]
    public void Throws_when_nested_runtime_types_are_expected_to_match()
    {
        // Arrange
        var subject = new FooWithNestedClass();
        
        var expectation = new BarWithNested();

        // Act 
        Action act = () => subject.Should().BeEquivalentTo(expectation, x => x.WithStrictTypingFor(x => x.Nested));
        
        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expect subject to be of type BarWithNested, but found FooWithNestedClass");
    }

    
    private class FooWithNestedClass
    {
        public Nested Nested { get; set; }
    }

    private class BarWithNested
    {
        public Nested Nested { get; set; }
    }

    private class Nested
    {
    }
    
    private class NestedSubtype
    {
        
    }
}
