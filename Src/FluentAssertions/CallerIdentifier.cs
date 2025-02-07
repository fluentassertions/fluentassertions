using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using FluentAssertions.CallerIdentification;
using FluentAssertions.Common;

namespace FluentAssertions;

/// <summary>
/// Tries to extract the name of the variable or invocation on which the assertion is executed.
/// </summary>
// REFACTOR: Should be internal and treated as an implementation detail of the AssertionScope
public static class CallerIdentifier
{
    public static Action<string> Logger { get; set; } = _ => { };

    /// <summary>
    /// Gets the identifier that precedes the first Should call in the chain.
    /// </summary>
    public static string DetermineCallerIdentity()
    {
        return DetermineCallerIdentities().FirstOrDefault();
    }

    /// <summary>
    /// Gets all identifiers of all assertions in order of appearance.
    /// </summary>
    /// <example>
    /// For example, given the following code
    /// <code>collection.Should().ContainSingle()
    ///     .Which.Parameters.Should().ContainSingle()
    ///     .Which.Should().Be(3)
    /// </code>
    /// <see cref="DetermineCallerIdentity"/> will return <c>collection</c>, <c>Parameters</c> and an empty string.
    /// </example>
    public static string[] DetermineCallerIdentities()
    {
        string[] callers = [];

        try
        {
            var stack = new StackTrace(fNeedFileInfo: true);

            var allStackFrames = GetFrames(stack);

            int searchStart = allStackFrames.Length - 1;

            if (StartStackSearchAfterStackFrame.Value is not null)
            {
                searchStart = Array.FindLastIndex(
                    allStackFrames,
                    allStackFrames.Length - StartStackSearchAfterStackFrame.Value.SkipStackFrameCount,
                    frame => !IsCurrentAssembly(frame));
            }

            int lastUserStackFrameBeforeFluentAssertionsCodeIndex = Array.FindIndex(
                allStackFrames,
                startIndex: 0,
                count: searchStart + 1,
                frame => !IsCurrentAssembly(frame) && !IsDynamic(frame) && !IsDotNet(frame));

            for (int i = lastUserStackFrameBeforeFluentAssertionsCodeIndex; i < allStackFrames.Length; i++)
            {
                var frame = allStackFrames[i];

                Logger(frame.ToString());

                if (frame.GetMethod() is not null
                    && !IsDynamic(frame)
                    && !IsDotNet(frame)
                    && !IsCustomAssertion(frame)
                    && !IsCurrentAssembly(frame))
                {
                    callers = ExtractCallersFrom(frame).ToArray();
                    break;
                }
            }
        }
        catch (Exception e)
        {
            // Ignore exceptions, as determination of caller identity is only a nice-to-have
            Logger(e.ToString());
        }

        return callers;
    }

    private sealed class StackFrameReference : IDisposable
    {
        public int SkipStackFrameCount { get; }

        private readonly StackFrameReference previousReference;

        public StackFrameReference()
        {
            var stack = new StackTrace();

            var allStackFrames = GetFrames(stack);

            int firstUserCodeFrameIndex = 0;

            while (firstUserCodeFrameIndex < allStackFrames.Length
                   && IsCurrentAssembly(allStackFrames[firstUserCodeFrameIndex]))
            {
                firstUserCodeFrameIndex++;
            }

            SkipStackFrameCount = (allStackFrames.Length - firstUserCodeFrameIndex) + 1;

            previousReference = StartStackSearchAfterStackFrame.Value;
            StartStackSearchAfterStackFrame.Value = this;
        }

        public void Dispose()
        {
            StartStackSearchAfterStackFrame.Value = previousReference;
        }
    }

    private static readonly AsyncLocal<StackFrameReference> StartStackSearchAfterStackFrame = new();

    internal static IDisposable OverrideStackSearchUsingCurrentScope()
    {
        return new StackFrameReference();
    }

    internal static bool OnlyOneFluentAssertionScopeOnCallStack()
    {
        var allStackFrames = GetFrames(new StackTrace());

        int firstNonFluentAssertionsStackFrameIndex = Array.FindIndex(
            allStackFrames,
            frame => !IsCurrentAssembly(frame));

        if (firstNonFluentAssertionsStackFrameIndex < 0)
        {
            return true;
        }

        int startOfSecondFluentAssertionsScopeStackFrameIndex = Array.FindIndex(
            allStackFrames,
            startIndex: firstNonFluentAssertionsStackFrameIndex + 1,
            frame => IsCurrentAssembly(frame));

        return startOfSecondFluentAssertionsScopeStackFrameIndex < 0;
    }

