using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace FluentAssertions
{
    /// <summary>
    /// Contains static methods to help with exception assertions on actions.
    /// </summary>
    [DebuggerNonUserCode]
    public static class FluentActions
    {
        /// <summary>
        /// Invokes the specified action so that you can assert that it throws an exception.
        /// </summary>
        [Pure]
        public static Action Invoking(Action action) => action;

        /// <summary>
        /// Invokes the specified action so that you can assert that it throws an exception.
        /// </summary>
        [Pure]
        public static Func<T> Invoking<T>(Func<T> func) => func;

        /// <summary>
        /// Invokes the specified action so that you can assert that it throws an exception.
        /// </summary>
        [Pure]
        public static Func<Task> Awaiting(Func<Task> action) => action;

        /// <summary>
        /// Invokes the specified action so that you can assert that it throws an exception.
        /// </summary>
        [Pure]
        public static Func<Task<T>> Awaiting<T>(Func<Task<T>> func) => func;

        /// <summary>
        /// Forces enumerating a collection. Should be used to assert that a method that uses the
        /// <c>yield</c> keyword throws a particular exception.
        /// </summary>
        [Pure]
        public static Action Enumerating(Func<IEnumerable> enumerable) => enumerable.Enumerating();

        /// <summary>
        /// Forces enumerating a collection. Should be used to assert that a method that uses the
        /// <c>yield</c> keyword throws a particular exception.
        /// </summary>
        [Pure]
        public static Action Enumerating<T>(Func<IEnumerable<T>> enumerable) => enumerable.Enumerating();
    }
}
