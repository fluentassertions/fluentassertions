using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Equivalency.Specs;

public class CyclicReferencesSpecs
{
    [Fact]
    public async Task Graphs_up_to_the_maximum_depth_are_supported()
    {
        // Arrange
        var actual = new ClassWithFiniteRecursiveProperty(recursiveDepth: 10);
        var expectation = new ClassWithFiniteRecursiveProperty(recursiveDepth: 10);

        // Act/Assert
        await actual.Should().BeEquivalentToAsync(expectation);
    }

    [Fact]
    public async Task Graphs_deeper_than_the_maximum_depth_are_not_supported()
    {
        // Arrange
        var actual = new ClassWithFiniteRecursiveProperty(recursiveDepth: 11);
        var expectation = new ClassWithFiniteRecursiveProperty(recursiveDepth: 11);

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().ThrowAsync<XunitException>().WithMessage("*maximum*depth*10*");
    }

    [Fact]
    public async Task By_default_cyclic_references_are_not_valid()
    {
        // Arrange
        var cyclicRoot = new CyclicRoot
        {
            Text = "Root"
        };

        cyclicRoot.Level = new CyclicLevel1
        {
            Text = "Level1",
            Root = cyclicRoot
        };

        var cyclicRootDto = new CyclicRootDto
        {
            Text = "Root"
        };

        cyclicRootDto.Level = new CyclicLevel1Dto
        {
            Text = "Level1",
            Root = cyclicRootDto
        };

        // Act
        Func<Task> act = () => cyclicRoot.Should().BeEquivalentToAsync(cyclicRootDto);

        // Assert
        await act
            .Should().ThrowAsync<XunitException>()
            .WithMessage("Expected property cyclicRoot.Level.Root to be*but it contains a cyclic reference*");
    }

    [Fact]
    public async Task Two_graphs_with_ignored_cyclic_references_can_be_compared()
    {
        // Arrange
        var actual = new Parent();
        actual.Child1 = new Child(actual, 1);
        actual.Child2 = new Child(actual);

        var expected = new Parent();
        expected.Child1 = new Child(expected);
        expected.Child2 = new Child(expected);

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected, x => x
            .Excluding(y => y.Child1)
            .IgnoringCyclicReferences());

