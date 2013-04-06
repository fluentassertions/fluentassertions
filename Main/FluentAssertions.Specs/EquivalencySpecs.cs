using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

using FluentAssertions.Equivalency;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class EquivalencySpecs
    {
        #region General

        [TestMethod]
        public void When_expectation_is_null_it_should_throw()
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
            Action act = () => subject.ShouldBeEquivalentTo(null);

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
            Action act = () => subject.ShouldBeEquivalentTo(new
            {
            });

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected object to be*, but found <null>*", ComparisonMode.Wildcard);
        }

        #endregion

        #region Selection Rules

        [TestMethod]
        public void When_specific_properties_have_been_specified_it_should_ignore_the_other_properties()
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
            Action act = () => subject.ShouldBeEquivalentTo(customer, options => options
                .Including(d => d.Age)
                .Including(d => d.Birthdate));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_non_property_expression_is_provided_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new CustomerDto();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dto.ShouldBeEquivalentTo(dto, options => options.Including(d => d.GetType()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentException>().WithMessage(
                "Cannot use <d.GetType()> when a property expression is expected.");
        }

        [TestMethod]
        public void When_null_is_provided_as_property_expression_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dto = new CustomerDto();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dto.ShouldBeEquivalentTo(dto, options => options.Including(null));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Expected a property expression, but found <null>.");
        }

        [TestMethod]
        public void When_only_the_excluded_property_doesnt_match_it_should_not_throw()
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
            dto.ShouldBeEquivalentTo(customer, options => options.Excluding(d => d.Name));
        }

        [TestMethod]
        public void When_all_shared_properties_match_it_should_not_throw()
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
            Action act = () => dto.ShouldBeEquivalentTo(customer, options => options.ExcludingMissingProperties());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_property_shared_by_anonymous_types_doesnt_match_it_should_throw()
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
            Action act = () => subject.ShouldBeEquivalentTo(other, options => options.ExcludingMissingProperties());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_a_property_is_write_only_it_should_be_ignored()
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
            Action action = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_property_is_private_it_should_be_ignored()
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
            Action act = () => subject.ShouldBeEquivalentTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
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
            Action action = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected property VehicleId*99999*but*1*", ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_a_reference_to_an_interface_is_provided_it_should_only_include_those_properties()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
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

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_deeply_nested_property_with_a_value_mismatch_is_excluded_it_should_not_throw()
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
                        Text = "Mismatch",
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
            Action act = () => subject.ShouldBeEquivalentTo(expected,
                options => options.Excluding(r => r.Level.Level.Text));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_deeply_nested_property_of_a_collection_with_an_invalid_value_is_excluded_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Text = "Root",
                Level = new
                {
                    Text = "Level1",
                    Level = new
                    {
                        Text = "Level2",
                    },
                    Collection = new[]
                    {
                        new { Number = 1, Text = "Text" },
                        new { Number = 2, Text = "Actual" }
                    }
                }
            };

            var expected = new
            {
                Text = "Root",
                Level = new
                {
                    Text = "Level1",
                    Level = new
                    {
                        Text = "Level2",
                    },
                    Collection = new[]
                    {
                        new { Number = 1, Text = "Text" },
                        new { Number = 2, Text = "Expected" }
                    }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expected, options => options.
                Excluding(x => x.Level.Collection[1].Number).
                Excluding(x => x.Level.Collection[1].Text)
                );

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_property_with_a_value_mismatch_is_excluded_using_a_predicate_it_should_not_throw()
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
                        Text = "Mismatch",
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
            Action act = () => subject.ShouldBeEquivalentTo(expected, config =>
                config.Excluding(ctx => ctx.PropertyPath == "Level.Level.Text"));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_subject_has_a_property_not_available_on_expected_object_it_should_throw()
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
            Action act = () => subject.ShouldBeEquivalentTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Subject has property City that the other object does not have", ComparisonMode.StartWith);
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
            Action act = () => subject.ShouldBeEquivalentTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected property Type to be*Int32*, but found*String*", ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_multiple_properties_mismatch_it_should_report_all_of_them()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Property1 = "A",
                Property2 = "B",
                SubType1 = new
                {
                    SubProperty1 = "C",
                    SubProperty2 = "D",
                }
            };

            var other = new
            {
                Property1 = "1",
                Property2 = "2",
                SubType1 = new
                {
                    SubProperty1 = "3",
                    SubProperty2 = "D",
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage("*property Property1 to be \"1\", but \"A\" differs near \"A\"*", ComparisonMode.Wildcard)
                .WithMessage("*property Property2 to be \"2\", but \"B\" differs near \"B\"*", ComparisonMode.Wildcard)
                .WithMessage("*property SubType1.SubProperty1 to be \"3\", but \"C\" differs near \"C\"*", ComparisonMode.Wildcard);
        }

        #endregion

        #region Collection Equivalence

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
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldAllBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_two_different_collections_contain_the_same_structurally_equal_objects_in_any_order_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new[]
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            IEnumerable<Customer> expectation = new Collection<Customer>
            {
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                },
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldAllBeEquivalentTo(expectation);

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
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 30,
                    Id = 2
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldAllBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected*item[1].Age*30*24*", ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_no_collection_item_matches_it_should_report_the_closest_match()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 30,
                    Id = 2
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "Jane",
                    Age = 30,
                    Id = 2
                },
                new Customer
                {
                    Name = "John",
                    Age = 28,
                    Id = 1
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldAllBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected*item[1].Age*28*27*", ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_two_lists_only_differ_in_excluded_properties_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            var expectation = new List<CustomerDto>
            {
                new CustomerDto
                {
                    Name = "John",
                    Age = 27,
                },
                new CustomerDto
                {
                    Name = "Jane",
                    Age = 30,
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldAllBeEquivalentTo(expectation, options => options
                .ExcludingMissingProperties()
                .Excluding(c => c.Age));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
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
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldAllBeEquivalentTo(expectation);

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
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
            };

            var expectation = new List<Customer>
            {
                new Customer
                {
                    Name = "John",
                    Age = 27,
                    Id = 1
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 24,
                    Id = 2
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldAllBeEquivalentTo(expectation);

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
            Action action = () => subject.ShouldAllBeEquivalentTo("hello");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Subject is a collection and cannot be compared with a non-collection type", ComparisonMode.StartWith);
        }

        #endregion

        #region Assertion Rules

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
            subject.ShouldBeEquivalentTo(other);
        }

        [TestMethod]
        public void When_two_objects_have_the_same_nullable_property_values_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Age = 36,
                Birthdate = (DateTime?)(new DateTime(1973, 9, 20)),
                Name = "Dennis"
            };

            var other = new
            {
                Age = 36,
                Birthdate = (DateTime?)new DateTime(1973, 9, 20),
                Name = "Dennis"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.ShouldBeEquivalentTo(other);
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
            Action act = () => subject.ShouldBeEquivalentTo(expectation, "because {0} are the same", "they");

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
                Name = (string)null
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(other);

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
            Action act = () => subject.ShouldBeEquivalentTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected property Values[1] to be 4, but found 2", ComparisonMode.StartWith);
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
            Action act = () => subject.ShouldBeEquivalentTo(other);

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
            Action act = () => subject.ShouldBeEquivalentTo(other, options => options.Including(d => d.Name));

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
            Action act = () => subject.ShouldBeEquivalentTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_an_assertion_is_overridden_for_a_predicate_it_should_use_the_provided_action()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Date = 14.July(2012).At(12, 59, 59)
            };

            var expectation = new
            {
                Date = 14.July(2012).At(13, 0, 0)
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expectation, options => options
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
                .When(info => info.PropertyPath.EndsWith("Date")));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_an_assertion_is_overridden_for_all_types_it_should_use_the_provided_action_for_all_properties()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Date = 21.July(2012).At(11, 8, 59),
                Nested = new
                {
                    NestedDate = 14.July(2012).At(12, 59, 59)
                }
            };

            var expectation = new
            {
                Date = 21.July(2012).At(11, 9, 0),
                Nested = new
                {
                    NestedDate = 14.July(2012).At(13, 0, 0)
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expectation, options =>
                options
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
                    .WhenTypeIs<DateTime>());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region Nested Properties

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
            Action act = () => subject.ShouldBeEquivalentTo(expected);

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
                subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected property Level.Text to be \"Level2\", but \"Level1\" differs near \"1\" (index 5)",
                    ComparisonMode.StartWith);
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
                subject.ShouldBeEquivalentTo(expected);

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
            Action act = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Subject has property Level.OtherProperty that the other object does not have",
                    ComparisonMode.Substring);
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
            Action act = () => subject.ShouldBeEquivalentTo(expected, options => options.ExcludingMissingProperties());

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
            Action act = () => root.ShouldBeEquivalentTo(rootDto);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected property Level.Level.Text to be *A wrong text value*but \r\n\"Level2\"*length*",
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
            Action act = () => cyclicRoot.ShouldBeEquivalentTo(cyclicRootDto);

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
            Action act = () => cyclicRoot.ShouldBeEquivalentTo(cyclicRootDto, options => options.IgnoringCyclicReferences());

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
            Action act = () => c1.ShouldBeEquivalentTo(c2);

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
            Action act = () => c1.ShouldBeEquivalentTo(c2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected property RefOne.ValTwo to be 2, but found 3", ComparisonMode.StartWith);
        }

        #endregion

        #region Nested Enumerables

        [TestMethod]
        public void When_a_collection_property_contains_objects_with_matching_properties_in_any_order_it_should_not_throw()
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
                        Age = 32,
                        Birthdate = 31.July(1978),
                        Name = "Jane"
                    },
                    new Customer
                    {
                        Age = 38,
                        Birthdate = 20.September(1973),
                        Name = "John"
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
            Action act = () => subject.ShouldBeEquivalentTo(expected);

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
            Action act = () => subject.ShouldBeEquivalentTo(expected);

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
            Action act = () => subject.ShouldBeEquivalentTo(expected);

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
            Action act = () => subject.ShouldBeEquivalentTo(expected);

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
            Action act = () => subject.ShouldBeEquivalentTo(expected);

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
            Action act = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_dictionary_property_is_detected_it_should_ignore_the_order_of_the_pairs()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new
            {
                Customers = new Dictionary<string, string>
                {
                    { "Key2", "Value2" },
                    { "Key1", "Value1" }
                }
            };

            var subject = new
            {
                Customers = new Dictionary<string, string>
                {
                    { "Key1", "Value1" },
                    { "Key2", "Value2" }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_the_other_property_is_not_a_dictionary_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new
            {
                Customers = "I am a string"
            };

            var subject = new
            {
                Customers = new Dictionary<string, string>
                {
                    { "Key2", "Value2" },
                    { "Key1", "Value1" }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("property*Customers*dictionary*non-dictionary*", ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_the_other_dictionary_does_not_contain_enough_items_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new
            {
                Customers = new Dictionary<string, string>
                {
                    { "Key1", "Value1" },
                    { "Key2", "Value2" }
                }
            };

            var subject = new
            {
                Customers = new Dictionary<string, string>
                {
                    { "Key1", "Value1" },
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected*Customers*dictionary*2 item(s)*but*1 item(s)*", ComparisonMode.Wildcard);
        }

        #endregion

        #region Custom Rules

        [TestMethod]
        public void When_a_selection_rule_is_added_it_should_be_evaluated_after_all_existing_rules()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                NameId = "123",
                SomeValue = "hello"
            };

            var expected = new
            {
                SomeValue = "hello"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new ExcludeForeignKeysSelectionRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        internal class ExcludeForeignKeysSelectionRule : ISelectionRule
        {
            public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, ISubjectInfo context)
            {
                return properties.Where(pi => !pi.Name.EndsWith("Id")).ToArray();
            }
        }

        [TestMethod]
        public void When_a_matching_rule_is_added_it_should_preceed_all_existing_rules()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                NameId = "123",
                SomeValue = "hello"
            };

            var expected = new
            {
                Name = "123",
                SomeValue = "hello"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new ForeignKeyMatchingRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        internal class ForeignKeyMatchingRule : IMatchingRule
        {
            public PropertyInfo Match(PropertyInfo subjectProperty, object expectation, string propertyPath)
            {
                string name = subjectProperty.Name;
                if (name.EndsWith("Id"))
                {
                    name = name.Replace("Id", "");
                }

                return expectation.GetType().GetProperty(name);
            }
        }

        [TestMethod]
        public void When_an_assertion_rule_is_added_it_should_preceed_all_existing_rules()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Created = 8.July(2012).At(22, 9)
            };

            var expected = new
            {
                Created = 8.July(2012).At(22, 10)
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new RelaxingDateTimeAssertionRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        internal class RelaxingDateTimeAssertionRule : IAssertionRule
        {
            public bool AssertEquality(IEquivalencyValidationContext context)
            {
                if (context.Subject is DateTime)
                {
                    ((DateTime)context.Subject).Should().BeCloseTo((DateTime)context.Expectation, 1000 * 60);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion

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
}