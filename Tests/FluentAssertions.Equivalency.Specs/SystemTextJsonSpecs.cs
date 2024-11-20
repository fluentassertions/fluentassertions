#if NET6_0_OR_GREATER
#nullable enable
using System.Text.Json;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

public class SystemTextJsonSpecs
{
    [Fact]
    public void EmptyJsonDocument_ShouldBeEquivalent()
    {
        using var expected = JsonDocument.Parse("{}");
        using var actual = JsonDocument.Parse("{}");
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void EqualJsonDocument_ShouldBeEquivalent()
    {
        using var expected = JsonDocument.Parse("""{"a":123}""");
        using var actual = JsonDocument.Parse("""{"a":123}""");
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void DifferentJsonDocument_ShouldNotBeEquivalent()
    {
        var action = () =>
        {
            using var expected = JsonDocument.Parse("234");
            using var actual = JsonDocument.Parse("44");
            return actual.Should().BeEquivalentTo(expected);
        };
        action.Should().Throw<XunitException>().WithMessage("*234*44*");
    }

    [Fact]
    public void EmptyJsonElement_ShouldBeEquivalent()
    {
        using var expected = JsonDocument.Parse("{}");
        using var actual = JsonDocument.Parse("{}");
        actual.RootElement.Should().BeEquivalentTo(expected.RootElement);
    }

    [Fact]
    public void EqualJsonElement_ShouldBeEquivalent()
    {
        using var expected = JsonDocument.Parse("""{"a":123}""");
        using var actual = JsonDocument.Parse("""{"a":123}""");
        actual.RootElement.Should().BeEquivalentTo(expected.RootElement);
    }

    [Fact]
    public void DifferentOrderJsonObject_ShouldBeEquivalent()
    {
        using var expected = JsonDocument.Parse("""{"a":123,"b":234}""");
        using var actual = JsonDocument.Parse("""{"b":234,"a":123}""");
        actual.RootElement.Should().BeEquivalentTo(expected.RootElement);
    }

    [Fact]
    public void DifferentOrderDifferentValueJsonObject_ShouldNotBeEquivalent()
    {
        var action = () =>
        {
            using var expected = JsonDocument.Parse("""{"a":123,"b":234}""");
            using var actual = JsonDocument.Parse("""{"b":234,"a":1}""");
            actual.RootElement.Should().BeEquivalentTo(expected.RootElement);
        };
        action.Should().Throw<XunitException>().WithMessage("""*actual.RootElement[a]*"123"*"1"*""");
    }

    [Fact]
    public void DifferentJsonElement_ShouldNotBeEquivalent()
    {
        var action = () =>
        {
            using var actual = JsonDocument.Parse("44");
            using var expected = JsonDocument.Parse("234");
            return actual.RootElement.Should().BeEquivalentTo(expected.RootElement);
        };
        action.Should().Throw<XunitException>().WithMessage("*234*44*");
    }

    [Fact]
    public void DifferentJsonTypes_ShouldNotBeEquivalent()
    {
        var action = () =>
        {
            using var actual = JsonDocument.Parse("true");
            using var expected = JsonDocument.Parse("\"true\"");
            return actual.RootElement.Should().BeEquivalentTo(expected.RootElement);
        };
        action.Should().Throw<XunitException>().WithMessage("""*"String"*"True"*""");
    }

    [Fact]
    public void DifferenceDeepRecursive_ShouldIncludePath()
    {
        var action = () =>
        {
            using var actual = JsonDocument.Parse("""
                  {
                    "list":[
                      1,
                      {
                        "a":2,
                        "b":3,
                        "c":4,
                        "d":{
                          "e":true
                        }
                      },
                      3,
                      {"error":[{"a":1}]}
                    ]
                  }
                  """);
            using var expected = JsonDocument.Parse("""
                {
                  "list":[
                    1,
                    {
                      "a":2,
                      "c":4,
                      "b":3,
                      "d":{
                        "e":true
                      }
                    },
                    3,
                    {"error":[{"a":2}]}
                  ]
                }
                """);
            return actual.RootElement.Should().BeEquivalentTo(expected.RootElement);
        };
        action.Should().Throw<XunitException>().WithMessage("*actual.RootElement[list][4][error][1][a]*");
    }

    [Theory]
    [InlineData("""
        {
            "numbers": [1,2,3]
        }
        """,
        """{"numbers":[1,2,3]}""",
        null
        )]
    [InlineData("""
        {
            "numbers": [1,2,3]
        }
        """,
        """{"numbers":[1,2,4]}""",
        "*4*3*"
        )]
    [InlineData("6","6.0000", null)]
    [InlineData("\"6\"","\"6.0000\"", "*\"6.0000\"*\"6\"*")]
    [InlineData("[]","[]", null)]
    [InlineData("[]","[2]", "*array length*")]
    [InlineData("[2]","[]", "*array length*")]
    public void TestTheory(string actual, string expected, string? errorPattern)
    {
        using var expectedDoc = JsonDocument.Parse(expected);
        using var actualDoc = JsonDocument.Parse(actual);

        if (errorPattern is null)
        {
            actualDoc.Should().BeEquivalentTo(expectedDoc);
        }
        else
        {
            // ReSharper disable line AccessToDisposedClosure
            Record.Exception(() => actualDoc.Should().BeEquivalentTo(expectedDoc))
                .Should().BeOfType<XunitException>()
                .Which
                .Message.Should().Match(errorPattern);
        }
    }
}
#endif
