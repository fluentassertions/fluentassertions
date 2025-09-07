#if NETCOREAPP3_1_OR_GREATER

using System.Text.Json;
using FluentAssertions;

namespace ExampleExtensions;

public static class HttpResponseMessageExtensions
{
    public static HttpResponseMessageAssertions Should(this HttpResponseMessage response)
    {
        return new HttpResponseMessageAssertions(response);
    }
}

public class HttpResponseMessageAssertions(HttpResponseMessage response)
{
    public async Task BeEquivalentTo<T>(T expectation)
    {
        string body = await response.Content.ReadAsStringAsync();

        object actual = JsonSerializer.Deserialize(body, expectation.GetType(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });

        actual.Should().BeEquivalentTo(expectation);
    }
}

#endif
