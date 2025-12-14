using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

public partial class SelectionRulesSpecs
{
    public class ExcludingByType
    {
        [Fact]
        public void Can_exclude_exact_type_using_a_type_parameter()
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
        public void Can_exclude_exact_type_using_a_type_instance()
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
        public void Other_members_are_still_selected()
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
                .WithMessage("*Exclude members of type System.TimeSpan*");
        }

        [Fact]
        public void Can_exclude_all_types_implementing_a_generic_interface()
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
        public void Can_exclude_all_closed_generic_types()
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
        public void Excluding_applies_to_all_levels_in_a_nested_structure()
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
        public void Excluding_applies_to_collections_as_well()
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
        public void Null_is_not_a_valid_type()
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
        public void Reports_the_interface_type()
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
                .WithMessage("*Exclude members whose type is/derives from*");
        }

        [Fact]
        public void Reports_the_open_generic_type()
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
                .WithMessage("*Exclude members whose type derives from/is a closed generic type of*");
        }

        [Fact]
        public void Can_exclude_all_types_deriving_from_an_abstract_type()
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
        public void Excluding_string_type_excludes_all_string_members()
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
        public void Excluding_by_type_with_nested_generics_works()
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
        public void Can_exclude_value_types()
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

        [Fact]
        public void Excluding_non_abstract_base_type_excludes_derived_types()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                BaseObject = new DerivedFromNonAbstract { Value = 10 },
                Age = 30
            };

            var expectation = new
            {
                Name = "John",
                BaseObject = new DerivedFromNonAbstract { Value = 20 }, // Different value
                Age = 30
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding<NonAbstractBase>());
        }

        [Fact]
        public void Excluding_open_generic_excludes_closed_generic_derived_type()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                ClosedGeneric = new ClosedGeneric { Value = 10 },
                Age = 30
            };

            var expectation = new
            {
                Name = "John",
                ClosedGeneric = new ClosedGeneric { Value = 20 }, // Different value
                Age = 30
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding(typeof(OpenGeneric<>)));
        }

        [Fact]
        public void Excluding_open_generic_excludes_derived_open_generic_type()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                DerivedOpenGeneric = new DerivedOpenGeneric<int> { Value = 10 },
                Age = 30
            };

            var expectation = new
            {
                Name = "John",
                DerivedOpenGeneric = new DerivedOpenGeneric<int> { Value = 20 }, // Different value
                Age = 30
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding(typeof(OpenGeneric<>)));
        }

        [Fact]
        public void Excluding_open_generic_excludes_closed_derived_generic_type()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                ClosedDerivedGeneric = new ClosedDerivedGeneric { Value = 10 },
                Age = 30
            };

            var expectation = new
            {
                Name = "John",
                ClosedDerivedGeneric = new ClosedDerivedGeneric { Value = 20 }, // Different value
                Age = 30
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding(typeof(OpenGeneric<>)));
        }

        [Fact]
        public void Excluding_sealed_type_excludes_only_exact_type()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                Text = "Test",
                Age = 30
            };

            var expectation = new
            {
                Name = "John",
                Text = "Different", // Different value - will be excluded
                Age = 30
            };

            // Act / Assert - string is sealed, so only exact matches are excluded
            subject.Should().BeEquivalentTo(expectation, options => options
                .Excluding<string>());
        }

        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        private abstract class AbstractBaseClass
        {
            public int Value { get; set; }
        }

        private class ConcreteClass : AbstractBaseClass;

        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        private class NonAbstractBase
        {
            public int Value { get; set; }
        }

        private class DerivedFromNonAbstract : NonAbstractBase;

        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        private class OpenGeneric<T>
        {
            public int Value { get; set; }
        }

        private class ClosedGeneric : OpenGeneric<int>;

        private class DerivedOpenGeneric<T> : OpenGeneric<T>;

        private class ClosedDerivedGeneric : DerivedOpenGeneric<int>;
    }
}
