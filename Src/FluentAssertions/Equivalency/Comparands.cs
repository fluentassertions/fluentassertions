using System;
using System.Globalization;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency;

public class Comparands
{
    public Comparands()
    {
    }

    public Comparands(object subject, object expectation, Type compileTimeType)
    {
        CompileTimeType = compileTimeType;
        Subject = subject;
        Expectation = expectation;
    }

    /// <summary>
    /// Gets the value of the subject object graph.
    /// </summary>
    public object Subject { get; set; }

    /// <summary>
    /// Gets the value of the expected object graph.
    /// </summary>
    public object Expectation { get; set; }

    /// <summary>
    /// Gets the compile-time type of the expectation object.
    /// </summary>
    public Type CompileTimeType
    {
        get
        {
            return field != typeof(object) || Expectation is null ? field : RuntimeType;
        }

        // SMELL: Do we really need this? Can we replace it by making Comparands generic or take a constructor parameter?
        set;
    }

    /// <summary>
    /// Gets the run-time type of the current expectation object.
    /// </summary>
    public Type RuntimeType
    {
        get
        {
            if (Expectation is not null)
            {
                return Expectation.GetType();
            }

            return CompileTimeType;
        }
    }

    /// <summary>
    /// Returns either the run-time or compile-time type of the expectation based on the options provided by the caller.
    /// </summary>
    /// <remarks>
    /// If the expectation is a nullable type, it should return the type of the wrapped object.
    /// </remarks>
    public Type GetExpectedType(IEquivalencyOptions options)
    {
        Type type = options.UseRuntimeTyping ? RuntimeType : CompileTimeType;

        return type.NullableOrActualType();
    }

    public override string ToString()
    {
        return string.Create(CultureInfo.InvariantCulture, $"{{Subject={Subject}, Expectation={Expectation}}}");
    }
}
