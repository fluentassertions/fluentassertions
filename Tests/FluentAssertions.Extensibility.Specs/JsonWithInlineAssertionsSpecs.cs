#if NET8_0_OR_GREATER
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ExampleExtensions;
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

        // Act
        await HttpResponseMessageExtensions.Should(response).BeEquivalentTo(new
        {
            Id = "Pathy",
            Versions = new[]
            {
                new
                {
                    Description = Value.ThatMatches<string>(s => s.Contains("paths")),
                    Owner = "dennisdoomen",
                    RepositoryUrl = "https://github.com/dennisdoomen/pathy",
                    Version = "1.5.0"
                }
            }
        });

        // Assert
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

        // Act
        await HttpResponseMessageExtensions.Should(response).BeEquivalentTo(new
        {
            Id = "Pathy",
            Versions = new[]
            {
                new
                {
                    Description = Value.ThatSatisfies<string>(s => s.Should().Contain("paths")),
                    Owner = "dennisdoomen",
                    RepositoryUrl = "https://github.com/dennisdoomen/pathy",
                    Version = "1.5.0"
                }
            }
        });

        // Assert
    }
}
#endif
