using System.Collections.Generic;
using System.Text;

namespace FluentAssertions.Formatting
{
    public interface IValueFormatter
    {
        bool CanHandle(object value);
        string ToString(object value);
    }
}
