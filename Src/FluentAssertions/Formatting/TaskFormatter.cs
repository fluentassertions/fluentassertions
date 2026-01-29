using System.Threading.Tasks;

namespace FluentAssertions.Formatting;

/// <summary>
/// Provides a human-readable version of a generic or non-generic <see cref="Task"/>
/// including its state.
/// </summary>
public class TaskFormatter : IValueFormatter
{
    public bool CanHandle(object value)
    {
        return value is Task;
    }

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        var task = (Task)value;
        formatChild("type", task.GetType(), formattedGraph);
        formattedGraph.AddFragment($" {{Status={task.Status}}}");
    }
}
