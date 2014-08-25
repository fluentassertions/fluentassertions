using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using FluentAssertions.Equivalency;
#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

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
                "Expected subject to be <null>, but found { }*");
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
                "Expected subject to be*, but found <null>*");
        }

        [TestMethod]
        public void When_subject_and_expectation_are_null_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            SomeDto subject = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_equivilence_on_a_type_from_System_it_should_not_do_a_structural_comparision()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------

            // DateTime is used as an example because the current implemention
            // would hit the recusion-depth limit if structural equivilence were attempted.
            DateTime date1 = DateTime.Parse("2011-01-01");
            DateTime date2 = DateTime.Parse("2011-01-01");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => date1.ShouldBeEquivalentTo(date2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_equivilence_on_a_string_it_should_use_string_specific_failure_messages()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------

            // DateTime is used as an example because the current implemention
            // would hit the recusion-depth limit if structural equivilence were attempted.
            string s1= "hello";
            string s2 = "good-bye";

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => s1.ShouldBeEquivalentTo(s2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("*to be \"good-bye\" with a length of 8, but \"hello\" has a length of 5*");
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
        public void When_a_predicate_for_properties_to_include_has_been_specified_it_should_ignore_the_other_properties()
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
                .Including(info => info.PropertyPath.EndsWith("Age"))
                .Including(info => info.PropertyPath.EndsWith("Birthdate")));

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
            Action act = () => dto.ShouldBeEquivalentTo(dto, options => options.Including((Expression<Func<CustomerDto, object>>)null));

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
        public void When_a_property_is_protected_it_should_be_ignored()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new Customer
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "John",
            };

            subject.SetProtected("ActualValue");

            var expected = new Customer
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "John"
            };

            expected.SetProtected("ExpectedValue");

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
        public void When_a_property_is_hidden_in_a_derived_class_it_should_ignore_it()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new SubclassA<string> { Foo = "test" };
            var expectation = new SubclassB<string> { Foo = "test" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.ShouldBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_property_is_an_indexer_it_should_be_ignored()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new ClassWithIndexer {Foo = "test"};
            var result = new ClassWithIndexer { Foo = "test" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => result.ShouldBeEquivalentTo(expected);


            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        public class BaseWithFoo
        {
            public object Foo { get; set; }
        }

        public class SubclassA<T> : BaseWithFoo
        {
            public new T Foo
            {
                get { return (T)base.Foo; }

                set { base.Foo = value; }
            }
        }

        public class D
        {
            public object Foo { get; set; }
        }

        public class SubclassB<T> : D
        {
            public new T Foo
            {
                get { return (T)base.Foo; }

                set { base.Foo = value; }
            }
        }

        public class ClassWithIndexer
        {
            public object Foo { get; set; }

            public string this[int n]
            {
                get
                {
                    return
                        n.ToString(
                            System.Globalization.CultureInfo.InvariantCulture);
                }
            }
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
                .WithMessage("Expected property VehicleId*99999*but*1*");
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
        public void When_a_reference_to_an_explicit_interface_impl_is_provided_it_should_only_include_those_properties()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IVehicle expected = new ExplicitCar
            {
                Wheels = 4
            };

            IVehicle subject = new ExplicitCar
            {
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
                "Subject has property City that the other object does not have*");
        }

        [TestMethod]
        public void When_equally_named_properties_are_type_incompatible_it_should_throw()
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
                .WithMessage("Expected property Type to be*Int32*, but found*String*");
        }

        [TestMethod]
        public void When_equally_named_properties_are_type_incompatible_and_assertion_rule_exists_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Type = typeof(String),
            };

            var other = new
            {
                Type = typeof(String).AssemblyQualifiedName,
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(other,
                o => o
                    .Using<object>(c => ((Type)c.Subject).AssemblyQualifiedName.Should().Be((string)c.Expectation))
                    .When(si => si.PropertyPath == "Type"));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
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
                .WithMessage("*property Property1 to be \"1\", but \"A\" differs near \"A\"*")
                .WithMessage("*property Property2 to be \"2\", but \"B\" differs near \"B\"*")
                .WithMessage("*property SubType1.SubProperty1 to be \"3\", but \"C\" differs near \"C\"*");
        }

        #endregion

        #region Collection Equivalence

        [TestMethod]
        public void When_a_non_collection_is_compared_to_a_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expectation = new List<Customer>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => 123.ShouldBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>();
        }

        #endregion

        #region DateTime Properties

        [TestMethod]
        public void When_two_properties_are_datetime_and_both_are_nullable_and_both_are_null_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject =
                new
                {
                    Time = (DateTime?) null
                };

            var other =
                new
                {
                    Time = (DateTime?) null
                };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
                subject.ShouldBeEquivalentTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_two_properties_are_datetime_and_both_are_nullable_and_are_equal_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject =
                new
                {
                    Time = (DateTime?) new DateTime(2013, 12, 9, 15, 58, 0)
                };
            
            var other = 
                new
                {
                    Time = (DateTime?)new DateTime(2013, 12, 9, 15, 58, 0)
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
        public void
            When_two_properties_are_datetime_and_both_are_nullable_and_expectation_is_null_it_should_throw_and_state_the_difference
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject =
                new
                {
                    Time = (DateTime?) new DateTime(2013, 12, 9, 15, 58, 0)
                };

            var other =
                new
                {
                    Time = (DateTime?) null
                };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected property Time to be <null>, but found <2013-12-09 15:58:00>.*");
        }

        [TestMethod]
        public void
            When_two_properties_are_datetime_and_both_are_nullable_and_subject_is_null_it_should_throw_and_state_the_difference()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject =
                new
                {
                    Time = (DateTime?) null
                };

            var other =
                new
                {
                    Time = (DateTime?) new DateTime(2013, 12, 9, 15, 58, 0)
                };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected property Time to be <2013-12-09 15:58:00>, but found <null>.*");
        }

        [TestMethod]
        public void When_two_properties_are_datetime_and_expectation_is_nullable_and_are_equal_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject =
                new
                {
                    Time = new DateTime(2013, 12, 9, 15, 58, 0)
                };

            var other =
                new
                {
                    Time = (DateTime?) new DateTime(2013, 12, 9, 15, 58, 0)
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
        public void
            When_two_properties_are_datetime_and_expectation_is_nullable_and_expectation_is_null_it_should_throw_and_state_the_difference
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject =
                new
                {
                    Time = new DateTime(2013, 12, 9, 15, 58, 0)
                };

            var other =
                new
                {
                    Time = (DateTime?) null
                };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected property Time to be <null>, but found <2013-12-09 15:58:00>.*");
        }

        [TestMethod]
        public void When_two_properties_are_datetime_and_subject_is_nullable_and_are_equal_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject =
                new
                {
                    Time = (DateTime?) new DateTime(2013, 12, 9, 15, 58, 0)
                };

            var other =
                new
                {
                    Time = new DateTime(2013, 12, 9, 15, 58, 0)
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
        public void
            When_two_properties_are_datetime_and_subject_is_nullable_and_subject_is_null_it_should_throw_and_state_the_difference()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject =
                new
                {
                    Time = (DateTime?) null
                };

            var other =
                new
                {
                    Time = new DateTime(2013, 12, 9, 15, 58, 0)
                };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(other);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected property Time to be <2013-12-09 15:58:00>, but found <null>.*");
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
                "Expected property Age to be 37 because they are the same, but found 36*");
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
                "Expected property Name to be <null>, but found \"Dennis\"*");
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
                "Expected property Values[1] to be 4, but found 2*");
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
                "Expected property Name to be \"Dennis\", but \"Dennes\" differs near \"es\" (index 4)*");
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
        public void
            When_two_properties_have_the_same_declared_type_but_different_runtime_types_and_are_equivilent_according_to_the_declared_type_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Type = (CustomerType)new DerivedCustomerType("123")
            };

            var other = new
            {
                Type = new CustomerType("123")
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

        [TestMethod]
        public void When_a_nullable_property_is_overriden_with_a_custom_asserrtion_it_should_use_it()
        {
            var actual = new SimpleWithNullable
            {
                nullableIntegerProperty = 1,
                strProperty = "I haz a string!"
            };

            var expected = new SimpleWithNullable
            {
                strProperty = "I haz a string!"
            };

            actual.ShouldBeEquivalentTo(expected,
                opt => opt.Using<Int64>(c => c.Subject.Should().BeInRange(0, 10)).WhenTypeIs<Int64>()
                );
        }

        internal class SimpleWithNullable
        {
            public Int64? nullableIntegerProperty { get; set; }

            public string strProperty { get; set; }
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
        public void When_the_expectation_contains_a_nested_null_it_should_properly_report_the_difference()
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
                    Level = new Level2()
                }
            };

            var expected = new RootDto
            {
                Text = "Root",
                Level = new Level1Dto
                {
                    Text = "Level1",
                    Level = null
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
                .WithMessage("*Expected*Level.Level to be <null>, but found*Level2*");
        }

        [TestMethod]
        public void When_not_all_the_properties_of_the_nested_objects_are_equal_but_nested_objects_are_excluded_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new 
            {
                Property = new ClassWithValueSemanticsOnSingleProperty
                {
                    Key = "123",
                    NestedProperty = "Should be ignored"
                }
            };

            var expected = new
            {
                Property = new ClassWithValueSemanticsOnSingleProperty
                {
                    Key = "123",
                    NestedProperty = "Should be ignored as well"
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expected,
                options => options.ExcludingNestedObjects());

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
                    "Expected property Level.Text to be \"Level2\", but \"Level1\" differs near \"1\" (index 5)*");
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
                Level = new Level1
                {
                    Text = "Level2",
                }
            };

            var expected = new RootDto
            {
                Text = "Root",
                Level = null
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
                .WithMessage("Expected property Level to be <null>*, but found*Level1*Level2*");
        }

        [TestMethod]
        public void When_the_nested_object_property_is_null_it_should_throw()
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
                .WithMessage("Expected property Level to be*Level1Dto*Level2*, but found <null>*");
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
                .WithMessage("Subject has property Level.OtherProperty that the other object does not have*");
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
                    "Expected property Level.Level.Text to be *A wrong text value*but \r\n\"Level2\"*length*");
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
                .WithMessage("Expected property RefOne.ValTwo to be 2, but found 3*");
        }

        #endregion

        #region Cyclic References

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
                .WithMessage("Expected property Level.Root.Level to be*but it contains a cyclic reference*");
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
        public void When_validating_nested_properties_that_are_null_it_should_not_throw_on_cyclic_references()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var actual = new CyclicRoot
            {
                Text = null,
            };

            actual.Level = new CyclicLevel1
            {
                Text = null,
                Root = null,
            };

            var expectation = new CyclicRootDto
            {
                Text = null,
            };

            expectation.Level = new CyclicLevel1Dto
            {
                Text = null,
                Root = null,
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => actual.ShouldBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_the_graph_contains_the_same_value_object_it_should_not_be_treated_as_a_cyclic_reference()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var actual = new CyclicRootWithValueObject()
            {
                Object = new ValueObject("MyValue")
            };

            actual.Level = new CyclicLevelWithValueObject
            {
                Object = new ValueObject("MyValue"),
                Root = null,
            };

            var expectation = new CyclicRootWithValueObject()
            {
                Object = new ValueObject("MyValue")
            };

            expectation.Level = new CyclicLevelWithValueObject
            {
                Object = new ValueObject("MyValue"),
                Root = null,
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => actual.ShouldBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_types_with_infinite_oject_graphs_are_equivilent_it_should_not_overflow_the_stack()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var recursiveClass1 = new ClassWithInfinitelyRecursiveProperty();
            var recursiveClass2 = new ClassWithInfinitelyRecursiveProperty();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => recursiveClass1.ShouldBeEquivalentTo(recursiveClass2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
#if !WINRT && !WINDOWS_PHONE_APP
            act.ShouldNotThrow<StackOverflowException>();
#endif
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_equivilence_on_objects_needing_high_recursion_depth_and_disabling_recursion_depth_limit_it_should_recurse_to_completion()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var recursiveClass1 = new ClassWithFiniteRecursiveProperty(15);
            var recursiveClass2 = new ClassWithFiniteRecursiveProperty(15);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => recursiveClass1.ShouldBeEquivalentTo(recursiveClass2,
                    options => options.AllowingInfiniteRecursion());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        internal class LogbookEntryProjection
        {
            public virtual LogbookCode Logbook { get; set; }
            public virtual ICollection<LogbookRelation> LogbookRelations { get; set; }
        }

        internal class LogbookRelation
        {
            public virtual LogbookCode Logbook { get; set; }
        }

        internal class LogbookCode
        {
            public LogbookCode(string key)
            {
                Key = key;
            }

            public string Key { get; protected set; }
        }

        [TestMethod]
        public void When_the_root_object_is_referenced_from_a_nested_object_it_should_treat_it_as_a_cyclic_reference()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var company1 = new MyCompany { Name = "Company" };
            var user1 = new MyUser { Name = "User", Company = company1 };
            var logo1 = new MyCompanyLogo { Url = "blank", Company = company1, CreatedBy = user1 };
            company1.Logo = logo1;

            var company2 = new MyCompany { Name = "Company" };
            var user2 = new MyUser { Name = "User", Company = company2 };
            var logo2 = new MyCompanyLogo { Url = "blank", Company = company2, CreatedBy = user2 };
            company2.Logo = logo2;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => company1.ShouldBeEquivalentTo(company2, o => o.IgnoringCyclicReferences());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
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
            public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> selectedProperties, ISubjectInfo context)
            {
                return selectedProperties.Where(pi => !pi.Name.EndsWith("Id")).ToArray();
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

#if !WINRT && !WINDOWS_PHONE_APP
                return expectation.GetType().GetProperty(name);
#else
                return expectation.GetType()
                                  .GetRuntimeProperty(name);
#endif
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

        [TestMethod]
        public void When_an_assertion_rule_matches_the_root_object_the_assertion_rule_should_not_apply_to_the_root_object()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = 8.July(2012).At(22, 9);

            var expected = 8.July(2012).At(22, 10);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new RelaxingDateTimeAssertionRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
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

        #region Enums

        [TestMethod]
        public void When_asserting_the_same_enum_member_is_equivilent_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => EnumOne.One.ShouldBeEquivalentTo(EnumOne.One);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_different_enum_members_are_equivilent_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => EnumOne.One.ShouldBeEquivalentTo(EnumOne.Two);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected subject to be Two, but found One*");
        }

        [TestMethod]
        public void When_asserting_members_from_different_enum_types_are_equivilent_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => EnumOne.One.ShouldBeEquivalentTo(EnumTwo.One);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected subject to be One, but found One*");
        }

        #endregion

        #region Memberless Objects

        [TestMethod]
        public void When_asserting_instances_of_an_anonymous_type_having_no_members_are_equivalent_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => new {}.ShouldBeEquivalentTo(new {});

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void When_asserting_instances_of_a_class_having_no_members_are_equivalent_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => new ClassWithNoMembers().ShouldBeEquivalentTo(new ClassWithNoMembers());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void When_asserting_instances_of_a_struct_having_no_memebers_are_equivilent_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => new StructWithNoMembers().ShouldBeEquivalentTo(new StructWithNoMembers());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void When_asserting_instances_of_Object_are_equivalent_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => new object().ShouldBeEquivalentTo(new object());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<InvalidOperationException>();
        }


        [TestMethod]
        public void When_asserting_instances_of_arrays_of_types_in_System_are_equivalent_it_should_respect_the_declared_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            object actual = new int[0];
            object expectation = new int[0];

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => actual.ShouldBeEquivalentTo(expectation);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<InvalidOperationException>();
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

        private enum EnumOne
        {
            One = 0,
            Two = 3
        }

        private enum EnumTwo
        {
            One = 0,
            Two = 3
        }

        private class ClassWithNoMembers
        {
        }

        private struct StructWithNoMembers
        {
        }

        [TestClass]
        public class CollectionEquivalencySpecs
        {
            [TestMethod]
            public void
                When_a_deeply_nested_property_of_a_collection_with_an_invalid_value_is_excluded_it_should_not_throw
                ()
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
                            new {Number = 1, Text = "Text"},
                            new {Number = 2, Text = "Actual"}
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
                            new {Number = 1, Text = "Text"},
                            new {Number = 2, Text = "Expected"}
                        }
                    }
                };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act =
                    () =>
                        subject.ShouldBeEquivalentTo(expected,
                            options => options.
                                Excluding(x => x.Level.Collection[1].Number).
                                Excluding(x => x.Level.Collection[1].Text)
                            );

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow();
            }

            #region Non-Generic Collections

            [TestMethod]
            public void When_asserting_equivalence_of_collections_it_should_respect_the_declared_type()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                ICollection collection1 = new NonGenericCollection(new[] { new Car() });
                ICollection collection2 = new NonGenericCollection(new[] { new Customer() });

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => collection1.ShouldBeEquivalentTo(collection2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow("the declared type is object");
            }

            [TestMethod]
            public void When_asserting_equivalence_of_collections_and_configured_to_use_runtime_properties_it_should_respect_the_runtime_type()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                ICollection collection1 = new NonGenericCollection(new[] { new Car() });
                ICollection collection2 = new NonGenericCollection(new[] { new Customer() });

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act =
                    () =>
                        collection1.ShouldBeEquivalentTo(collection2,
                            opts => opts.IncludingAllRuntimeProperties());

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>("the types have different properties");
            }

            private class NonGenericCollection : ICollection
            {
                private readonly IList<object> inner;

                public NonGenericCollection(IList<object> inner)
                {
                    this.inner = inner;
                }

                public IEnumerator GetEnumerator()
                {
                    foreach (var @object in inner)
                    {
                        yield return @object;
                    }
                }

                public void CopyTo(Array array, int index)
                {
                    ((ICollection)inner).CopyTo(array, index);
                }

                public int Count
                {
                    get
                    {
                        return inner.Count;
                    }
                }

                public object SyncRoot
                {
                    get
                    {
                        return ((ICollection)inner).SyncRoot;
                    }
                }

                public bool IsSynchronized
                {
                    get
                    {
                        return ((ICollection)inner).IsSynchronized;
                    }
                }
            }

            #endregion

            #region Generics

            [TestMethod]
            public void When_a_type_implements_multiple_IEnumerable_interfaces_it_should_fail_descriptively()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var enumerable1 = new EnumerableOfStringAndObject();
                var enumerable2 = new EnumerableOfStringAndObject();

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => enumerable1.ShouldBeEquivalentTo(enumerable2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>()
                    .WithMessage(
                        "Subject is enumerable for more than one type.  " +
                        "It is not known which type should be use for equivalence.\r\n" +
                        "IEnumerable is implemented for the following types: System.String, System.Object*");
            }

            [TestMethod]
            public void When_asserting_equivalence_of_generic_collections_it_should_respect_the_declared_type()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var collection1 = new Collection<CustomerType> { new DerivedCustomerType("123") };
                var collection2 = new Collection<CustomerType> { new CustomerType("123") };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => collection1.ShouldBeEquivalentTo(collection2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow("the objects are equivalent according to the members on the declared type");
            }

            [TestMethod]
            public void When_asserting_equivalence_of_generic_collections_and_configured_to_use_runtime_properties_it_should_respect_the_runtime_type()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var collection1 = new Collection<CustomerType> { new DerivedCustomerType("123") };
                var collection2 = new Collection<CustomerType> { new CustomerType("123") };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act =
                    () =>
                        collection1.ShouldBeEquivalentTo(collection2,
                            opts => opts.IncludingAllRuntimeProperties());

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>("the runtime types have different properties");
            }

            [TestMethod]
            public void When_a_strongly_typed_collection_is_declared_as_an_untyped_collection_is_should_respect_the_declared_type()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                ICollection collection1 = new List<Car> { new Car() };
                ICollection collection2 = new List<Customer> { new Customer() };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => collection1.ShouldBeEquivalentTo(collection2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow("the declared type is object");
            }

            [TestMethod]
            public void When_a_strongly_typed_collection_is_declared_as_an_untyped_collection_and_runtime_checking_is_configured_is_should_use_the_runtime_type()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                ICollection collection1 = new List<Car> { new Car() };
                ICollection collection2 = new List<Customer> { new Customer() };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act =
                    () => collection1.ShouldBeEquivalentTo(collection2, opts => opts.IncludingAllRuntimeProperties());

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>("the items have different runtime types");
            }

            [TestMethod]
            public void When_an_object_implements_multiple_IEnumerable_interfaces_but_the_declared_type_is_assignable_to_only_one_it_should_respect_the_declared_type()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                IEnumerable<string> collection1 = new EnumerableOfStringAndObject();
                IEnumerable<string> collection2 = new EnumerableOfStringAndObject();

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => collection1.ShouldBeEquivalentTo(collection2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow("the declared type is assignable to only one IEnumerable interface");
            }

            [TestMethod]
            public void When_a_object_implements_multiple_IEnumerable_interfaces_but_the_declared_type_is_assignable_to_only_one_and_runtime_checking_is_configured_it_should_fail()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                IEnumerable<string> collection1 = new EnumerableOfStringAndObject();
                IEnumerable<string> collection2 = new EnumerableOfStringAndObject();

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act =
                    () => collection1.ShouldBeEquivalentTo(collection2, opts => opts.IncludingAllRuntimeProperties());

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>("the runtime type is assignable to two IEnumerable interfaces");
            }

            private class EnumerableOfStringAndObject : IEnumerable<string>, IEnumerable<object>
            {
                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }

                IEnumerator<object> IEnumerable<object>.GetEnumerator()
                {
                    return GetEnumerator();
                }

                public IEnumerator<string> GetEnumerator()
                {
                    yield return string.Empty;
                }
            }

            #endregion

            #region Collection Equivalence

            #region ShouldAllBeEquivalentTo

            [TestMethod]
            public void
                When_two_unordered_lists_are_structurally_equivalent_and_order_is_strict_it_should_fail
                ()
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

                var expectation = new Collection<Customer>
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
                Action action =
                    () =>
                        subject.ShouldAllBeEquivalentTo(expectation,
                            options => options.WithStrictOrdering());

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldThrow<AssertFailedException>()
                    .WithMessage(
                        "Expected item[0].Name*Jane*John*item[1].Name*John*Jane*");
            }

            [TestMethod]
            public void
                When_an_unordered_collection_must_be_strict_using_an_expression_it_should_throw
                ()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var subject = new[]
                {
                    new
                    {
                        Name = "John",
                        UnorderedCollection = new[] {1, 2, 3, 4, 5}
                    },
                    new
                    {
                        Name = "Jane",
                        UnorderedCollection = new int[0]
                    }
                };

                var expectation = new[]
                {
                    new
                    {
                        Name = "John",
                        UnorderedCollection = new[] {5, 4, 3, 2, 1}
                    },
                    new
                    {
                        Name = "Jane",
                        UnorderedCollection = new int[0]
                    },
                };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action action =
                    () =>
                        subject.ShouldAllBeEquivalentTo(expectation,
                            options => options
                                .WithStrictOrderingFor(
                                    s => s.UnorderedCollection));

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldThrow<AssertFailedException>()
                    .WithMessage(
                        "*Expected item[0].UnorderedCollection*5 item(s)*0*");
            }

            [TestMethod]
            public void
                When_an_unordered_collection_must_be_strict_using_a_predicate_it_should_throw
                ()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var subject = new[]
                {
                    new
                    {
                        Name = "John",
                        UnorderedCollection = new[] {1, 2, 3, 4, 5}
                    },
                    new
                    {
                        Name = "Jane",
                        UnorderedCollection = new int[0]
                    }
                };

                var expectation = new[]
                {
                    new
                    {
                        Name = "John",
                        UnorderedCollection = new[] {5, 4, 3, 2, 1}
                    },
                    new
                    {
                        Name = "Jane",
                        UnorderedCollection = new int[0]
                    },
                };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action action =
                    () =>
                        subject.ShouldAllBeEquivalentTo(expectation,
                            options => options
                                .WithStrictOrderingFor(
                                    s =>
                                        s.PropertyPath.Contains(
                                            "UnorderedCollection")));

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldThrow<AssertFailedException>()
                    .WithMessage(
                        "*Expected item[0].UnorderedCollection*5 item(s)*0*");
            }

            [TestMethod]
            public void
                When_two_lists_only_differ_in_excluded_properties_it_should_not_throw
                ()
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
                Action action =
                    () =>
                        subject.ShouldAllBeEquivalentTo(expectation,
                            options => options
                                .ExcludingMissingProperties()
                                .Excluding(c => c.Age));

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldNotThrow();
            }

            [TestMethod]
            public void When_ShouldAllBeEquivalentTo_utilizes_custom_assertion_rules_the_rules_should_be_respected()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var subject = new[]
                {
                    new {Value = 
                        new Customer
                        {
                            Name = "John",
                            Age = 27,
                            Id = 1
                        }
                    },
                    new {Value = 
                        new Customer
                        {
                            Name = "Jane",
                            Age = 24,
                            Id = 2
                        }
                    }
                };

                var expectation = new[]
                {
                    new {Value = 
                        new CustomerDto
                        {
                            Name = "John",
                            Age = 27,
                        }
                    },
                     new { Value =
                        new CustomerDto
                        {
                            Name = "Jane",
                            Age = 30,
                        }
                    }
                };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act =
                    () =>
                    subject.ShouldAllBeEquivalentTo(
                        expectation,
                        opts =>
                        opts.Using<Customer>(
                            ctx =>
                                {
                                    throw new Exception("Interestingly, Using cannot cross types so this is never hit");
                                })
                            .WhenTypeIs<Customer>());

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>()
                    .WithMessage(string.Format("*to be a {0}, but found a {1}*", typeof(CustomerDto), typeof(Customer)));
            }

            #endregion

            [TestMethod]
            public void
                When_two_ordered_lists_are_structurally_equivalent_it_should_succeed
                ()
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
                Action action =
                    () => subject.ShouldAllBeEquivalentTo(expectation);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldNotThrow();
            }

            [TestMethod]
            public void
                When_two_unordered_lists_are_structurally_equivalent_it_should_succeed
                ()
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

                var expectation = new Collection<Customer>
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
                Action action =
                    () => subject.ShouldAllBeEquivalentTo(expectation);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldNotThrow();
            }

            [TestMethod]
            public void
                When_two_lists_dont_contain_the_same_structural_equal_objects_it_should_throw
                ()
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
                Action action =
                    () => subject.ShouldAllBeEquivalentTo(expectation);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldThrow<AssertFailedException>()
                    .WithMessage("Expected*item[1].Age*30*24*");
            }

            [TestMethod]
            public void
                When_a_byte_array_does_not_match_strictly_it_should_throw()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var subject = new byte[] { 1, 2, 3, 4, 5, 6 };

                var expectation = new byte[] { 6, 5, 4, 3, 2, 1 };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action action = () => subject.ShouldBeEquivalentTo(expectation);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldThrow<AssertFailedException>()
                    .WithMessage("Expected*item[0]*6*1*");
            }

            [TestMethod]
            public void
                When_no_collection_item_matches_it_should_report_the_closest_match
                ()
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
                Action action =
                    () => subject.ShouldAllBeEquivalentTo(expectation);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldThrow<AssertFailedException>()
                    .WithMessage("Expected*item[1].Age*28*27*");
            }

            [TestMethod]
            public void
                When_the_subject_contains_same_number_of_items_but_subject_contains_duplicates_it_should_throw
                ()
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
                Action action =
                    () => subject.ShouldAllBeEquivalentTo(expectation);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldThrow<AssertFailedException>()
                    .WithMessage(
                        "Expected item[1].Name to be \"Jane\", but \"John\" differs near*");
            }

            [TestMethod]
            public void
                When_the_subject_contains_more_items_than_expected_it_should_throw
                ()
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
                Action action =
                    () => subject.ShouldAllBeEquivalentTo(expectation);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldThrow<AssertFailedException>()
                    .WithMessage(
                        "Expected subject to be a collection with 1 item(s), but found 2*");
            }

            [TestMethod]
            public void
                When_the_subject_contains_less_items_than_expected_it_should_throw
                ()
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
                Action action =
                    () => subject.ShouldAllBeEquivalentTo(expectation);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldThrow<AssertFailedException>()
                    .WithMessage(
                        "*subject to be a collection with 2 item(s), but found 1*");
            }

            [TestMethod]
            public void
                When_the_subject_contains_same_number_of_items_but_expectation_contains_duplicates_it_should_throw
                ()
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
                        Name = "John",
                        Age = 27,
                        Id = 1
                    },
                };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action action =
                    () => subject.ShouldAllBeEquivalentTo(expectation);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldThrow<AssertFailedException>()
                    .WithMessage(
                        "Expected item[1].Name to be \"John\", but \"Jane\" differs near*");
            }

            [TestMethod]
            public void
                When_the_subject_contains_same_number_of_items_and_both_contain_duplicates_it_should_succeed
                ()
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
                Action action =
                    () => subject.ShouldAllBeEquivalentTo(expectation);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldNotThrow();
            }

            [TestMethod]
            public void
                When_a_collection_is_compared_to_a_non_collection_it_should_throw
                ()
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
                    .WithMessage(
                        "Subject is a collection and cannot be compared with a non-collection type*");
            }

            #endregion

            #region Cyclic References

            [TestMethod]
            public void When_the_root_object_is_referenced_from_an_object_in_a_nested_collection_it_should_treat_it_as_a_cyclic_reference()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var company1 = new MyCompany { Name = "Company" };
                var user1 = new MyUser { Name = "User", Company = company1 };
                company1.Users = new List<MyUser> { user1 };
                var logo1 = new MyCompanyLogo { Url = "blank", Company = company1, CreatedBy = user1 };
                company1.Logo = logo1;

                var company2 = new MyCompany { Name = "Company" };
                var user2 = new MyUser { Name = "User", Company = company2 };
                company2.Users = new List<MyUser> { user2 };
                var logo2 = new MyCompanyLogo { Url = "blank", Company = company2, CreatedBy = user2 };
                company2.Logo = logo2;

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action action = () => company1.ShouldBeEquivalentTo(company2, o => o.IgnoringCyclicReferences());

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                action.ShouldNotThrow();
            }

            [TestMethod]
            public void
                When_a_collection_contains_a_reference_to_an_object_that_is_also_in_its_parent_it_should_not_be_treated_as_a_cyclic_reference
                ()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var logbook = new EquivalencySpecs.LogbookCode("SomeKey");

                var logbookEntry = new EquivalencySpecs.LogbookEntryProjection
                {
                    Logbook = logbook,
                    LogbookRelations = new[]
                    {
                        new EquivalencySpecs.LogbookRelation
                        {
                            Logbook = logbook
                        }
                    }
                };

                var equivalentLogbookEntry = new EquivalencySpecs.
                    LogbookEntryProjection
                {
                    Logbook = logbook,
                    LogbookRelations = new[]
                    {
                        new EquivalencySpecs.LogbookRelation
                        {
                            Logbook = logbook
                        }
                    }
                };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act =
                    () =>
                        logbookEntry.ShouldBeEquivalentTo(equivalentLogbookEntry);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow();
            }

            #endregion

            #region Nested Enumerables

            [TestMethodAttribute]
            public void
                When_a_collection_property_contains_objects_with_matching_properties_in_any_order_it_should_not_throw
                ()
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

            [TestMethodAttribute]
            public void
                When_a_collection_property_contains_objects_with_mismatching_properties_it_should_throw
                ()
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
                    .WithMessage("*Customers[0].Name*John*Jane*");
            }

            [TestMethod]
            public void
                When_a_collection_property_was_expected_but_the_property_is_not_a_collection_it_should_throw
                ()
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
                    .WithMessage(
                        "*property Customers to be*Customer[]*, but*System.String*");
            }


            [TestMethod]
            public void
                When_a_collection_contains_more_items_than_expected_it_should_throw
                ()
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
                    .WithMessage(
                        "*property Customers to be a collection with 1 item(s), but found 2*");
            }

            [TestMethod]
            public void
                When_a_collection_contains_less_items_than_expected_it_should_throw
                ()
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
                    .WithMessage(
                        "*property Customers to be a collection with 2 item(s), but found 1*");
            }

            [TestMethodAttribute]
            public void
                When_a_complex_object_graph_with_collections_matches_expectations_it_should_not_throw
                ()
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

