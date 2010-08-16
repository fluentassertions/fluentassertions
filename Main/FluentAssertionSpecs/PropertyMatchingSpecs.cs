using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class PropertyMatchingSpecs
    {
        [TestMethod]
        public void When_two_objects_have_the_same_property_values_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new CustomerDto
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "Dennis"
            };

            var customer = new Customer
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "Dennis"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            dto.ShouldHave().AllProperties().EqualTo(customer);
        }

        [TestMethod]
        public void When_two_objects_have_the_same_properties_but_a_different_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new CustomerDto
            {
                Age = 36,
            };

            var customer = new Customer
            {
                Age = 37,
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dto.ShouldHave().AllProperties().EqualTo(customer, "because {0} are the same", "they");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<SpecificationMismatchException>().WithMessage(
                "Expected property Age to have value <37> because they are the same, but found <36>.");
        }

        [TestMethod]
        public void When_subject_has_a_valid_property_that_is_compared_with_a_null_property_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new Customer
            {
                Name = "Dennis"
            };

            var customer = new Customer
            {
                Name = null
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dto.ShouldHave().AllProperties().EqualTo(customer);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<SpecificationMismatchException>().WithMessage(
                "Expected property Name to have value <null>, but found \"Dennis\".");
        }       

        
        [TestMethod]
        public void When_two_objects_have_the_same_properties_with_convertable_values_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new CustomerDtoWithStringBasedProperties()
            {
                Age = "37",
                Birthdate = "1973-09-20",
            };

            var customer = new Customer
            {
                Age = 37,
                Birthdate = new DateTime(1973, 9, 20)
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            dto.ShouldHave().AllProperties().EqualTo(customer);
        }

        [TestMethod]
        public void When_subject_has_a_property_not_available_in_expected_object_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new CustomerDtoWithExtraneousProperty()
            {
                City = "Rijswijk"
            };

            var customer = new Customer();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dto.ShouldHave().AllProperties().EqualTo(customer);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<SpecificationMismatchException>().WithMessage(
                "Subject has property City that is not available in comparee.");
        }
        
        [TestMethod]
        public void When_subjects_properties_are_compared_to_null_object_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new CustomerDto();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dto.ShouldHave().AllProperties().EqualTo(null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot compare subject's properties with a <null> object.");
        }
        
        [TestMethod]
        public void When_subject_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            CustomerDto dto = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dto.ShouldHave().AllProperties().EqualTo(new CustomerDto());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot compare the properties of a <null> object");
        }

        [TestMethod]
        public void When_subject_has_a_specific_property_which_value_is_similar_to_another_object_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new CustomerDto
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "John"
            };

            var customer = new Customer
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "Dennis"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assrrt
            //-----------------------------------------------------------------------------------------------------------
            dto.ShouldHave().Properties(d => d.Age, d => d.Birthdate).EqualTo(customer);
        }

        [TestMethod]
        public void When_subject_has_a_specific_property_which_value_is_not_similar_to_another_object_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new CustomerDto
            {
                Name = "John"
            };

            var customer = new Customer
            {
                Name = "Dennis"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dto.ShouldHave().Properties(d => d.Name).EqualTo(customer);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<SpecificationMismatchException>().WithMessage(
                "Expected property Name to have value \"Dennis\", but found \"John\".");
        }

        [TestMethod]
        public void When_property_is_compared_but_a_nonproperty_is_specified_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new CustomerDto();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dto.ShouldHave().Properties(d => d.GetType()).EqualTo(dto);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentException>().WithMessage(
                "Cannot use <d.GetType()> when a property expression is expected.");
        }

        [TestMethod]
        public void When_subject_property_lambda_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new CustomerDto();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dto.ShouldHave().Properties(null).EqualTo(dto);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Expected a property expression, but found <null>.");
        }

        [TestMethod]
        public void When_all_properties_but_one_match_and_that_is_expected_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new CustomerDto
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "John"
            };

            var customer = new Customer
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "Dennis"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            dto.ShouldHave().AllPropertiesBut(d => d.Name).EqualTo(customer);
        }        
        
        [TestMethod]
        public void When_comparing_objects_by_their_properties_and_no_properties_have_been_specified_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new CustomerDto();
            var customer = new Customer();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dto.ShouldHave().EqualTo(customer);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<InvalidOperationException>().WithMessage(
                "Please specify some properties to include in the comparison.");
        }
    }

    internal class CustomerDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthdate { get; set; }
    }

    internal class CustomerDtoWithStringBasedProperties
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public string Birthdate { get; set; }
    }
    
    internal class CustomerDtoWithExtraneousProperty
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthdate { get; set; }
        public string City { get; set; }
    }

    internal class Customer
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthdate { get; set; }
        public long Id { get; set; }
    }
}