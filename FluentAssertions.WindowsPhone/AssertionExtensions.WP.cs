using FluentAssertions.Common;

namespace FluentAssertions
{
    public static partial class AssertionExtensions
    {
        static AssertionExtensions()
        {
            Services.ReflectionProvider = new WPReflectionProvider();
        }
    }
}