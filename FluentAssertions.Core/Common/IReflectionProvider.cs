using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAssertions.Common
{
    public interface IReflectionProvider
    {
        IEnumerable<Type> GetAllTypesFromAppDomain(Func<Assembly, bool> predicate);
    }
}