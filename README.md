<a href="https://www.fluentassertions.com"><img src="docs/assets/images/fluent_assertions_large_horizontal.svg" style="width:400px"/></a>

# Extension methods to fluently assert the outcome of .NET tests
[![](https://img.shields.io/github/actions/workflow/status/fluentassertions/fluentassertions/build.yml?branch=develop)](https://github.com/fluentassertions/fluentassertions/actions?query=branch%3Adevelop)
[![Coveralls branch](https://img.shields.io/coverallsCoverage/github/fluentassertions/fluentassertions?branch=develop)](https://coveralls.io/github/fluentassertions/fluentassertions?branch=develop)
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

A very extensive set of extension methods that allow you to more naturally specify the expected outcome of a TDD or BDD-style unit tests. Targets .NET Framework 4.7, as well as .NET 6, .NET Standard 2.0 and 2.1.

See https://www.fluentassertions.com for [background information](https://fluentassertions.com/about/), [usage documentation](https://fluentassertions.com/introduction), an [extensibility guide](https://fluentassertions.com/extensibility/), support information and more [tips & tricks](https://fluentassertions.com/tips/).

![](https://repobeats.axiom.co/api/embed/282ed7bca0ede1ac7751ebde6b3ef091a0c6c52d.svg)

# Who created this?
Originally authored by Dennis Doomen with Jonas Nyrup as the productive side-kick. Notable contributions were provided by Artur Krajewski, Lukas Grützmacher and David Omid.

# How do I build this?
Install Visual Studio 2022 17.0+ or JetBrains Rider 2021.3 as well as the Build Tools 2022 (including the Universal Windows Platform build tools). You will also need to have .NET Framework 4.7 SDK and .NET 7.0 SDK installed. Check [global.json](global.json) for the current minimum required version.

# What are these Approval.Tests?
This is a special set of tests that use the [Verify](https://github.com/VerifyTests/Verify) project to verify whether you've introduced any breaking changes in the public API of the library.

If you've verified the changes and decided they are valid, you can accept them  using `AcceptApiChanges.ps1` or `AcceptApiChanges.sh`. Alternatively, you can use the [Verify Support](https://plugins.jetbrains.com/plugin/17240-verify-support) plug-in to compare the changes and accept them right from inside Rider. See also the [Contribution Guidelines](CONTRIBUTING.md).

# Powered By
<a href="https://www.infosupport.com/"><img src="docs/assets/images/info-support.jpg" style="width:100px"/></a>&nbsp;
<a href="https://www.jetbrains.com/rider/"><img src="docs/assets/images/jetbrainsrider.svg" style="width:150px"/></a>&nbsp;

With support from the following public [sponsors](https://github.com/sponsors/fluentassertions)
<a href="https://github.com/BestKru"><img src="https://avatars.githubusercontent.com/u/159320286?s=52&v=4"/></a>
<a href="https://github.com/Infra-Workleap"><img src="https://avatars.githubusercontent.com/u/53535748?s=52&v=4"/></a>
<a href="https://github.com/ken-swyfft"><img src="https://avatars.githubusercontent.com/u/65305317?s=52&v=4"/></a>
<a href="https://github.com/MGundersen"><img src="https://avatars.githubusercontent.com/u/15629960?s=52&v=4"/></a>
<a href="https://github.com/mediaclip"><img src="https://avatars.githubusercontent.com/u/6798228?s=52&v=4"/></a>
<a href="https://github.com/hassanhabib"><img src="https://avatars.githubusercontent.com/u/1453985?s=52&v=4"/></a>
