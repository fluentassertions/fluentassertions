#nullable enable

namespace FluentAssertions;

public interface IAssertions<out TSubject>
{
    TSubject? Subject { get; }
}
