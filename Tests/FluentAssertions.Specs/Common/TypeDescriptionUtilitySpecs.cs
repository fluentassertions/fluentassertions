using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Common;

using Xunit;

namespace FluentAssertions.Specs
{
    public class TypeDescriptionUtilitySpecs
    {
        [Fact]
        public void When_object_is_null_it_should_work()
        {
            // Act & Assert
            TypeDescriptionUtility.GetDescriptionOfObjectType(null).Should().Be("<null>");
        }

        [Fact]
        public void When_object_is_value_type_it_should_work()
        {
            // Act & Assert
            TypeDescriptionUtility.GetDescriptionOfObjectType(37).Should().Be("a value of type System.Int32");
        }

        [Fact]
        public void When_object_is_reference_type_it_should_work()
        {
            // Act & Assert
            TypeDescriptionUtility.GetDescriptionOfObjectType(new object()).Should().Be("an instance of System.Object");
        }

        [Fact]
        public void When_object_is_generic_value_type_it_should_work()
        {
            // Arrange
            var box = new Box<int>() { Value = 37 };

            // Act & Assert
            TypeDescriptionUtility.GetDescriptionOfObjectType(box).Should()
                .Match("a value of type *+Box`1[[System.Int32*]]");
        }

        [Fact]
        public void When_object_is_generic_reference_type_it_should_work()
        {
            // Act & Assert
            TypeDescriptionUtility.GetDescriptionOfObjectType(new List<int>()).Should()
                .Match("an instance of System.Collections.Generic.List`1[[System.Int32*]]");
        }

        [Fact]
        public void When_object_is_LINQ_anonymous_iterator_type_it_should_work()
        {
            // Arrange
            var value = new[] { 1, 2, 3 }.Select(x => 2 * x);

            TypeDescriptionUtility.GetDescriptionOfObjectType(value).Should().Be(
                "an anonymous iterator from a LINQ expression with element type System.Int32");
        }

        private struct Box<T>
        {
            public T Value { get; set; }
        }
    }
}
