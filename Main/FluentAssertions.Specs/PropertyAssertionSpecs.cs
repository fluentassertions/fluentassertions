using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using FluentAssertions.Equivalency;
using FluentAssertions.Primitives;
using FluentAssertions.Specs;
#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class PropertyAssertionSpecs
    {
#if !WINDOWS_PHONE && !WINRT
        #region Property Comparison

        [TestMethod]
        public void When_two_objects_have_the_same_property_values_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "Dennis"
            };

            var other = new
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "Dennis"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.ShouldHave().AllProperties().EqualTo(other);
        }

        [TestMethod]
        public void When_two_objects_have_the_same_properties_but_a_different_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Age = 36,
            };

            var expectation = new
            {
                Age = 37,
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().EqualTo(expectation, "because {0} are the same", "they");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected property Age to be 37 because they are the same, but found 36", ComparisonMode.StartWith);
        }

        [TestMethod]
        public void When_subject_has_a_valid_property_that_is_compared_with_a_null_property_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Name = "Dennis"
            };

            var other = new
            {
                Name = (string) null
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().EqualTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected property Name to be <null>, but found \"Dennis\"", ComparisonMode.StartWith);
        }

        [TestMethod]
        public void When_two_collection_properties_dont_match_it_should_throw_and_specify_the_difference()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Values = new[]
                {
                    1, 2, 3
                }
            };

            var other = new
            {
                Values = new[]
                {
                    1, 4, 3
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().EqualTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected property Values*1, 4, 3}*but*1, 2, 3*differs at index 1*", 
                ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_two_objects_have_the_same_properties_with_convertable_values_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Age = "37",
                Birthdate = "1973-09-20",
            };

            var other = new
            {
                Age = 37,
                Birthdate = new DateTime(1973, 9, 20)
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().EqualTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_two_string_properties_do_not_match_it_should_throw_and_state_the_difference()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Name = "Dennes"
            };

            var other = new
            {
                Name = "Dennis"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().Properties(d => d.Name).EqualTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected property Name to be \"Dennis\", but \"Dennes\" differs near \"es\" (index 4)", ComparisonMode.StartWith);
        }

        [TestMethod]
        public void When_two_properties_are_of_derived_types_but_are_equal_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Type = new CustomerType("123")
            };

            var other = new
            {
                Type = new DerivedCustomerType("123")
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.ShouldHave().AllProperties().EqualTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region Structure Comparison

        [TestMethod]
        public void When_subject_has_a_property_not_available_in_expected_object_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                City = "Rijswijk"
            };

            var other = new
            {
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().EqualTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Subject has property City that the other object does not have", ComparisonMode.StartWith);
        }

        [TestMethod]
        public void When_subjects_properties_are_compared_to_null_object_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().EqualTo(null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected subject to be <null>, but found { }", ComparisonMode.StartWith);
        }

        [TestMethod]
        public void When_subject_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            SomeDto subject = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().EqualTo(new
            {
            });

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot compare the properties of a <null> object.");
        }

        [TestMethod]
        public void When_comparing_objects_it_should_ignore_private_properties()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new Customer("MyPassword")
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "John",
            };

            var other = new Customer("SomeOtherPassword")
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "John"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().EqualTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_equally_named_properties_are_type_incompatiblle_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Type = "A",
            };

            var other = new
            {
                Type = 36,
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().EqualTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected property Type to be*Int32*, but found*String*", ComparisonMode.Wildcard);
        }

        #endregion

        #region Property Inclusion & Exclusion

        [TestMethod]
        public void
            When_specific_properties_of_a_subject_are_compared_with_another_object_it_should_ignore_the_other_properties
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "John"
            };

            var customer = new
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "Dennis"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act 
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().Properties(d => d.Age, d => d.Birthdate).EqualTo(customer);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
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
        public void When_comparing_objects_by_their_shared_properties_and_all_match_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new CustomerDto
            {
                Version = 2,
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "John"
            };

            var customer = new Customer
            {
                Id = 1,
                Version = 2,
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "John"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => customer.ShouldHave().SharedProperties().EqualTo(dto);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_comparing_anonymous_objects_by_their_shared_properties_and_all_match_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Age = 36,
            };

            var other = new
            {
                Age = 37,
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().SharedProperties().EqualTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
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
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Please specify some properties to include in the comparison", ComparisonMode.StartWith);
        }

        [TestMethod]
        public void When_a_subject_has_a_write_only_property_it_should_ignore_that_property()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ClassWithWriteOnlyProperty
            {
                WriteOnlyProperty = 123,
                SomeOtherProperty = "whatever"
            };

            var expected = new
            {
                SomeOtherProperty = "whatever"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldHave().AllProperties().EqualTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_an_interface_hierarchy_is_used_it_should_include_all_inherited_properties()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
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

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldHave().AllProperties().EqualTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected property VehicleId*99999*but*1*", ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_an_interface_reference_is_compared_it_should_ignore_other_properties()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IVehicle expected = new Car
            {
                VehicleId = 1, 
                Wheels = 4
            };
            
            IVehicle actual = new Car
            {
                VehicleId = 1, 
                Wheels = 99999
            };
            
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => actual.ShouldHave().AllProperties().EqualTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        #endregion

        #region Nested property validation

        [TestMethod]
        public void When_all_the_properties_of_the_nested_objects_are_equal_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new Root
            {
                Text = "Root",
                Level = new Level1
                {
                    Text = "Level1",
                    Level = new Level2
                    {
                        Text = "Level2",
                    }
                }
            };

            var expected = new RootDto
            {
                Text = "Root",
                Level = new Level1Dto
                {
                    Text = "Level1",
                    Level = new Level2Dto
                    {
                        Text = "Level2",
                    }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_not_all_the_properties_of_the_nested_objects_are_equal_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new Root
            {
                Text = "Root",
                Level = new Level1
                {
                    Text = "Level1",
                }
            };

            var expected = new RootDto
            {
                Text = "Root",
                Level = new Level1Dto
                {
                    Text = "Level2",
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected property Level.Text to be \"Level2\", but \"Level1\" differs near \"1\" (index 5)", ComparisonMode.StartWith);
        }

        [TestMethod]
        public void When_the_actual_nested_object_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new Root
            {
                Text = "Root",
                Level = null
            };

            var expected = new RootDto
            {
                Text = "Root",
                Level = new Level1Dto
                {
                    Text = "Level2",
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected property Level to be*Level1Dto*Level2*, but found <null>*", ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_not_all_the_properties_of_the_nested_object_exist_on_the_expected_object_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Level = new
                {
                    Text = "Level1",
                    OtherProperty = "OtherProperty"
                }
            };

            var expected = new
            {
                Level = new
                {
                    Text = "Level1"
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Subject has property Level.OtherProperty that the other object does not have", ComparisonMode.StartWith);
        }

        [TestMethod]
        public void When_all_the_shared_properties_of_the_nested_objects_are_equal_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Level = new
                {
                    Text = "Level1",
                    Property = "Property"
                }
            };

            var expected = new
            {
                Level = new
                {
                    Text = "Level1",
                    OtherProperty = "OtherProperty"
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.ShouldHave().SharedProperties().IncludingNestedObjects().EqualTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_deeply_nested_properties_do_not_have_all_equal_values_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var root = new Root
            {
                Text = "Root",
                Level = new Level1
                {
                    Text = "Level1",
                    Level = new Level2
                    {
                        Text = "Level2",
                    }
                }
            };

            var rootDto = new RootDto
            {
                Text = "Root",
                Level = new Level1Dto
                {
                    Text = "Level1",
                    Level = new Level2Dto
                    {
                        Text = "A wrong text value",
                    }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
                root.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(rootDto);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected property Level.Level.Text to be *A wrong text value*but \r\n\"Level2\"*",
                    ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_validating_nested_properties_that_have_cyclic_references_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var cyclicRoot = new CyclicRoot
            {
                Text = "Root",
            };
            cyclicRoot.Level = new CyclicLevel1
            {
                Text = "Level1",
                Root = cyclicRoot
            };

            var cyclicRootDto = new CyclicRootDto
            {
                Text = "Root",
            };
            cyclicRootDto.Level = new CyclicLevel1Dto
            {
                Text = "Level1",
                Root = cyclicRootDto
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
                cyclicRoot.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(cyclicRootDto);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected property Level.Root to be*but it contains a cyclic reference*",
                    ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_validating_nested_properties_and_ignoring_cyclic_references_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var cyclicRoot = new CyclicRoot
            {
                Text = "Root",
            };
            cyclicRoot.Level = new CyclicLevel1
            {
                Text = "Level1",
                Root = cyclicRoot,
            };

            var cyclicRootDto = new CyclicRootDto
            {
                Text = "Root",
            };
            cyclicRootDto.Level = new CyclicLevel1Dto
            {
                Text = "Level1",
                Root = cyclicRootDto,
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => cyclicRoot.ShouldHave()
                .AllProperties()
                .IncludingNestedObjects(CyclicReferenceHandling.Ignore)
                .EqualTo(cyclicRootDto);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_two_objects_have_the_same_nested_objects_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var c1 = new ClassOne();
            var c2 = new ClassOne();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => c1.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(c2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_property_of_a_nested_object_doesnt_match_it_should_clearly_indicate_the_path()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var c1 = new ClassOne();
            var c2 = new ClassOne();
            c2.RefOne.ValTwo = 2;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => c1.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(c2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected property RefOne.ValTwo to be 2, but found 3", ComparisonMode.StartWith);
        }

        #endregion

        #region Nested collection validation

        [TestMethod]
        public void When_a_collection_property_contains_objects_with_matching_properties_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new
            {
                Customers = new[]
                {
                    new Customer
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "John"
                    },
                    new Customer
                    {
                        Age = 32,
                        Birthdate = 31.July(1978),
                        Name = "Jane"
                    }
                }
            };

            var subject = new
            {
                Customers = new[]
                {
                    new CustomerDto
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "John"
                    },
                    new CustomerDto
                    {
                        Age = 32,
                        Birthdate = 31.July(1978),
                        Name = "Jane"
                    }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_collection_property_contains_objects_with_mismatching_properties_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new
            {
                Customers = new[]
                {
                    new Customer
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "John"
                    },
                }
            };

            var subject = new
            {
                Customers = new[]
                {
                    new CustomerDto
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "Jane"
                    },
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("*Customers[0].Name*John*Jane*", ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_a_collection_property_was_expected_but_the_property_is_not_a_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Customers = "Jane, John"
            };

            var expected = new
            {
                Customers = new[]
                {
                    new Customer
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "John"
                    },
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("*property Customers to be*Customer[]*, but*System.String*",
                    ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_a_collection_contains_more_items_than_expected_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new
            {
                Customers = new[]
                {
                    new Customer
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "John"
                    },
                }
            };

            var subject = new
            {
                Customers = new[]
                {
                    new CustomerDto
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "Jane"
                    },
                    new CustomerDto
                    {
                        Age = 24,
                        Birthdate = 21.September(1973),
                        Name = "John"
                    },
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("*property Customers to be a collection with 1 item(s), but found 2*",
                    ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_a_collection_contains_less_items_than_expected_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new
            {
                Customers = new[]
                {
                    new Customer
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "John"
                    },
                    new Customer
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "Jane"
                    }
                }
            };

            var subject = new
            {
                Customers = new[]
                {
                    new CustomerDto
                    {
                        Age = 24,
                        Birthdate = 21.September(1973),
                        Name = "John"
                    },
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("*property Customers to be a collection with 2 item(s), but found 1*",
                    ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_a_complex_object_graph_with_collections_matches_expectations_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Bytes = new byte[]
                {
                    1, 2, 3, 4
                },
                Object = new
                {
                    A = 1,
                    B = 2
                }
            };

            var expected = new
            {
                Bytes = new byte[]
                {
                    1, 2, 3, 4
                },
                Object = new
                {
                    A = 1,
                    B = 2
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region Collection Validation

        [TestMethod]
        public void When_two_lists_contain_the_same_structural_equal_objects_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John", Age = 27, Id = 1
                },
                new Customer
                {
                    Name = "Jane", Age = 24, Id = 2
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "John", Age = 27, Id = 1
                },
                new Customer
                {
                    Name = "Jane", Age = 24, Id = 2
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_two_different_enumerables_contain_the_same_structural_equal_objects_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Customer[] subject = new[]
            {
                new Customer
                {
                    Name = "John", Age = 27, Id = 1
                },
                new Customer
                {
                    Name = "Jane", Age = 24, Id = 2
                }
            };

            IEnumerable<Customer> expectation = new Collection<Customer>
            {
                new Customer
                {
                    Name = "John", Age = 27, Id = 1
                },
                new Customer
                {
                    Name = "Jane", Age = 24, Id = 2
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_two_lists_dont_contain_the_same_structural_equal_objects_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John", Age = 27, Id = 1
                },
                new Customer
                {
                    Name = "Jane", Age = 24, Id = 2
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "John", Age = 27, Id = 1
                },
                new Customer
                {
                    Name = "Jane", Age = 30, Id = 2
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldHave().AllProperties().IncludingNestedObjects().EqualTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected*item[1].Age*30*24*", ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_the_subject_contains_more_items_than_expected_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John", Age = 27, Id = 1
                },
                new Customer
                {
                    Name = "Jane", Age = 24, Id = 2
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "John", Age = 27, Id = 1
                },
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldHave().AllProperties().EqualTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected subject to be a collection with 1 item(s), but found 2*", ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_the_subject_contains_less_items_than_expected_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John", Age = 27, Id = 1
                },
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "John", Age = 27, Id = 1
                },
                new Customer
                {
                    Name = "Jane", Age = 24, Id = 2
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldHave().AllProperties().EqualTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("*subject to be a collection with 2 item(s), but found 1*", ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_a_collection_is_compared_to_a_non_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldHave().AllProperties().EqualTo("hello");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Subject is a collection and cannot be compared with a non-collection type", ComparisonMode.StartWith);
        }

        #endregion

#endif

        public class ClassOne
        {
            private ClassTwo refOne = new ClassTwo();
            private int valOne = 1;

            public ClassTwo RefOne
            {
                get { return refOne; }
                set { refOne = value; }
            }

            public int ValOne
            {
                get { return valOne; }
                set { valOne = value; }
            }
        }

        public class ClassTwo
        {
            private int valTwo = 3;

            public int ValTwo
            {
                get { return valTwo; }
                set { valTwo = value; }
            }
        }

        public class ClassWithWriteOnlyProperty
        {
            private int writeOnlyPropertyValue;

            public int WriteOnlyProperty
            {
                set { writeOnlyPropertyValue = value; }
            }

            public string SomeOtherProperty { get; set; }
        }
    }

    public class Customer : Entity
    {
        private string Password { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthdate { get; set; }
        public long Id { get; set; }

        public Customer()
        {
        }

        public Customer(string password)
        {
            Password = password;
        }
    }

    public class Entity
    {
        internal long Version { get; set; }
    }

    public class CustomerDto
    {
        public long Version { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthdate { get; set; }
    }

    public class CustomerType
    {
        public CustomerType(string code)
        {
            Code = code;
        }

        public string Code { get; private set; }

        public override bool Equals(object obj)
        {
            var other = obj as CustomerType;
            return (other != null) && (Code.Equals(other.Code));
        }

        public override int GetHashCode()
        {
            return (Code != null ? Code.GetHashCode() : 0);
        }

        public static bool operator ==(CustomerType a, CustomerType b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object) a == null) || ((object) b == null))
            {
                return false;
            }

            return a.Code.Equals(b.Code);
        }

        public static bool operator !=(CustomerType a, CustomerType b)
        {
            return !(a == b);
        }
    }

    public class DerivedCustomerType : CustomerType
    {
        public DerivedCustomerType(string code) : base(code)
        {
        }
    }

    #region Nested classes for comparison

    public class Root
    {
        public string Text { get; set; }
        public Level1 Level { get; set; }
    }

    public class Level1
    {
        public string Text { get; set; }
        public Level2 Level { get; set; }
    }

    public class Level2
    {
        public string Text { get; set; }
    }

    public class RootDto
    {
        public string Text { get; set; }
        public Level1Dto Level { get; set; }
    }

    public class Level1Dto
    {
        public string Text { get; set; }
        public Level2Dto Level { get; set; }
    }

    public class Level2Dto
    {
        public string Text { get; set; }
    }

    public class CyclicRoot
    {
        public string Text { get; set; }
        public CyclicLevel1 Level { get; set; }
    }

    public class CyclicLevel1
    {
        public string Text { get; set; }
        public CyclicRoot Root { get; set; }
    }

    public class CyclicRootDto
    {
        public string Text { get; set; }
        public CyclicLevel1Dto Level { get; set; }
    }

    public class CyclicLevel1Dto
    {
        public string Text { get; set; }
        public CyclicRootDto Root { get; set; }
    }

    #endregion

    #region Interfaces for verifying inheritance of properties

    public class Car : Vehicle, ICar
    {
        public int Wheels { get; set; }
    }

    public class Vehicle : IVehicle
    {
        public int VehicleId { get; set; }
    }

    public interface ICar : IVehicle
    {
        int Wheels { get; set; }
    }

    public interface IVehicle
    {
        int VehicleId { get; set; }
    }

    #endregion
}