        // Assert
        await act.Should().NotThrowAsync();
    }

    private class Parent
    {
        public Child Child1 { get; set; }

        public Child Child2 { get; set; }
    }

    private class Child
    {
        public Child(Parent parent, int stuff = 0)
        {
            Parent = parent;
            Stuff = stuff;
        }

        public Parent Parent { get; }

        public int Stuff { get; }
    }

    [Fact]
    public async Task Nested_properties_that_are_null_are_not_treated_as_cyclic_references()
    {
        // Arrange
        var actual = new CyclicRoot
        {
            Text = null,
            Level = new CyclicLevel1
            {
                Text = null,
                Root = null
            }
        };

        var expectation = new CyclicRootDto
        {
            Text = null,
            Level = new CyclicLevel1Dto
            {
                Text = null,
                Root = null
            }
        };

        // Act / Assert
        await actual.Should().BeEquivalentToAsync(expectation);
    }

    [Fact]
    public async Task Equivalent_value_objects_are_not_treated_as_cyclic_references()
    {
        // Arrange
        var actual = new CyclicRootWithValueObject
        {
            Value = new ValueObject("MyValue"),
            Level = new CyclicLevelWithValueObject
            {
                Value = new ValueObject("MyValue"),
                Root = null
            }
        };

        var expectation = new CyclicRootWithValueObject
        {
            Value = new ValueObject("MyValue"),
            Level = new CyclicLevelWithValueObject
            {
                Value = new ValueObject("MyValue"),
                Root = null
            }
        };

        // Act  / Assert
        await actual.Should().BeEquivalentToAsync(expectation);
    }

    [Fact]
    public async Task Cyclic_references_do_not_trigger_stack_overflows()
    {
        // Arrange
        var recursiveClass1 = new ClassWithInfinitelyRecursiveProperty();
        var recursiveClass2 = new ClassWithInfinitelyRecursiveProperty();

        // Act
        Func<Task> act =
            () => recursiveClass1.Should().BeEquivalentToAsync(recursiveClass2);

        // Assert
        await act.Should().NotThrowAsync<StackOverflowException>();
    }

    [Fact]
    public async Task Cyclic_references_can_be_ignored_in_equivalency_assertions()
    {
        // Arrange
        var recursiveClass1 = new ClassWithFiniteRecursiveProperty(15);
        var recursiveClass2 = new ClassWithFiniteRecursiveProperty(15);

        // Act / Assert
        await recursiveClass1.Should().BeEquivalentToAsync(recursiveClass2, options => options.AllowingInfiniteRecursion());
    }

    [Fact]
    public async Task Allowing_infinite_recursion_is_reported_in_the_failure_message()
    {
        // Arrange
        var recursiveClass1 = new ClassWithFiniteRecursiveProperty(1);
        var recursiveClass2 = new ClassWithFiniteRecursiveProperty(2);

        // Act
        Func<Task> act = () => recursiveClass1.Should().BeEquivalentToAsync(recursiveClass2,
            options => options.AllowingInfiniteRecursion());

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*Recurse indefinitely*");
    }

    [Fact]
    public async Task Can_ignore_cyclic_references_for_inequivalency_assertions()
    {
        // Arrange
        var recursiveClass1 = new ClassWithFiniteRecursiveProperty(15);
        var recursiveClass2 = new ClassWithFiniteRecursiveProperty(16);

        // Act / Assert
        await recursiveClass1.Should().NotBeEquivalentToAsync(recursiveClass2,
                options => options.AllowingInfiniteRecursion());
    }

    [Fact]
    public async Task Can_detect_cyclic_references_in_enumerables()
    {
        // Act
        var instance1 = new SelfReturningEnumerable();
        var instance2 = new SelfReturningEnumerable();

        var actual = new List<SelfReturningEnumerable>
        {
            instance1,
            instance2
        };

        // Assert
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(
            new[] { new SelfReturningEnumerable(), new SelfReturningEnumerable() },
            "we want to test the failure {0}", "message");

        // Assert
        await act.Should().ThrowAsync<XunitException>().WithMessage("*we want to test the failure message*cyclic reference*");
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

    [Fact]
    public async Task Can_detect_cyclic_references_in_nested_objects_referring_to_the_root()
    {
        // Arrange
        var company1 = new MyCompany
        {
            Name = "Company"
        };

        var user1 = new MyUser
        {
            Name = "User",
            Company = company1
        };

        var logo1 = new MyCompanyLogo
        {
            Url = "blank",
            Company = company1,
            CreatedBy = user1
        };

        company1.Logo = logo1;

        var company2 = new MyCompany
        {
            Name = "Company"
        };

        var user2 = new MyUser
        {
            Name = "User",
            Company = company2
        };

        var logo2 = new MyCompanyLogo
        {
            Url = "blank",
            Company = company2,
            CreatedBy = user2
        };

        company2.Logo = logo2;

        // Act / Assert
        await company1.Should().BeEquivalentToAsync(company2, o => o.IgnoringCyclicReferences());
    }

    [Fact]
    public async Task Allow_ignoring_cyclic_references_in_value_types_compared_by_members()
    {
        // Arrange
        var expectation = new ValueTypeCircularDependency
        {
            Title = "First"
        };

        var second = new ValueTypeCircularDependency
        {
            Title = "Second",
            Previous = expectation
        };

        expectation.Next = second;

        var subject = new ValueTypeCircularDependency
        {
            Title = "First"
        };

        var secondCopy = new ValueTypeCircularDependency
        {
            Title = "SecondDifferent",
            Previous = subject
        };

        subject.Next = secondCopy;

        // Act
        Func<Task> act = () => subject.Should()
            .BeEquivalentToAsync(expectation, opt => opt
                .ComparingByMembers<ValueTypeCircularDependency>()
                .IgnoringCyclicReferences());

        // Assert
        (await act.Should().ThrowAsync<XunitException>())
            .WithMessage("*subject.Next.Title*SecondDifferent*Second*")
            .Which.Message.Should().NotContain("maximum recursion depth was reached");
    }

    private class ValueTypeCircularDependency
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

            return obj is ValueTypeCircularDependency baseObj && baseObj.Title == Title;
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
    }

    private class ClassWithInfinitelyRecursiveProperty
    {
        public ClassWithInfinitelyRecursiveProperty Self
        {
            get { return new ClassWithInfinitelyRecursiveProperty(); }
        }
    }

    private class ClassWithFiniteRecursiveProperty
    {
        private readonly int depth;

        public ClassWithFiniteRecursiveProperty(int recursiveDepth)
        {
            depth = recursiveDepth;
        }

        public ClassWithFiniteRecursiveProperty Self
        {
            get
            {
                return depth > 0
                    ? new ClassWithFiniteRecursiveProperty(depth - 1)
                    : null;
            }
        }
    }
}
