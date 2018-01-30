---
layout: default
---

<!-- -->

## What is Fluent Assertions?

> With Fluent Assertions, the assertions look beautiful, natural and most  importantly, extremely readable -[_Girish_](https://twitter.com/girishracharya)

> everyone.Should().Like(FluentAssertions, "because everything is so damn cool and readable"); - [Jim Speaker](https://twitter.com/jspeaker) and [Bart Roozendaal](https://twitter.com/BartRoozendaal) 

Fluent Assertions is a set of .NET extension methods that allow you to more naturally specify the expected outcome of unit tests. It offers a plethora of features:

* Targets .NET 4.5, .NET Standard 1.4, 1.6 and 2.0.
* Works with MSTest, xUnit, NUnit, Gallio, MBUnit, MSpec and NSpec.
* Very detailed and self-explanatory failure message. 
* Parses the C# code to identify the subject-under-test.
* Allows asserting the equivalency of (collections of ) deep object graphs.
* Fully thread-safe.
* Specialized assertions for `async` code.
* Execution time assertions.
* Type, `[attribute]` and assembly-level dependency assertions.
* Assert on raised `event`s and usage of `INotifyPropertyChanged`.
* Represents dates and times as `20.September(1973).At(8, 1)`.
* Lots of community extensions. 
* Almost 7 millions downloads on Nuget.
* Well-documented extensibility points.

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

## How to get help
* For general questions, check out  [StackOverflow](https://stackoverflow.com/questions/tagged/fluent-assertions?mixed=1).
* To discuss issues or ask questions to the team directly, use [Slack](https://fluentassertionsslack.herokuapp.com/)
* To report an issue or request, create an issue on [Github](https://github.com/fluentassertions/fluentassertions/issues).

## Support us
As you can imagine, a lot of our private time is put into this wonderful project. Please support us by becoming a sponsor through Patreon. 

<a href="https://www.patreon.com/bePatron?u=9250052" data-patreon-widget-type="become-patron-button">Become a Patron!</a><script async src="https://c6.patreon.com/becomePatronButton.bundle.js"></script>

I also welcome one-time donations through PayPal.

<form action="https://www.paypal.com/cgi-bin/webscr" method="post" target="_top">
<input type="hidden" name="cmd" value="_s-xclick">
<input type="hidden" name="encrypted" value="-----BEGIN PKCS7-----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-----END PKCS7-----
">
<input type="image" src="https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif" border="0" name="submit" alt="PayPal - The safer, easier way to pay online!">
<img alt="" border="0" src="https://www.paypalobjects.com/nl_NL/i/scr/pixel.gif" width="1" height="1">
</form>


## Community Extensions

There are a number of community maintained extension projects. The ones we are aware of a listed below. To add yours please fork the [repository](https://github.com/fluentassertions/fluentassertions/) and send a pull request.

* [FluentAssertions.Json](https://github.com/fluentassertions/fluentassertions.json)
*   [FluentAssertions.Ioc.Ninject](https://github.com/kevinkuszyk/FluentAssertions.Ioc.Ninject) for testing Ninject bindings.
*   [FluentAssertions.Ioc.Autofac](https://github.com/awesome-inc/FluentAssertions.Autofac) for testing Autofac configurations.
*   [FluentAssertions.Mvc](https://github.com/CaseyBurns/FluentAssertions.MVC) for testing MVC applications.
* [FluentAssertions.AspnetCore.Mvc](https://github.com/fluentassertions/fluentassertions.aspnetcore.mvc)
*   [FluentAssertions.Analyzers](https://github.com/fluentassertions/fluentassertions.analyzers) which adds Roslyn based Visual Studio integration and quick fixes.

## Special thanks

This project would not have been possible without the support of [JetBrains](http://www.jetbrains.com/). We thank them generously for providing us with the [ReSharper](http://www.jetbrains.com/resharper/) licenses necessary to make us productive developers.  

![Resharper](./images/logo_resharper.png)

{% include twitter.html %}
