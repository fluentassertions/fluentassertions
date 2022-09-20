[![build](https://github.com/fluentassertions/fluentassertions/actions/workflows/build.yml/badge.svg)](https://github.com/fluentassertions/fluentassertions/actions/workflows/build.yml)
[![](https://img.shields.io/github/release/FluentAssertions/FluentAssertions.svg?label=latest%20release)](https://github.com/FluentAssertions/FluentAssertions/releases/latest)
[![](https://img.shields.io/nuget/dt/FluentAssertions.svg?label=nuget%20downloads)](https://www.nuget.org/packages/FluentAssertions)
[![](https://img.shields.io/librariesio/dependents/nuget/FluentAssertions.svg?label=dependent%20libraries)](https://libraries.io/nuget/FluentAssertions)
![](https://img.shields.io/badge/release%20strategy-githubflow-orange.svg)
[![Coverage Status](https://coveralls.io/repos/github/fluentassertions/fluentassertions/badge.svg?branch=master)](https://coveralls.io/github/fluentassertions/fluentassertions?branch=master)
[![kandi X-Ray](https://kandi.openweaver.com/badges/xray.svg)](https://kandi.openweaver.com/csharp/fluentassertions/fluentassertions)

# About this project
A very extensive set of extension methods that allow you to more naturally specify the expected outcome of a TDD or BDD-style unit tests. Targets .NET Framework 4.7, as well as .NET Core 2.1, .NET Core 3.0, .NET 6, .NET Standard 2.0 and 2.1.

See https://www.fluentassertions.com for [background information](https://fluentassertions.com/about/), [usage documentation](https://fluentassertions.com/introduction), an [extensibility guide](https://fluentassertions.com/extensibility/), support information and more [tips & tricks](https://fluentassertions.com/tips/).

# Who created this?
Originally authored by Dennis Doomen with Jonas Nyrup as the productive side-kick. Notable contributions were provided by Artur Krajewski, Lukas Grützmacher and David Omid.

# How do I build this?
Install Visual Studio 2022 17.0+ or JetBrains Rider 2021.3 as well as the Build Tools 2022 (including the Universal Windows Platform build tools). You will also need to have .NET Framework 4.7 SDK and .NET 6.0 SDK installed. Check [global.json](global.json) for the current minimum required version.

# What are these Approval.Tests?
This is a special set of tests that use the [Verify](https://github.com/VerifyTests/Verify) project to verify whether you've introduced any breaking changes in the public API of the library.

If you've verified the changes and decided they are valid, you can accept them  using `AcceptApiChanges.ps1` or `AcceptApiChanges.sh`. Alternatively, you can use the [Verify Support](https://plugins.jetbrains.com/plugin/17240-verify-support) plug-in to compare the changes and accept them right from inside Rider. See also the [Contribution Guidelines](CONTRIBUTING.md).

# Powered By
<a href="https://aws.amazon.com/"><img src="docs/assets/images/aws.png" style="width:100px"/></a>&nbsp;
<a href="https://www.infosupport.com/"><img src="docs/assets/images/info-support.jpg" style="width:100px"/></a>&nbsp;
<a href="https://www.jetbrains.com/rider/"><img src="docs/assets/images/jetbrainsrider.svg" style="width:150px"/></a>&nbsp;<a href="https://www.semanticmerge.com/"><img src="docs/assets/images/semantic-merge.png" style="width:150px"/></a>

With support from the following public [sponsors](https://github.com/sponsors/fluentassertions)  
<a href="https://github.com/waywedo"><img src="https://avatars.githubusercontent.com/u/20328638?s=52&v=4"/></a>
<a href="https://github.com/rena0157"><img src="https://avatars.githubusercontent.com/u/33334607?s=52&v=4"/></a>
<a href="https://github.com/hassanhabib"><img src="https://avatars.githubusercontent.com/u/1453985?s=52&v=4"/></a>
<a href="https://github.com/DerAlbertCom"><img src="https://avatars.githubusercontent.com/u/136992?s=52&v=4"/></a>
<a href="https://github.com/mediaclip"><img src="https://avatars.githubusercontent.com/u/6798228?s=52&v=4"/></a>
<a href="https://github.com/eNeRGy164"><img src="https://avatars.githubusercontent.com/u/10671831?s=52&v=4"/></a>
<a href="https://github.com/rash127"><img src="https://avatars.githubusercontent.com/u/110633040?s=52&v=4"/></a>
<a href="https://github.com/michaeldera"><img src="https://avatars.githubusercontent.com/u/12817839?s=52&v=4"/></a>

