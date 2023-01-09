using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Tools.Xunit;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;
using static Nuke.Common.Tools.Xunit.XunitTasks;
using static Serilog.Log;

[UnsetVisualStudioEnvironmentVariables]
[DotNetVerbosityMapping]
class Build : NukeBuild
{
    /* Support plugins are available for:
       - JetBrains ReSharper        https://nuke.build/resharper
       - JetBrains Rider            https://nuke.build/rider
       - Microsoft VisualStudio     https://nuke.build/visualstudio
       - Microsoft VSCode           https://nuke.build/vscode
    */

    public static int Main() => Execute<Build>(x => x.SpellCheck, x => x.Push);

    [Parameter("A branch specification such as develop or refs/pull/1775/merge")]
    readonly string BranchSpec;

    [Parameter("An incrementing build number as provided by the build engine")]
    readonly string BuildNumber;

    [Parameter("The target branch for the pull request")]
    readonly string PullRequestBase;

    [Parameter("The key to push to Nuget")]
    readonly string ApiKey;

    [Solution(GenerateProjects = true)]
    readonly Solution Solution;

    [GitVersion(Framework = "net6.0")]
    readonly GitVersion GitVersion;

    [GitRepository]
    readonly GitRepository GitRepository;

    [PackageExecutable("nspec", "NSpecRunner.exe", Version = "3.1.0")]
    Tool NSpec3;

#if OS_WINDOWS
    [PackageExecutable("Node.js.redist", "node.exe", Version = "16.17.1", Framework = "win-x64")]
#elif OS_MAC
    [PackageExecutable("Node.js.redist", "node", Version = "16.17.1", Framework = "osx-x64")]
#else
    [PackageExecutable("Node.js.redist", "node", Version = "16.17.1", Framework = "linux-x64")]
#endif
    Tool Node;

    AbsolutePath ArtifactsDirectory => RootDirectory / "Artifacts";

    AbsolutePath TestResultsDirectory => RootDirectory / "TestResults";

    string SemVer;

