[![](https://ci.appveyor.com/api/projects/status/h60mq3e5uf5tuout/branch/master?svg=true)](https://ci.appveyor.com/project/dennisdoomen/fluentassertions/branch/master)
[![](https://img.shields.io/github/release/FluentAssertions/FluentAssertions.svg?label=latest%20release)](https://github.com/FluentAssertions/FluentAssertions/releases/latest)
[![](https://img.shields.io/nuget/dt/FluentAssertions.svg?label=nuget%20downloads)](https://www.nuget.org/packages/FluentAssertions)
[![](https://img.shields.io/librariesio/dependents/nuget/FluentAssertions.svg?label=dependent%20libraries)](https://libraries.io/nuget/FluentAssertions)
![](https://img.shields.io/badge/release%20strategy-githubflow-orange.svg)

# What is this and why do I need this?
See https://www.fluentassertions.com for background information, usage documentation, an extensibility guide, support information and more tips & tricks.

# How do I build this?
Install Visual Studio 2019 or JetBrains Rider 2017.1 and Build Tools 2017 and run

# What are these Approval.Tests?
This is a special set of tests that use the [ApprovalTests.Net](https://github.com/approvals/ApprovalTests.Net) project to verify whether you've introduced any breaking changes in the public API of the library.
By default, the right reporter is selected based on the run-time environment (e.g. Visual Studio diff viewer, Rider's diff viewer, etc).
You can change that by setting the environment variable `ApiApproval.Reporter` to the (fully-qualified) name of [one of the implementations](https://github.com/approvals/ApprovalTests.Net/tree/master/src/ApprovalTests/Reporters) of `IApprovalFailureReporter`.

If you've verified the changes and decided they are valid, you can accept them  using `AcceptApiChanges.ps1` or `AcceptApiChanges.sh`.
See also the [Contribution Guidelines.](CONTRIBUTING.md).

`build.ps1`


