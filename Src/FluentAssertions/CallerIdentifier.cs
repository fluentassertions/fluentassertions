using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using FluentAssertions.Common;

namespace FluentAssertions
{
    /// <summary>
    /// Tries to extract the name of the variable or invocation on which the assertion is executed.
    /// </summary>
    public static class CallerIdentifier
    {
#pragma warning disable CA2211, SA1401, SA1307 // TODO: fix in 6.0
        public static Action<string> logger = _ => { };
#pragma warning restore SA1307, SA1401, CA2211

        public static string DetermineCallerIdentity()
        {
            string caller = null;

            try
            {
                StackTrace stack = new StackTrace(fNeedFileInfo: true);

                var allStackFrames = stack.GetFrames()
                    .Where(frame => !IsCompilerServices(frame))
                    .ToArray();

                int searchStart = allStackFrames.Length - 1;

                if (StartStackSearchAfterStackFrame.Value != null)
                {
                    searchStart = Array.FindLastIndex(
                        allStackFrames,
                        allStackFrames.Length - StartStackSearchAfterStackFrame.Value.SkipStackFrameCount,
                        frame => !IsCurrentAssembly(frame));
                }

                int firstFluentAssertionsCodeIndex = Array.FindLastIndex(
                    allStackFrames,
                    searchStart,
                    frame => IsCurrentAssembly(frame));

                int lastUserStackFrameBeforeFluentAssertionsCodeIndex =
                    firstFluentAssertionsCodeIndex + 1;

                for (int i = lastUserStackFrameBeforeFluentAssertionsCodeIndex; i < allStackFrames.Length; i++)
                {
                    var frame = allStackFrames[i];

                    logger(frame.ToString());

                    if (frame.GetMethod() is object
                        && !IsDynamic(frame)
                        && !IsDotNet(frame)
                        && !IsCustomAssertion(frame))
                    {
                        caller = ExtractVariableNameFrom(frame) ?? caller;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                // Ignore exceptions, as determination of caller identity is only a nice-to-have
                logger(e.ToString());
            }

            return caller;
        }

        private class StackFrameReference : IDisposable
        {
            public int SkipStackFrameCount { get; set; }

            private readonly StackFrameReference previousReference;

            public StackFrameReference()
            {
                var stack = new StackTrace();

                var allStackFrames = stack.GetFrames()
                    .Where(frame => !IsCompilerServices(frame))
                    .ToArray();

                int firstUserCodeFrameIndex = 0;

                while ((firstUserCodeFrameIndex < allStackFrames.Length)
                    && IsCurrentAssembly(allStackFrames[firstUserCodeFrameIndex]))
                {
                    firstUserCodeFrameIndex++;
                }

                SkipStackFrameCount = allStackFrames.Length - firstUserCodeFrameIndex + 1;

                previousReference = StartStackSearchAfterStackFrame.Value;
                StartStackSearchAfterStackFrame.Value = this;
            }

            public void Dispose()
            {
                StartStackSearchAfterStackFrame.Value = previousReference;
            }
        }

        private static readonly AsyncLocal<StackFrameReference> StartStackSearchAfterStackFrame = new AsyncLocal<StackFrameReference>();

        internal static IDisposable OverrideStackSearchUsingCurrentScope()
        {
            return new StackFrameReference();
        }

        internal static bool OnlyOneFluentAssertionScopeOnCallStack()
        {
            var allStackFrames = new StackTrace().GetFrames()
                .Where(frame => !IsCompilerServices(frame))
                .ToArray();

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
            return frame.GetMethod().IsDecoratedWithOrInherit<CustomAssertionAttribute>();
        }

        private static bool IsDynamic(StackFrame frame)
        {
            return frame.GetMethod().DeclaringType is null;
        }

        private static bool IsCurrentAssembly(StackFrame frame)
        {
            return frame.GetMethod().DeclaringType.Assembly == typeof(CallerIdentifier).Assembly;
        }

        private static bool IsDotNet(StackFrame frame)
        {
            var frameNamespace = frame.GetMethod().DeclaringType.Namespace;
            var comparisonType = StringComparison.OrdinalIgnoreCase;

            return frameNamespace?.StartsWith("system.", comparisonType) == true ||
                frameNamespace?.Equals("system", comparisonType) == true;
        }

        private static bool IsCompilerServices(StackFrame frame)
        {
            return frame.GetMethod().DeclaringType.Namespace == "System.Runtime.CompilerServices";
        }

        private static string ExtractVariableNameFrom(StackFrame frame)
        {
            string caller = null;

            int column = frame.GetFileColumnNumber();
            string line = GetSourceCodeLineFrom(frame);

            if ((line != null) && (column != 0) && (line.Length > 0))
            {
                string statement = line.Substring(Math.Min(column - 1, line.Length - 1));

                logger(statement);

                int indexOfShould = statement.IndexOf(".Should", StringComparison.Ordinal);
                if (indexOfShould != -1)
                {
                    string candidate = statement.Substring(0, indexOfShould);

                    logger(candidate);

                    if (!IsBooleanLiteral(candidate) && !IsNumeric(candidate) && !IsStringLiteral(candidate) &&
                        !UsesNewKeyword(candidate))
                    {
                        caller = candidate;
                    }
                }
            }

            return caller;
        }

        private static string GetSourceCodeLineFrom(StackFrame frame)
        {
            string fileName = frame.GetFileName();
            int expectedLineNumber = frame.GetFileLineNumber();

            if (string.IsNullOrEmpty(fileName) || (expectedLineNumber == 0))
            {
                return null;
            }

            try
            {
                using StreamReader reader = new StreamReader(File.OpenRead(fileName));
                string line;
                int currentLine = 1;

                while ((line = reader.ReadLine()) != null && currentLine < expectedLineNumber)
                {
                    currentLine++;
                }

                return (currentLine == expectedLineNumber) ? line : null;
            }
            catch
            {
                // We don't care. Just assume the symbol file is not available or unreadable
                return null;
            }
        }

        private static bool UsesNewKeyword(string candidate)
        {
            return Regex.IsMatch(candidate, @"new(\s?\[|\s?\{|\s\w+)");
        }

        private static bool IsStringLiteral(string candidate)
        {
            return candidate.StartsWith("\"", StringComparison.Ordinal);
        }

        private static bool IsNumeric(string candidate)
        {
            const NumberStyles DefaultStyle = NumberStyles.Float | NumberStyles.AllowThousands;
            return double.TryParse(candidate, DefaultStyle, CultureInfo.InvariantCulture, out _);
        }

        private static bool IsBooleanLiteral(string candidate)
        {
            return candidate == "true" || candidate == "false";
        }
    }
}
