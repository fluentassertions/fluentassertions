using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

public class CyclicReferencesSpecs
{
    [Fact]
    public void When_validating_nested_properties_that_have_cyclic_references_it_should_throw()
    {
        // Arrange
        var cyclicRoot = new CyclicRoot { Text = "Root" };

        cyclicRoot.Level = new CyclicLevel1 { Text = "Level1", Root = cyclicRoot };

        var cyclicRootDto = new CyclicRootDto { Text = "Root" };

        cyclicRootDto.Level = new CyclicLevel1Dto { Text = "Level1", Root = cyclicRootDto };

        // Act
        Action act = () => cyclicRoot.Should().BeEquivalentTo(cyclicRootDto);

        // Assert
        act
            .Should().Throw<XunitException>()
            .WithMessage("Expected property cyclicRoot.Level.Root to be*but it contains a cyclic reference*");
    }

    [Fact]
    public void When_validating_nested_properties_and_ignoring_cyclic_references_it_should_succeed()
    {
        // Arrange
        var cyclicRoot = new CyclicRoot { Text = "Root" };
        cyclicRoot.Level = new CyclicLevel1 { Text = "Level1", Root = cyclicRoot };

        var cyclicRootDto = new CyclicRootDto { Text = "Root" };
        cyclicRootDto.Level = new CyclicLevel1Dto { Text = "Level1", Root = cyclicRootDto };

        // Act
        Action act = () =>
            cyclicRoot.Should().BeEquivalentTo(cyclicRootDto, options => options.IgnoringCyclicReferences());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_two_cyclic_graphs_are_equivalent_when_ignoring_cycle_references_it_should_succeed()
    {
        // Arrange
        var actual = new Parent();
        actual.Child1 = new Child(actual, 1);
        actual.Child2 = new Child(actual);

        var expected = new Parent();
        expected.Child1 = new Child(expected);
        expected.Child2 = new Child(expected);

        // Act
        Action act = () => actual.Should().BeEquivalentTo(expected, x => x
            .Excluding(y => y.Child1)
            .IgnoringCyclicReferences());

        // Assert
        act.Should().NotThrow();
    }

    public class Parent
    {
        public Child Child1 { get; set; }

        public Child Child2 { get; set; }
    }

    public class Child
    {
        public Child(Parent parent, int stuff = 0)
        {
            Parent = parent;
            Stuff = stuff;
        }

        public Parent Parent { get; set; }

        public int Stuff { get; set; }
    }

    [Fact]
    public void When_validating_nested_properties_that_are_null_it_should_not_throw_on_cyclic_references()
    {
        // Arrange
        var actual = new CyclicRoot { Text = null };

        actual.Level = new CyclicLevel1 { Text = null, Root = null };

        var expectation = new CyclicRootDto { Text = null };

        expectation.Level = new CyclicLevel1Dto { Text = null, Root = null };

        // Act
        Action act = () => actual.Should().BeEquivalentTo(expectation);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_the_graph_contains_the_same_value_object_it_should_not_be_treated_as_a_cyclic_reference()
    {
        // Arrange
        var actual = new CyclicRootWithValueObject { Object = new ValueObject("MyValue") };

        actual.Level = new CyclicLevelWithValueObject { Object = new ValueObject("MyValue"), Root = null };

        var expectation = new CyclicRootWithValueObject { Object = new ValueObject("MyValue") };

        expectation.Level = new CyclicLevelWithValueObject { Object = new ValueObject("MyValue"), Root = null };

        // Act
        Action act = () => actual.Should().BeEquivalentTo(expectation);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_types_with_infinite_object_graphs_are_equivalent_it_should_not_overflow_the_stack()
    {
        // Arrange
        var recursiveClass1 = new ClassWithInfinitelyRecursiveProperty();
        var recursiveClass2 = new ClassWithInfinitelyRecursiveProperty();

        // Act
        Action act =
            () => recursiveClass1.Should().BeEquivalentTo(recursiveClass2);

        // Assert
        act.Should().NotThrow<StackOverflowException>();
    }

    [Fact]
    public void
        When_asserting_equivalence_on_objects_needing_high_recursion_depth_and_disabling_recursion_depth_limit_it_should_recurse_to_completion()
    {
        // Arrange
        var recursiveClass1 = new ClassWithFiniteRecursiveProperty(15);
        var recursiveClass2 = new ClassWithFiniteRecursiveProperty(15);

        // Act
        Action act =
            () => recursiveClass1.Should().BeEquivalentTo(recursiveClass2,
                options => options.AllowingInfiniteRecursion());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Allowing_infinite_recursion_is_described_in_the_failure_message()
    {
        // Arrange
        var recursiveClass1 = new ClassWithFiniteRecursiveProperty(1);
        var recursiveClass2 = new ClassWithFiniteRecursiveProperty(2);

        // Act
        Action act = () => recursiveClass1.Should().BeEquivalentTo(recursiveClass2,
            options => options.AllowingInfiniteRecursion());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Recurse indefinitely*");
    }

    [Fact]
    public void When_injecting_a_null_config_to_BeEquivalentTo_it_should_throw()
    {
        // Arrange
        var recursiveClass1 = new ClassWithFiniteRecursiveProperty(15);
        var recursiveClass2 = new ClassWithFiniteRecursiveProperty(15);

        // Act
        Action act = () => recursiveClass1.Should().BeEquivalentTo(recursiveClass2, config: null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("config");
    }

    [Fact]
    public void
        When_asserting_inequivalence_on_objects_needing_high_recursion_depth_and_disabling_recursion_depth_limit_it_should_recurse_to_completion()
    {
        // Arrange
        var recursiveClass1 = new ClassWithFiniteRecursiveProperty(15);
        var recursiveClass2 = new ClassWithFiniteRecursiveProperty(16);

        // Act
        Action act =
            () => recursiveClass1.Should().NotBeEquivalentTo(recursiveClass2,
                options => options.AllowingInfiniteRecursion());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_injecting_a_null_config_to_NotBeEquivalentTo_it_should_throw()
    {
        // Arrange
        var recursiveClass1 = new ClassWithFiniteRecursiveProperty(15);
        var recursiveClass2 = new ClassWithFiniteRecursiveProperty(16);

        // Act
        Action act = () => recursiveClass1.Should().NotBeEquivalentTo(recursiveClass2, config: null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("config");
    }

    [Fact]
    public void When_an_enumerable_collection_returns_itself_it_should_detect_the_cyclic_reference()
    {
        // Act
        var instance1 = new SelfReturningEnumerable();
        var instance2 = new SelfReturningEnumerable();
        var actual = new List<SelfReturningEnumerable> { instance1, instance2 };

        // Assert
        Action act = () => actual.Should().BeEquivalentTo(
            new[] { new SelfReturningEnumerable(), new SelfReturningEnumerable() },
            "we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*we want to test the failure message*cyclic reference*");
    }

    public class SelfReturningEnumerable : IEnumerable<SelfReturningEnumerable>
    {
        public IEnumerator<SelfReturningEnumerable> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class LogbookEntryProjection
    {
        public virtual LogbookCode Logbook { get; set; }

        public virtual ICollection<LogbookRelation> LogbookRelations { get; set; }
    }

    internal class LogbookRelation
    {
        public virtual LogbookCode Logbook { get; set; }
    }

    internal class LogbookCode
    {
        public LogbookCode(string key)
        {
            Key = key;
        }

        public string Key { get; protected set; }
    }

    [Fact]
    public void When_the_root_object_is_referenced_from_a_nested_object_it_should_treat_it_as_a_cyclic_reference()
    {
        // Arrange
        var company1 = new MyCompany { Name = "Company" };
        var user1 = new MyUser { Name = "User", Company = company1 };
        var logo1 = new MyCompanyLogo { Url = "blank", Company = company1, CreatedBy = user1 };
        company1.Logo = logo1;

        var company2 = new MyCompany { Name = "Company" };
        var user2 = new MyUser { Name = "User", Company = company2 };
        var logo2 = new MyCompanyLogo { Url = "blank", Company = company2, CreatedBy = user2 };
        company2.Logo = logo2;

        // Act
        Action action = () => company1.Should().BeEquivalentTo(company2, o => o.IgnoringCyclicReferences());

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Allow_ignoring_cyclic_references_in_value_types_compared_by_members()
    {
        // Arrange
        var expectation = new ValueTypeCircularDependency() { Title = "First" };

        var second = new ValueTypeCircularDependency() { Title = "Second", Previous = expectation };

        expectation.Next = second;

        var subject = new ValueTypeCircularDependency() { Title = "First" };

        var secondCopy = new ValueTypeCircularDependency() { Title = "SecondDifferent", Previous = subject };

        subject.Next = secondCopy;

        // Act
        Action act = () => subject.Should()
            .BeEquivalentTo(expectation, opt => opt
                .ComparingByMembers<ValueTypeCircularDependency>()
                .IgnoringCyclicReferences());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*subject.Next.Title*Second*SecondDifferent*")
            .Which.Message.Should().NotContain("maximum recursion depth was reached");
    }

    public class ValueTypeCircularDependency
    {
        public string Title { get; set; }

        public ValueTypeCircularDependency Previous { get; set; }

        public ValueTypeCircularDependency Next { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return (obj is ValueTypeCircularDependency baseObj) && baseObj.Title == Title;
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
    }
}
