namespace FluentAssertions.Execution;

/// <summary>
/// This is a marker interface for xUnit.net v3 to set the test failure cause as an assertion failure.
/// See <a href="https://xunit.net/docs/getting-started/v3/whats-new#third-party-assertion-library-extension-points">What’s New in xUnit.net v3 - Third party assertion library extension points</a>.
/// </summary>
internal interface IAssertionException;
