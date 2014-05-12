using System;
using AssemblyA;
using AssemblyB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class AssemblyAssertionSpecs
    {
        [TestMethod]
        public void Should_succeed_when_asserting_an_assembly_is_not_referenced()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyB.Should().NotReference(assemblyA);

            // Assert
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_fail_when_asserting_an_assembly_is_not_referenced()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();
            
            // Act
            Action act = () => assemblyA.Should().NotReference(assemblyB);

            // Assert
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void Should_succeed_when_asserting_an_assembly_is_referenced()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();

            // Act
            Action act = () => assemblyA.Should().Reference(assemblyB);

            // Assert
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_fail_when_asserting_an_assembly_is_referenced()
        {
            // Arrange
            var assemblyA = FindAssembly.Containing<ClassA>();
            var assemblyB = FindAssembly.Containing<ClassB>();
            
            // Act
            Action act = () => assemblyB.Should().Reference(assemblyA);

            // Assert
            act.ShouldThrow<AssertFailedException>();
        }
    }
}
