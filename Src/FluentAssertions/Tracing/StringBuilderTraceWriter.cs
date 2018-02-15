using System;
using System.Text;

namespace FluentAssertions.Equivalency
{
    public class StringBuilderTraceWriter : ITraceWriter
    {
        private readonly StringBuilder builder = new StringBuilder();
        private int depth = 1;

        public void AddSingle(string trace)
        {
            WriteLine(trace);
        }

        public IDisposable AddBlock(string trace)
        {
            WriteLine(trace);
            WriteLine("{");
            depth++;

            return new Disposable(() =>
            {
                depth--;
                WriteLine("}");
            });
        }

        private void WriteLine(string trace)
        {
            foreach (string traceLine in trace.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                builder.Append(new string(' ', depth * 2)).AppendLine(traceLine);
            }
        }

        public override string ToString()
        {
            return builder.ToString();
        }
    }
}
