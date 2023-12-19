using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Equivalency.Specs;

public class NonEquivalencySpecs
{
    [Fact]
    public async Task When_asserting_inequivalence_of_equal_ints_as_object_it_should_fail()
    {
        // Arrange
        object i1 = 1;
        object i2 = 1;

        // Act
        Func<Task> act = () => i1.Should().NotBeEquivalentToAsync(i2);

        // Assert
        await act.Should().ThrowAsync<XunitException>();
    }

    [Fact]
    public async Task When_asserting_inequivalence_of_unequal_ints_as_object_it_should_succeed()
    {
        // Arrange
        object i1 = 1;
        object i2 = 2;

        // Act
        Func<Task> act = () => i1.Should().NotBeEquivalentToAsync(i2);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_asserting_inequivalence_of_equal_strings_as_object_it_should_fail()
    {
        // Arrange
        object s1 = "A";
        object s2 = "A";

        // Act
        Func<Task> act = () => s1.Should().NotBeEquivalentToAsync(s2);

        // Assert
        await act.Should().ThrowAsync<XunitException>();
    }

    [Fact]
    public async Task When_asserting_inequivalence_of_unequal_strings_as_object_it_should_succeed()
    {
        // Arrange
        object s1 = "A";
        object s2 = "B";

        // Act
        Func<Task> act = () => s1.Should().NotBeEquivalentToAsync(s2);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_asserting_inequivalence_of_equal_classes_it_should_fail()
    {
        // Arrange
        var o1 = new { Name = "A" };
        var o2 = new { Name = "A" };

        // Act
        Func<Task> act = () => o1.Should().NotBeEquivalentToAsync(o2, "some {0}", "reason");

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*some reason*");
    }

    [Fact]
    public async Task When_asserting_inequivalence_of_unequal_classes_it_should_succeed()
    {
        // Arrange
        var o1 = new { Name = "A" };
        var o2 = new { Name = "B" };

        // Act
        Func<Task> act = () => o1.Should().NotBeEquivalentToAsync(o2);

        // Assert
        await act.Should().NotThrowAsync();
    }
}
