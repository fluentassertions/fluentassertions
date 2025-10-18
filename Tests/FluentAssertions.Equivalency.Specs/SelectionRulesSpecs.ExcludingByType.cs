using System;
using System.Collections.Generic;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

public partial class SelectionRulesSpecs
{
    public class ExcludingByType
    {
        [Fact]
        public void When_excluding_members_by_type_it_should_exclude_exact_type_matches()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                Age = 30,
                Duration = TimeSpan.FromHours(5),
                Score = 100
            };

            var expectation = new
            {
                Name = "John",
                Age = 30,
                Duration = TimeSpan.FromHours(10), // Different value
                Score = 100
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding<TimeSpan>());
        }

        [Fact]
        public void When_excluding_members_by_type_using_Type_parameter_it_should_exclude_exact_type_matches()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                Age = 30,
                Duration = TimeSpan.FromHours(5),
                Score = 100
            };

            var expectation = new
            {
                Name = "John",
                Age = 30,
                Duration = TimeSpan.FromHours(10), // Different value
                Score = 100
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding(typeof(TimeSpan)));
        }

        [Fact]
        public void When_excluding_members_by_type_it_should_fail_if_non_excluded_members_differ()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                Age = 30,
                Duration = TimeSpan.FromHours(5)
            };

            var expectation = new
            {
                Name = "Jack", // Different value
                Age = 30,
                Duration = TimeSpan.FromHours(10)
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding<TimeSpan>());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Name*Jack*John*");
        }

        [Fact]
        public void When_excluding_interface_type_it_should_exclude_all_members_assignable_to_that_interface()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                Items = new List<int> { 1, 2, 3 },
                Values = new[] { 4, 5, 6 },
                Age = 30
            };

            var expectation = new
            {
                Name = "John",
                Items = new List<int> { 7, 8, 9 }, // Different value
                Values = new[] { 10, 11, 12 }, // Different value
                Age = 30
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding<IEnumerable<int>>());
        }

        [Fact]
        public void When_excluding_open_generic_type_it_should_exclude_all_closed_generics()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                NullableInt = (int?)42,
                NullableDouble = (double?)3.14,
                NullableDateTime = (DateTime?)DateTime.Now,
                Age = 30
            };

            var expectation = new
            {
                Name = "John",
                NullableInt = (int?)99, // Different value
                NullableDouble = (double?)2.71, // Different value
                NullableDateTime = (DateTime?)DateTime.Now.AddDays(1), // Different value
                Age = 30
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding(typeof(Nullable<>)));
        }

        [Fact]
        public void When_excluding_by_type_it_should_be_described_in_the_failure_message()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                Age = 30,
                Duration = TimeSpan.FromHours(5)
            };

            var expectation = new
            {
                Name = "Jack",
                Age = 30,
                Duration = TimeSpan.FromHours(10)
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding<TimeSpan>());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Exclude members of type System.TimeSpan*");
        }

        [Fact]
        public void When_excluding_nested_members_by_type_it_should_exclude_them_at_all_levels()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                Settings = new
                {
                    Timeout = TimeSpan.FromSeconds(30),
                    Name = "Config"
                },
                Interval = TimeSpan.FromMinutes(5)
            };

            var expectation = new
            {
                Name = "John",
                Settings = new
                {
                    Timeout = TimeSpan.FromSeconds(60), // Different value
                    Name = "Config"
                },
                Interval = TimeSpan.FromMinutes(10) // Different value
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding<TimeSpan>());
        }

        [Fact]
        public void When_excluding_members_by_type_in_collections_it_should_exclude_them()
        {
            // Arrange
            var subject = new[]
            {
                new
                {
                    Name = "John",
                    Duration = TimeSpan.FromHours(5)
                },
                new
                {
                    Name = "Jane",
                    Duration = TimeSpan.FromHours(3)
                }
            };

            var expectation = new[]
            {
                new
                {
                    Name = "John",
                    Duration = TimeSpan.FromHours(10) // Different value
                },
                new
                {
                    Name = "Jane",
                    Duration = TimeSpan.FromHours(8) // Different value
                }
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding<TimeSpan>());
        }

        [Fact]
        public void When_excluding_null_type_it_should_throw()
        {
            // Arrange
            var subject = new { Name = "John" };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(subject, options => options
                .Excluding((Type)null));

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_excluding_interface_type_description_should_mention_assignability()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                Items = new List<int> { 1, 2, 3 }
            };

            var expectation = new
            {
                Name = "Jack",
                Items = new List<int> { 4, 5, 6 }
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding<IEnumerable<int>>());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Exclude members assignable to type*");
        }

        [Fact]
        public void When_excluding_open_generic_type_description_should_mention_closed_generic()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                NullableInt = (int?)42
            };

            var expectation = new
            {
                Name = "Jack",
                NullableInt = (int?)99
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding(typeof(Nullable<>)));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Exclude members of closed generic type*");
        }

        [Fact]
        public void When_excluding_abstract_class_type_it_should_exclude_all_derived_types()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                BaseObject = new ConcreteClass { Value = 10 },
                Age = 30
            };

            var expectation = new
            {
                Name = "John",
                BaseObject = new ConcreteClass { Value = 20 }, // Different value
                Age = 30
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding<AbstractBaseClass>());
        }

        [Fact]
        public void Can_exclude_multiple_types()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                Age = 30,
                Duration = TimeSpan.FromHours(5),
                Score = 100.5
            };

            var expectation = new
            {
                Name = "John",
                Age = 30,
                Duration = TimeSpan.FromHours(10), // Different value
                Score = 200.5 // Different value
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding<TimeSpan>()
                .Excluding<double>());
        }

        [Fact]
        public void When_excluding_string_type_it_should_exclude_all_string_members()
        {
            // Arrange
            var subject = new
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30
            };

            var expectation = new
            {
                FirstName = "Jane", // Different value
                LastName = "Smith", // Different value
                Age = 30
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding<string>());
        }

        [Fact]
        public void When_excluding_by_type_with_nested_generics_it_should_work()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                ListOfLists = new List<List<int>> { new() { 1, 2 } },
                Age = 30
            };

            var expectation = new
            {
                Name = "John",
                ListOfLists = new List<List<int>> { new() { 3, 4 } }, // Different value
                Age = 30
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding<List<List<int>>>());
        }

        [Fact]
        public void When_excluding_value_types_it_should_work()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                Age = 30,
                Score = 100,
                Rating = 5
            };

            var expectation = new
            {
                Name = "John",
                Age = 35, // Different value
                Score = 200, // Different value
                Rating = 10 // Different value
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding<int>());
        }

        private abstract class AbstractBaseClass
        {
            public int Value { get; set; }
        }

        private class ConcreteClass : AbstractBaseClass
        {
        }
    }
}
