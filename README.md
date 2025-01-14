<a href="https://www.fluentassertions.com"><img src="docs/assets/images/FA_Partner_Logo.png" style="width:500px"/></a>

# Extension methods to fluently assert the outcome of .NET tests
[![](https://img.shields.io/github/actions/workflow/status/fluentassertions/fluentassertions/build.yml?branch=develop)](https://github.com/fluentassertions/fluentassertions/actions?query=branch%3Adevelop)
[![Coveralls branch](https://img.shields.io/coverallsCoverage/github/fluentassertions/fluentassertions?branch=main)](https://coveralls.io/github/fluentassertions/fluentassertions?branch=main)
[![qodana](https://github.com/fluentassertions/fluentassertions/actions/workflows/code_quality.yml/badge.svg)](https://github.com/fluentassertions/fluentassertions/actions/workflows/code_quality.yml)
[![](https://img.shields.io/github/release/FluentAssertions/FluentAssertions.svg?label=latest%20release&color=007edf)](https://github.com/FluentAssertions/FluentAssertions/releases/latest)
[![](https://img.shields.io/nuget/dt/FluentAssertions.svg?label=downloads&color=007edf&logo=nuget)](https://www.nuget.org/packages/FluentAssertions)
[![](https://img.shields.io/librariesio/dependents/nuget/FluentAssertions.svg?label=dependent%20libraries)](https://libraries.io/nuget/FluentAssertions)
[![GitHub Repo stars](https://img.shields.io/github/stars/fluentassertions/fluentassertions)](https://github.com/fluentassertions/fluentassertions/stargazers)
[![GitHub contributors](https://img.shields.io/github/contributors/fluentassertions/fluentassertions)](https://github.com/fluentassertions/fluentassertions/graphs/contributors)
[![GitHub last commit](https://img.shields.io/github/last-commit/fluentassertions/fluentassertions)](https://github.com/fluentassertions/fluentassertions)
[![GitHub commit activity](https://img.shields.io/github/commit-activity/m/fluentassertions/fluentassertions)](https://github.com/fluentassertions/fluentassertions/graphs/commit-activity)
[![open issues](https://img.shields.io/github/issues/fluentassertions/fluentassertions)](https://github.com/fluentassertions/fluentassertions/issues)
![](https://img.shields.io/badge/release%20strategy-githubflow-orange.svg)

Fluent Assertions provides a comprehensive set of extension methods that enable developers to express the expected outcomes of TDD (Test-Driven Development) and BDD (Behavior-Driven Development) unit tests in a natural, readable style. It is compatible with .NET Standard 2.0+, .NET Framework 4.7+, and .NET 6+.

Visit https://www.fluentassertions.com for [background information](https://fluentassertions.com/about/), [usage documentation](https://fluentassertions.com/introduction), an [extensibility guide](https://fluentassertions.com/extensibility/), support information and more [tips & tricks](https://fluentassertions.com/tips/).

![](https://repobeats.axiom.co/api/embed/282ed7bca0ede1ac7751ebde6b3ef091a0c6c52d.svg)

# Xceed Partnership FAQ
Xceed is now an official Partner to Fluent Assertions! [Learn what this partnership means for our users](https://xceed.com/fluent-assertions-faq/). After extensive discussions with the Fluent Assertions team, we are thrilled about the future of the product and look forward to its continued growth and development.

# Who created this?
Originally authored by Dennis Doomen with Jonas Nyrup as the productive side-kick. Notable contributions were provided by Artur Krajewski, Lukas Grützmacher and David Omid.

# How do I build this?
Install Visual Studio 2022 17.8+ or JetBrains Rider 2021.3 as well as the Build Tools 2022 (including the Universal Windows Platform build tools). You will also need to have .NET Framework 4.7 SDK and .NET 8.0 SDK installed. Check [global.json](global.json) for the current minimum required version.

# What are these Approval.Tests?
This is a special set of tests that use the [Verify](https://github.com/VerifyTests/Verify) project to verify whether you've introduced any breaking changes in the public API of the library.

If you've verified the changes and decided they are valid, you can accept them  using `AcceptApiChanges.ps1` or `AcceptApiChanges.sh`. Alternatively, you can use the [Verify Support](https://plugins.jetbrains.com/plugin/17240-verify-support) plug-in to compare the changes and accept them right from inside Rider. See also the [Contribution Guidelines](CONTRIBUTING.md).

# Powered By
<a href="https://www.xceed.com"><img src="docs/assets/images/xceed_logo_whiteB.png" style="width:192px"/></a>
