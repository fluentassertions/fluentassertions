---
layout: default
---
<img src="./images/logo.png" width="250" style="float:right">

## Installation

Install with [NuGet](https://www.nuget.org/packages/FluentAssertions/) [![NuGet](https://img.shields.io/nuget/vpre/FluentAssertions.svg)](https://www.nuget.org/packages/FluentAssertions)

> PM > Install-Package FluentAssertions

## News
{% for release in site.github.releases limit: 1 %}

**Fluent Assertions {{ release.name }}** was released on **{{ release.published_at | date_to_long_string }}**

Changes: 
  > {{ release.body }}

View [Release on GitHub]( {{ release.html_url }} )
    
{% endfor %}

## Supported Frameworks and Libraries

Fluent Assertions supports the following .NET versions:

*   .NET 4.0, 4.5 and 4.6
    * [Fluent Assertions 2.2](https://www.nuget.org/packages/FluentAssertions/2.2.0) supports .NET 3.5
*   CoreCLR, .NET Native, and Universal Windows Platform
*   Windows Store Apps for Windows 8.1
*   Silverlight 5
*   Windows Phone 8.1
*   Windows Phone Silverlight 8.0 and 8.1
*   Portable Class Libraries

Fluent Assertions supports the following unit test frameworks:

*   MSTest (Visual Studio 2010, 2012 Update 2, 2013 and 2015)
*   MSTest2 (Visual Studio 2017)
*   [NUnit](http://www.nunit.org/)
*   [XUnit](http://xunit.codeplex.com/)
*   [XUnit2](https://github.com/xunit/xunit/releases)
*   [MBUnit](http://code.google.com/p/mb-unit/)
*   [Gallio](http://code.google.com/p/mb-unit/)
*   [NSpec](http://nspec.org/)
*   [MSpec](https://github.com/machine/machine.specifications)

## Community Extensions

There are a number of community maintained extension projects. The ones we are aware of a listed below. To add yours please fork the [repository](https://github.com/dennisdoomen/fluentassertions/tree/gh-pages) and send a pull request.

*   [FluentAssertions.Ioc.Ninject](https://github.com/kevinkuszyk/FluentAssertions.Ioc.Ninject) for testing Ninject bindings.
*   [FluentAssertions.Mvc](https://github.com/CaseyBurns/FluentAssertions.MVC) for testing MVC applications.
*   [Xamarin](https://github.com/onovotny/fluentassertions) version for Mono support.
*   [FluentAssertions.Autofac](https://github.com/awesome-inc/FluentAssertions.Autofac) for testing Autofac configurations.

## Special thanks

This project would not have been possible without the support of [JetBrains](http://www.jetbrains.com/). We thank them generously for providing us with the [ReSharper](http://www.jetbrains.com/resharper/) licenses necessary to make us productive developers.  
![Resharper](./images/logo_resharper.png)

{% include twitter.html %}