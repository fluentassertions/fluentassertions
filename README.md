[![](https://ci.appveyor.com/api/projects/status/h60mq3e5uf5tuout/branch/master?svg=true)](https://ci.appveyor.com/project/dennisdoomen/fluentassertions/branch/master)
[![](https://img.shields.io/github/release/FluentAssertions/FluentAssertions.svg?label=latest%20release)](https://github.com/FluentAssertions/FluentAssertions/releases/latest)
[![](https://img.shields.io/nuget/dt/FluentAssertions.svg?label=nuget%20downloads)](https://www.nuget.org/packages/FluentAssertions)
[![](https://img.shields.io/librariesio/dependents/nuget/FluentAssertions.svg?label=dependent%20libraries)](https://libraries.io/nuget/FluentAssertions)
![](https://img.shields.io/badge/release%20strategy-githubflow-orange.svg)

# About this project
A very extensive set of extension methods that allow you to more naturally specify the expected outcome of a TDD or BDD-style unit tests. Targets .NET Framework 4.7, as well as .NET Core 2.1, .NET Core 3.0, .NET Standard 2.0 and 2.1.

See https://www.fluentassertions.com for [background information](https://fluentassertions.com/about/), [usage documentation](https://fluentassertions.com/introduction), an [extensibility guide](https://fluentassertions.com/extensibility/), support information and more [tips & tricks](https://fluentassertions.com/tips/).

# Who created this?
Originally authored by Dennis Doomen with Jonas Nyrup as the productive side-kick. Notable contributions were provided by Artur Krajewski, Lukas Grützmacher and David Omid.

# How do I build this?
Install Visual Studio 2019 16.9+ or JetBrains Rider 2017.1 and Build Tools 2017 and run.
You will also need to have .NET Framework 4.7 SDK and .NET 5.0 SDK installed - check [global.json](global.json) for the current minimum required version.

# What are these Approval.Tests?
This is a special set of tests that use the [Verify](https://github.com/VerifyTests/Verify) project to verify whether you've introduced any breaking changes in the public API of the library.

If you've verified the changes and decided they are valid, you can accept them  using `AcceptApiChanges.ps1` or `AcceptApiChanges.sh`.
See also the [Contribution Guidelines.](CONTRIBUTING.md).

`build.ps1`

# Powered By
Supported by Patreons [**Michaël Hompus**](https://hompus.nl/), [**Artur Krajewski**](https://github.com/krajek) and [**Matthias Koch**](https://ithrowexceptions.com/)

Also made possible by<br>
    <a href="https://www.jetbrains.com/rider/"><img src="docs/assets/images/jetbrainsrider.svg" style="width:150px"/></a> <br/>and <br>
    <a href="https://www.semanticmerge.com/"><img src="docs/assets/images/semantic-merge.png" style="width:150px"/></a>
