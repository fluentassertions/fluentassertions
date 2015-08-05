using System.Reflection;

#if !NETFX_CORE && !WINRT

namespace FluentAssertions
{
    public static class FindAssembly
    {
        public static Assembly Containing<T>()
        {
            return typeof(T).Assembly;
        }
    }
}

#endif