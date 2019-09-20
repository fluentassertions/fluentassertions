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
#if NETCOREAPP1_1
                "Expected collection to be an enumerable because we want to test the failure message, but found <null>.");
#else
                "Expected someObject to be an enumerable because we want to test the failure message, but found <null>.");
#endif
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
#if NETCOREAPP1_1
                @"Expected collection to be an enumerable because we want to test the failure message, but ""System.Object"" is missing a valid 'GetEnumerator' method.");
#else
                @"Expected someObject to be an enumerable because we want to test the failure message, but ""System.Object"" is missing a valid 'GetEnumerator' method.");
#endif
        }

        [Fact]
        public void When_enumerator_does_not_have_Current_it_should_fail_with_a_descriptive_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object someObject = new MissingCurrentEnumerable<int>();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => someObject.Should().BeEnumerable("because we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                @"Expected collection to be an enumerable because we want to test the failure message, but ""FluentAssertions.Specs.MissingCurrentEnumerable`1[System.Int32]"" is missing a valid 'Current' property.");
#else
                @"Expected someObject to be an enumerable because we want to test the failure message, but ""FluentAssertions.Specs.MissingCurrentEnumerable`1[System.Int32]"" is missing a valid 'Current' property.");
#endif
        }

        [Fact]
        public void When_enumerator_does_not_have_MoveNext_it_should_fail_with_a_descriptive_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object someObject = new MissingMoveNextEnumerable<int>();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => someObject.Should().BeEnumerable("because we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                @"Expected collection to be an enumerable because we want to test the failure message, but ""FluentAssertions.Specs.MissingMoveNextEnumerable`1[System.Int32]"" is missing a valid 'MoveNext' method.");
#else
                @"Expected someObject to be an enumerable because we want to test the failure message, but ""FluentAssertions.Specs.MissingMoveNextEnumerable`1[System.Int32]"" is missing a valid 'MoveNext' method.");
#endif
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
            object someObject = new RangeEnumerable(5);

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
        public void When_object_does_not_have_GetEnumerator_with_comparison_it_should_fail_with_a_descriptive_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object someObject = new object();
            IEnumerable collection2 = new[] { 0, 1, 20, 3, 4 };

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => someObject.Should().BeEnumerable(collection2, (a, e) => int.Equals(a, e), "because we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                @"Expected collection to be an enumerable because we want to test the failure message, but ""System.Object"" is missing a valid 'GetEnumerator' method.");
#else
                @"Expected someObject to be an enumerable because we want to test the failure message, but ""System.Object"" is missing a valid 'GetEnumerator' method.");
#endif
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

        [Fact]
        public void When_two_collections_are_not_equal_because_one_item_differs_it_should_throw_using_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object collection1 = new RangeEnumerable(5);
            IEnumerable collection2 = new[] { 0, 1, 20, 3, 4 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => collection1.Should().BeEnumerable(collection2, (a, e) => int.Equals(a, e), "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection to be equal to {0, 1, 20, 3, 4} because we want to test the failure message, but {0, 1, 2, 3, 4} differs at index 2 when using 'foreach'.");
#else
                "Expected collection1 to be equal to {0, 1, 20, 3, 4} because we want to test the failure message, but {0, 1, 2, 3, 4} differs at index 2 when using 'foreach'.");
#endif
        }

        [Fact]
        public void When_two_collections_are_not_equal_because_the_actual_collection_contains_more_items_it_should_throw_using_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object collection1 = new RangeEnumerable(5);
            IEnumerable collection2 = new[] { 0, 1, 2 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => collection1.Should().BeEnumerable(collection2, (a, e) => int.Equals(a, e), "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection to be equal to {0, 1, 2} because we want to test the failure message, but {0, 1, 2, 3, 4} differs at index 3 when using 'foreach'.");
#else
                "Expected collection1 to be equal to {0, 1, 2} because we want to test the failure message, but {0, 1, 2, 3, 4} differs at index 3 when using 'foreach'.");
#endif
        }

        [Fact]
        public void When_two_collections_are_not_equal_because_the_actual_collection_contains_less_items_it_should_throw_using_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            object collection1 = new RangeEnumerable(5);
            IEnumerable collection2 = new[] { 0, 1, 2, 3, 4, 5, 6 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => collection1.Should().BeEnumerable(collection2, (a, e) => int.Equals(a, e), "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection to be equal to {0, 1, 2, 3, 4, 5, 6} because we want to test the failure message, but {0, 1, 2, 3, 4} differs at index 5 when using 'foreach'.");
#else
                "Expected collection1 to be equal to {0, 1, 2, 3, 4, 5, 6} because we want to test the failure message, but {0, 1, 2, 3, 4} differs at index 5 when using 'foreach'.");
#endif
        }
    }

    internal class MissingCurrentEnumerable<T>
    {
        public MissingCurrentEnumerable<T> GetEnumerator() => this;

        private T Current => default;

        public bool MoveNext() => false;
    }

    internal class MissingMoveNextEnumerable<T>
    {
        public MissingMoveNextEnumerable<T> GetEnumerator() => this;

        public T Current => default;

        private bool MoveNext() => false;
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

    internal class RangeEnumerable
    {
        int count;

        public RangeEnumerable(int enumerableCount)
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
