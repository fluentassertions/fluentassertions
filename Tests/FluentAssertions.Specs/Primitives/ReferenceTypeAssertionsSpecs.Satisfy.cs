using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class ReferenceTypeAssertionsSpecs
{
    public class Satisfy
    {
        [Fact]
        public void When_object_satisfies_inspector_it_should_not_throw()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().Satisfy<object>(x => x.Should().NotBeNull());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_typed_object_satisfies_inspector_it_should_not_throw()
        {
            // Arrange
            var someObject = new PersonDto
            {
                Name = "Name Nameson",
                Birthdate = new DateTime(2000, 1, 1),
            };

            // Act
            Action act = () => someObject.Should().Satisfy<PersonDto>(o => o.Age.Should().BeGreaterThan(0));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_object_does_not_satisfy_the_inspector_it_should_throw()
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
        public void When_a_typed_object_does_not_satisfy_the_inspector_it_should_throw()
        {
            // Arrange
            const string personName = "Name Nameson";

            var somePerson = new PersonDto
            {
                Name = personName,
                Birthdate = new DateTime(1973, 9, 20),
            };

            // Act
            Action act = () =>
                somePerson.Should().Satisfy<PersonDto>(d => d.Name.Should().HaveLength(0, "it is not initialized yet"));

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                $"""
                 Expected {nameof(somePerson)} to match inspector, but the inspector was not satisfied:
                 *Expected d.Name with length 0 because it is not initialized yet, but found string "{personName}" with length {personName.Length}.
                 """);
        }

        [Fact]
        public void When_a_complex_typed_object_does_not_satisfy_inspector_it_should_throw()
        {
            // Arrange
            var someComplexDto = new PersonAndAddressDto
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
            Action act = () => someComplexDto.Should().Satisfy<PersonAndAddressDto>(dto =>
            {
                dto.Person.Should().Satisfy<PersonDto>(person =>
                {
                    person.Name.Should().Be("Biff Tannen");
                    person.Age.Should().Be(48);
                    person.Birthdate.Should().Be(new DateTime(1937, 3, 26));
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
                 Expected {nameof(someComplexDto)} to match inspector, but the inspector was not satisfied:
                 *Expected dto.Person to match inspector*
                 *Expected person.Name*
                 *Expected dto.Address to match inspector*
                 *Expected address.City*
                 """);
        }

        [Fact]
        public void When_a_typed_object_is_satisfied_against_an_incorrect_type_it_should_throw()
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
        public void When_a_typed_object_does_not_match_multiple_inspectors_it_should_throw()
        {
            // Arrange
            var somePerson = new PersonDto
            {
                Name = "Name Nameson",
                Birthdate = new DateTime(2000, 1, 1),
            };

            // Act
            Action act = () => somePerson.Should().Satisfy<PersonDto>(d =>
            {
                d.Name.Should().Be("Someone Else");
                d.Age.Should().BeLessThan(20);
                d.Birthdate.Should().BeAfter(new DateTime(2001, 1, 1));
            });

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                $"""
                 Expected {nameof(somePerson)} to match inspector, but the inspector was not satisfied:
                 *Expected d.Name*
                 *Expected d.Age*
                 *Expected d.Birthdate*
                 """);
        }

        [Fact]
        public void When_object_is_satisfied_against_a_null_inspector_it_should_throw()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().Satisfy<object>(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot verify an object against a <null> inspector.*");
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
    }
}
