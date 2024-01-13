#if NET6_0_OR_GREATER
using System.Text.Json;
using Xunit;

namespace FluentAssertions.Specs.Json;

public partial class JsonElementAssertionsSpecs
{
    [Fact]
    public void Allow_chaining_multiple_checks()
    {
        // Arrange
        var subject = JsonDocument.Parse("""{ "id": 42 }""").RootElement;

        // Act / Assert
        subject.Should()
            .HaveCount(1).And
            .HaveElement("id").And
            .NotHaveElement("bar");
    }
}
#endif
