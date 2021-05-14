using System;
using System.Reflection;
using AssemblyA;
using AssemblyB;
using FluentAssertions.Specs.Types;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Specialized
{
    public class AssemblyAssertionSpecs
    {
        #region NotReference

        [Fact]
        public void When_an_assembly_is_not_referenced_and_should_not_reference_is_asserted_it_should_succeed()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyB.Should().NotReference(assemblyA);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_assembly_is_not_referenced_it_should_allow_chaining()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyB.Should().NotReference(assemblyA)
                .And.NotBeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_assembly_is_referenced_and_should_not_reference_is_asserted_it_should_fail()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyA.Should().NotReference(assemblyB);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_subject_is_null_not_reference_should_fail()
        {
            // Arrange
            Assembly assemblyA = null;
            Assembly assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyA.Should().NotReference(assemblyB, "we want to to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected assembly not to reference assembly \"AssemblyB\" *failure message*, but assemblyA is <null>.");
        }

        #endregion

        #region Reference

        [Fact]
        public void When_an_assembly_is_referenced_and_should_reference_is_asserted_it_should_succeed()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyA.Should().Reference(assemblyB);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_assembly_is_referenced_it_should_allow_chaining()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyA.Should().Reference(assemblyB)
                .And.NotBeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_assembly_is_not_referenced_and_should_reference_is_asserted_it_should_fail()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyB.Should().Reference(assemblyA);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_subject_is_null_reference_should_fail()
        {
            // Arrange
            Assembly assemblyA = null;
            Assembly assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyA.Should().Reference(assemblyB, "we want to to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected assembly to reference assembly \"AssemblyB\" *failure message*, but assemblyA is <null>.");
        }

        #endregion

        #region DefineType

        [Fact]
        public void When_an_assembly_defines_a_type_and_Should_DefineType_is_asserted_it_should_succeed()
        {
            // Arrange
            var thisAssembly = GetType().Assembly;

            // Act
            Action act = () => thisAssembly
                .Should().DefineType(GetType().Namespace, typeof(WellKnownClassWithAttribute).Name)
                .Which.Should().BeDecoratedWith<DummyClassAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_assembly_does_not_define_a_type_and_Should_DefineType_is_asserted_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var thisAssembly = GetType().Assembly;

            // Act
            Action act = () => thisAssembly.Should().DefineType("FakeNamespace", "FakeName",
                "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"Expected assembly \"{thisAssembly.FullName}\" " +
                             "to define type \"FakeNamespace\".\"FakeName\" " +
                             "because we want to test the failure message, but it does not.");
        }

        [Fact]
        public void When_subject_is_null_define_type_should_fail()
        {
            // Arrange
            Assembly thisAssembly = null;

            // Act
            Action act = () =>
                thisAssembly.Should().DefineType(GetType().Namespace, "WellKnownClassWithAttribute",
                    "we want to to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected assembly to define type *.\"WellKnownClassWithAttribute\" *failure message*" +
                    ", but thisAssembly is <null>.");
        }

        #endregion

        #region BeNull

        [Fact]
        public void When_an_assembly_is_null_and_Should_BeNull_is_asserted_it_should_succeed()
        {
            // Arrange
            Assembly thisAssembly = null;

            // Act
            Action act = () => thisAssembly
                .Should().BeNull();

            // Assert
            act.Should().NotThrow();
        }

        #endregion
    }

    [DummyClass("name", true)]
    public class WellKnownClassWithAttribute
    {
    }
}
