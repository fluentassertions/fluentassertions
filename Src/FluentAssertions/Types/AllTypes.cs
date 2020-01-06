using System.Reflection;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Static class that allows for a 'fluent' selection of the types from an <see cref="Assembly"/>.
    /// </summary>
    /// <example>
    /// AllTypes.From(myAssembly)<br />
    /// .ThatImplement&lt;ISomeInterface&gt;<br />
    /// .Should()<br />
    /// .BeDecoratedWith&lt;SomeAttribute&gt;()
    /// </example>
    public static class AllTypes
    {
        /// <summary>
        /// Returns a <see cref="TypeSelector"/> for selecting the types that are visible outside the
        /// specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly from which to select the types.</param>
        public static TypeSelector From(Assembly assembly)
        {
            return assembly.Types();
        }
    }
}
