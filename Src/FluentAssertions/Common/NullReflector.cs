#if NETSTANDARD1_3
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAssertions.Common
{
    internal class NullReflector : IReflector
    {
        public IEnumerable<Type> GetAllTypesFromAppDomain(Func<Assembly, bool> predicate)
        {
            return new Type[0];
        }
    }
}
#endif
