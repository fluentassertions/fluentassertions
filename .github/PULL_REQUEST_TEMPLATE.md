<!-- Please provide a description of your changes above the IMPORTANT checklist -->


## IMPORTANT 

* [ ] If the PR touches the public API, the changes have been approved in a separate issue with the "api-approved" label.
* [ ] The code complies with the [Coding Guidelines for C#](https://www.csharpcodingguidelines.com/).
* [ ] The changes are covered by unit tests which follow the Arrange-Act-Assert syntax and the naming conventions such as is used [in these tests](../tree/develop/Tests/FluentAssertions.Equivalency.Specs/MemberMatchingSpecs.cs#L51-L430).
* [ ] If the PR adds a feature or fixes a bug, please update [the release notes](../tree/develop/docs/_pages/releases.md) with a functional description that explains what the change means to consumers of this library, which are published on the [website](https://fluentassertions.com/releases).
* [ ] If the PR changes the public API the changes needs to be included by running [AcceptApiChanges.ps1](../tree/develop/AcceptApiChanges.ps1) or [AcceptApiChanges.sh](../tree/develop/AcceptApiChanges.sh).
* [ ] If the PR affects [the documentation](../tree/develop/docs/_pages), please include your changes in this pull request so the documentation will appear on the [website](https://www.fluentassertions.com/introduction).
    * [ ] Please also run `./build.sh --target spellcheck` or `.\build.ps1 --target spellcheck` before pushing and check the good outcome

## CONTRIBUTOR LICENSE GRANT

By submitting this contribution, the contributor hereby irrevocably grants to the project owners and maintainers a perpetual, worldwide, royalty-free, irrevocable license to use, reproduce, modify, distribute, sublicense, and create derivative works of the contribution for any purpose and under any terms, including proprietary licensing.

The contributor waives any moral rights in the contribution to the extent permitted by law and agrees not to assert any claim of authorship or control over the contribution. The contributor represents that they are the sole author of the contribution and that it is provided free of any third-party claims.

The contributor understands and agrees that the maintainers may, at their sole discretion, use, license, or redistribute the contribution as part of any work and under any terms they choose, without further permission or attribution.

* [ ] I have read the Contributor License Grant and accept the conditions
* [ ] I'm interested in a free license for Fluent Assertions and will share my email address through sales@xceed.com