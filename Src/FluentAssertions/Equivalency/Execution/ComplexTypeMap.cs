using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Execution
{
    internal class ComplexTypeMap
    {
        private readonly ConcurrentDictionary<Type, bool> isComplexTypeMap = new();

        public bool IsComplexType(object instance)
        {
            if (instance is null)
            {
                return false;
            }

            Type type = instance.GetType();

            if (!isComplexTypeMap.TryGetValue(type, out bool isComplexType))
            {
                isComplexType = !type.OverridesEquals();
                isComplexTypeMap[type] = isComplexType;
            }

            return isComplexType;
        }
    }
}
