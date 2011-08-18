using FluentAssertions.Formatting;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class DefaultValueFormatterSpecs
    {
        [TestMethod]
        public void When_value_contains_cyclic_reference_it_should_create_descriptive_error_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var parent = new Parent();
            var child = new Child();
            parent.Child = child;
            child.Parent = parent;

            var formatter = new DefaultValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            string result = formatter.ToString(parent);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().ContainEquivalentOf("cyclic reference");
        }
    }

    public class Parent
    {
        public Child Child { get; set; }
    }

    public class Child
    {
        public Parent Parent { get; set; }
    }
}