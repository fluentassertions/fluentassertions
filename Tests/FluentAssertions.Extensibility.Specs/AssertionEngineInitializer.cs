using System;
using System.Threading;

// With specific initialization code to invoke before the first assertion happens
[assembly: FluentAssertions.Extensibility.AssertionEngineInitializer(
    typeof(FluentAssertions.Extensibility.Specs.AssertionEngineInitializer),
    nameof(FluentAssertions.Extensibility.Specs.AssertionEngineInitializer.InitializeBeforeFirstAssertion))]

[assembly: FluentAssertions.Extensibility.AssertionEngineInitializer(
    typeof(FluentAssertions.Extensibility.Specs.AssertionEngineInitializer),
    nameof(FluentAssertions.Extensibility.Specs.AssertionEngineInitializer.InitializeBeforeFirstAssertionButThrow))]

namespace FluentAssertions.Extensibility.Specs;

public static class AssertionEngineInitializer
{
    private static int shouldBeCalledOnlyOnce;

    public static int ShouldBeCalledOnlyOnce => shouldBeCalledOnlyOnce;

    public static void InitializeBeforeFirstAssertion()
    {
        Interlocked.Increment(ref shouldBeCalledOnlyOnce);
    }

    public static void InitializeBeforeFirstAssertionButThrow()
    {
        throw new InvalidOperationException("Bogus exception to make sure the engine ignores them");
    }
}
