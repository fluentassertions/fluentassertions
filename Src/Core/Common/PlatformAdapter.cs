// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// http://pclcontrib.codeplex.com/
// -----------------------------------------------------------------------


using System;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Facade to resolve an implementation of a particular interface using a platform-specific assembly.
    /// </summary>
    internal static class PlatformAdapter
    {
        private static readonly IAdapterResolver resolver = new ProbingAdapterResolver();

        public static T Resolve<T>()
        {
            T value = (T)resolver.Resolve(typeof(T));
            if (value == null)
            {
                throw new PlatformNotSupportedException("Platform doesn't support interface " + typeof(T).Name);
            }

            return value;
        }
    }
}