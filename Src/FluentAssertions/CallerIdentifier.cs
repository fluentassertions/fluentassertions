using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using FluentAssertions.Common;

namespace FluentAssertions
{
    /// <summary>
    /// Tries to extract the name of the variable or invocation on which the assertion is executed.
    /// </summary>
    public static class CallerIdentifier
    {
#pragma warning disable CA2211 // TODO: fix in 6.0
        public static Action<string> logger = str => { };
#pragma warning restore CA2211

#if !NETSTANDARD1_3 && !NETSTANDARD1_6

        public static string DetermineCallerIdentity()
        {
            string caller = null;

            try
            {
                StackTrace stack = new StackTrace(true);

                foreach (StackFrame frame in stack.GetFrames())
                {
                    logger(frame.ToString());

                    if (frame.GetMethod() is object
                        && !IsDynamic(frame)
                        && !IsDotNet(frame)
                        && !IsCurrentAssembly(frame)
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
            var comparisonType = StringComparison.InvariantCultureIgnoreCase;

            return frameNamespace?.StartsWith("system.", comparisonType) == true ||
                frameNamespace?.Equals("system", comparisonType) == true;
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

                int indexOfShould = statement.IndexOf("Should", StringComparison.InvariantCulture);
                if (indexOfShould != -1)
                {
                    string candidate = statement.Substring(0, indexOfShould - 1);

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
                using (StreamReader reader = new StreamReader(File.OpenRead(fileName)))
                {
                    string line;
                    int currentLine = 1;

                    while ((line = reader.ReadLine()) != null && currentLine < expectedLineNumber)
                    {
                        currentLine++;
                    }

                    return (currentLine == expectedLineNumber) ? line : null;
                }
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
            return double.TryParse(candidate, out _);
        }

        private static bool IsBooleanLiteral(string candidate)
        {
            return candidate == "true" || candidate == "false";
        }

#else
        public static string DetermineCallerIdentity()
        {
            return null;
        }
#endif
    }
}
