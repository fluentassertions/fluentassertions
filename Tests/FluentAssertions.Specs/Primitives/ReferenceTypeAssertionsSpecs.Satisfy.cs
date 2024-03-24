using System;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class ReferenceTypeAssertionsSpecs
{
    public class Satisfy
    {
        [Fact]
        public void Object_satisfying_inspector_does_not_throw()
        {
            // Arrange
            var someObject = new object();

            // Act / Assert
            someObject.Should().Satisfy<object>(x => x.Should().NotBeNull());
        }

        [Fact]
        public void Object_not_satisfying_inspector_throws()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().Satisfy<object>(o => o.Should().BeNull("it is not initialized yet"));

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                $"""
                 Expected {nameof(someObject)} to match inspector, but the inspector was not satisfied:
                 *Expected o to be <null> because it is not initialized yet, but found System.Object*
                 """);
        }

        [Fact]
        public void Object_satisfied_against_null_throws()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().Satisfy<object>(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot verify an object against a <null> inspector.*");
        }

        [Fact]
        public void Typed_object_satisfying_inspector_does_not_throw()
        {
            // Arrange
            var personDto = new PersonDto
            {
                Name = "Name Nameson",
                Birthdate = new DateTime(2000, 1, 1),
            };

            // Act / Assert
            personDto.Should().Satisfy<PersonDto>(o => o.Age.Should().BeGreaterThan(0));
        }

        [Fact]
        public void Complex_typed_object_satisfying_inspector_does_not_throw()
        {
            // Arrange
            var complexDto = new PersonAndAddressDto
            {
                Person = new PersonDto
                {
                    Name = "Name Nameson",
                    Birthdate = new DateTime(2000, 1, 1),
                },
                Address = new AddressDto
                {
                    Street = "Named St.",
                    Number = "42",
                    City = "Nowhere",
                    Country = "Neverland",
                    PostalCode = "12345",
                }
            };

            // Act / Assert
            complexDto.Should().Satisfy<PersonAndAddressDto>(dto =>
            {
                dto.Person.Should().Satisfy<PersonDto>(person =>
                {
                    person.Name.Should().Be("Name Nameson");
                    person.Age.Should().BeGreaterThan(0);
                    person.Birthdate.Should().Be(1.January(2000));
                });

                dto.Address.Should().Satisfy<AddressDto>(address =>
                {
                    address.Street.Should().Be("Named St.");
                    address.Number.Should().Be("42");
                    address.City.Should().Be("Nowhere");
                    address.Country.Should().Be("Neverland");
                    address.PostalCode.Should().Be("12345");
                });
            });
        }

        [Fact]
        public void Typed_object_not_satisfying_inspector_throws()
        {
            // Arrange
            var personDto = new PersonDto
            {
                Name = "Name Nameson",
                Birthdate = new DateTime(2000, 1, 1),
            };

            // Act
            Action act = () => personDto.Should().Satisfy<PersonDto>(d =>
            {
                d.Name.Should().Be("Someone Else");
                d.Age.Should().BeLessThan(20);
                d.Birthdate.Should().BeAfter(1.January(2001));
            });

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                $"""
                 Expected {nameof(personDto)} to match inspector, but the inspector was not satisfied:
                 *Expected d.Name*
                 *Expected d.Age*
                 *Expected d.Birthdate*
                 """);
        }

        [Fact]
        public void Complex_typed_object_not_satisfying_inspector_throws()
        {
            // Arrange
            var complexDto = new PersonAndAddressDto
            {
                Person = new PersonDto
                {
                    Name = "Buford Howard Tannen",
                    Birthdate = new DateTime(1937, 3, 26),
                },
                Address = new AddressDto
                {
                    Street = "Mason Street",
                    Number = "1809",
                    City = "Hill Valley",
                    Country = "United States",
                    PostalCode = "CA 91905",
                },
            };

            // Act
            Action act = () => complexDto.Should().Satisfy<PersonAndAddressDto>(dto =>
            {
                dto.Person.Should().Satisfy<PersonDto>(person =>
                {
                    person.Name.Should().Be("Biff Tannen");
                    person.Age.Should().Be(48);
                    person.Birthdate.Should().Be(26.March(1937));
                });

                dto.Address.Should().Satisfy<AddressDto>(address =>
                {
                    address.Street.Should().Be("Mason Street");
                    address.Number.Should().Be("1809");
                    address.City.Should().Be("Hill Valley, San Diego County, California");
                    address.Country.Should().Be("United States");
                    address.PostalCode.Should().Be("CA 91905");
                });
            });

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                $"""
                 Expected {nameof(complexDto)} to match inspector, but the inspector was not satisfied:
                 *Expected dto.Person to match inspector*
                 *Expected person.Name*
                 *Expected dto.Address to match inspector*
                 *Expected address.City*
                 """);
        }

        [Fact]
        public void Typed_object_satisfied_against_incorrect_type_throws()
        {
            // Arrange
            var personDto = new PersonDto();

            // Act
            Action act = () => personDto.Should().Satisfy<AddressDto>(dto => dto.Should().NotBeNull());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    $"Expected {nameof(personDto)} to be assignable to {typeof(AddressDto)}, but {typeof(PersonDto)} is not.");
        }

        [Fact]
        public void Sub_class_satisfied_against_base_class_does_not_throw()
        {
            // Arrange
            var subClass = new SubClass
            {
                Number = 42,
                Date = new DateTime(2021, 1, 1),
                Text = "Some text"
            };

            // Act / Assert
            subClass.Should().Satisfy<BaseClass>(x =>
            {
                x.Number.Should().Be(42);
                x.Date.Should().Be(1.January(2021));
            });
        }

        [Fact]
        public void Base_class_satisfied_against_sub_class_throws()
        {
            // Arrange
            var baseClass = new BaseClass
            {
                Number = 42,
                Date = new DateTime(2021, 1, 1),
            };

            // Act
            Action act = () => baseClass.Should().Satisfy<SubClass>(x =>
            {
                x.Number.Should().Be(42);
                x.Date.Should().Be(1.January(2021));
                x.Text.Should().Be("Some text");
            });

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    $"Expected {nameof(baseClass)} to be assignable to {typeof(SubClass)}, but {typeof(BaseClass)} is not.");
        }

        [Fact]
        public void Nested_assertion_on_null_throws()
        {
            // Arrange
            var complexDto = new PersonAndAddressDto
            {
                Person = new PersonDto
                {
                    Name = "Buford Howard Tannen",
                },
                Address = null,
            };

            // Act
            Action act = () => complexDto.Should().Satisfy<PersonAndAddressDto>(dto =>
            {
                dto.Person.Name.Should().Be("Buford Howard Tannen");
                dto.Address.Should().Satisfy<AddressDto>(address => address.City.Should().Be("Hill Valley"));
            });

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected dto.Address to be assignable to *AddressDto, but found <null>.");
        }

        private class PersonDto
        {
            public string Name { get; init; }

            public DateTime Birthdate { get; init; }

            public int Age => DateTime.UtcNow.Subtract(Birthdate).Days / 365;
        }

        private class PersonAndAddressDto
        {
            public PersonDto Person { get; init; }

            public AddressDto Address { get; init; }
        }

        private class AddressDto
        {
            public string Street { get; init; }

            public string Number { get; init; }

            public string City { get; init; }

            public string PostalCode { get; init; }

            public string Country { get; init; }
        }

        private class BaseClass
        {
            public int Number { get; init; }

            public DateTime Date { get; init; }
        }

        private sealed class SubClass : BaseClass
        {
            public string Text { get; init; }
        }
    }
}
