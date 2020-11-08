using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
            var comparisonType = StringComparison.OrdinalIgnoreCase;

            return frameNamespace?.StartsWith("system.", comparisonType) == true ||
                frameNamespace?.Equals("system", comparisonType) == true;
        }

        private static string ExtractVariableNameFrom(StackFrame frame)
        {
            string caller = null;
            string statement = GetSourceCodeStatementFrom(frame);

            if (statement != null)
            {
                logger(statement);
                if (!IsBooleanLiteral(statement) && !IsNumeric(statement) && !IsStringLiteral(statement) &&
                    !UsesNewKeyword(statement))
                {
                    caller = statement;
                }
            }

            return caller;
        }

        private static string GetSourceCodeStatementFrom(StackFrame frame)
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

                if (currentLine == expectedLineNumber && line != null)
                {
                    return GetSourceCodeStatementFrom(frame, reader, line);
                }

                return null;
            }
            catch
            {
                // We don't care. Just assume the symbol file is not available or unreadable
                return null;
            }
        }

        private static string GetSourceCodeStatementFrom(StackFrame frame, StreamReader reader, string line)
        {
            int column = frame.GetFileColumnNumber();
            if (column > 0)
            {
                line = line.Substring(Math.Min(column - 1, line.Length - 1));
            }

            var sb = new StatementBuilder();
            StatementBuilder.Result state;
            do
            {
                state = sb.Append(line);
            }
            while (state == StatementBuilder.Result.InProgress && (line = reader.ReadLine()) != null);

            return state == StatementBuilder.Result.Retrieved ? sb.ToString() : null;
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

        private class StatementBuilder
        {
            private const int ShouldCallLength = 7;
            private readonly StringBuilder stringBuilder = new StringBuilder();
            private char? previousChar;
            private char isQuoteEscapeSymbol = '\\';
            private bool isQuoteContext;

            public Result Append(string symbols)
            {
                foreach (char currentChar in symbols)
                {
                    if (!char.IsWhiteSpace(currentChar) || isQuoteContext)
                    {
                        stringBuilder.Append(currentChar);
                    }

                    if (currentChar == '"')
                    {
                        if (isQuoteContext)
                        {
                            if (previousChar != isQuoteEscapeSymbol)
                            {
                                isQuoteContext = false;
                                isQuoteEscapeSymbol = '\\';
                            }
                        }
                        else
                        {
                            isQuoteContext = true;
                            if (IsAtEscaped())
                            {
                                isQuoteEscapeSymbol = '"';
                            }
                        }
                    }

                    previousChar = currentChar;
                    if (!isQuoteContext)
                    {
                        if (currentChar == ';')
                        {
                            return Result.NoStatement;
                        }
                        else if (IsShouldCall())
                        {
                            stringBuilder.Remove(stringBuilder.Length - ShouldCallLength, ShouldCallLength);
                            return Result.Retrieved;
                        }
                    }
                }

                return Result.InProgress;
            }

            private bool IsAtEscaped()
            {
                if (previousChar == '@')
                {
                    return true;
                }

                if (stringBuilder.Length > 1)
                {
                    var idx = stringBuilder.Length - 1;
                    return stringBuilder[idx--] == '$'
                        && stringBuilder[idx] == '@';
                }

                return false;
            }

            private bool IsShouldCall()
            {
                if (stringBuilder.Length >= ShouldCallLength)
                {
                    var idx = stringBuilder.Length - 1;
                    return stringBuilder[idx--] == 'd'
                        && stringBuilder[idx--] == 'l'
                        && stringBuilder[idx--] == 'u'
                        && stringBuilder[idx--] == 'o'
                        && stringBuilder[idx--] == 'h'
                        && stringBuilder[idx--] == 'S'
                        && stringBuilder[idx] == '.';
                }

                return false;
            }

            public override string ToString() => stringBuilder.ToString();

            public enum Result
            {
                InProgress,
                Retrieved,
                NoStatement
            }
        }
    }
}
