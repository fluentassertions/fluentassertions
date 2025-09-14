#if NET6_0_OR_GREATER
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentAssertions.Extensibility.Specs;

public class JsonWithInlineAssertionsSpecs
{
    [Fact]
    public async Task Can_use_inline_predicates_during_json_assertions()
    {
        // Arrange
        string json =
            """
            {
                "Id": "TestPackage",
                "Versions": [
                    {
                        "Version": "1.0.0",
                        "Description": "Test package description",
                        "RepositoryUrl": "https://github.com/test/package",
                        "Owner": "testowner"
                    }
                ]
            }
            """;

        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = new StringContent(json);

        // Act / Assert
        await response.Should().BeEquivalentTo(new
        {
            Id = "TestPackage",
            Versions = new[]
            {
                new
                {
                    Version = "1.0.0",
                    Description = Value.ThatMatches<string>(s => s.Contains("Test")),
                    RepositoryUrl = "https://github.com/test/package",
                    Owner = "testowner",
                }
            }
        });
    }

    [Fact]
    public async Task Can_use_inline_assertions_during_json_assertions()
    {
        // Arrange
        string json =
            """
            {
                "Id": "TestPackage",
                "Versions": [
                    {
                        "Version": "1.0.0",
                        "Description": "Test package description",
                        "RepositoryUrl": "https://github.com/test/package",
                        "Owner": "testowner"
                    }
                ]
            }
            """;

        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = new StringContent(json);

        // Act / Assert
        await response.Should().BeEquivalentTo(new
        {
            Id = "TestPackage",
            Versions = new[]
            {
                new
                {
                    Version = "1.0.0",
                    Description = Value.ThatSatisfies<string>(s => s.Should().Contain("description")),
                    RepositoryUrl = "https://github.com/test/package",
                    Owner = "testowner",
                }
            }
        });
    }
}
#endif
