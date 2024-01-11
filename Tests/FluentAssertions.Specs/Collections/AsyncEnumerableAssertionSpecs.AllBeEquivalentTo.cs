using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Extensions;
using FluentAssertions.Specs.Primitives;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

/// <content>
/// The AllBeEquivalentTo specs.
/// </content>
public partial class AsyncEnumerableAssertionSpecs
{
    public class AllBeEquivalentTo
    {
        [Fact]
        public void When_all_subject_items_are_equivalent_to_expectation_object_it_should_succeed()
        {
            // Arrange
            var subject = new List<SomeDto>
            {
                new() { Name = "someDto", Age = 1 },
                new() { Name = "someDto", Age = 1 },
                new() { Name = "someDto", Age = 1 }
            };

            // Act
            Action action = () => subject.ToAsyncEnumerable().Should().AllBeEquivalentTo(new
            {
                Name = "someDto",
                Age = 1,
                Birthdate = default(DateTime)
            });

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_all_subject_items_are_equivalent_to_expectation_object_it_should_allow_chaining()
        {
            // Arrange
            var subject = new List<SomeDto>
            {
                new() { Name = "someDto", Age = 1 },
                new() { Name = "someDto", Age = 1 },
                new() { Name = "someDto", Age = 1 }
            };

            // Act
            Action action = () =>
            {
                var expectation = new
                {
                    Name = "someDto",
                    Age = 1,
                    Birthdate = default(DateTime)
                };

                subject.ToAsyncEnumerable().Should().AllBeEquivalentTo(expectation).And.HaveCount(3);
            };

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_some_subject_items_are_not_equivalent_to_expectation_object_it_should_throw()
        {
            // Arrange
            int[] array = [1, 2, 3];
            var subject = array.ToAsyncEnumerable();

            // Act
            Action action = () => subject.Should().AllBeEquivalentTo(1);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected subject[1]*to be 1, but found 2.*Expected subject[2]*to be 1, but found 3*");
        }

        [Fact]
        public void When_more_than_10_subjects_items_are_not_equivalent_to_expectation_only_10_are_reported()
        {
            // Arrange
            var subject = AsyncEnumerable.Repeat(2, 11);

            // Act
            Action action = () => subject.Should().AllBeEquivalentTo(1);

            // Assert
            action.Should().Throw<XunitException>().Which
                .Message.Should().Contain("subject[9] to be 1, but found 2")
                .And.NotContain("item[10]");
        }

        [Fact]
        public void
            When_some_subject_items_are_not_equivalent_to_expectation_for_huge_table_execution_time_should_still_be_short()
        {
            // Arrange
            const int N = 100000;
            var subject = new List<int>(N) { 1 };

            for (int i = 1; i < N; i++)
            {
                subject.Add(2);
            }

            // Act
            Action action = () =>
            {
                try
                {
                    subject.ToAsyncEnumerable().Should().AllBeEquivalentTo(1);
                }
                catch
                {
                    // ignored, we only care about execution time
                }
            };

            // Assert
            action.ExecutionTime().Should().BeLessThan(1.Seconds());
        }
    }
}
