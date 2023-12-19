using System;
using System.Threading.Tasks;
using FluentAssertionsAsync.Equivalency.Matching;
using FluentAssertionsAsync.Equivalency.Ordering;
using FluentAssertionsAsync.Equivalency.Selection;
using Xunit;

namespace FluentAssertionsAsync.Equivalency.Specs;

public partial class SelectionRulesSpecs
{
    [Fact]
    public async Task Public_methods_follow_fluent_syntax()
    {
        // Arrange
        var subject = new Root();
        var expected = new RootDto();

        // Act / Assert
        await subject.Should().BeEquivalentToAsync(expected,
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
                .ExcludingNestedObjects()
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
                .RespectingDeclaredTypes()
                .RespectingRuntimeTypes()
                .ThrowingOnMissingMembers()
                .Using(new ExtensibilitySpecs.DoEquivalencyStep(() => { }))
                .Using(new MustMatchByNameRule())
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
