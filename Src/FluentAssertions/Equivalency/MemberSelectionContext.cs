using System;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Equivalency;

/// <summary>
/// Provides contextual information to an <see cref="IMemberSelectionRule"/>.
/// </summary>
public class MemberSelectionContext
{
    private readonly Type compileTimeType;
    private readonly Type runtimeType;
    private readonly IEquivalencyOptions options;

    public MemberSelectionContext(Type compileTimeType, Type runtimeType, IEquivalencyOptions options)
    {
        this.runtimeType = runtimeType;
        this.compileTimeType = compileTimeType;
        this.options = options;
    }

    /// <summary>
    /// Gets a value indicating whether and which properties should be considered.
    /// </summary>
    public MemberVisibility IncludedProperties => options.IncludedProperties;

    /// <summary>
    /// Gets a value indicating whether and which fields should be considered.
    /// </summary>
    public MemberVisibility IncludedFields => options.IncludedFields;

    /// <summary>
    /// Gets either the compile-time or run-time type depending on the options provided by the caller.
    /// </summary>
    public Type Type
    {
        get
        {
            Type type = options.UseRuntimeTyping ? runtimeType : compileTimeType;

            return type.NullableOrActualType();
        }
    }
}
