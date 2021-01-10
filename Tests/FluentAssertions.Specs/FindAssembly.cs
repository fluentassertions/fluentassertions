using System.Reflection;

namespace FluentAssertions.Specs
{
    public static class FindAssembly
    {
        public static Assembly Containing<T>()
        {
            return typeof(T).Assembly;
        }
    }
}
