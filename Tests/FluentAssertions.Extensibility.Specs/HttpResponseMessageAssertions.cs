#if NET6_0_OR_GREATER

using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace FluentAssertions.Extensibility.Specs;

internal static class HttpResponseMessageExtensions
{
    public static HttpResponseMessageAssertions Should(this HttpResponseMessage response)
    {
        return new HttpResponseMessageAssertions(response);
    }
}

internal class HttpResponseMessageAssertions(HttpResponseMessage response)
{
    public async Task BeEquivalentTo<T>(T expectation)
    {
        string body = await response.Content.ReadAsStringAsync();

        JsonNode.Parse(body).Should().BeEquivalentTo(expectation);
    }
}

#endif
