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

## Support us
As you can imagine, a lot of our private time is put into this wonderful project. Please support us:

<form action="https://www.paypal.com/cgi-bin/webscr" method="post" target="_top">
<input type="hidden" name="cmd" value="_s-xclick">
<input type="hidden" name="encrypted" value="-----BEGIN PKCS7-----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-----END PKCS7-----
">
<input type="image" src="https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif" border="0" name="submit" alt="PayPal - The safer, easier way to pay online!">
<img alt="" border="0" src="https://www.paypalobjects.com/nl_NL/i/scr/pixel.gif" width="1" height="1">
</form>


## Special thanks

This project would not have been possible without the support of [JetBrains](http://www.jetbrains.com/). We thank them generously for providing us with the [ReSharper](http://www.jetbrains.com/resharper/) licenses necessary to make us productive developers.  
![Resharper](./images/logo_resharper.png)

{% include twitter.html %}
