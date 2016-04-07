using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace FluentAssertions.Json
{
    [TestClass]
    public class ObjectDiffPatchSpecs
    {
        [TestMethod]
        public void Should_Generate_Diffs_for_POCOs_and_JObjects()
        {
            var sut = ObjectDiffPatch.GenerateDiff<Dummy>(null, null);
            sut.AreEqual.Should().BeTrue();
            sut.OldValues.Should().BeNull();
            sut.NewValues.Should().BeNull();

            var a = new Dummy { Id = "foo" };
            var ja = JObject.FromObject(a);

            sut = ObjectDiffPatch.GenerateDiff(a, null);
            sut.AreEqual.Should().BeFalse();
            sut.OldValues.Should().Be(ja);
            sut.NewValues.Should().BeNull();

            sut = ObjectDiffPatch.GenerateDiff(null, a);
            sut.AreEqual.Should().BeFalse();
            sut.OldValues.Should().BeNull();
            sut.NewValues.Should().Be(ja);

            var b = new Dummy { Id = "bar" };
            var jb = JObject.FromObject(b);
            sut = ObjectDiffPatch.GenerateDiff(a, b);
            sut.AreEqual.Should().BeFalse();
            JToken.DeepEquals(sut.OldValues, ja).Should().BeTrue();
            JToken.DeepEquals(sut.NewValues, jb).Should().BeTrue();

            // now for JObjects
            sut = ObjectDiffPatch.GenerateDiff(ja, jb);
            sut.AreEqual.Should().BeFalse();
            JToken.DeepEquals(sut.OldValues, ja).Should().BeTrue();
            JToken.DeepEquals(sut.NewValues, jb).Should().BeTrue();
        }

        [TestMethod]
        [Description("coverage")]
        public void Test_DiffField_handling_nulls()
        {
            var token = JToken.Parse("{\"field\":\"value\"}");

            var result = new ObjectDiffPatchResult();
            ObjectDiffPatch.DiffField("field", null, token, result);
            result.NewValues["field"].Should().Be(token);
            result.NewValues["field"].HasValues.Should().BeTrue();
            result.OldValues["field"].HasValues.Should().BeFalse();

            result = new ObjectDiffPatchResult();
            ObjectDiffPatch.DiffField("field", token, null, result);
            result.OldValues["field"].Should().Be(token);
            result.OldValues["field"].HasValues.Should().BeTrue();
            result.NewValues["field"].HasValues.Should().BeFalse();
        }

        private class Dummy
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Id { get; set; }
        }
    }
}