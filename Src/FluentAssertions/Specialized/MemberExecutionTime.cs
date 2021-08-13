using System;
using System.Linq.Expressions;
using FluentAssertions.Common;

namespace FluentAssertions.Specialized
{
    public class MemberExecutionTime<T> : ExecutionTime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberExecutionTime{T}"/> class.
        /// </summary>
        /// <param name="subject">The object that exposes the method or property.</param>
        /// <param name="action">A reference to the method or property to measure the execution time of.</param>
        /// <exception cref="NullReferenceException"><paramref name="subject"/> is <c>null</c>.</exception>
        /// <exception cref="NullReferenceException"><paramref name="action"/> is <c>null</c>.</exception>
        public MemberExecutionTime(T subject, Expression<Action<T>> action, StartTimer createTimer)
            : base(() => action.Compile()(subject), "(" + action.Body + ")", createTimer)
        {
        }
    }
}
