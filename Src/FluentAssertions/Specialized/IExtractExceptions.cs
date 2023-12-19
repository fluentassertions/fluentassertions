using System;
using System.Collections.Generic;

namespace FluentAssertionsAsync.Specialized;

public interface IExtractExceptions
{
    IEnumerable<T> OfType<T>(Exception actualException)
        where T : Exception;
}
