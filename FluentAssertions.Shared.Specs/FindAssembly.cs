using System.Reflection;

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
