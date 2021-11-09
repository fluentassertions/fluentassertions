# Contributing to Fluent Assertions

No open-source project is going to be successful without contributions. After we decided to move to Github, the involvement of the .NET community has increased significantly. However, contributing to this project involves a few steps that will seriously increase the chance we will accept it.

* The [Pull Request](https://help.github.com/articles/using-pull-requests) is targeted at the `master` branch.
* The code complies with the [Coding Guidelines for C#](https://csharpcodingguidelines.com/).
* The changes are covered by a new or existing set of unit tests which follow the Arrange-Act-Assert syntax such as is used [in this example](https://github.com/fluentassertions/fluentassertions/blob/daaf35b9b59b622c96d0c034e8972a020b2bee55/Tests/FluentAssertions.Shared.Specs/BasicEquivalencySpecs.cs#L33).
* If the contribution adds a feature or fixes a bug, please update the [**release notes**](https://github.com/fluentassertions/fluentassertions/blob/master/docs/_pages/releases.md), which is published on the [website](https://fluentassertions.com/releases).
* If the contribution changes the public API the changes needs to be included by running [`AcceptApiChanges.ps1`](https://github.com/fluentassertions/fluentassertions/tree/master/AcceptApiChanges.ps1)/[`AcceptApiChanges.sh`](https://github.com/fluentassertions/fluentassertions/tree/master/AcceptApiChanges.sh).
* If the contribution affects the documentation, please update the [**documentation**](https://github.com/fluentassertions/fluentassertions/tree/master/docs/_pages), under the appropriate file (i.e. [strings.md](https://github.com/fluentassertions/fluentassertions/blob/master/docs/_pages/strings.md) for changes to string assertions), which is published on the [website](https://fluentassertions.com/introduction).
