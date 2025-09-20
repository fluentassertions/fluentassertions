using System;
using FluentAssertions.Equivalency.Matching;
using FluentAssertions.Equivalency.Ordering;
using FluentAssertions.Equivalency.Selection;
using Xunit;

namespace FluentAssertions.Equivalency.Specs;

public partial class SelectionRulesSpecs
{
    [Fact]
    public void Public_methods_follow_fluent_syntax()
    {
        // Arrange
        var subject = new Root();
        var expected = new RootDto();

        // Act / Assert
        subject.Should().BeEquivalentTo(expected,
            options => options
                .AllowingInfiniteRecursion()
                .ComparingByMembers(typeof(Root))
                .ComparingByMembers<RootDto>()
                .ComparingByValue(typeof(Customer))
                .ComparingByValue<CustomerDto>()
                .ComparingEnumsByName()
                .ComparingEnumsByValue()
                .ComparingRecordsByMembers()
                .ComparingRecordsByValue()
                .Excluding(r => r.Level)
                .ExcludingFields()
                .ExcludingMissingMembers()
                .WithoutRecursing()
                .ExcludingNonBrowsableMembers()
                .ExcludingProperties()
                .IgnoringCyclicReferences()
                .IgnoringNonBrowsableMembersOnSubject()
                .Including(r => r.Level)
                .IncludingAllDeclaredProperties()
                .IncludingAllRuntimeProperties()
                .IncludingFields()
                .IncludingInternalFields()
                .IncludingInternalProperties()
                .IncludingNestedObjects()
                .IncludingProperties()
                .PreferringDeclaredMemberTypes()
                .PreferringRuntimeMemberTypes()
                .ThrowingOnMissingMembers()
                .Using(new ExtensibilitySpecs.DoEquivalencyStep(() => { }))
                .Using(new MustMatchMemberByNameRule())
                .Using(new AllFieldsSelectionRule())
                .Using(new ByteArrayOrderingRule())
                .Using(StringComparer.OrdinalIgnoreCase)
                .WithAutoConversion()
                .WithAutoConversionFor(_ => false)
                .WithoutAutoConversionFor(_ => true)
                .WithoutMatchingRules()
                .WithoutSelectionRules()
                .WithoutStrictOrdering()
                .WithoutStrictOrderingFor(r => r.Level)
                .WithStrictOrdering()
                .WithStrictOrderingFor(r => r.Level)
                .WithTracing()
            );
    }
}
