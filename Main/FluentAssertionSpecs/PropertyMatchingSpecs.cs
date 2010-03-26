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
            dto.ShouldHave().AllProperties.EqualTo(customer);
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
            Action act = () => dto.ShouldHave().AllProperties.EqualTo(customer, "because {0} are the same", "they");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected property Age to have value <37> because they are the same, but found <36>.");
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
            dto.ShouldHave().AllProperties.EqualTo(customer);
        }

        // TODO: subject has more properties than other object
        // TODO: subject has less properties than other object
        // TODO: only specified property values
        // TODO: all but specified property values
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

    internal class Customer
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthdate { get; set; }
        public long Id { get; set; }
    }
}