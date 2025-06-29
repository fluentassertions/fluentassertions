using System;
using System.Text;
using System.Threading;

namespace FluentAssertions.Equivalency.Tracing;

public class StringBuilderTraceWriter : ITraceWriter
{
    private readonly AsyncLocal<AsyncState> stateProvider = new();

    public void AddSingle(string trace)
    {
        WriteLine(trace);
    }

    public IDisposable AddBlock(string trace)
    {
        WriteLine(trace);
        WriteLine("{");
        State.Depth++;

        return new Disposable(() =>
        {
            State.Depth--;
            WriteLine("}");
        });
    }

    private void WriteLine(string trace)
    {
        StringBuilder stringBuilder = State.Builder;

        foreach (string traceLine in trace.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            stringBuilder.Append(new string(' ', State.Depth * 2)).AppendLine(traceLine);
        }
    }

    public override string ToString()
    {
        return State.Builder.ToString();
    }

    private AsyncState State
    {
        get
        {
            stateProvider.Value ??= new();

            return stateProvider.Value;
        }
    }

    private class AsyncState
    {
        public StringBuilder Builder { get; } = new();

        public int Depth { get; set; } = 1;
    }
}
