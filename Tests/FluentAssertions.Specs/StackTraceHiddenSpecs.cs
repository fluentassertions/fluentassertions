#if NET6_0_OR_GREATER
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentAssertions.Equivalency;
using FluentAssertions.Types;
using Reflectify;
using Xunit;

namespace FluentAssertions.Specs;

public class StackTraceHiddenSpecs
{
    [Fact]
    public void All_non_assertion_classes_have_their_stack_trace_hidden()
    {
        // Arrange / Act
        var allFluentAssertionsClasses = GetAnnotatableTypes()
            .Except(ShouldNotBeAnnotated())
            .Types();

        // Assert
        allFluentAssertionsClasses.Should().BeDecoratedWith<StackTraceHiddenAttribute>(
            "because all \"internal\" code should be hidden from the stacktrace to improve the debugging experience");
    }

    [Fact]
    public void Assertion_classes_do_not_have_their_stack_trace_hidden()
    {
        // Arrange / Act
        var allFluentAssertionsClasses = ShouldNotBeAnnotated();

        // Assert
        allFluentAssertionsClasses.Should().NotBeDecoratedWith<StackTraceHiddenAttribute>(
            "because we do want to see the actual assertion method being used");
    }

    private static TypeSelector GetAnnotatableTypes()
    {
        return AllTypes
            .From(typeof(FluentAssertions.AssertionExtensions).Assembly)
            .ThatAreUnderNamespace("FluentAssertions")
            .ThatAreClasses()
            .ThatSatisfy(t => !t.IsNested)
            .ThatSatisfy(t => !t.IsSameOrInherits<Delegate>())
            .ThatSatisfy(t => !t.IsCompilerGenerated() && !t.HasAttribute<CompilerGeneratedAttribute>());
    }

    private static TypeSelector ShouldNotBeAnnotated()
    {
        string[] typeNameSuffixes =
        [
            "Assertions",
            "AssertionsBase",
            "AssertionExtensions",
            "AssertionsExtensions",
            "DateTimeExtensions",
            "EventRaisingExtensions",
            "FluentDateTimeExtensions",
            "FluentTimeSpanExtensions",
            "OccurrenceConstraintExtensions",
            "Common.TypeExtensions",
            "TypeEnumerableExtensions",
            "FluentAssertions.TypeExtensions",
        ];

        string[] namespaces =
        [
            "FluentAssertions.CallerIdentification",
            "FluentAssertions.Collections.MaximumMatching",
            "FluentAssertions.Equivalency.Matching",
            "FluentAssertions.Equivalency.Ordering",
            "FluentAssertions.Equivalency.Selection",
            "FluentAssertions.Equivalency.Tracing",
            "FluentAssertions.Formatting",
        ];

        return GetAnnotatableTypes()
              .ThatSatisfy(t =>
                  t == typeof(EquivalencyOptions) ||
                  typeNameSuffixes.Any(bt => bt == t.FullName || t.GetNonGenericName().EndsWith(bt, StringComparison.Ordinal)) ||
                  namespaces.Any(ns => t?.Namespace?.StartsWith(ns, StringComparison.Ordinal) is true)
              );
    }

}
#endif
