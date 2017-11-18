using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Text.RegularExpressions;
using FluentAssertions.Execution;

namespace FluentAssertions
{
    public class CallerIdentifier
    {
#if NET45 || NETSTANDARD2_0
        public static string DetermineCallerIdentity()
        {
            string caller = null;

            var stack = new StackTrace(true);

            foreach (StackFrame frame in stack.GetFrames())
            {
                if (!IsDynamic(frame) && !IsDotNet(frame) && !IsCurrentAssembly(frame))
                {
                    caller = ExtractVariableNameFrom(frame) ?? caller;
                    break;
                }
            }

            return caller;
        }

        private static bool IsDynamic(StackFrame frame)
        {
            return frame.GetMethod().DeclaringType == null;
        }

        private static bool IsCurrentAssembly(StackFrame frame)
        {
            return frame.GetMethod().DeclaringType.Assembly == typeof(CallerIdentifier).Assembly;
        }

        private static bool IsDotNet(StackFrame frame)
        {
            return frame.GetMethod().DeclaringType.Namespace
                .StartsWith("system", StringComparison.InvariantCultureIgnoreCase);
        }

        private static string ExtractVariableNameFrom(StackFrame frame)
        {
            string caller = null;

            string sourceFileName = frame.GetFileName();
            int lineNumber = frame.GetFileLineNumber();
            int column = frame.GetFileColumnNumber();

            if (!string.IsNullOrEmpty(sourceFileName) && (lineNumber != 0) && (column != 0))
            {
                // SMELL: read line by line
                string[] lines = File.ReadAllLines(frame.GetFileName());
                string line = lines[lineNumber - 1];
                string statement = line.Substring(column - 1);

                int indexOfShould = statement.IndexOf("Should", StringComparison.InvariantCulture);
                if (indexOfShould != -1)
                {
                    string candidate  = statement.Substring(0, indexOfShould - 1);
                    if (!IsBooleanLiteral(candidate) && !IsNumeric(candidate) && !IsStringLiteral(candidate) && !UsesNewKeyword(candidate))
                    {
                        caller = candidate;
                    }
                }
            }

            return caller;
        }

        private static bool UsesNewKeyword(string candidate)
        {
            return new Regex(@"new(\s?\[|\s?\{|\s\w+)").IsMatch(candidate);
        }

        private static bool IsStringLiteral(string candidate)
        {
            return candidate.StartsWith("\"");
        }

        private static bool IsNumeric(string candidate)
        {
            return double.TryParse(candidate, out _);
        }

        private static bool IsBooleanLiteral(string candidate)
        {
            return (candidate == "true") || (candidate == "false");
        }

#else
        public static string DetermineCallerIdentity()
        {
            return null;
        }
#endif
    }
}
