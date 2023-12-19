using System;
using System.Linq;
using System.Reflection;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Equivalency;

namespace FluentAssertionsAsync.Formatting;

public class DefaultValueFormatter : IValueFormatter
{
    /// <summary>
    /// Determines whether this instance can handle the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// <see langword="true"/> if this instance can handle the specified value; otherwise, <see langword="false"/>.
    /// </returns>
    public virtual bool CanHandle(object value)
    {
        return true;
    }

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        if (value.GetType() == typeof(object))
        {
            formattedGraph.AddFragment($"System.Object (HashCode={value.GetHashCode()})");
            return;
        }

        if (HasCompilerGeneratedToStringImplementation(value))
        {
            WriteTypeAndMemberValues(value, formattedGraph, formatChild);
        }
        else if (context.UseLineBreaks)
        {
            formattedGraph.AddFragmentOnNewLine(value.ToString());
        }
        else
        {
            formattedGraph.AddFragment(value.ToString());
        }
    }

    /// <summary>
    /// Selects which members of <paramref name="type"/> to format.
    /// </summary>
    /// <param name="type">The <see cref="System.Type"/> of the object being formatted.</param>
    /// <returns>The members of <paramref name="type"/> that will be included when formatting this object.</returns>
    /// <remarks>The default is all non-private members.</remarks>
    protected virtual MemberInfo[] GetMembers(Type type)
    {
        return type.GetMembers(MemberVisibility.Public);
    }

    private static bool HasCompilerGeneratedToStringImplementation(object value)
    {
        Type type = value.GetType();

        return HasDefaultToStringImplementation(value) || type.IsCompilerGenerated();
    }

    private static bool HasDefaultToStringImplementation(object value)
    {
        string str = value.ToString();

        return str is null || str == value.GetType().ToString();
    }

    private void WriteTypeAndMemberValues(object obj, FormattedObjectGraph formattedGraph, FormatChild formatChild)
    {
        Type type = obj.GetType();
        WriteTypeName(formattedGraph, type);
        WriteTypeValue(obj, formattedGraph, formatChild, type);
    }

    private void WriteTypeName(FormattedObjectGraph formattedGraph, Type type)
    {
        var typeName = type.HasFriendlyName() ? TypeDisplayName(type) : string.Empty;
        formattedGraph.AddFragment(typeName);
    }

    private void WriteTypeValue(object obj, FormattedObjectGraph formattedGraph, FormatChild formatChild, Type type)
    {
        MemberInfo[] members = GetMembers(type);
        if (members.Length == 0)
        {
            formattedGraph.AddFragment("{ }");
        }
        else
        {
            formattedGraph.EnsureInitialNewLine();
            formattedGraph.AddLine("{");
            WriteMemberValues(obj, members, formattedGraph, formatChild);
            formattedGraph.AddFragmentOnNewLine("}");
        }
    }

    private static void WriteMemberValues(object obj, MemberInfo[] members, FormattedObjectGraph formattedGraph, FormatChild formatChild)
    {
        using var iterator = new Iterator<MemberInfo>(members.OrderBy(mi => mi.Name, StringComparer.Ordinal));

        while (iterator.MoveNext())
        {
            WriteMemberValueTextFor(obj, iterator.Current, formattedGraph, formatChild);

            if (!iterator.IsLast)
            {
                formattedGraph.AddFragment(", ");
            }
        }
    }

    /// <summary>
    /// Selects the name to display for <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The <see cref="System.Type"/> of the object being formatted.</param>
    /// <returns>The name to be displayed for <paramref name="type"/>.</returns>
    /// <remarks>The default is <see cref="System.Type.FullName"/>.</remarks>
    protected virtual string TypeDisplayName(Type type) => type.FullName;

    private static void WriteMemberValueTextFor(object value, MemberInfo member, FormattedObjectGraph formattedGraph,
        FormatChild formatChild)
    {
        object memberValue;

        try
        {
            memberValue = member switch
            {
                FieldInfo fi => fi.GetValue(value),
                PropertyInfo pi => pi.GetValue(value),
                _ => throw new InvalidOperationException()
            };
        }
        catch (Exception ex)
        {
            ex = (ex as TargetInvocationException)?.InnerException ?? ex;
            memberValue = $"[Member '{member.Name}' threw an exception: '{ex.Message}']";
        }

        formattedGraph.AddFragmentOnNewLine($"{new string(' ', FormattedObjectGraph.SpacesPerIndentation)}{member.Name} = ");
        formatChild(member.Name, memberValue, formattedGraph);
    }
}
