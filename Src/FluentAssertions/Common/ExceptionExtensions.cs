using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace FluentAssertions.Common;

[StackTraceHidden]
internal static class ExceptionExtensions
{
    public static ExceptionDispatchInfo Unwrap(this TargetInvocationException exception)
    {
        Exception result = exception;

        while (result is TargetInvocationException)
        {
            result = result.InnerException;
        }

        return ExceptionDispatchInfo.Capture(result);
    }
}

