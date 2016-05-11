using System.Collections.Generic;
using FluentAssertions.Equivalency;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace FluentAssertions.Json
{
    [TestClass]
    // ReSharper disable InconsistentNaming
    public class JTokenAssertionsSpecs
    {
        [TestMethod]
        public void When_both_values_are_the_same_or_equal_Be_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var a = JToken.Parse("{\"id\":1}");
            var b = JToken.Parse("{\"id\":1}");

            //-----------------------------------------------------------------------------------------------------------
            // Act & Assert
            //-----------------------------------------------------------------------------------------------------------
            a.Should().Be(a);
            b.Should().Be(b);
            a.Should().Be(b);
        }

        [TestMethod]
        public void When_values_differ_NotBe_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var a = JToken.Parse("{\"id\":1}");
            var b = JToken.Parse("{\"id\":2}");

            //-----------------------------------------------------------------------------------------------------------
            // Act & Assert
            //-----------------------------------------------------------------------------------------------------------
            a.Should().NotBeNull();
            a.Should().NotBe(null);
            a.Should().NotBe(b);
        }

        [TestMethod]
        public void When_values_are_equal_or_equivalent_NotBe_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var a = JToken.Parse("{\"id\":1}");
            var b = JToken.Parse("{\"id\":1}");

            //-----------------------------------------------------------------------------------------------------------
            // Act & Assert
            //-----------------------------------------------------------------------------------------------------------
            a.Invoking(x => x.Should().NotBe(b))
                .ShouldThrow<AssertFailedException>()
                .WithMessage($"Expected JSON document not to be {_formatter.ToString(b)}.");
        }


        [TestMethod]
        public void When_both_values_are_equal_BeEquivalentTo_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var testCases = new Dictionary<string, string>
            {
                { "{friends:[{id:123,name:\"Corby Page\"},{id:456,name:\"Carter Page\"}]}", "{friends:[{name:\"Corby Page\",id:123},{id:456,name:\"Carter Page\"}]}" },
                { "{id:2,admin:true}", "{admin:true,id:2}" }
            };

            foreach (var testCase in testCases)
            {
                var actualJson = testCase.Key;
                var expectedJson = testCase.Value;
                var a = JToken.Parse(actualJson);
                var b = JToken.Parse(expectedJson);

                //-----------------------------------------------------------------------------------------------------------
                // Act & Assert
                //-----------------------------------------------------------------------------------------------------------
                a.Should().BeEquivalentTo(b);
            }
        }

        [TestMethod]
        public void When_values_differ_NotBeEquivalentTo_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var testCases = new Dictionary<string, string>
            {
                { "{id:1,admin:true}", "{id:1,admin:false}" }
            };

            foreach (var testCase in testCases)
            {
                var actualJson = testCase.Key;
                var expectedJson = testCase.Value;

                var a = JToken.Parse(actualJson);
                var b = JToken.Parse(expectedJson);

                //-----------------------------------------------------------------------------------------------------------
                // Act & Assert
                //-----------------------------------------------------------------------------------------------------------
                a.Should().NotBeEquivalentTo(b);
            }
        }

        [TestMethod]
        public void When_values_differ_Be_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var testCases = new Dictionary<string, string>
            {
                { "{id:1}", "{id:2}" }
                , { "{id:1,admin:true}", "{id:1,admin:false}" }
            };

            foreach (var testCase in testCases)
            {
                var actualJson = testCase.Key;
                var expectedJson = testCase.Value;

                var a = JToken.Parse(actualJson);
                var b = JToken.Parse(expectedJson);

                var expectedMessage =
                    $"Expected JSON document to be {_formatter.ToString(b)}, but found {_formatter.ToString(a)}.";

                //-----------------------------------------------------------------------------------------------------------
                // Act & Assert
                //-----------------------------------------------------------------------------------------------------------
                a.Should().Invoking(x => x.Be(b))
                    .ShouldThrow<AssertFailedException>()
                    .WithMessage(expectedMessage);
            }
        }

        [TestMethod]
        public void When_values_differ_BeEquivalentTo_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var testCases = new Dictionary<string, string>
            {
                { "{id:1,admin:true}", "{id:1,admin:false}" }
            };

            foreach (var testCase in testCases)
            {
                var actualJson = testCase.Key;
                var expectedJson = testCase.Value;

                var a = JToken.Parse(actualJson);
                var b = JToken.Parse(expectedJson);

                var expectedMessage = GetNotEquivalentMessage(a, b);

                //-----------------------------------------------------------------------------------------------------------
                // Act & Assert
                //-----------------------------------------------------------------------------------------------------------
                a.Should().Invoking(x => x.BeEquivalentTo(b))
                    .ShouldThrow<AssertFailedException>()
                    .WithMessage(expectedMessage);
            }
        }

        [TestMethod]
        public void Fail_with_descriptive_message_when_child_element_differs()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = JToken.Parse("{child:{subject:'foo'}}");
            var expected = JToken.Parse("{child:{expected:'bar'}}");

            var expectedMessage = GetNotEquivalentMessage(subject, expected, "we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Act & Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.Should().Invoking(x => x.BeEquivalentTo(expected, "we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(expectedMessage);
        }

        [TestMethod]
        public void When_jtoken_has_value_HaveValue_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = JToken.Parse("{ 'id':42}");

            //-----------------------------------------------------------------------------------------------------------
            // Act & Assert
            //-----------------------------------------------------------------------------------------------------------
            subject["id"].Should().HaveValue("42");
        }

        [TestMethod]
        public void When_jtoken_not_has_value_HaveValue_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = JToken.Parse("{ 'id':42}");

            //-----------------------------------------------------------------------------------------------------------
            // Act & Assert
            //-----------------------------------------------------------------------------------------------------------
            subject["id"].Should().Invoking(x => x.HaveValue("43", "because foo"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected JSON property \"id\" to have value \"43\" because foo, but found \"42\".");
        }

        [TestMethod]
        public void When_jtoken_has_element_HaveElement_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = JToken.Parse("{ 'id':42}");

            //-----------------------------------------------------------------------------------------------------------
            // Act & Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.Should().HaveElement("id");

            subject.Should().Invoking(x => x.HaveElement("name", "because foo"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage($"Expected JSON document {_formatter.ToString(subject)} to have element \"name\" because foo, but no such element was found.");
        }

        [TestMethod]
        public void When_jtoken_not_has_element_HaveElement_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = JToken.Parse("{ 'id':42}");

            //-----------------------------------------------------------------------------------------------------------
            // Act & Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.Should().Invoking(x => x.HaveElement("name", "because foo"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage($"Expected JSON document {_formatter.ToString(subject)} to have element \"name\" because foo, but no such element was found.");
        }

        private static readonly JTokenFormatter _formatter = new JTokenFormatter();

        private static string GetNotEquivalentMessage(JToken actual, JToken expected,
            string reason = null, params object[] reasonArgs)
        {
            var diff = ObjectDiffPatch.GenerateDiff(actual, expected);
            var key = diff.NewValues?.First ?? diff.OldValues?.First;

            var because = string.Empty;
            if (!string.IsNullOrWhiteSpace(reason))
                because = " because " + string.Format(reason, reasonArgs);

            var expectedMessage = $"Expected JSON document {_formatter.ToString(actual)}" +
                                  $" to be equivalent to {_formatter.ToString(expected)}" +
                                  $"{because}, but differs at {_formatter.ToString(key)}.";
            return expectedMessage;
        }
    }
}