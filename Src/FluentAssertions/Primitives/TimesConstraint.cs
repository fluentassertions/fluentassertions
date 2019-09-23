using System;

namespace FluentAssertions.Primitives
{
    public class TimesConstraint
    {
        private readonly int expectedCount;
        private readonly string mode;

        internal const string AtLeastMode = "at least";
        internal const string AtMostMode = "at most";
        internal const string MoreThanMode = "more than";
        internal const string LessThanMode = "less than";
        internal const string ExactlyMode = "exactly";

        internal TimesConstraint(int expectedCount, string mode)
        {
            this.expectedCount = expectedCount;
            this.mode = mode;
        }

        public string Expected { get; set; }

        public string Subject { get; set; }

        public string Containment { get; set; }

        public StringComparison StringComparison { get; set; }

        public bool IsMatch
        {
            get
            {
                int count = CountOccurrences(Subject, Expected, StringComparison);

                switch (mode)
                {
                    case AtLeastMode:
                        return count >= expectedCount;
                    case AtMostMode:
                        return count <= expectedCount;
                    case MoreThanMode:
                        return count > expectedCount;
                    case LessThanMode:
                        return count < expectedCount;
                    case ExactlyMode:
                        return count == expectedCount;
                    default:
                        return false;
                }
            }
        }

        public string MessageFormat
        {
            get
            {
                int count = CountOccurrences(Subject, Expected, StringComparison);
                string mode = $"{this.mode} {(expectedCount == 1 ? "1 time" : $"{expectedCount} times")}";
                string format = GenerateMessageFormat(Containment, mode, count);

                return format;
            }
        }

        private static int CountOccurrences(string subject, string expected, StringComparison comparison)
        {
            string actual = subject ?? "";
            string substring = expected ?? "";

            int count = 0;
            int index = 0;

            while ((index = actual.IndexOf(substring, index, comparison)) >= 0)
            {
                index += substring.Length;
                count++;
            }

            return count;
        }

        private static string GenerateMessageFormat(string containment, string mode, int count)
        {
            string times = count == 1 ? "1 time" : $"{count} times";

            if (mode is null)
            {
                return $"Expected {{context:string}} {{0}} to {containment} {{1}} a custom number of times{{reason}}, but found {times}.";
            }

            return $"Expected {{context:string}} {{0}} to {containment} {{1}} {mode}{{reason}}, but found {times}.";
        }
    }
}
