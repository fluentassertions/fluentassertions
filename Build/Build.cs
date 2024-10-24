using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Tools.Xunit;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
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

    GitHubActions GitHubActions => GitHubActions.Instance;

    string BranchSpec => GitHubActions?.Ref;

    string BuildNumber => GitHubActions?.RunNumber.ToString();

    string PullRequestBase => GitHubActions?.BaseRef;

    [Parameter("Use this parameter if you encounter build problems in any way, " +
        "to generate a .binlog file which holds some useful information.")]
    readonly bool? GenerateBinLog;

    [Parameter("The key to push to Nuget")]
    [Secret]
    readonly string NuGetApiKey;

    [Solution(GenerateProjects = true)]
    readonly Solution Solution;

    [GitVersion(Framework = "net6.0")]
    readonly GitVersion GitVersion;

    [GitRepository]
    readonly GitRepository GitRepository;

#if OS_WINDOWS
    [NuGetPackage("Node.js.redist", "node.exe", Version = "16.20.0", Framework = "win-x64")]
#elif OS_MAC
    [NuGetPackage("Node.js.redist", "node", Version = "16.20.0", Framework = "osx-x64")]
#else
    [NuGetPackage("Node.js.redist", "node", Version = "16.20.0", Framework = "linux-x64")]
#endif
    Tool Node;

    string YarnCli => $"{NuGetToolPathResolver.GetPackageExecutable("Yarn.MSBuild", "yarn.js", "1.22.19")}";

    AbsolutePath ArtifactsDirectory => RootDirectory / "Artifacts";

    AbsolutePath TestResultsDirectory => RootDirectory / "TestResults";

    string SemVer;

    Target Clean => _ => _
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            ArtifactsDirectory.CreateOrCleanDirectory();
            TestResultsDirectory.CreateOrCleanDirectory();
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

    bool IsPullRequest => GitHubActions?.IsPullRequest ?? false;

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
            ReportSummary(s => s
                .WhenNotNull(GitVersion, (_, o) => _
                    .AddPair("Version", o.SemVer)));

            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration.CI)
                .When(GenerateBinLog is true, _ => _
                    .SetBinaryLog(ArtifactsDirectory / $"{Solution.Core.FluentAssertions.Name}.binlog")
                )
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
            Project project = Solution.Specs.Approval_Tests;

            DotNetTest(s => s
                .SetConfiguration(Configuration.Release)
                .SetProcessEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", "en-US")
                .EnableNoBuild()
                .SetResultsDirectory(TestResultsDirectory)
                .CombineWith(cc => cc
                    .SetProjectFile(project)
                    .AddLoggers($"trx;LogFileName={project.Name}.trx")), completeOnFailure: true);

            ReportTestOutcome(globFilters: $"*{project.Name}.trx");
        });

    Project[] Projects => new[]
    {
        Solution.Specs.FluentAssertions_Specs,
        Solution.Specs.FluentAssertions_Equivalency_Specs,
        Solution.Specs.FSharp_Specs,
        Solution.Specs.VB_Specs
    };

    Target UnitTestsNetFramework => _ => _
        .Unlisted()
        .DependsOn(Compile)
        .OnlyWhenDynamic(() => EnvironmentInfo.IsWin && (RunAllTargets || HasSourceChanges))
        .Executes(() =>
        {
            string[] testAssemblies = Projects
                    .SelectMany(project => project.Directory.GlobFiles("bin/Debug/net47/*.Specs.dll"))
                    .Select(_ => _.ToString())
                    .ToArray();

            Assert.NotEmpty(testAssemblies.ToList());

            Xunit2(s => s
                .SetFramework("net47")
                .AddTargetAssemblies(testAssemblies)
            );
        });

    Target UnitTestsNetCore => _ => _
        .Unlisted()
        .DependsOn(Compile)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            const string NET47 = "net47";

            DotNetTest(s => s
                .SetConfiguration(Configuration.Debug)
                .SetProcessEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", "en-US")
                .EnableNoBuild()
                .SetDataCollector("XPlat Code Coverage")
                .SetResultsDirectory(TestResultsDirectory)
                .AddRunSetting(
                    "DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.DoesNotReturnAttribute",
                    "DoesNotReturnAttribute")
                .CombineWith(
                    Projects,
                    (_, project) => _
                        .SetProjectFile(project)
                        .CombineWith(
                            project.GetTargetFrameworks().Except(new[] { NET47 }),
                            (_, framework) => _
                                .SetFramework(framework)
                                .AddLoggers($"trx;LogFileName={project.Name}_{framework}.trx")
                        )
                ), completeOnFailure: true
            );

            ReportTestOutcome(globFilters: $"*[!*{NET47}].trx");
        });

    Target UnitTests => _ => _
        .DependsOn(UnitTestsNetFramework)
        .DependsOn(UnitTestsNetCore);

    static string[] Outcomes(AbsolutePath path)
        => XmlTasks.XmlPeek(
                path,
                "/xn:TestRun/xn:Results/xn:UnitTestResult/@outcome",
                ("xn", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")).ToArray();

    void ReportTestOutcome(params string[] globFilters)
    {
        var resultFiles = TestResultsDirectory.GlobFiles(globFilters);
        var outcomes = resultFiles.SelectMany(Outcomes).ToList();
        var passedTests = outcomes.Count(outcome => outcome is "Passed");
        var failedTests = outcomes.Count(outcome => outcome is "Failed");
        var skippedTests = outcomes.Count(outcome => outcome is "NotExecuted");

        ReportSummary(_ => _
            .When(failedTests > 0, _ => _
                .AddPair("Failed", failedTests.ToString()))
            .AddPair("Passed", passedTests.ToString())
            .When(skippedTests > 0, _ => _
                .AddPair("Skipped", skippedTests.ToString())));
    }

    Target CodeCoverage => _ => _
        .DependsOn(TestFrameworks)
        .DependsOn(UnitTests)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            ReportGenerator(s => s
                .SetProcessToolPath(NuGetToolPathResolver.GetPackageExecutable("ReportGenerator", "ReportGenerator.dll", framework: "net6.0"))
                .SetTargetDirectory(TestResultsDirectory / "reports")
                .AddReports(TestResultsDirectory / "**/coverage.cobertura.xml")
                .AddReportTypes(
                    ReportTypes.lcov,
                    ReportTypes.HtmlInline_AzurePipelines_Dark)
                .AddFileFilters("-*.g.cs")
                .AddFileFilters("-*.nuget*")
                .SetAssemblyFilters("+FluentAssertions"));

            string link = TestResultsDirectory / "reports" / "index.html";
            Information($"Code coverage report: \x1b]8;;file://{link.Replace('\\', '/')}\x1b\\{link}\x1b]8;;\x1b\\");
        });

    Target VSTestFrameworks => _ => _
        .DependsOn(Compile)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            var projects = new[]
            {
                Solution.TestFrameworks.MSpec_Specs,
                Solution.TestFrameworks.MSTestV2_Specs,
                Solution.TestFrameworks.NUnit3_Specs,
                Solution.TestFrameworks.XUnit2_Specs,
                Solution.TestFrameworks.XUnit3_Specs,
                Solution.TestFrameworks.XUnit3Core_Specs,
            };

            var testCombinations =
                from project in projects
                let frameworks = project.GetTargetFrameworks()
                let supportedFrameworks = EnvironmentInfo.IsWin ? frameworks : frameworks.Except(new[] { "net47" })
                from framework in supportedFrameworks
                select new { project, framework };

            DotNetTest(s => s
                .SetConfiguration(Configuration.Debug)
                .SetProcessEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", "en-US")
                .EnableNoBuild()
                .SetDataCollector("XPlat Code Coverage")
                .SetResultsDirectory(TestResultsDirectory)
                .AddRunSetting(
                    "DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.DoesNotReturnAttribute",
                    "DoesNotReturnAttribute")
                .CombineWith(
                    testCombinations,
                    (_, v) => _
                        .SetProjectFile(v.project)
                        .SetFramework(v.framework)
                        .AddLoggers($"trx;LogFileName={v.project.Name}_{v.framework}.trx")), completeOnFailure: true);

            ReportTestOutcome(projects.Select(p => $"*{p.Name}*.trx").ToArray());
        });

    Target TestingPlatformFrameworks => _ => _
        .DependsOn(Compile)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            Project[] projects =
            [
                Solution.TestFrameworks.TUnit_Specs
            ];

            var testCombinations =
                from project in projects
                let frameworks = project.GetTargetFrameworks()
                from framework in frameworks
                select new { project, framework };

            DotNetTest(s => s
                .SetConfiguration(Configuration.Debug)
                .SetProcessEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", "en-US")
                .EnableNoBuild()
                .CombineWith(
                    testCombinations,
                    (settings, v) => settings
                        .SetProjectFile(v.project)
                        .SetFramework(v.framework)
                        .SetProcessAdditionalArguments(
                            "--",
                            "--coverage",
                            "--report-trx",
                            $"--report-trx-filename {v.project.Name}_{v.framework}.trx",
                            $"--results-directory {TestResultsDirectory}"
                        )
                )
            );
        });

    Target TestFrameworks => _ => _
        .DependsOn(VSTestFrameworks)
        .DependsOn(TestingPlatformFrameworks);

    Target Pack => _ => _
        .DependsOn(ApiChecks)
        .DependsOn(TestFrameworks)
        .DependsOn(UnitTests)
        .DependsOn(CodeCoverage)
        .DependsOn(CalculateNugetVersion)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            ReportSummary(s => s
                .WhenNotNull(SemVer, (_, semVer) => _
                    .AddPair("Packed version", semVer)));

            DotNetPack(s => s
                .SetProject(Solution.Core.FluentAssertions)
                .SetOutputDirectory(ArtifactsDirectory)
                .SetConfiguration(Configuration.Release)
                .EnableNoLogo()
                .EnableNoRestore()
                .EnableContinuousIntegrationBuild() // Necessary for deterministic builds
                .SetVersion(SemVer));
        });

    Target Push => _ => _
        .DependsOn(Pack)
        .OnlyWhenDynamic(() => IsTag)
        .ProceedAfterFailure()
        .Executes(() =>
        {
            var packages = ArtifactsDirectory.GlobFiles("*.nupkg");

            Assert.NotEmpty(packages);

            DotNetNuGetPush(s => s
                .SetApiKey(NuGetApiKey)
                .EnableSkipDuplicate()
                .SetSource("https://api.nuget.org/v3/index.json")
                .EnableNoSymbols()
                .CombineWith(packages,
                    (v, path) => v.SetTargetPath(path)));
        });

    Target SpellCheck => _ => _
        .OnlyWhenDynamic(() => RunAllTargets || HasDocumentationChanges)
        .ProceedAfterFailure()
        .Executes(() =>
        {
            Node($"{YarnCli} --silent install", workingDirectory: RootDirectory);
            Node($"{YarnCli} --silent run cspell", workingDirectory: RootDirectory,
                logger: (_, msg) => Error(msg));
        });

    bool HasDocumentationChanges => Changes.Any(x => IsDocumentation(x));

    bool HasSourceChanges => Changes.Any(x => !IsDocumentation(x));

    static bool IsDocumentation(string x) =>
        x.StartsWith("docs") ||
        x.StartsWith("CONTRIBUTING.md") ||
        x.StartsWith("cSpell.json") ||
        x.StartsWith("LICENSE") ||
        x.StartsWith("package.json") ||
        x.StartsWith("README.md");

    string[] Changes =>
        Repository.Diff
            .Compare<TreeChanges>(TargetBranch, SourceBranch)
            .Where(x => x.Exists)
            .Select(x => x.Path)
            .ToArray();

    Repository Repository => new(GitRepository.LocalDirectory);

    Tree TargetBranch => Repository.Branches[PullRequestBase].Tip.Tree;

    Tree SourceBranch => Repository.Branches[Repository.Head.FriendlyName].Tip.Tree;

    bool RunAllTargets => string.IsNullOrWhiteSpace(PullRequestBase) || Changes.Any(x => x.StartsWith("Build"));

    bool IsTag => BranchSpec != null && BranchSpec.Contains("refs/tags", StringComparison.OrdinalIgnoreCase);
}
