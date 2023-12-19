namespace FluentAssertionsAsync.Extensibility.Specs;

public class ExtensionAssemblyAttributeSpecs
{
    [Fact]
    public void Calls_assembly_initialization_code_only_once()
    {
        for (int i = 0; i < 10; i++)
        {
            var act = () => AssertionEngineInitializer.ShouldBeCalledOnlyOnce.Should().Be(1);

            act.Should().NotThrow();
        }
    }
}
