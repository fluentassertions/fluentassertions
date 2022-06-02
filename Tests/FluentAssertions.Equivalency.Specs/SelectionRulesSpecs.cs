using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using FluentAssertions.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

public class SelectionRulesSpecs
{
    private enum LocalOtherType : byte
    {
        Default,
        NonDefault
    }

    private enum LocalType : byte
    {
        Default,
        NonDefault
    }

    [Fact]
    public void When_specific_properties_have_been_specified_it_should_ignore_the_other_properties()
    {
        // Arrange
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

        // Act
        Action act = () => subject.Should().BeEquivalentTo(customer, options => options
            .Including(d => d.Age)
            .Including(d => d.Birthdate));

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void A_member_included_by_path_is_described_in_the_failure_message()
    {
        // Arrange
        var subject = new
        {
            Name = "John"
        };

        var customer = new
        {
            Name = "Jack"
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(customer, options => options
            .Including(d => d.Name));

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Include*Name*");
    }

    [Fact]
    public void A_member_included_by_predicate_is_described_in_the_failure_message()
    {
        // Arrange
        var subject = new
        {
            Name = "John"
        };

        var customer = new
        {
            Name = "Jack"
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(customer, options => options
            .Including(ctx => ctx.Path == "Name"));

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Include member when*Name*");
    }

    [Fact]
    public void A_member_excluded_by_path_is_described_in_the_failure_message()
    {
        // Arrange
        var subject = new
        {
            Name = "John",
            Age = 13
        };

        var customer = new
        {
            Name = "Jack",
            Age = 37
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(customer, options => options
            .Excluding(d => d.Age));

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Exclude*Age*");
    }

    [Fact]
    public void A_member_excluded_by_predicate_is_described_in_the_failure_message()
    {
        // Arrange
        var subject = new
        {
            Name = "John",
            Age = 13
        };

        var customer = new
        {
            Name = "Jack",
            Age = 37
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(customer, options => options
            .Excluding(ctx => ctx.Path == "Age"));

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Exclude member when*Age*");
    }

    [Fact]
    public void When_a_predicate_for_properties_to_include_has_been_specified_it_should_ignore_the_other_properties()
    {
        // Arrange
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

        // Act
        Action act = () => subject.Should().BeEquivalentTo(customer, options => options
            .Including(info => info.Path.EndsWith("Age", StringComparison.Ordinal))
            .Including(info => info.Path.EndsWith("Birthdate", StringComparison.Ordinal)));

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_a_non_property_expression_is_provided_it_should_throw()
    {
        // Arrange
        var dto = new CustomerDto();

        // Act
        Action act = () => dto.Should().BeEquivalentTo(dto, options => options.Including(d => d.GetType()));

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Expression <d.GetType()> cannot be used to select a member.*")
            .WithParameterName("expression");
    }

    [Fact]
    public void When_including_a_property_it_should_exactly_match_the_property()
    {
        // Arrange
        var actual = new
        {
            DeclaredType = LocalOtherType.NonDefault,
            Type = LocalType.NonDefault
        };

        var expectation = new
        {
            DeclaredType = LocalOtherType.NonDefault
        };

        // Act
        Action act = () => actual.Should().BeEquivalentTo(expectation,
            config => config.Including(o => o.DeclaredType));

        // Assert
        act.Should().NotThrow();
    }

    public class CustomType
    {
        public string Name { get; set; }
    }

    public class ClassA
    {
        public List<CustomType> ListOfCustomTypes { get; set; }
    }

    [Fact]
    public void When_including_a_property_using_an_expression_it_should_evaluate_it_from_the_root()
    {
        // Arrange
        var list1 = new List<CustomType>
        {
            new CustomType { Name = "A" },
            new CustomType { Name = "B" }
        };

        var list2 = new List<CustomType>
        {
            new CustomType { Name = "C" },
            new CustomType { Name = "D" }
        };

        var objectA = new ClassA { ListOfCustomTypes = list1 };
        var objectB = new ClassA { ListOfCustomTypes = list2 };

        // Act
        Action act = () => objectA.Should().BeEquivalentTo(objectB, options => options.Including(x => x.ListOfCustomTypes));

        // Assert
        act.Should().Throw<XunitException>().
            WithMessage("*C*but*A*D*but*B*");
    }

    [Fact]
    public void When_null_is_provided_as_property_expression_it_should_throw()
    {
        // Arrange
        var dto = new CustomerDto();

        // Act
        Action act =
            () => dto.Should().BeEquivalentTo(dto, options => options.Including(null));

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage(
            "Expected an expression, but found <null>.*");
    }

    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public void When_including_fields_it_should_succeed_if_just_the_included_field_match()
    {
        // Arrange
        var class1 = new ClassWithSomeFieldsAndProperties
        {
            Field1 = "Lorem",
            Field2 = "ipsum",
            Field3 = "dolor",
            Property1 = "sit",
            Property2 = "amet",
            Property3 = "consectetur"
        };

        var class2 = new ClassWithSomeFieldsAndProperties { Field1 = "Lorem", Field2 = "ipsum" };

        // Act
        Action act =
            () =>
                class1.Should().BeEquivalentTo(class2, opts => opts.Including(_ => _.Field1).Including(_ => _.Field2));

        // Assert
        act.Should().NotThrow("the only selected fields have the same value");
    }

    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public void When_including_fields_it_should_fail_if_any_included_field_do_not_match()
    {
        // Arrange
        var class1 = new ClassWithSomeFieldsAndProperties
        {
            Field1 = "Lorem",
            Field2 = "ipsum",
            Field3 = "dolor",
            Property1 = "sit",
            Property2 = "amet",
            Property3 = "consectetur"
        };

        var class2 = new ClassWithSomeFieldsAndProperties { Field1 = "Lorem", Field2 = "ipsum" };

        // Act
        Action act =
            () =>
                class1.Should().BeEquivalentTo(class2,
                    opts => opts.Including(_ => _.Field1).Including(_ => _.Field2).Including(_ => _.Field3));

        // Assert
        act.Should().Throw<XunitException>().WithMessage("Expected field class1.Field3*");
    }

    [Fact]
    public void When_only_the_excluded_property_doesnt_match_it_should_not_throw()
    {
        // Arrange
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

        // Act / Assert
        dto.Should().BeEquivalentTo(customer, options => options
            .Excluding(d => d.Name)
            .Excluding(d => d.Id));
    }

    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public void When_excluding_members_it_should_pass_if_only_the_excluded_members_are_different()
    {
        // Arrange
        var class1 = new ClassWithSomeFieldsAndProperties
        {
            Field1 = "Lorem",
            Field2 = "ipsum",
            Field3 = "dolor",
            Property1 = "sit"
        };

        var class2 = new ClassWithSomeFieldsAndProperties { Field1 = "Lorem", Field2 = "ipsum" };

        // Act
        Action act =
            () =>
                class1.Should().BeEquivalentTo(class2,
                    opts => opts.Excluding(_ => _.Field3).Excluding(_ => _.Property1));

        // Assert
        act.Should().NotThrow("the non-excluded fields have the same value");
    }

    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public void When_excluding_members_it_should_fail_if_any_non_excluded_members_are_different()
    {
        // Arrange
        var class1 = new ClassWithSomeFieldsAndProperties
        {
            Field1 = "Lorem",
            Field2 = "ipsum",
            Field3 = "dolor",
            Property1 = "sit"
        };

        var class2 = new ClassWithSomeFieldsAndProperties { Field1 = "Lorem", Field2 = "ipsum" };

        // Act
        Action act =
            () => class1.Should().BeEquivalentTo(class2, opts => opts.Excluding(_ => _.Property1));

        // Assert
        act.Should().Throw<XunitException>().WithMessage("Expected*Field3*");
    }

    [Fact]
    public void When_all_shared_properties_match_it_should_not_throw()
    {
        // Arrange
        var dto = new CustomerDto
        {
            Age = 36,
            Birthdate = new DateTime(1973, 9, 20),
            Name = "John"
        };

        var customer = new Customer
        {
            Id = 1,
            Age = 36,
            Birthdate = new DateTime(1973, 9, 20),
            Name = "John"
        };

        // Act
        Action act = () => dto.Should().BeEquivalentTo(customer, options => options.ExcludingMissingMembers());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_a_property_is_write_only_it_should_be_ignored()
    {
        // Arrange
        var subject = new ClassWithWriteOnlyProperty
        {
            WriteOnlyProperty = 123,
            SomeOtherProperty = "whatever"
        };

        var expected = new
        {
            SomeOtherProperty = "whatever"
        };

        // Act
        Action action = () => subject.Should().BeEquivalentTo(expected);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void When_a_property_is_private_it_should_be_ignored()
    {
        // Arrange
        var subject = new Customer("MyPassword")
        {
            Age = 36,
            Birthdate = new DateTime(1973, 9, 20),
            Name = "John"
        };

        var other = new Customer("SomeOtherPassword")
        {
            Age = 36,
            Birthdate = new DateTime(1973, 9, 20),
            Name = "John"
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(other);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_a_field_is_private_it_should_be_ignored()
    {
        // Arrange
        var subject = new ClassWithAPrivateField(1234) { Value = 1 };

        var other = new ClassWithAPrivateField(54321) { Value = 1 };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(other);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_a_property_is_protected_it_should_be_ignored()
    {
        // Arrange
        var subject = new Customer
        {
            Age = 36,
            Birthdate = new DateTime(1973, 9, 20),
            Name = "John"
        };

        subject.SetProtected("ActualValue");

        var expected = new Customer
        {
            Age = 36,
            Birthdate = new DateTime(1973, 9, 20),
            Name = "John"
        };

        expected.SetProtected("ExpectedValue");

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_a_property_is_hidden_in_a_derived_class_it_should_ignore_it()
    {
        // Arrange
        var subject = new SubclassA<string> { Foo = "test" };
        var expectation = new SubclassB<string> { Foo = "test" };

        // Act
        Action action = () => subject.Should().BeEquivalentTo(expectation);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void When_including_a_property_that_is_hidden_in_a_derived_class_it_should_select_the_correct_one()
    {
        // Arrange
        var b1 = new ClassThatHidesBaseClassProperty();
        var b2 = new ClassThatHidesBaseClassProperty();

        // Act / Assert
        b1.Should().BeEquivalentTo(b2, config => config.Including(b => b.Property));
    }

    [Fact]
    public void Excluding_a_property_hiding_a_base_class_property_should_not_reveal_the_latter()
    {
        // Arrange
        var b1 = new ClassThatHidesBaseClassProperty();
        var b2 = new ClassThatHidesBaseClassProperty();

        // Act
        Action act = () => b1.Should().BeEquivalentTo(b2, config => config.Excluding(b => b.Property));

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("*No members were found *");
    }

    private class ClassWithGuidProperty
    {
        public string Property { get; set; } = Guid.NewGuid().ToString();
    }

    private class ClassThatHidesBaseClassProperty : ClassWithGuidProperty
    {
        public new string[] Property { get; set; }
    }

    [Fact]
    public void When_a_property_is_internal_it_should_be_excluded_from_the_comparison()
    {
        // Arrange
        var actual = new ClassWithInternalProperty
        {
            PublicProperty = "public",
            InternalProperty = "internal",
            ProtectedInternalProperty = "internal"
        };

        var expected = new ClassWithInternalProperty
        {
            PublicProperty = "public",
            InternalProperty = "also internal",
            ProtectedInternalProperty = "also internal"
        };

        // Act / Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void When_a_property_is_internal_and_it_should_be_included_it_should_fail_the_assertion()
    {
        // Arrange
        var actual = new ClassWithInternalProperty
        {
            PublicProperty = "public",
            InternalProperty = "internal",
            ProtectedInternalProperty = "internal"
        };

        var expected = new ClassWithInternalProperty
        {
            PublicProperty = "public",
            InternalProperty = "also internal",
            ProtectedInternalProperty = "also internal"
        };

        // Act
        Action act = () => actual.Should().BeEquivalentTo(expected, options => options.IncludingInternalProperties());

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*InternalProperty*also internal*internal*ProtectedInternalProperty*");
    }

    private class ClassWithInternalProperty
    {
        public string PublicProperty { get; set; }

        internal string InternalProperty { get; set; }

        protected internal string ProtectedInternalProperty { get; set; }
    }

    [Fact]
    public void When_a_field_is_internal_it_should_be_excluded_from_the_comparison()
    {
        // Arrange
        var actual = new ClassWithInternalField
        {
            PublicField = "public",
            InternalField = "internal",
            ProtectedInternalField = "internal"
        };

        var expected = new ClassWithInternalField
        {
            PublicField = "public",
            InternalField = "also internal",
            ProtectedInternalField = "also internal"
        };

        // Act / Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void When_a_field_is_internal_and_it_should_be_included_it_should_fail_the_assertion()
    {
        // Arrange
        var actual = new ClassWithInternalField
        {
            PublicField = "public",
            InternalField = "internal",
            ProtectedInternalField = "internal"
        };

        var expected = new ClassWithInternalField
        {
            PublicField = "public",
            InternalField = "also internal",
            ProtectedInternalField = "also internal"
        };

        // Act
        Action act = () => actual.Should().BeEquivalentTo(expected, options => options.IncludingInternalFields());

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*InternalField*also internal*internal*ProtectedInternalField*");
    }

    private class ClassWithInternalField
    {
        public string PublicField;

        internal string InternalField;

        protected internal string ProtectedInternalField;
    }

    [Fact]
    public void When_a_property_is_an_indexer_it_should_be_ignored()
    {
        // Arrange
        var expected = new ClassWithIndexer { Foo = "test" };
        var result = new ClassWithIndexer { Foo = "test" };

        // Act
        Action act = () => result.Should().BeEquivalentTo(expected);

        // Assert
        act.Should().NotThrow();
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
                        CultureInfo.InvariantCulture);
            }
        }
    }

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
    public void When_respecting_declared_types_explicit_interface_member_on_interfaced_subject_should_be_used()
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
        Action action = () => subject.Should().BeEquivalentTo(expected, opt => opt.RespectingDeclaredTypes());

        // Assert
        action.Should().NotThrow();
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
    public void When_respecting_declared_types_explicit_interface_member_on_subject_should_not_be_used()
    {
        // Arrange
        var expected = new Vehicle
        {
            VehicleId = 1
        };

        var subject = new ExplicitVehicle
        {
            VehicleId = 2
        };
        ((IVehicle)subject).VehicleId = 1;

        // Act
        Action action = () => subject.Should().BeEquivalentTo(expected, opt => opt.RespectingDeclaredTypes());

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
        Action action = () => subject.Should().BeEquivalentTo(expected, opt => opt.RespectingDeclaredTypes());

        // Assert
        action.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_respecting_runtime_types_explicit_interface_member_on_subject_should_not_be_used()
    {
        // Arrange
        var expected = new Vehicle
        {
            VehicleId = 1
        };

        var subject = new ExplicitVehicle
        {
            VehicleId = 2
        };
        ((IVehicle)subject).VehicleId = 1;

        // Act
        Action action = () => subject.Should().BeEquivalentTo(expected, opt => opt.RespectingRuntimeTypes());

        // Assert
        action.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_respecting_runtime_types_explicit_interface_member_on_expectation_should_not_be_used()
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
        Action action = () => subject.Should().BeEquivalentTo(expected, opt => opt.RespectingRuntimeTypes());

        // Assert
        action.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_a_deeply_nested_property_with_a_value_mismatch_is_excluded_it_should_not_throw()
    {
        // Arrange
        var subject = new Root
        {
            Text = "Root",
            Level = new Level1
            {
                Text = "Level1",
                Level = new Level2
                {
                    Text = "Mismatch"
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
                    Text = "Level2"
                }
            }
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected,
            options => options.Excluding(r => r.Level.Level.Text));

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_a_property_with_a_value_mismatch_is_excluded_using_a_predicate_it_should_not_throw()
    {
        // Arrange
        var subject = new Root
        {
            Text = "Root",
            Level = new Level1
            {
                Text = "Level1",
                Level = new Level2
                {
                    Text = "Mismatch"
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
                    Text = "Level2"
                }
            }
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected, config =>
            config.Excluding(ctx => ctx.Path == "Level.Level.Text"));

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_members_are_excluded_by_the_access_modifier_of_the_getter_using_a_predicate_they_should_be_ignored()
    {
        // Arrange
        var subject = new ClassWithAllAccessModifiersForMembers(
            "public",
            "protected",
            "internal",
            "protected-internal",
            "private",
            "private-protected");

        var expected = new ClassWithAllAccessModifiersForMembers(
            "public",
            "protected",
            "ignored-internal",
            "ignored-protected-internal",
            "private",
            "ignore-private-protected");

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected, config =>
            config.Excluding(ctx => ctx.WhichGetterHas(CSharpAccessModifier.Internal) ||
                                    ctx.WhichGetterHas(CSharpAccessModifier.ProtectedInternal) ||
                                    ctx.WhichGetterHas(CSharpAccessModifier.PrivateProtected)));

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_members_are_excluded_by_the_access_modifier_of_the_setter_using_a_predicate_they_should_be_ignored()
    {
        // Arrange
        var subject = new ClassWithAllAccessModifiersForMembers(
            "public",
            "protected",
            "internal",
            "protected-internal",
            "private",
            "private-protected");

        var expected = new ClassWithAllAccessModifiersForMembers(
            "public",
            "protected",
            "ignored-internal",
            "ignored-protected-internal",
            "ignored-private",
            "ignore-private-protected");

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected, config =>
            config.Excluding(ctx => ctx.WhichSetterHas(CSharpAccessModifier.Internal) ||
                                    ctx.WhichSetterHas(CSharpAccessModifier.ProtectedInternal) ||
                                    ctx.WhichSetterHas(CSharpAccessModifier.Private) ||
                                    ctx.WhichSetterHas(CSharpAccessModifier.PrivateProtected)));

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_the_expected_object_has_a_property_not_available_on_the_subject_it_should_throw()
    {
        // Arrange
        var subject = new
        {
        };

        var other = new
        {
            // ReSharper disable once StringLiteralTypo
            City = "Rijswijk"
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(other);

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expectation has property subject.City that the other object does not have*");
    }

    [Fact]
    public void When_equally_named_properties_are_type_incompatible_it_should_throw()
    {
        // Arrange
        var subject = new
        {
            Type = "A"
        };

        var other = new
        {
            Type = 36
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(other);

        // Assert
        act
            .Should().Throw<XunitException>()
            .WithMessage("Expected property subject.Type to be 36, but found*\"A\"*");
    }

    [Fact]
    public void When_multiple_properties_mismatch_it_should_report_all_of_them()
    {
        // Arrange
        var subject = new
        {
            Property1 = "A",
            Property2 = "B",
            SubType1 = new
            {
                SubProperty1 = "C",
                SubProperty2 = "D"
            }
        };

        var other = new
        {
            Property1 = "1",
            Property2 = "2",
            SubType1 = new
            {
                SubProperty1 = "3",
                SubProperty2 = "D"
            }
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(other);

        // Assert
        act
            .Should().Throw<XunitException>()
            .WithMessage("*property subject.Property1*to be \"1\", but \"A\" differs near \"A\"*")
            .WithMessage("*property subject.Property2*to be \"2\", but \"B\" differs near \"B\"*")
            .WithMessage("*property subject.SubType1.SubProperty1*to be \"3\", but \"C\" differs near \"C\"*");
    }

    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public void When_excluding_properties_it_should_still_compare_fields()
    {
        // Arrange
        var class1 = new ClassWithSomeFieldsAndProperties
        {
            Field1 = "Lorem",
            Field2 = "ipsum",
            Field3 = "dolor",
            Property1 = "sit",
            Property2 = "amet",
            Property3 = "consectetur"
        };

        var class2 = new ClassWithSomeFieldsAndProperties { Field1 = "Lorem", Field2 = "ipsum", Field3 = "color" };

        // Act
        Action act =
            () => class1.Should().BeEquivalentTo(class2, opts => opts.ExcludingProperties());

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*color*dolor*");
    }

    [Fact]
    public void When_excluding_properties_via_non_array_indexers_it_should_exclude_the_specified_paths()
    {
        // Arrange
        var subject = new
        {
            List = new[] { new { Foo = 1, Bar = 2 }, new { Foo = 3, Bar = 4 } }.ToList(),
            Dictionary = new Dictionary<string, ClassWithOnlyAProperty>
            {
                ["Foo"] = new ClassWithOnlyAProperty { Value = 1 },
                ["Bar"] = new ClassWithOnlyAProperty { Value = 2 }
            }
        };

        var expected = new
        {
            List = new[] { new { Foo = 1, Bar = 2 }, new { Foo = 2, Bar = 4 } }.ToList(),
            Dictionary = new Dictionary<string, ClassWithOnlyAProperty>
            {
                ["Foo"] = new ClassWithOnlyAProperty { Value = 1 },
                ["Bar"] = new ClassWithOnlyAProperty { Value = 3 }
            }
        };

        // Act
        Action act = () =>
            subject.Should().BeEquivalentTo(expected,
                options => options
                    .Excluding(x => x.List[1].Foo)
                    .Excluding(x => x.Dictionary["Bar"].Value));

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_excluding_properties_via_non_array_indexers_it_should_not_exclude_paths_with_different_indexes()
    {
        // Arrange
        var subject = new
        {
            List = new[] { new { Foo = 1, Bar = 2 }, new { Foo = 3, Bar = 4 } }.ToList(),
            Dictionary = new Dictionary<string, ClassWithOnlyAProperty>
            {
                ["Foo"] = new ClassWithOnlyAProperty { Value = 1 },
                ["Bar"] = new ClassWithOnlyAProperty { Value = 2 }
            }
        };

        var expected = new
        {
            List = new[] { new { Foo = 5, Bar = 2 }, new { Foo = 2, Bar = 4 } }.ToList(),
            Dictionary = new Dictionary<string, ClassWithOnlyAProperty>
            {
                ["Foo"] = new ClassWithOnlyAProperty { Value = 6 },
                ["Bar"] = new ClassWithOnlyAProperty { Value = 3 }
            }
        };

        // Act
        Action act = () =>
            subject.Should().BeEquivalentTo(expected,
                options => options
                    .Excluding(x => x.List[1].Foo)
                    .Excluding(x => x.Dictionary["Bar"].Value));

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public void When_configured_for_runtime_typing_and_properties_are_excluded_the_runtime_type_should_be_used_and_properties_should_be_ignored()
    {
        // Arrange
        object class1 = new ClassWithSomeFieldsAndProperties
        {
            Field1 = "Lorem",
            Field2 = "ipsum",
            Field3 = "dolor",
            Property1 = "sit",
            Property2 = "amet",
            Property3 = "consectetur"
        };

        object class2 = new ClassWithSomeFieldsAndProperties { Field1 = "Lorem", Field2 = "ipsum", Field3 = "dolor" };

        // Act
        Action act =
            () => class1.Should().BeEquivalentTo(class2, opts => opts.ExcludingProperties().RespectingRuntimeTypes());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public void When_using_IncludingAllDeclaredProperties_fields_should_be_ignored()
    {
        // Arrange
        var class1 = new ClassWithSomeFieldsAndProperties
        {
            Field1 = "Lorem",
            Field2 = "ipsum",
            Field3 = "dolor",
            Property1 = "sit",
            Property2 = "amet",
            Property3 = "consectetur"
        };

        var class2 = new ClassWithSomeFieldsAndProperties
        {
            Property1 = "sit",
            Property2 = "amet",
            Property3 = "consectetur"
        };

        // Act
        Action act =
            () => class1.Should().BeEquivalentTo(class2, opts => opts.IncludingAllDeclaredProperties());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public void When_using_IncludingAllRuntimeProperties_the_runtime_type_should_be_used_and_fields_should_be_ignored()
    {
        // Arrange
        object class1 = new ClassWithSomeFieldsAndProperties
        {
            Field1 = "Lorem",
            Field2 = "ipsum",
            Field3 = "dolor",
            Property1 = "sit",
            Property2 = "amet",
            Property3 = "consectetur"
        };

        object class2 = new ClassWithSomeFieldsAndProperties
        {
            Property1 = "sit",
            Property2 = "amet",
            Property3 = "consectetur"
        };

        // Act
        Action act =
            () => class1.Should().BeEquivalentTo(class2, opts => opts.IncludingAllRuntimeProperties());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public void When_both_field_and_properties_are_configured_for_inclusion_both_should_be_included()
    {
        // Arrange
        var class1 = new ClassWithSomeFieldsAndProperties
        {
            Field1 = "Lorem",
            Property1 = "sit"
        };

        var class2 = new ClassWithSomeFieldsAndProperties();

        // Act
        Action act =
            () => class1.Should().BeEquivalentTo(class2, opts => opts.IncludingFields().IncludingProperties());

        // Assert
        act.Should().Throw<XunitException>().Which.Message.Should().Contain("Field1").And.Contain("Property1");
    }

    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public void When_respecting_the_runtime_type_is_configured_the_runtime_type_should_be_used_and_both_properties_and_fields_included()
    {
        // Arrange
        object class1 = new ClassWithSomeFieldsAndProperties
        {
            Field1 = "Lorem",
            Property1 = "sit"
        };

        object class2 = new ClassWithSomeFieldsAndProperties();

        // Act
        Action act =
            () => class1.Should().BeEquivalentTo(class2, opts => opts.RespectingRuntimeTypes());

        // Assert
        act.Should().Throw<XunitException>().Which.Message.Should().Contain("Field1").And.Contain("Property1");
    }

    [Fact]
    public void When_excluding_virtual_or_abstract_property_exclusion_works_properly()
    {
        var obj1 = new Derived { DerivedProperty1 = "Something", DerivedProperty2 = "A" };
        var obj2 = new Derived { DerivedProperty1 = "Something", DerivedProperty2 = "B" };

        obj1.Should().BeEquivalentTo(obj2, opt => opt
            .Excluding(o => o.AbstractProperty)
            .Excluding(o => o.VirtualProperty)
            .Excluding(o => o.DerivedProperty2));
    }

    [Fact]
    public void A_nested_class_without_properties_inside_a_collection_is_fine()
    {
        // Arrange
        var sut = new List<BaseClassPointingToClassWithoutProperties>
        {
            new()
            {
                Name = "theName"
            }
        };

        // Act / Assert
        sut.Should().BeEquivalentTo(new[]
        {
            new BaseClassPointingToClassWithoutProperties
            {
                Name = "theName"
            }
        });
    }

    internal class BaseClassPointingToClassWithoutProperties
    {
        public string Name { get; set; }

        public ClassWithoutProperty ClassWithoutProperty { get; } = new ClassWithoutProperty();
    }

    internal class ClassWithoutProperty
    {
    }

#if NET5_0_OR_GREATER

    [Fact]
    public void Excluding_a_covariant_property_should_work()
    {
        // Arrange
        var actual = new DerivedWithCovariantOverride(new DerivedWithProperty { DerivedProperty = "a", BaseProperty = "a_base" })
        {
            OtherProp = "other"
        };

        var expectation = new DerivedWithCovariantOverride(new DerivedWithProperty { DerivedProperty = "b", BaseProperty = "b_base" })
        {
            OtherProp = "other"
        };

        // Act / Assert
        actual.Should().BeEquivalentTo(expectation, opts => opts
            .Excluding(d => d.Property));
    }

    [Fact]
    public void Excluding_a_covariant_property_through_the_base_class_excludes_the_base_class_property()
    {
        // Arrange
        var actual = new DerivedWithCovariantOverride(new DerivedWithProperty { DerivedProperty = "a", BaseProperty = "a_base" })
        {
            OtherProp = "other"
        };

        BaseWithAbstractProperty expectation = new DerivedWithCovariantOverride(new DerivedWithProperty { DerivedProperty = "b", BaseProperty = "b_base" })
        {
            OtherProp = "other"
        };

        // Act
        Action act = () => actual.Should().BeEquivalentTo(expectation, opts => opts
            .Excluding(d => d.Property));

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("No members*");
    }

    private class BaseWithProperty
    {
        public string BaseProperty { get; set; }
    }

    private class DerivedWithProperty : BaseWithProperty
    {
        public string DerivedProperty { get; set; }
    }

    private abstract class BaseWithAbstractProperty
    {
        public abstract BaseWithProperty Property { get; }
    }

    private sealed class DerivedWithCovariantOverride : BaseWithAbstractProperty
    {
        public override DerivedWithProperty Property { get; }

        public string OtherProp { get; set; }

        public DerivedWithCovariantOverride(DerivedWithProperty prop)
        {
            Property = prop;
        }
    }

#endif

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

    public class DerivedClassImplementingInterface : BaseProvidingSamePropertiesAsInterface, IInterfaceWithTwoProperties
    {
    }

    [Fact]
    public void Excluding_an_interface_property_through_inheritance_should_work()
    {
        // Arrange
        var actual = new IInterfaceWithTwoProperties[]
        {
            new DerivedClassImplementingInterface { Value1 = 1, Value2 = 2 }
        };

        var expected = new IInterfaceWithTwoProperties[]
        {
            new DerivedClassImplementingInterface { Value1 = 999, Value2 = 2 }
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
        var actual = new IInterfaceWithTwoProperties[]
        {
            new DerivedClassImplementingInterface { Value1 = 1, Value2 = 2 }
        };

        var expected = new IInterfaceWithTwoProperties[]
        {
            new DerivedClassImplementingInterface { Value1 = 999, Value2 = 2 }
        };

        // Act / Assert
        actual.Should().BeEquivalentTo(expected, options => options
            .Including(a => a.Value2)
            .RespectingRuntimeTypes());
    }

    [Fact]
    public void When_browsable_field_differs_excluding_non_browsable_members_should_not_affect_result()
    {
        // Arrange
        var subject = new ClassWithNonBrowsableMembers() { BrowsableField = 0 };
        var expectation = new ClassWithNonBrowsableMembers() { BrowsableField = 1 };

        // Act
        Action action =
            () => subject.Should().BeEquivalentTo(expectation, config => config.ExcludingNonBrowsableMembers());

        // Assert
        action.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_browsable_property_differs_excluding_non_browsable_members_should_not_affect_result()
    {
        // Arrange
        var subject = new ClassWithNonBrowsableMembers() { BrowsableProperty = 0 };
        var expectation = new ClassWithNonBrowsableMembers() { BrowsableProperty = 1 };

        // Act
        Action action =
            () => subject.Should().BeEquivalentTo(expectation, config => config.ExcludingNonBrowsableMembers());

        // Assert
        action.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_advanced_browsable_field_differs_excluding_non_browsable_members_should_not_affect_result()
    {
        // Arrange
        var subject = new ClassWithNonBrowsableMembers() { AdvancedBrowsableField = 0 };
        var expectation = new ClassWithNonBrowsableMembers() { AdvancedBrowsableField = 1 };

        // Act
        Action action =
            () => subject.Should().BeEquivalentTo(expectation, config => config.ExcludingNonBrowsableMembers());

        // Assert
        action.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_advanced_browsable_property_differs_excluding_non_browsable_members_should_not_affect_result()
    {
        // Arrange
        var subject = new ClassWithNonBrowsableMembers() { AdvancedBrowsableProperty = 0 };
        var expectation = new ClassWithNonBrowsableMembers() { AdvancedBrowsableProperty = 1 };

        // Act
        Action action =
            () => subject.Should().BeEquivalentTo(expectation, config => config.ExcludingNonBrowsableMembers());

        // Assert
        action.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_explicitly_browsable_field_differs_excluding_non_browsable_members_should_not_affect_result()
    {
        // Arrange
        var subject = new ClassWithNonBrowsableMembers() { ExplicitlyBrowsableField = 0 };
        var expectation = new ClassWithNonBrowsableMembers() { ExplicitlyBrowsableField = 1 };

        // Act
        Action action =
            () => subject.Should().BeEquivalentTo(expectation, config => config.ExcludingNonBrowsableMembers());

        // Assert
        action.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_explicitly_browsable_property_differs_excluding_non_browsable_members_should_not_affect_result()
    {
        // Arrange
        var subject = new ClassWithNonBrowsableMembers() { ExplicitlyBrowsableProperty = 0 };
        var expectation = new ClassWithNonBrowsableMembers() { ExplicitlyBrowsableProperty = 1 };

        // Act
        Action action =
            () => subject.Should().BeEquivalentTo(expectation, config => config.ExcludingNonBrowsableMembers());

        // Assert
        action.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_non_browsable_field_differs_excluding_non_browsable_members_should_make_it_succeed()
    {
        // Arrange
        var subject = new ClassWithNonBrowsableMembers() { NonBrowsableField = 0 };
        var expectation = new ClassWithNonBrowsableMembers() { NonBrowsableField = 1 };

        // Act & Assert
        subject.Should().BeEquivalentTo(expectation, config => config.ExcludingNonBrowsableMembers());
    }

    [Fact]
    public void When_non_browsable_property_differs_excluding_non_browsable_members_should_make_it_succeed()
    {
        // Arrange
        var subject = new ClassWithNonBrowsableMembers() { NonBrowsableProperty = 0 };
        var expectation = new ClassWithNonBrowsableMembers() { NonBrowsableProperty = 1 };

        // Act & Assert
        subject.Should().BeEquivalentTo(expectation, config => config.ExcludingNonBrowsableMembers());
    }

    [Fact]
    public void When_property_is_non_browsable_only_in_subject_excluding_non_browsable_members_should_not_make_it_succeed()
    {
        // Arrange
        var subject = new ClassWhereMemberThatCouldBeNonBrowsableIsNonBrowsable() { PropertyThatMightBeNonBrowsable = 0 };
        var expectation = new ClassWhereMemberThatCouldBeNonBrowsableIsBrowsable() { PropertyThatMightBeNonBrowsable = 1 };

        // Act
        Action action =
            () => subject.Should().BeEquivalentTo(expectation, config => config.ExcludingNonBrowsableMembers());

        // Assert
        action.Should().Throw<XunitException>().WithMessage("Expected property subject.PropertyThatMightBeNonBrowsable to be 1, but found 0.*");
    }

    [Fact]
    public void When_property_is_non_browsable_only_in_subject_ignoring_non_browsable_members_on_subject_should_make_it_succeed()
    {
        // Arrange
        var subject = new ClassWhereMemberThatCouldBeNonBrowsableIsNonBrowsable() { PropertyThatMightBeNonBrowsable = 0 };
        var expectation = new ClassWhereMemberThatCouldBeNonBrowsableIsBrowsable() { PropertyThatMightBeNonBrowsable = 1 };

        // Act & Assert
        subject.Should().BeEquivalentTo(
            expectation,
            config => config.IgnoringNonBrowsableMembersOnSubject().ExcludingMissingMembers());
    }

    [Fact]
    public void When_non_browsable_property_on_subject_is_ignored_but_is_present_on_expectation_it_should_fail()
    {
        // Arrange
        var subject = new ClassWhereMemberThatCouldBeNonBrowsableIsNonBrowsable() { PropertyThatMightBeNonBrowsable = 0 };
        var expectation = new ClassWhereMemberThatCouldBeNonBrowsableIsBrowsable() { PropertyThatMightBeNonBrowsable = 1 };

        // Act
        Action action =
            () => subject.Should().BeEquivalentTo(expectation, config => config.IgnoringNonBrowsableMembersOnSubject());

        // Assert
        action.Should().Throw<XunitException>().WithMessage(
            $"Expectation has * subject.*ThatMightBeNonBrowsable that is non-browsable in the other object, and non-browsable " +
            $"members on the subject are ignored with the current configuration*");
    }

    [Fact]
    public void When_property_is_non_browsable_only_in_expectation_excluding_non_browsable_members_should_make_it_succeed()
    {
        // Arrange
        var subject = new ClassWhereMemberThatCouldBeNonBrowsableIsBrowsable() { PropertyThatMightBeNonBrowsable = 0 };
        var expectation = new ClassWhereMemberThatCouldBeNonBrowsableIsNonBrowsable() { PropertyThatMightBeNonBrowsable = 1 };

        // Act & Assert
        subject.Should().BeEquivalentTo(expectation, config => config.ExcludingNonBrowsableMembers());
    }

    [Fact]
    public void When_field_is_non_browsable_only_in_subject_excluding_non_browsable_members_should_not_make_it_succeed()
    {
        // Arrange
        var subject = new ClassWhereMemberThatCouldBeNonBrowsableIsNonBrowsable() { FieldThatMightBeNonBrowsable = 0 };
        var expectation = new ClassWhereMemberThatCouldBeNonBrowsableIsBrowsable() { FieldThatMightBeNonBrowsable = 1 };

        // Act
        Action action =
            () => subject.Should().BeEquivalentTo(expectation, config => config.ExcludingNonBrowsableMembers());

        // Assert
        action.Should().Throw<XunitException>().WithMessage("Expected field subject.FieldThatMightBeNonBrowsable to be 1, but found 0.*");
    }

    [Fact]
    public void When_field_is_non_browsable_only_in_subject_ignoring_non_browsable_members_on_subject_should_make_it_succeed()
    {
        // Arrange
        var subject = new ClassWhereMemberThatCouldBeNonBrowsableIsNonBrowsable() { FieldThatMightBeNonBrowsable = 0 };
        var expectation = new ClassWhereMemberThatCouldBeNonBrowsableIsBrowsable() { FieldThatMightBeNonBrowsable = 1 };

        // Act & Assert
        subject.Should().BeEquivalentTo(
            expectation,
            config => config.IgnoringNonBrowsableMembersOnSubject().ExcludingMissingMembers());
    }

    [Fact]
    public void When_field_is_non_browsable_only_in_expectation_excluding_non_browsable_members_should_make_it_succeed()
    {
        // Arrange
        var subject = new ClassWhereMemberThatCouldBeNonBrowsableIsBrowsable() { FieldThatMightBeNonBrowsable = 0 };
        var expectation = new ClassWhereMemberThatCouldBeNonBrowsableIsNonBrowsable() { FieldThatMightBeNonBrowsable = 1 };

        // Act & Assert
        subject.Should().BeEquivalentTo(expectation, config => config.ExcludingNonBrowsableMembers());
    }

    public class NonBrowsableOnOneButMissingFromTheOther
    {
        [Fact]
        public void When_property_is_missing_from_subject_excluding_non_browsable_members_should_make_it_succeed()
        {
            // Arrange
            var subject =
                new
                {
                    BrowsableField = 1,
                    BrowsableProperty = 1,
                    ExplicitlyBrowsableField = 1,
                    ExplicitlyBrowsableProperty = 1,
                    AdvancedBrowsableField = 1,
                    AdvancedBrowsableProperty = 1,
                    NonBrowsableField = 2,
                    /* NonBrowsableProperty missing */
                };

            var expected =
                    new ClassWithNonBrowsableMembers
                    {
                        BrowsableField = 1,
                        BrowsableProperty = 1,
                        ExplicitlyBrowsableField = 1,
                        ExplicitlyBrowsableProperty = 1,
                        AdvancedBrowsableField = 1,
                        AdvancedBrowsableProperty = 1,
                        NonBrowsableField = 2,
                        NonBrowsableProperty = 2,
                    };

            // Act & Assert
            subject.Should().BeEquivalentTo(expected, opt => opt.ExcludingNonBrowsableMembers());
        }

        [Fact]
        public void When_field_is_missing_from_subject_excluding_non_browsable_members_should_make_it_succeed()
        {
            // Arrange
            var subject =
                new
                {
                    BrowsableField = 1,
                    BrowsableProperty = 1,
                    ExplicitlyBrowsableField = 1,
                    ExplicitlyBrowsableProperty = 1,
                    AdvancedBrowsableField = 1,
                    AdvancedBrowsableProperty = 1,
                    /* NonBrowsableField missing */
                    NonBrowsableProperty = 2,
                };

            var expected =
                    new ClassWithNonBrowsableMembers
                    {
                        BrowsableField = 1,
                        BrowsableProperty = 1,
                        ExplicitlyBrowsableField = 1,
                        ExplicitlyBrowsableProperty = 1,
                        AdvancedBrowsableField = 1,
                        AdvancedBrowsableProperty = 1,
                        NonBrowsableField = 2,
                        NonBrowsableProperty = 2,
                    };

            // Act & Assert
            subject.Should().BeEquivalentTo(expected, opt => opt.ExcludingNonBrowsableMembers());
        }

        [Fact]
        public void When_property_is_missing_from_expectation_excluding_non_browsable_members_should_make_it_succeed()
        {
            // Arrange
            var subject =
                    new ClassWithNonBrowsableMembers
                    {
                        BrowsableField = 1,
                        BrowsableProperty = 1,
                        ExplicitlyBrowsableField = 1,
                        ExplicitlyBrowsableProperty = 1,
                        AdvancedBrowsableField = 1,
                        AdvancedBrowsableProperty = 1,
                        NonBrowsableField = 2,
                        NonBrowsableProperty = 2,
                    };

            var expected =
                new
                {
                    BrowsableField = 1,
                    BrowsableProperty = 1,
                    ExplicitlyBrowsableField = 1,
                    ExplicitlyBrowsableProperty = 1,
                    AdvancedBrowsableField = 1,
                    AdvancedBrowsableProperty = 1,
                    NonBrowsableField = 2,
                    /* NonBrowsableProperty missing */
                };

            // Act & Assert
            subject.Should().BeEquivalentTo(expected, opt => opt.ExcludingNonBrowsableMembers());
        }

        [Fact]
        public void When_field_is_missing_from_expectation_excluding_non_browsable_members_should_make_it_succeed()
        {
            // Arrange
            var subject =
                    new ClassWithNonBrowsableMembers
                    {
                        BrowsableField = 1,
                        BrowsableProperty = 1,
                        ExplicitlyBrowsableField = 1,
                        ExplicitlyBrowsableProperty = 1,
                        AdvancedBrowsableField = 1,
                        AdvancedBrowsableProperty = 1,
                        NonBrowsableField = 2,
                        NonBrowsableProperty = 2,
                    };

            var expected =
                new
                {
                    BrowsableField = 1,
                    BrowsableProperty = 1,
                    ExplicitlyBrowsableField = 1,
                    ExplicitlyBrowsableProperty = 1,
                    AdvancedBrowsableField = 1,
                    AdvancedBrowsableProperty = 1,
                    /* NonBrowsableField missing */
                    NonBrowsableProperty = 2,
                };

            // Act & Assert
            subject.Should().BeEquivalentTo(expected, opt => opt.ExcludingNonBrowsableMembers());
        }

        [Fact]
        public void When_non_browsable_members_are_excluded_it_should_still_be_possible_to_explicitly_include_non_browsable_field()
        {
            // Arrange
            var subject =
                new ClassWithNonBrowsableMembers()
                {
                    NonBrowsableField = 1,
                };

            var expectation =
                new ClassWithNonBrowsableMembers()
                {
                    NonBrowsableField = 2,
                };

            // Act
            Action action =
                () => subject.Should().BeEquivalentTo(
                    expectation,
                    opt => opt.IncludingFields().ExcludingNonBrowsableMembers().Including(e => e.NonBrowsableField));

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected field subject.NonBrowsableField to be 2, but found 1.*");
        }

        [Fact]
        public void When_non_browsable_members_are_excluded_it_should_still_be_possible_to_explicitly_include_non_browsable_property()
        {
            // Arrange
            var subject =
                new ClassWithNonBrowsableMembers()
                {
                    NonBrowsableProperty = 1,
                };

            var expectation =
                new ClassWithNonBrowsableMembers()
                {
                    NonBrowsableProperty = 2,
                };

            // Act
            Action action =
                () => subject.Should().BeEquivalentTo(
                    expectation,
                    opt => opt.IncludingProperties().ExcludingNonBrowsableMembers().Including(e => e.NonBrowsableProperty));

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected property subject.NonBrowsableProperty to be 2, but found 1.*");
        }
    }

    private class ClassWithNonBrowsableMembers
    {
        public int BrowsableField = -1;

        public int BrowsableProperty { get; set; }

        [EditorBrowsable(EditorBrowsableState.Always)]
        public int ExplicitlyBrowsableField = -1;

        [EditorBrowsable(EditorBrowsableState.Always)]
        public int ExplicitlyBrowsableProperty { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int AdvancedBrowsableField = -1;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int AdvancedBrowsableProperty { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public int NonBrowsableField = -1;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public int NonBrowsableProperty { get; set; }
    }

    private class ClassWhereMemberThatCouldBeNonBrowsableIsBrowsable
    {
        public int BrowsableField = -1;

        public int BrowsableProperty { get; set; }

        public int FieldThatMightBeNonBrowsable = -1;

        public int PropertyThatMightBeNonBrowsable { get; set; }
    }

    private class ClassWhereMemberThatCouldBeNonBrowsableIsNonBrowsable
    {
        public int BrowsableField = -1;

        public int BrowsableProperty { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public int FieldThatMightBeNonBrowsable = -1;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public int PropertyThatMightBeNonBrowsable { get; set; }
    }
}
