using System;
using System.Collections;
using System.Collections.Generic;

using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class ObjectEnumerableSpecs
    {

        [Fact]
        public void When_object_is_null_it_should_fail_with_a_descriptive_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object someObject = null;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => someObject.Should().BeEnumerable("because we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>().WithMessage(
                "Expected someObject to be an enumerable because we want to test the failure message, but found <null>.");
        }

        [Fact]
        public void When_object_does_not_have_GetEnumerator_it_should_fail_with_a_descriptive_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object someObject = new object();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => someObject.Should().BeEnumerable("because we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>().WithMessage(
                @"Expected someObject to be an enumerable because we want to test the failure message, but ""System.Object"" is missing a valid 'GetEnumerator' method.");
        }

        [Fact]
        public void When_enumerator_does_not_have_Current_it_should_fail_with_a_descriptive_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object someObject = new MissingCurrentEnumerable();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => someObject.Should().BeEnumerable("because we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>().WithMessage(
                @"Expected someObject to be an enumerable because we want to test the failure message, but ""FluentAssertions.Specs.MissingCurrentEnumerable"" is missing a valid 'Current' property.");
        }

        [Fact]
        public void When_enumerator_does_not_have_MoveNext_it_should_fail_with_a_descriptive_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object someObject = new MissingMoveNextEnumerable();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => someObject.Should().BeEnumerable("because we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>().WithMessage(
                @"Expected someObject to be an enumerable because we want to test the failure message, but ""FluentAssertions.Specs.MissingMoveNextEnumerable"" is missing a valid 'MoveNext' method.");
        }

        [Fact]
        public void When_object_is_enumerable_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object someObject = new EmptyEnumerable<int>();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => someObject.Should().BeEnumerable();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void When_object_is_enumerable_with_explicit_interfaces_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object someObject = new EmptyEnumerableExplicitInterfaces<int>();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => someObject.Should().BeEnumerable();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_is_equal_to_the_same_list_of_elements()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object someObject = new CustomRangeEnumerable(5);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => someObject.Should().BeEnumerable(new[] { 0, 1, 2, 3, 4 }, (a, e) => int.Equals(a, e));

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void When_both_collections_are_null_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object nullColl = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => nullColl.Should().BeEnumerable(null, (a, e) => int.Equals(a, e));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }
    }

    internal class MissingCurrentEnumerable
    {
        public MissingCurrentEnumerable GetEnumerator() => this;

        public bool MoveNext() => false;
    }

    internal class MissingMoveNextEnumerable
    {
        public MissingMoveNextEnumerable GetEnumerator() => this;

        public int Current => 0;
    }

    internal class EmptyEnumerable<T>
    {
        public EmptyEnumerable<T> GetEnumerator() => this;

        public T Current => default;

        public bool MoveNext() => false;
    }

    internal class EmptyEnumerableExplicitInterfaces<T> : IEnumerable<T>, IEnumerator<T>
    {
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;

        T IEnumerator<T>.Current => default;
        object IEnumerator.Current => default;

        bool IEnumerator.MoveNext() => false;
        void IEnumerator.Reset() {}
        void IDisposable.Dispose() {}
    }

    internal class CustomRangeEnumerable
    {
        int count;

        public CustomRangeEnumerable(int enumerableCount)
        {
            count = enumerableCount;
        }

        public Enumerator GetEnumerator() => new Enumerator(count);

        public struct Enumerator
        {
            readonly int count;
            int current;

            public Enumerator(int count)
            {
                this.count = count;
                current = -1;
            }

            public int Current => current;

            public bool MoveNext() => ++current < count;
        }
    }

}