    Target Clean => _ => _
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            EnsureCleanDirectory(ArtifactsDirectory);
            EnsureCleanDirectory(TestResultsDirectory);
        });

    Target CalculateNugetVersion => _ => _
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            SemVer = GitVersion.SemVer;
            if (IsPullRequest)
            {
                Information(
                    "Branch spec {branchspec} is a pull request. Adding build number {buildnumber}",
                    BranchSpec, BuildNumber);

                SemVer = string.Join('.', GitVersion.SemVer.Split('.').Take(3).Union(new[] { BuildNumber }));
            }

            Information("SemVer = {semver}", SemVer);
        });

    bool IsPullRequest => BranchSpec != null && BranchSpec.Contains("pull", StringComparison.InvariantCultureIgnoreCase);

    Target Restore => _ => _
        .DependsOn(Clean)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution)
                .EnableNoCache()
                .SetConfigFile(RootDirectory / "nuget.config"));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration("CI")
                .EnableNoLogo()
                .EnableNoRestore()
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion));
        });

    Target ApiChecks => _ => _
        .DependsOn(Compile)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetConfiguration("Release")
                .SetProcessEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", "en-US")
                .EnableNoBuild()
                .CombineWith(
                    cc => cc.SetProjectFile(Solution.Specs.Approval_Tests)));
        });

    Target UnitTests => _ => _
        .DependsOn(Compile)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            Project[] projects = new[]
            {
                Solution.Specs.FluentAssertions_Specs,
                Solution.Specs.FluentAssertions_Equivalency_Specs
            };

            if (EnvironmentInfo.IsWin)
            {
                Xunit2(s =>
                {
                    IEnumerable<string> testAssemblies = projects
                        .SelectMany(project => GlobFiles(project.Directory, "bin/Debug/net47/*.Specs.dll"));

                    Assert.NotEmpty(testAssemblies.ToList());

                    return s
                        .SetFramework("net47")
                        .AddTargetAssemblies(testAssemblies);
                });
            }

            DotNetTest(s => s
                .SetConfiguration("Debug")
                .SetProcessEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", "en-US")
                .EnableNoBuild()
                .SetDataCollector("XPlat Code Coverage")
                .SetResultsDirectory(TestResultsDirectory)
                .AddRunSetting(
                    "DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.DoesNotReturnAttribute",
                    "DoesNotReturnAttribute")
                .CombineWith(
                    projects,
                    (_, project) => _
                        .SetProjectFile(project)
                        .CombineWith(
                            project.GetTargetFrameworks().Except(new[] { "net47" }),
                            (_, framework) => _.SetFramework(framework)
                        )
                )
            );
        });

    Target CodeCoverage => _ => _
        .DependsOn(TestFrameworks)
        .DependsOn(UnitTests)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            ReportGenerator(s => s
                .SetProcessToolPath(ToolPathResolver.GetPackageExecutable("ReportGenerator", "ReportGenerator.dll", framework: "net6.0"))
                .SetTargetDirectory(TestResultsDirectory / "reports")
                .AddReports(TestResultsDirectory / "**/coverage.cobertura.xml")
                .AddReportTypes("HtmlInline_AzurePipelines_Dark", "lcov")
                .AddFileFilters("-*.g.cs")
                .SetAssemblyFilters("+FluentAssertions"));

            string link = TestResultsDirectory / "reports" / "index.html";
            Information($"Code coverage report: \x1b]8;;file://{link.Replace('\\', '/')}\x1b\\{link}\x1b]8;;\x1b\\");
        });

    Target TestFrameworks => _ => _
        .DependsOn(Compile)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            var testCombinations =
                from project in new[]
                {
                    Solution.TestFrameworks.MSpec_Specs,
                    Solution.TestFrameworks.MSTestV2_Specs,
                    Solution.TestFrameworks.NUnit3_Specs,
                    Solution.TestFrameworks.XUnit2_Specs
                }
                let frameworks = project.GetTargetFrameworks()
                let supportedFrameworks = EnvironmentInfo.IsWin ? frameworks : frameworks.Except(new[] { "net47" })
                from framework in supportedFrameworks
                select new { project, framework };

            DotNetTest(s => s
                .SetConfiguration("Debug")
                .SetProcessEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", "en-US")
                .EnableNoBuild()
                .SetDataCollector("XPlat Code Coverage")
                .SetResultsDirectory(TestResultsDirectory)
                .AddRunSetting(
                    "DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.DoesNotReturnAttribute",
                    "DoesNotReturnAttribute")
                .CombineWith(
                    testCombinations,
                    (_, v) => _.SetProjectFile(v.project).SetFramework(v.framework)));

            if (EnvironmentInfo.IsWin)
            {
                NSpec3(Solution.TestFrameworks.NSpec3_Net47_Specs.Directory / "bin" / "Debug" / "net47" / "NSpec3.Specs.dll");
            }
        });

    Target Pack => _ => _
        .DependsOn(ApiChecks)
        .DependsOn(TestFrameworks)
        .DependsOn(UnitTests)
        .DependsOn(CodeCoverage)
        .DependsOn(CalculateNugetVersion)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution.Core.FluentAssertions)
                .SetOutputDirectory(ArtifactsDirectory)
                .SetConfiguration("Release")
                .EnableNoLogo()
                .EnableNoRestore()
                .EnableContinuousIntegrationBuild() // Necessary for deterministic builds
                .SetVersion(SemVer));
        });

    Target Push => _ => _
        .DependsOn(Pack)
        .OnlyWhenDynamic(() => IsTag)
        .Executes(() =>
        {
            IReadOnlyCollection<string> packages = GlobFiles(ArtifactsDirectory, "*.nupkg");

            Assert.NotEmpty(packages.ToList());

            DotNetNuGetPush(s => s
                .SetApiKey(ApiKey)
                .EnableSkipDuplicate()
                .SetSource("https://api.nuget.org/v3/index.json")
                .EnableNoSymbols()
                .CombineWith(packages,
                    (v, path) => v.SetTargetPath(path)));
        });

    Target SpellCheck => _ => _
        .OnlyWhenDynamic(() => RunAllTargets || HasDocumentationChanges)
        .Executes(() =>
        {
            Node($"{YarnCli} install", workingDirectory: RootDirectory);
            Node($"{YarnCli} run cspell", workingDirectory: RootDirectory,
                customLogger: (_, msg) => Error(msg));
        });

    string YarnCli => $"{ToolPathResolver.GetPackageExecutable("Yarn.MSBuild", "yarn.js", "1.22.19")} --silent";
    
    bool HasDocumentationChanges =>
        Changes.Any(x => x.StartsWith("docs"));

    bool HasSourceChanges =>
        Changes.Any(x => !x.StartsWith("docs"));

    string[] Changes =>
        Repository.Diff
            .Compare<TreeChanges>(TargetBranch, SourceBranch)
            .Where(x => x.Exists)
            .Select(x => x.Path)
            .ToArray();

    Repository Repository => new Repository(GitRepository.LocalDirectory);
    Tree TargetBranch => Repository.Branches[PullRequestBase].Tip.Tree;
    Tree SourceBranch => Repository.Branches[Repository.Head.FriendlyName].Tip.Tree;

    bool RunAllTargets => string.IsNullOrWhiteSpace(PullRequestBase);

    bool IsTag => BranchSpec != null && BranchSpec.Contains("refs/tags", StringComparison.InvariantCultureIgnoreCase);
}
