using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using FluentAssertions;
using FluentAssertions.Json;

// NOTE that we are using both namespaces 'FluentAssertions' & 'FluentAssertions.Json' from an external namespace to force compiler disambiguation warnings
namespace SomeOtherNamespace
{
    [TestClass]
    // ReSharper disable InconsistentNaming
    public class JsonAssertionExtensionsSpecs
    {
        [TestMethod]
        public void Should_Provide_Unambiguos_JTokenAssertions()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assertions = new []
            {
                JToken.Parse("{\"token\":\"value\"}").Should()
                , new JProperty("property","value").Should()
                , new JObject(new JProperty("object", "value")).Should()
                , new JArray(new [] { 42, 43}).Should()
                , new JConstructor("property","value").Should()
                , new JValue("value").Should()
                , new JRaw("value").Should()
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act & Assert
            //-----------------------------------------------------------------------------------------------------------
            foreach(var sut in assertions)
                sut.Should().BeOfType<JTokenAssertions>("extensions should provide assertions for all JSon primitives, i.e. JObject, JToken and JProperty");
        }
    }
}