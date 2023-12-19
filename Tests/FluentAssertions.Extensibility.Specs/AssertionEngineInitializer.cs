using System.Threading;
using FluentAssertionsAsync.Extensibility;

// With specific initialization code to invoke before the first assertion happens
[assembly: AssertionEngineInitializer(
    typeof(FluentAssertions.Extensibility.Specs.AssertionEngineInitializer),
    nameof(FluentAssertions.Extensibility.Specs.AssertionEngineInitializer.InitializeBeforeFirstAssertion))]

namespace FluentAssertions.Extensibility.Specs;

public static class AssertionEngineInitializer
{
    private static int shouldBeCalledOnlyOnce;

    public static int ShouldBeCalledOnlyOnce => shouldBeCalledOnlyOnce;

    public static void InitializeBeforeFirstAssertion()
    {
        Interlocked.Increment(ref shouldBeCalledOnlyOnce);
    }
}