#endregion

 #region (Nested) Dictionaries

            [TestMethodAttribute]
            public void
                When_a_dictionary_property_is_detected_it_should_ignore_the_order_of_the_pairs
                ()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var expected = new
                {
                    Customers = new Dictionary<string, string>
                    {
                        {"Key2", "Value2"},
                        {"Key1", "Value1"}
                    }
                };

                var subject = new
                {
                    Customers = new Dictionary<string, string>
                    {
                        {"Key1", "Value1"},
                        {"Key2", "Value2"}
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

            [TestMethodAttribute]
            public void
                When_the_other_property_is_not_a_dictionary_it_should_throw()
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
                        {"Key2", "Value2"},
                        {"Key1", "Value1"}
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
                    .WithMessage("Property*Customers*dictionary*non-dictionary*");
            }

            [TestMethodAttribute]
            public void
                When_the_other_dictionary_does_not_contain_enough_items_it_should_throw
                ()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var expected = new
                {
                    Customers = new Dictionary<string, string>
                    {
                        {"Key1", "Value1"},
                        {"Key2", "Value2"}
                    }
                };

                var subject = new
                {
                    Customers = new Dictionary<string, string>
                    {
                        {"Key1", "Value1"},
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
                    "Expected*Customers*dictionary*2 item(s)*but*1 item(s)*");
            }

            [TestMethod]
            public void When_two_equivalent_dictionaries_are_compared_directly_it_should_succeed()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var result = new Dictionary<string, int>
                {
                    { "C", 0 },
                    { "B", 0 },
                    { "A", 0 }
                };
                
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => result.ShouldBeEquivalentTo(new Dictionary<string, int>
                {
                    { "A", 0 },
                    { "B", 0 },
                    { "C", 0 }
                });

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow();
            }
            
            [TestMethod]
            public void When_two_equivalent_dictionaries_are_compared_directly_as_if_it_is_a_collection_it_should_succeed()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var result = new Dictionary<string, int?>
                {
                    { "C", null },
                    { "B", 0 },
                    { "A", 0 }
                };
                
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => result.ShouldAllBeEquivalentTo(new Dictionary<string, int?>
                {
                    { "A", 0 },
                    { "B", 0 },
                    { "C", null }
                });

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow();
            }

            [TestMethod]
            public void When_two_nested_dictionaries_do_not_match_it_should_throw()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var projection = new
                {
                    ReferencedEquipment = new Dictionary<int, string>
                    {
                        { 1, "Bla1" }
                    }
                };

                var persistedProjection = new
                {
                    ReferencedEquipment = new Dictionary<int, string>
                    {
                        { 1, "Bla2" }
                    }
                };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => persistedProjection.ShouldBeEquivalentTo(projection);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>().WithMessage(
                    "Expected*ReferencedEquipment[1]*Bla1*Bla2*2*index 3*");
            }            
            
            [TestMethod]
            public void When_two_nested_dictionaries_contain_null_values_it_should_not_crash()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var projection = new
                {
                    ReferencedEquipment = new Dictionary<int, string>
                    {
                        { 1, null }
                    }
                };

                var persistedProjection = new
                {
                    ReferencedEquipment = new Dictionary<int, string>
                    {
                        { 1, null }
                    }
                };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => persistedProjection.ShouldBeEquivalentTo(projection);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow();
            }
            
            [TestMethod]
            public void When_the_dictionary_values_are_handled_by_the_enumerable_equivalency_step_the_type_information_should_be_preserved()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                Guid userId = Guid.NewGuid();
                
                var actual = new UserRolesLookupElement();
                actual.Add(userId, "Admin", "Special");

                var expected = new UserRolesLookupElement();
                expected.Add(userId, "Admin", "Other");

               //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => actual.ShouldBeEquivalentTo(expected);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>()
                    .WithMessage("Expected*Roles[*][1]*Other*Special*");
            }

            public class UserRolesLookupElement
            {
                private readonly Dictionary<Guid, List<string>> innerRoles = new Dictionary<Guid, List<string>>();

                public virtual Dictionary<Guid, IEnumerable<string>> Roles
                {
                    get { return innerRoles.ToDictionary(x => x.Key, y => y.Value.Select(z => z)); }
                }

                public void Add(Guid userId, params string[] roles)
                {
                    innerRoles[userId] = roles.ToList();
                }
            }

            #endregion
        }

        [TestClass]
        public class DictionaryEquivalencySpecs
        {
            #region Non-Generic Dictionaries

            [TestMethod]
            public void When_asserting_equivalence_of_dictionaries_it_should_respect_the_declared_type()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                IDictionary dictionary1 = new NonGenericDictionary { { 2001, new Car() } };
                IDictionary dictionary2 = new NonGenericDictionary { { 2001, new Customer() } };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => dictionary1.ShouldBeEquivalentTo(dictionary2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow("the declared type of the items is object");
            }

            [TestMethod]
            public void When_asserting_equivalence_of_dictionaries_and_configured_to_use_runtime_properties_it_should_respect_the_runtime_type()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                IDictionary dictionary1 = new NonGenericDictionary { { 2001, new Car() } };
                IDictionary dictionary2 = new NonGenericDictionary { { 2001, new Customer() } };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act =
                    () =>
                        dictionary1.ShouldBeEquivalentTo(dictionary2,
                            opts => opts.IncludingAllRuntimeProperties());

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>("the types have different properties");
            }

            [TestMethod]
            public void When_a_non_generic_dictionary_is_typed_as_object_and_runtime_typing_has_not_been_specified_the_declared_type_should_be_respected()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                object object1 = new NonGenericDictionary();
                object object2 = new NonGenericDictionary();

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => object1.ShouldBeEquivalentTo(object2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<InvalidOperationException>("the type of the subject is object");
            }

            [TestMethod]
            public void When_a_non_generic_dictionary_is_typed_as_object_and_runtime_typing_is_specified_the_runtime_type_should_be_respected()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                object object1 = new NonGenericDictionary { { "greeting", "hello" } };
                object object2 = new NonGenericDictionary { { "greeting", "hello" } };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => object1.ShouldBeEquivalentTo(object2, opts => opts.IncludingAllRuntimeProperties());

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow("the runtime type is a dictionary and the dictionaries are equivalent");
            }

            #endregion

            [TestMethod]
            public void When_an_object_implements_two_IDictionary_interfaces_it_should_fail_descriptively()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var object1 = new ClassWithTwoDictionaryImplementations();
                var object2 = new ClassWithTwoDictionaryImplementations();

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => object1.ShouldBeEquivalentTo(object2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>()
                    .WithMessage(
                        "Subject implements multiple dictionary types.  "
                        + "It is not known which type should be use for equivalence.*");
            }

            [TestMethod]
            public void When_two_dictionaries_asserted_to_be_equivalent_have_different_lengths_it_should_fail_descriptively()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var dictionary1 = new Dictionary<string, string> { { "greeting", "hello" } };
                var dictionary2 = new Dictionary<string, string> { { "greeting", "hello" }, {"farewell", "goodbye"} };
                

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act1 = () => dictionary1.ShouldBeEquivalentTo(dictionary2);
                Action act2 = () => dictionary2.ShouldBeEquivalentTo(dictionary1);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act1.ShouldThrow<AssertFailedException>().WithMessage("Expected subject to be a dictionary with 2 item(s), but found 1 item(s)*");
                act2.ShouldThrow<AssertFailedException>().WithMessage("Expected subject to be a dictionary with 1 item(s), but found 2 item(s)*");
            }

            [TestMethod]
            public void When_a_dictionary_does_not_implement_IDictionary_it_should_still_be_treated_as_a_dictionary()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                IDictionary<string, int> dictionary = new GenericDictionaryNotImplementingIDictionary<string, int> { { "hi", 1 } };
                ICollection<KeyValuePair<string, int>> collection = new List<KeyValuePair<string, int>> { new KeyValuePair<string, int>( "hi", 1 ) };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => dictionary.ShouldBeEquivalentTo(collection);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>()
                    .WithMessage("*is a dictionary and cannot be compared with a non-dictionary type.*");
            }

            [TestMethod]
            public void When_asserting_equivalence_of_generic_dictionaries_and_the_expectation_key_type_is_assignable_from_the_subjects_it_should_pass_if_compatible()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var dictionary1 = new Dictionary<string, string> { { "greeting", "hello" } };
                var dictionary2 = new Dictionary<object, string> { { "greeting", "hello" } };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => dictionary1.ShouldBeEquivalentTo(dictionary2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow("the keys are still strings");
            }

            [TestMethod]
            public void When_asserting_equivalence_of_generic_dictionaries_and_the_expectation_key_type_is_assignable_from_the_subjects_it_should_fail_if_incompatible()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var dictionary1 = new Dictionary<string, string> { { "greeting", "hello" } };
                var dictionary2 = new Dictionary<object, string> { { new object(), "hello" } };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => dictionary1.ShouldBeEquivalentTo(dictionary2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>().WithMessage("Subject contains unexpected key \"greeting\"*");
            }

            [TestMethod]
            public void When_asserting_equivalence_of_generic_dictionaries_and_the_expectation_key_type_is_not_assignable_from_the_subjects_it_should_fail()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var dictionary1 = new Dictionary<string, string> { { "greeting", "hello" } };
                var dictionary2 = new Dictionary<int, string> { { 1234, "hello" } };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => dictionary1.ShouldBeEquivalentTo(dictionary2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>()
                    .WithMessage(
                        string.Format(
                            "*The subject dictionary has keys of type System.String; however, the expected dictionary is not keyed with any compatible types.*{0}*",
                            typeof(IDictionary<int, string>)));
            }

            [TestMethod]
            public void When_a_generic_dictionary_is_typed_as_object_and_runtime_typing_has_not_been_specified_the_declared_type_should_be_respected()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                object object1 = new Dictionary<string, string> { { "greeting", "hello" } };
                object object2 = new Dictionary<string, string> { { "greeting", "hello" } };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => object1.ShouldBeEquivalentTo(object2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<InvalidOperationException>("the type of the subject is object");
            }

            [TestMethod]
            public void When_a_generic_dictionary_is_typed_as_object_and_runtime_typing_has_is_specified_it_should_use_the_runtime_type()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                object object1 = new Dictionary<string, string> { { "greeting", "hello" } };
                object object2 = new Dictionary<string, string> { { "greeting", "hello" } };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => object1.ShouldBeEquivalentTo(object2, opts => opts.IncludingAllRuntimeProperties());

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow("the runtime type is a dictionary and the dictionaries are equivalent");
            }

            [TestMethod]
            public void When_asserting_the_equivalence_of_generic_dictionaries_it_should_respect_the_declared_type()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var dictionary1 = new Dictionary<int, CustomerType> { { 0, new DerivedCustomerType("123") } };
                var dictionary2 = new Dictionary<int, CustomerType> { { 0, new CustomerType("123") } };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => dictionary1.ShouldBeEquivalentTo(dictionary2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow("the objects are equivalent according to the members on the declared type");
            }

            [TestMethod]
            public void When_asserting_equivalence_of_generic_dictionaries_and_configured_to_use_runtime_properties_it_should_respect_the_runtime_type()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var dictionary1 = new Dictionary<int, CustomerType> { { 0, new DerivedCustomerType("123") } };
                var dictionary2 = new Dictionary<int, CustomerType> { { 0, new CustomerType("123") } };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act =
                    () =>
                        dictionary1.ShouldBeEquivalentTo(dictionary2,
                            opts => opts.IncludingAllRuntimeProperties());

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>("the runtime types have different properties");
            }

            [TestMethod]
            public void When_asserting_equivalence_of_generic_dictionaries_the_type_information_should_be_preserved_for_other_equivalency_steps()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                Guid userId = Guid.NewGuid();

                var dictionary1 = new Dictionary<Guid, IEnumerable<string>> { { userId, new List<string> { "Admin", "Special" } } };
                var dictionary2 = new Dictionary<Guid, IEnumerable<string>> { { userId, new List<string> { "Admin", "Other" } } };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => dictionary1.ShouldBeEquivalentTo(dictionary2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldThrow<AssertFailedException>();
            }

            [TestMethod]
            public void When_asserting_equivalence_of_non_generic_dictionaries_the_lack_of_type_information_should_be_preserved_for_other_equivalency_steps()
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                Guid userId = Guid.NewGuid();

                var dictionary1 = new NonGenericDictionary { { userId, new List<string> { "Admin", "Special" } } };
                var dictionary2 = new NonGenericDictionary { { userId, new List<string> { "Admin", "Other" } } };

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                Action act = () => dictionary1.ShouldBeEquivalentTo(dictionary2);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                act.ShouldNotThrow("the declared type of the values is 'object'");
            }

            public class UserRolesLookupElement
            {
                private readonly Dictionary<Guid, List<string>> innerRoles = new Dictionary<Guid, List<string>>();

                public virtual Dictionary<Guid, IEnumerable<string>> Roles
                {
                    get { return innerRoles.ToDictionary(x => x.Key, y => y.Value.Select(z => z)); }
                }

                public void Add(Guid userId, params string[] roles)
                {
                    innerRoles[userId] = roles.ToList();
                }
            }

            private class NonGenericDictionary : IDictionary
            {
                private readonly IDictionary dictionary = new Dictionary<object, object>();

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }

                public void CopyTo(Array array, int index)
                {
                    dictionary.CopyTo(array, index);
                }

                public int Count
                {
                    get
                    {
                        return dictionary.Count;
                    }
                }

                public bool IsSynchronized
                {
                    get
                    {
                        return dictionary.IsSynchronized;
                    }
                }

                public object SyncRoot
                {
                    get
                    {
                        return dictionary.SyncRoot;
                    }
                }

                public void Add(object key, object value)
                {
                    dictionary.Add(key, value);
                }

                public void Clear()
                {
                    dictionary.Clear();
                }

                public bool Contains(object key)
                {
                    return dictionary.Contains(key);
                }

                public IDictionaryEnumerator GetEnumerator()
                {
                    return dictionary.GetEnumerator();
                }

                public void Remove(object key)
                {
                    dictionary.Remove(key);
                }

                public bool IsFixedSize
                {
                    get
                    {
                        return dictionary.IsFixedSize;
                    }
                }

                public bool IsReadOnly
                {
                    get
                    {
                        return dictionary.IsReadOnly;
                    }
                }

                public object this[object key]
                {
                    get
                    {
                        return dictionary[key];
                    }
                    set
                    {
                        dictionary[key] = value;
                    }
                }

                public ICollection Keys
                {
                    get
                    {
                        return dictionary.Keys;
                    }
                }

                public ICollection Values
                {
                    get
                    {
                        return dictionary.Values;
                    }
                }
            }

            private class GenericDictionaryNotImplementingIDictionary<TKey, TValue> : IDictionary<TKey, TValue>
            {
                private readonly Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }

                public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
                {
                    return dictionary.GetEnumerator();
                }

                void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
                {
                    ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Add(item);
                }

                public void Clear()
                {
                    dictionary.Clear();
                }

                bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
                {
                    return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Contains(item);
                }

                void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
                {
                    ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).CopyTo(array, arrayIndex);
                }

                bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
                {
                    return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Remove(item);
                }

                public int Count
                {
                    get
                    {
                        return dictionary.Count;
                    }
                }

                public bool IsReadOnly
                {
                    get
                    {
                        return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).IsReadOnly;
                    }
                }

                public bool ContainsKey(TKey key)
                {
                    return dictionary.ContainsKey(key);
                }

                public void Add(TKey key, TValue value)
                {
                    dictionary.Add(key, value);
                }

                public bool Remove(TKey key)
                {
                    return dictionary.Remove(key);
                }

                public bool TryGetValue(TKey key, out TValue value)
                {
                    return dictionary.TryGetValue(key, out value);
                }

                public TValue this[TKey key]
                {
                    get
                    {
                        return dictionary[key];
                    }
                    set
                    {
                        dictionary[key] = value;
                    }
                }

                public ICollection<TKey> Keys
                {
                    get
                    {
                        return dictionary.Keys;
                    }
                }

                public ICollection<TValue> Values
                {
                    get
                    {
                        return dictionary.Values;
                    }
                }
            }

            /// <summary>
            /// FakeItEasy can probably handle this in a couple lines, but then it would not be portable.
            /// </summary>
            private class ClassWithTwoDictionaryImplementations : Dictionary<int, object>, IDictionary<string, object>
            {
                IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
                {
                    return
                        ((ICollection<KeyValuePair<int, object>>)this).Select(
                            item =>
                            new KeyValuePair<string, object>(
                                item.Key.ToString(CultureInfo.InvariantCulture),
                                item.Value)).GetEnumerator();
                }

                public void Add(KeyValuePair<string, object> item)
                {
                    ((ICollection<KeyValuePair<int, object>>)this).Add(new KeyValuePair<int, object>(Parse(item.Key), item.Value));
                }

                public bool Contains(KeyValuePair<string, object> item)
                {
                    return
                        ((ICollection<KeyValuePair<int, object>>)this).Contains(
                            new KeyValuePair<int, object>(Parse(item.Key), item.Value));
                }

                public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
                {
                    ((ICollection<KeyValuePair<int, object>>)this).Select(
                        item =>
                        new KeyValuePair<string, object>(item.Key.ToString(CultureInfo.InvariantCulture), item.Value))
                        .ToArray()
                        .CopyTo(array, arrayIndex);
                }

                public bool Remove(KeyValuePair<string, object> item)
                {
                    return
                        ((ICollection<KeyValuePair<int, object>>)this).Remove(
                            new KeyValuePair<int, object>(Parse(item.Key), item.Value));
                }

                bool ICollection<KeyValuePair<string, object>>.IsReadOnly
                {
                    get
                    {
                        return ((ICollection<KeyValuePair<int, object>>)this).IsReadOnly;
                    }
                }

                public bool ContainsKey(string key)
                {
                    return ContainsKey(Parse(key));
                }

                public void Add(string key, object value)
                {
                    Add(Parse(key), value);
                }

                public bool Remove(string key)
                {
                    return Remove(Parse(key));
                }

                public bool TryGetValue(string key, out object value)
                {
                    return TryGetValue(Parse(key), out value);
                }

                public object this[string key]
                {
                    get
                    {
                        return this[Parse(key)];
                    }
                    set
                    {
                        this[Parse(key)] = value;
                    }
                }

                ICollection<string> IDictionary<string, object>.Keys
                {
                    get
                    {
                        return Keys.Select(_ => _.ToString(CultureInfo.InvariantCulture)).ToList();
                    }
                }

                ICollection<object> IDictionary<string, object>.Values
                {
                    get
                    {
                        return Values;
                    }
                }

                private int Parse(string key)
                {
                    return int.Parse(key);
                }
            }
        }
    }

    internal class ClassWithInfinitelyRecursiveProperty
    {
        public ClassWithInfinitelyRecursiveProperty Self
        {
            get
            {
                return new ClassWithInfinitelyRecursiveProperty();
            }
        }
    }

    internal class ClassWithFiniteRecursiveProperty
    {
        private readonly int depth;

        public ClassWithFiniteRecursiveProperty(int recursiveDepth)
        {
            depth = recursiveDepth;
        }

        public ClassWithFiniteRecursiveProperty Self
        {
            get
            {
                return depth > 0
                    ? new ClassWithFiniteRecursiveProperty(depth - 1)
                    : null;
            }
        }
    }

    internal class MyCompanyLogo
    {
        public string Url { get; set; }
        public MyCompany Company { get; set; }
        public MyUser CreatedBy { get; set; }
    }

    internal class MyUser
    {
        public string Name { get; set; }
        public MyCompany Company { get; set; }
    }

    internal class MyCompany
    {
        public string Name { get; set; }
        public MyCompanyLogo Logo { get; set; }
        public List<MyUser> Users { get; set; }
    }

    public class Customer : Entity
    {
        private string PrivateProperty { get; set; }

        protected string ProtectedProperty { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthdate { get; set; }
        public long Id { get; set; }

        public void SetProtected(string value)
        {
            ProtectedProperty = value;
        }

        public Customer()
        {
        }

        public Customer(string privateProperty)
        {
            PrivateProperty = privateProperty;
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

            if (((object)a == null) || ((object)b == null))
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
        public string DerivedInfo { get; set; }

        public DerivedCustomerType(string code) : base(code)
        {
        }
    }

    #region Nested classes for comparison

    public class ClassWithValueSemanticsOnSingleProperty
    {
        public string Key { get; set; }
        public string NestedProperty { get; set; }


        protected bool Equals(ClassWithValueSemanticsOnSingleProperty other)
        {
            return string.Equals(Key, other.Key);
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((ClassWithValueSemanticsOnSingleProperty)obj);
        }


        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }


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

    public class CyclicRootWithValueObject
    {
        public ValueObject Object { get; set; }
        public CyclicLevelWithValueObject Level { get; set; }
    }

    public class ValueObject
    {
        private readonly string value;

        public ValueObject(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }

        public override bool Equals(object obj)
        {
            return ((ValueObject)obj).Value.Equals(Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    public class CyclicLevel1
    {
        public string Text { get; set; }
        public CyclicRoot Root { get; set; }
    }

    public class CyclicLevelWithValueObject
    {
        public ValueObject Object { get; set; }
        public CyclicRootWithValueObject Root { get; set; }
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

    public class ExplicitCar : ExplicitVehicle, ICar
    {
        public int Wheels { get; set; }
    }

    public class Vehicle : IVehicle
    {
        public int VehicleId { get; set; }
    }

    public class ExplicitVehicle : IVehicle
    {
        int IVehicle.VehicleId
        {
            get; set; 
        }
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