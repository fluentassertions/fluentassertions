using System.Threading;
using FluentAssertionsAsync.Extensibility;
using FluentAssertionsAsync.Extensibility.Specs;

// With specific initialization code to invoke before the first assertion happens
[assembly: AssertionEngineInitializer(
    typeof(AssertionEngineInitializer),
    nameof(AssertionEngineInitializer.InitializeBeforeFirstAssertion))]

namespace FluentAssertionsAsync.Extensibility.Specs;

public static class AssertionEngineInitializer
{
    private static int shouldBeCalledOnlyOnce;

    public static int ShouldBeCalledOnlyOnce => shouldBeCalledOnlyOnce;

    public static void InitializeBeforeFirstAssertion()
    {
        Interlocked.Increment(ref shouldBeCalledOnlyOnce);
    }
}
