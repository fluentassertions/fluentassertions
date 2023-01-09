using System.Linq;
using LibGit2Sharp;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.Tools.DotNet;

[UnsetVisualStudioEnvironmentVariables]
[DotNetVerbosityMapping]
partial class Build : NukeBuild
{
    [Parameter("The target branch for the pull request")]
    readonly string PullRequestBase;

    [GitRepository]
    readonly GitRepository GitRepository;

    Repository Repository => new(GitRepository.LocalDirectory);
    Tree TargetBranch => Repository.Branches[PullRequestBase].Tip.Tree;
    Tree SourceBranch => Repository.Branches[Repository.Head.FriendlyName].Tip.Tree;

    bool RunAllTargets => string.IsNullOrWhiteSpace(PullRequestBase);

    string[] Changes =>
        Repository.Diff
            .Compare<TreeChanges>(TargetBranch, SourceBranch)
            .Where(x => x.Exists)
            .Select(x => x.Path)
            .ToArray();

    /* Support plugins are available for:
       - JetBrains ReSharper        https://nuke.build/resharper
       - JetBrains Rider            https://nuke.build/rider
       - Microsoft VisualStudio     https://nuke.build/visualstudio
       - Microsoft VSCode           https://nuke.build/vscode
    */

    public static int Main() => Execute<Build>(x => x.SpellCheck, x => x.Push);
}
