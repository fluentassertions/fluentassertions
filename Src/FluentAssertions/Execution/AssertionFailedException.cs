using System;

namespace FluentAssertions.Execution;

/// <summary>
/// Represents the default exception in case no test framework is configured.
/// </summary>
/// <param name="message">The <em>mandatory</em> exception message</param>
#pragma warning disable CA1032, RCS1194 // AssertionFailedException should never be constructed with an empty message
public class AssertionFailedException(string message) : Exception(message), IAssertionException
#pragma warning restore CA1032, RCS1194
{
}
