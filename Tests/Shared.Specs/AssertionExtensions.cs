using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Specialized;

namespace FluentAssertions.Specs
{
    public static class AssertionExtensions
    {
        public static TaskAssertions Should(this Task task, ITimer timer)
        {
            return new TaskAssertions(task, timer);
        }

        public static TaskOfTAssertions<T> Should<T>(this Task<T> task, ITimer timer)
        {
            return new TaskOfTAssertions<T>(task, timer);
        }
    }
}
