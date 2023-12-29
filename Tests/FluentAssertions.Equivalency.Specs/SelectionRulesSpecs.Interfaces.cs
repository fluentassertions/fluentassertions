using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

public partial class SelectionRulesSpecs
{
    public class Interfaces
    {
        [Fact]
        public void When_an_interface_hierarchy_is_used_it_should_include_all_inherited_properties()
        {
            // Arrange
            ICar subject = new Car
            {
                VehicleId = 1,
                Wheels = 4
            };

            ICar expected = new Car
            {
                VehicleId = 99999,
                Wheels = 4
            };

            // Act
            Action action = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("Expected*VehicleId*99999*but*1*");
        }

        [Fact]
        public void When_a_reference_to_an_interface_is_provided_it_should_only_include_those_properties()
        {
            // Arrange
            IVehicle expected = new Car
            {
                VehicleId = 1,
                Wheels = 4
            };

            IVehicle subject = new Car
            {
                VehicleId = 1,
                Wheels = 99999
            };

            // Act
            Action action = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_a_reference_to_an_explicit_interface_impl_is_provided_it_should_only_include_those_properties()
        {
            // Arrange
            IVehicle expected = new ExplicitCar
            {
                Wheels = 4
            };

            IVehicle subject = new ExplicitCar
            {
                Wheels = 99999
            };

            // Act
            Action action = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Explicitly_implemented_subject_properties_are_ignored_if_a_normal_property_exists_with_the_same_name()
        {
            // Arrange
            IVehicle expected = new Vehicle
            {
                VehicleId = 1
            };

            IVehicle subject = new ExplicitVehicle
            {
                VehicleId = 2 // normal property
            };

            subject.VehicleId = 1; // explicitly implemented property

            // Act
            Action action = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Explicitly_implemented_read_only_subject_properties_are_ignored_if_a_normal_property_exists_with_the_same_name()
        {
            // Arrange
            IReadOnlyVehicle subject = new ExplicitReadOnlyVehicle(explicitValue: 1)
            {
                VehicleId = 2 // normal property
            };

            var expected = new Vehicle
            {
                VehicleId = 1
            };

            // Act
            Action action = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Explicitly_implemented_subject_properties_are_ignored_if_only_fields_are_included()
        {
            // Arrange
            var expected = new VehicleWithField
            {
                VehicleId = 1 // A field named like a property
            };

            var subject = new ExplicitVehicle
            {
                VehicleId = 2 // A real property
            };

            // Act
            Action action = () => subject.Should().BeEquivalentTo(expected, opt => opt
                .IncludingFields()
                .ExcludingProperties());

            // Assert
            action.Should().Throw<XunitException>().WithMessage("*field*VehicleId*other*");
        }

        [Fact]
        public void Explicitly_implemented_subject_properties_are_ignored_if_only_fields_are_included_and_they_may_be_missing()
        {
            // Arrange
            var expected = new VehicleWithField
            {
                VehicleId = 1 // A field named like a property
            };

            var subject = new ExplicitVehicle
            {
                VehicleId = 2 // A real property
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expected, opt => opt
                .IncludingFields()
                .ExcludingProperties()
                .ExcludingMissingMembers());
        }

        [Fact]
        public void Excluding_missing_members_does_not_affect_how_explicitly_implemented_subject_properties_are_dealt_with()
        {
            // Arrange
            IVehicle expected = new Vehicle
            {
                VehicleId = 1
            };

            IVehicle subject = new ExplicitVehicle
            {
                VehicleId = 2 // instance member
            };

            subject.VehicleId = 1; // interface member

            // Act
            Action action = () => subject.Should().BeEquivalentTo(expected, opt => opt.ExcludingMissingMembers());

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_respecting_declared_types_explicit_interface_member_on_interfaced_expectation_should_be_used()
        {
            // Arrange
            IVehicle expected = new ExplicitVehicle
            {
                VehicleId = 2 // instance member
            };

            expected.VehicleId = 1; // interface member

            IVehicle subject = new Vehicle
            {
                VehicleId = 1
            };

            // Act
            Action action = () => subject.Should().BeEquivalentTo(expected, opt => opt.RespectingDeclaredTypes());

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_respecting_runtime_types_explicit_interface_member_on_interfaced_subject_should_not_be_used()
        {
            // Arrange
            IVehicle expected = new Vehicle
            {
                VehicleId = 1
            };

            IVehicle subject = new ExplicitVehicle
            {
                VehicleId = 2 // instance member
            };

            subject.VehicleId = 1; // interface member

            // Act
            Action action = () => subject.Should().BeEquivalentTo(expected, opt => opt.RespectingRuntimeTypes());

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_respecting_runtime_types_explicit_interface_member_on_interfaced_expectation_should_not_be_used()
        {
            // Arrange
            IVehicle expected = new ExplicitVehicle
            {
                VehicleId = 2 // instance member
            };

            expected.VehicleId = 1; // interface member

            IVehicle subject = new Vehicle
            {
                VehicleId = 1
            };

            // Act
            Action action = () => subject.Should().BeEquivalentTo(expected, opt => opt.RespectingRuntimeTypes());

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_respecting_declared_types_explicit_interface_member_on_expectation_should_not_be_used()
        {
            // Arrange
            var expected = new ExplicitVehicle
            {
                VehicleId = 2
            };

            ((IVehicle)expected).VehicleId = 1;

            var subject = new Vehicle
            {
                VehicleId = 1
            };

            // Act
            Action action = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Can_find_explicitly_implemented_property_on_the_subject()
        {
            // Arrange
            IPerson person = new Person();
            person.Name = "Bob";

            // Act / Assert
            person.Should().BeEquivalentTo(new { Name = "Bob" });
        }

        private interface IPerson
        {
            string Name { get; set; }
        }

        private class Person : IPerson
        {
            string IPerson.Name { get; set; }
        }

        [Fact]
        public void Excluding_an_interface_property_through_inheritance_should_work()
        {
            // Arrange
            IInterfaceWithTwoProperties[] actual =
            {
                new DerivedClassImplementingInterface
                {
                    Value1 = 1,
                    Value2 = 2
                }
            };

            IInterfaceWithTwoProperties[] expected =
            {
                new DerivedClassImplementingInterface
                {
                    Value1 = 999,
                    Value2 = 2
                }
            };

            // Act / Assert
            actual.Should().BeEquivalentTo(expected, options => options
                .Excluding(a => a.Value1)
                .RespectingRuntimeTypes());
        }

        [Fact]
        public void Including_an_interface_property_through_inheritance_should_work()
        {
            // Arrange
            IInterfaceWithTwoProperties[] actual =
            {
                new DerivedClassImplementingInterface
                {
                    Value1 = 1,
                    Value2 = 2
                }
            };

            IInterfaceWithTwoProperties[] expected =
            {
                new DerivedClassImplementingInterface
                {
                    Value1 = 999,
                    Value2 = 2
                }
            };

            // Act / Assert
            actual.Should().BeEquivalentTo(expected, options => options
                .Including(a => a.Value2)
                .RespectingRuntimeTypes());
        }

        public interface IInterfaceWithTwoProperties
        {
            int Value1 { get; set; }

            int Value2 { get; set; }
        }

        public class BaseProvidingSamePropertiesAsInterface
        {
            public int Value1 { get; set; }

            public int Value2 { get; set; }
        }

        public class DerivedClassImplementingInterface : BaseProvidingSamePropertiesAsInterface, IInterfaceWithTwoProperties;
    }
}
