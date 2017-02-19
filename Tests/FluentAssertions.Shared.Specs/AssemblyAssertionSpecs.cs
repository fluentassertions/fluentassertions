using System;
using System.Reflection;
using Xunit;
using Xunit.Sdk;
#if !PORTABLE && !SILVERLIGHT && !NETFX_CORE && !WINRT
using AssemblyA;
using AssemblyB;
#endif

namespace FluentAssertions.Specs
{
    
    public class AssemblyAssertionSpecs
    {
#if !PORTABLE && !SILVERLIGHT && !NETFX_CORE && !WINRT
        [Fact]
        public void When_an_assembly_is_not_referenced_and_should_not_reference_is_asserted_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => assemblyB.Should().NotReference(assemblyA);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_an_assembly_is_referenced_and_should_not_reference_is_asserted_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => assemblyA.Should().NotReference(assemblyB);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>();
        }

        [Fact]
        public void When_an_assembly_is_referenced_and_should_reference_is_asserted_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => assemblyA.Should().Reference(assemblyB);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_an_assembly_is_not_referenced_and_should_reference_is_asserted_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => assemblyB.Should().Reference(assemblyA);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>();
        }
#endif

        [Fact]
        public void When_an_assembly_defines_a_type_and_Should_DefineType_is_asserted_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
#if !NETFX_CORE && !WINRT
            var thisAssembly = GetType().Assembly;
#else
            var thisAssembly = GetType().GetTypeInfo().Assembly;
#endif

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => thisAssembly
                .Should().DefineType(GetType().Namespace, typeof(WellKnownClassWithAttribute).Name)
                .Which.Should().BeDecoratedWith<SerializableAttribute>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_an_assembly_does_not_define_a_type_and_Should_DefineType_is_asserted_it_should_fail_with_a_useful_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
#if !NETFX_CORE && !WINRT
            var thisAssembly = GetType().Assembly;
#else
            var thisAssembly = GetType().GetTypeInfo().Assembly;
#endif

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => thisAssembly.Should().DefineType("FakeNamespace", "FakeName");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage(String.Format("Expected assembly \"{0}\" " +
                             "to define type \"FakeNamespace\".\"FakeName\", but it does not.", thisAssembly.FullName));
        }
        
        [Fact]
        public void When_an_assembly_is_null_and_Should_BeNull_is_asserted_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Assembly thisAssembly = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => thisAssembly
                .Should().BeNull();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }
    }

    [Serializable]
    public class WellKnownClassWithAttribute
    {
        
    }
}
