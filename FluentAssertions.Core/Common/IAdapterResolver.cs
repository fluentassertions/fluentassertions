// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// http://pclcontrib.codeplex.com/
// -----------------------------------------------------------------------


using System;

namespace FluentAssertions.Common
{
    internal interface IAdapterResolver
    {
        object Resolve(Type type);
    }
}
