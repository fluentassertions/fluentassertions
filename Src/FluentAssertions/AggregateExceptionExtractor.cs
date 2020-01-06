using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;
using FluentAssertions.Specialized;

namespace FluentAssertions
{
    public class AggregateExceptionExtractor : IExtractExceptions
    {
        public IEnumerable<T> OfType<T>(Exception actualException)
            where T : Exception
        {
            if (typeof(T).IsSameOrInherits(typeof(AggregateException)))
            {
                return (actualException is T exception) ? new[] { exception } : Enumerable.Empty<T>();
            }

            return GetExtractedExceptions<T>(actualException);
        }

        private static List<T> GetExtractedExceptions<T>(Exception actualException)
            where T : Exception
        {
            var exceptions = new List<T>();

            if (actualException is AggregateException aggregateException)
            {
                AggregateException flattenedExceptions = aggregateException.Flatten();

                exceptions.AddRange(flattenedExceptions.InnerExceptions.OfType<T>());
            }
            else if (actualException is T genericException)
            {
                exceptions.Add(genericException);
            }

            return exceptions;
        }
    }
}
