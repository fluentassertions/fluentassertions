#if NET6_0_OR_GREATER
using Xunit;

namespace FluentAssertions.Specs.Common;

public partial class TypeExtensionsSpecs
{
    public class GetProperties
    {
        [Fact]
        public void Test()
        {
            // Act
            var properties = typeof(SuperClass).GetProperties();

            // Assert
            properties.Should().HaveCount(4);
        }

        private class SuperClass : BaseClass, IInterfaceWithDefaultProperty
        {
            public string NormalProperty { get; set; }

            public new int NewProperty { get; set; }

            public string BarProperty { get; set; }

            public string FooProperty { get; set; }
        }

        private class BaseClass
        {
            public string NewProperty { get; set; }
        }

        private interface IInterfaceWithDefaultProperty : IInterfaceWithSingleProperty
        {
            string FooProperty { get; set; }

            string DefaultProperty => "Default";
        }

        private interface IInterfaceWithSingleProperty
        {
            string BarProperty { get; set; }
        }
    }
}
#endif
