using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Xunit;

namespace FluentAssertions.Specs.Primitives;

public partial class ObjectAssertionSpecs
{
    public class Miscellaneous
    {
        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            var someObject = new Exception();

            // Act / Assert
            someObject.Should()
                .BeOfType<Exception>()
                .And
                .NotBeNull();
        }

        [Fact]
        public void Should_throw_a_helpful_error_when_accidentally_using_equals()
        {
            // Arrange
            var someObject = new Exception();

            // Act
            var action = () => someObject.Should().Equals(null);

            // Assert
            action.Should().Throw<NotSupportedException>()
                .WithMessage("Equals is not part of Fluent Assertions. Did you mean Be() or BeSameAs() instead?");
        }
    }

    internal class DumbObjectEqualityComparer : IEqualityComparer<object>
    {
        // ReSharper disable once MemberHidesStaticFromOuterClass
        [SuppressMessage("Class Design", "AV1010:Member hides inherited member")]
        public new bool Equals(object x, object y)
        {
            return (x == y) || (x is not null && y is not null && x.Equals(y));
        }

        public int GetHashCode(object obj) => obj.GetHashCode();
    }
}

internal class DummyBaseClass;

internal sealed class DummyImplementingClass : DummyBaseClass, IDisposable
{
    public void Dispose()
    {
        // Ignore
    }
}

internal class SomeClass
{
    public SomeClass(int key)
    {
        Key = key;
    }

    public int Key { get; }

    public override string ToString() => $"SomeClass({Key})";
}

internal class SomeClassEqualityComparer : IEqualityComparer<SomeClass>
{
    public bool Equals(SomeClass x, SomeClass y)
    {
        return (x == y) || (x is not null && y is not null && x.Key.Equals(y.Key));
    }

    public int GetHashCode(SomeClass obj) => obj.Key;
}

internal class SomeClassAssertions : ObjectAssertions<SomeClass, SomeClassAssertions>
{
    public SomeClassAssertions(SomeClass value)
        : base(value, AssertionChain.GetOrCreate())
    {
    }
}

internal static class AssertionExtensions
{
    public static SomeClassAssertions Should(this SomeClass value) => new(value);
}
