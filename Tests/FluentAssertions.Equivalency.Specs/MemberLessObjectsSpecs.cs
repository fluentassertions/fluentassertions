using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Equivalency.Specs;

public class MemberLessObjectsSpecs
{
    [Fact]
    public async Task When_asserting_instances_of_an_anonymous_type_having_no_members_are_equivalent_it_should_fail()
    {
        // Arrange / Act
        Func<Task> act = () => new { }.Should().BeEquivalentToAsync(new { });

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task When_asserting_instances_of_a_class_having_no_members_are_equivalent_it_should_fail()
    {
        // Arrange / Act
        Func<Task> act = () => new ClassWithNoMembers().Should().BeEquivalentToAsync(new ClassWithNoMembers());

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task When_asserting_instances_of_Object_are_equivalent_it_should_fail()
    {
        // Arrange / Act
        Func<Task> act = () => new object().Should().BeEquivalentToAsync(new object());

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task When_asserting_instance_of_object_is_equivalent_to_null_it_should_fail_with_a_descriptive_message()
    {
        // Arrange
        object actual = new();
        object expected = null;

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected, "we want to test the failure {0}", "message");

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*Expected*to be <null>*we want to test the failure message*, but found System.Object*");
    }

    [Fact]
    public async Task When_asserting_null_is_equivalent_to_instance_of_object_it_should_fail()
    {
        // Arrange
        object actual = null;
        object expected = new();

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*Expected*to be System.Object*but found <null>*");
    }

    [Fact]
    public async Task When_an_type_only_exposes_fields_but_fields_are_ignored_in_the_equivalence_comparision_it_should_fail()
    {
        // Arrange
        var object1 = new ClassWithOnlyAField { Value = 1 };
        var object2 = new ClassWithOnlyAField { Value = 101 };

        // Act
        Func<Task> act = () => object1.Should().BeEquivalentToAsync(object2, opts => opts.IncludingAllDeclaredProperties());

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>("the objects have no members to compare.");
    }

    [Fact]
    public async Task
        When_an_type_only_exposes_properties_but_properties_are_ignored_in_the_equivalence_comparision_it_should_fail()
    {
        // Arrange
        var object1 = new ClassWithOnlyAProperty { Value = 1 };
        var object2 = new ClassWithOnlyAProperty { Value = 101 };

        // Act
        Func<Task> act = () => object1.Should().BeEquivalentToAsync(object2, opts => opts.ExcludingProperties());

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>("the objects have no members to compare.");
    }

    [Fact]
    public async Task When_asserting_instances_of_arrays_of_types_in_System_are_equivalent_it_should_respect_the_runtime_type()
    {
        // Arrange
        object actual = new int[0];
        object expectation = new int[0];

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_throwing_on_missing_members_and_there_are_no_missing_members_should_not_throw()
    {
        // Arrange
        var subject = new { Version = 2, Age = 36, };

        var expectation = new { Version = 2, Age = 36 };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expectation,
            options => options.ThrowingOnMissingMembers());

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_throwing_on_missing_members_and_there_is_a_missing_member_should_throw()
    {
        // Arrange
        var subject = new { Version = 2 };

        var expectation = new { Version = 2, Age = 36 };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expectation,
            options => options.ThrowingOnMissingMembers());

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expectation has property subject.Age that the other object does not have*");
    }

    [Fact]
    public async Task When_throwing_on_missing_members_and_there_is_an_additional_property_on_subject_should_not_throw()
    {
        // Arrange
        var subject = new { Version = 2, Age = 36, Additional = 13 };

        var expectation = new { Version = 2, Age = 36 };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expectation,
            options => options.ThrowingOnMissingMembers());

        // Assert
        await act.Should().NotThrowAsync();
    }
}
