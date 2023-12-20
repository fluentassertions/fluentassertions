using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertionsAsync.Collections;
using Xunit;

namespace FluentAssertionsAsync.Specs.Collections;

/// <summary>
/// This part of the class contains assertions of general generic string collections
/// </summary>
public partial class GenericCollectionAssertionOfStringSpecs
{
    [Fact]
    public void When_using_StringCollectionAssertions_the_AndConstraint_should_have_the_correct_type()
    {
        // Arrange
        MethodInfo[] methodInfo =
            typeof(StringCollectionAssertions<IEnumerable<string>>).GetMethods(
                BindingFlags.Public | BindingFlags.Instance);

        // Act
        var methods =
            from method in methodInfo
            where !method.IsSpecialName // Exclude Properties
            where method.DeclaringType != typeof(object)
            where method.Name != "Equals"
            select new { method.Name, method.ReturnType };

        // Assert
        var expectedTypes = new[]
        {
            typeof(Task<AndWhichConstraint<StringCollectionAssertions<IEnumerable<string>>, string>>),
            typeof(Task<AndConstraint<StringCollectionAssertions<IEnumerable<string>>>>),
            typeof(AndConstraint<StringCollectionAssertions<IEnumerable<string>>>),
            typeof(AndConstraint<SubsequentOrderingAssertions<string>>)
        };

        methods.Should().OnlyContain(method => expectedTypes.Any(e => e.IsAssignableFrom(method.ReturnType)));
    }

    [Fact]
    public void When_accidentally_using_equals_it_should_throw_a_helpful_error()
    {
        // Arrange
        var someCollection = new List<string> { "one", "two", "three" };

        // Act
        Action action = () => someCollection.Should().Equals(someCollection);

        // Assert
        action.Should().Throw<NotSupportedException>()
            .WithMessage(
                "Equals is not part of Fluent Assertions. Did you mean BeSameAs(), Equal(), or BeEquivalentTo() instead?");
    }
}
