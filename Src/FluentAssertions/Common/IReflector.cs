using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAssertionsAsync.Common;

public interface IReflector
{
    IEnumerable<Type> GetAllTypesFromAppDomain(Func<Assembly, bool> predicate);
}
