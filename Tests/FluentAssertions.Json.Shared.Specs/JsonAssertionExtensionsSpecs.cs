using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace FluentAssertions.Json
{
    [TestClass]
    // ReSharper disable InconsistentNaming
    public class JsonAssertionExtensionsSpecs
    {
        [TestMethod]
        public void Should_Provide_JTokenAssertions()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var assertions = new[]
            {
                JToken.Parse("{\"token\":\"value\"}").Should()
                , JObject.Parse("{\"object\":\"value\"}").Should()
                // ReSharper disable once AccessToStaticMemberViaDerivedType
                , JProperty.Parse("{\"property\":\"value\"}").Should()
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act & Assert
            //-----------------------------------------------------------------------------------------------------------
            foreach(var sut in assertions)
                sut.Should().BeOfType<JTokenAssertions>("extensions should provide assertions for all JSon primitives, i.e. JObject, JToken and JProperty");
        }
    }
}