    private static bool IsCustomAssertion(StackFrame frame)
    {
        MethodBase getMethod = frame.GetMethod();

        if (getMethod is not null)
        {
            return
                getMethod.IsDecoratedWithOrInherit<CustomAssertionAttribute>() ||
                getMethod.ReflectedType?.Assembly.IsDefined(typeof(CustomAssertionsAssemblyAttribute)) == true;
        }

        return false;
    }

    private static bool IsDynamic(StackFrame frame)
    {
        return frame.GetMethod() is { DeclaringType: null };
    }

    private static bool IsCurrentAssembly(StackFrame frame)
    {
        return frame.GetMethod()?.DeclaringType?.Assembly == typeof(CallerIdentifier).Assembly;
    }

    private static bool IsDotNet(StackFrame frame)
    {
        var frameNamespace = frame.GetMethod()?.DeclaringType?.Namespace;
        const StringComparison comparisonType = StringComparison.OrdinalIgnoreCase;

        return frameNamespace?.StartsWith("system.", comparisonType) == true ||
            frameNamespace?.Equals("system", comparisonType) == true;
    }

    private static bool IsCompilerServices(StackFrame frame)
    {
        return frame.GetMethod()?.DeclaringType?.Namespace is "System.Runtime.CompilerServices";
    }

    private static IEnumerable<string> ExtractCallersFrom(StackFrame frame)
    {
        string[] identifiers = GetCallerIdentifiersFrom(frame);

        foreach (string identifier in identifiers)
        {
            Logger(identifier);

            if (!IsBooleanLiteral(identifier) && !IsNumeric(identifier) && !IsStringLiteral(identifier) &&
                !StartsWithNewKeyword(identifier))
            {
                yield return identifier;
            }
        }
    }

    private static string[] GetCallerIdentifiersFrom(StackFrame frame)
    {
        string fileName = frame.GetFileName();
        int expectedLineNumber = frame.GetFileLineNumber();

        if (string.IsNullOrEmpty(fileName) || expectedLineNumber == 0)
        {
            return null;
        }

        try
        {
            using var reader = new StreamReader(File.OpenRead(fileName));
            string line;
            int currentLine = 1;

            while ((line = reader.ReadLine()) is not null && currentLine < expectedLineNumber)
            {
                currentLine++;
            }

            return currentLine == expectedLineNumber && line != null
                    ? GetCallerIdentifiersFrom(frame, reader, line)
                    : null;
        }
        catch
        {
            // We don't care. Just assume the symbol file is not available or unreadable
            return [];
        }
    }

    private static string[] GetCallerIdentifiersFrom(StackFrame frame, StreamReader reader, string line)
    {
        int column = frame.GetFileColumnNumber();

        if (column > 0)
        {
            line = line.Substring(Math.Min(column - 1, line.Length - 1));
        }

        var parser = new StatementParser();

        do
        {
            parser.Append(line);
        }
        while (!parser.IsDone() && (line = reader.ReadLine()) != null);

        return parser.Identifiers;
    }

    private static bool StartsWithNewKeyword(string candidate)
    {
        return Regex.IsMatch(candidate, @"(?:^|s+)new(?:\s?\[|\s?\{|\s\w+)");
    }

    private static bool IsStringLiteral(string candidate)
    {
        return candidate.StartsWith('\"');
    }

    private static bool IsNumeric(string candidate)
    {
        const NumberStyles numberStyle = NumberStyles.Float | NumberStyles.AllowThousands;
        return double.TryParse(candidate, numberStyle, CultureInfo.InvariantCulture, out _);
    }

    private static bool IsBooleanLiteral(string candidate)
    {
        return candidate is "true" or "false";
    }

    private static StackFrame[] GetFrames(StackTrace stack)
    {
        var frames = stack.GetFrames();
#if !NET6_0_OR_GREATER
        if (frames == null)
        {
            return [];
        }
#endif
        return frames
            .Where(frame => !IsCompilerServices(frame))
            .ToArray();
    }
}
