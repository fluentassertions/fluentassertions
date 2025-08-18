using System;
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
using Nuke.Components;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;
using static Nuke.Common.Tools.Xunit.XunitTasks;
using static Serilog.Log;
using static CustomNpmTasks;

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

    [Parameter("The solution configuration to build. Default is 'Debug' (local) or 'CI' (server).")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.CI;

    [Parameter("Use this parameter if you encounter build problems in any way, " +
        "to generate a .binlog file which holds some useful information.")]
    readonly bool? GenerateBinLog;

    [Parameter("The key to push to Nuget")]
    [Secret]
    readonly string NuGetApiKey;

    [Parameter("The key to use for scanning packages on GitHub")]
    [Secret]
    readonly string GitHubApiKey;

    [Solution(GenerateProjects = true)]
    readonly Solution Solution;

    [Required]
    [GitVersion(Framework = "net9.0", NoCache = true, NoFetch = true)]
    readonly GitVersion GitVersion;

    [Required]
    [GitRepository]
    readonly GitRepository GitRepository;

    [NuGetPackage("PackageGuard", "PackageGuard.dll")]
    Tool PackageGuard;

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

                SemVer = string.Join('.', GitVersion.SemVer.Split('.').Take(3).Union([BuildNumber]));
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
        .DependsOn(CalculateNugetVersion)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            ReportSummary(s => s
                .WhenNotNull(SemVer, (summary, semVer) => summary
                    .AddPair("Version", semVer)));

            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .When(_ => GenerateBinLog is true, c => c
                    .SetBinaryLog(ArtifactsDirectory / $"{Solution.Core.FluentAssertions.Name}.binlog")
                )
                .EnableNoLogo()
                .EnableNoRestore()
                .SetVersion(SemVer)
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
                .SetConfiguration(Configuration == Configuration.Debug ? "Debug" : "Release")
                .SetProcessEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", "en-US")
                .EnableNoBuild()
                .SetResultsDirectory(TestResultsDirectory)
                .CombineWith(cc => cc
                    .SetProjectFile(project)
                    .AddLoggers($"trx;LogFileName={project.Name}.trx")), completeOnFailure: true);
        });

    Project[] Projects =>
    [
        Solution.Specs.FluentAssertions_Specs,
        Solution.Specs.FluentAssertions_Equivalency_Specs,
        Solution.Specs.FluentAssertions_Extensibility_Specs,
        Solution.Specs.FSharp_Specs,
        Solution.Specs.VB_Specs
    ];

    Target UnitTestsNet47 => _ => _
        .Unlisted()
        .DependsOn(Compile)
        .OnlyWhenDynamic(() => EnvironmentInfo.IsWin && (RunAllTargets || HasSourceChanges))
        .Executes(() =>
        {
            string[] testAssemblies = Projects
                .SelectMany(project => project.Directory.GlobFiles("bin/Debug/net47/*.Specs.dll"))
                .Select(p => p.ToString())
                .ToArray();

            Assert.NotEmpty(testAssemblies.ToList());

            Xunit2(s => s
                .SetFramework("net47")
                .AddTargetAssemblies(testAssemblies)
            );
        });

    Target UnitTestsNet6OrGreater => _ => _
        .Unlisted()
        .DependsOn(Compile)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            const string net47 = "net47";

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
                        (settings, project) => settings
                            .SetProjectFile(project)
                            .CombineWith(
                                project.GetTargetFrameworks().Except([net47]),
                                (frameworkSettings, framework) => frameworkSettings
                                    .SetFramework(framework)
                                    .AddLoggers($"trx;LogFileName={project.Name}_{framework}.trx")
                            )
                    ), completeOnFailure: true
            );
        });

    Target UnitTests => _ => _
        .DependsOn(UnitTestsNet47)
        .DependsOn(UnitTestsNet6OrGreater);

    Target CodeCoverage => _ => _
        .DependsOn(TestFrameworks)
        .DependsOn(UnitTests)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            ReportGenerator(s => s
                .SetProcessToolPath(NuGetToolPathResolver.GetPackageExecutable("ReportGenerator", "ReportGenerator.dll",
                    framework: "net9.0"))
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
            Project[] projects =
            [
                Solution.TestFrameworks.MSpec_Specs,
                Solution.TestFrameworks.MSTestV2_Specs,
                Solution.TestFrameworks.NUnit3_Specs,
                Solution.TestFrameworks.NUnit4_Specs,
                Solution.TestFrameworks.XUnit2_Specs,
                Solution.TestFrameworks.XUnit3_Specs,
                Solution.TestFrameworks.XUnit3Core_Specs,
            ];

            var testCombinations =
                from project in projects
                let frameworks = project.GetTargetFrameworks()
                let supportedFrameworks = EnvironmentInfo.IsWin ? frameworks : frameworks.Except(["net47"])
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
                    (settings, v) => settings
                        .SetProjectFile(v.project)
                        .SetFramework(v.framework)
                        .AddLoggers($"trx;LogFileName={v.project.Name}_{v.framework}.trx")), completeOnFailure: true);
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

    Target ScanPackages => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            Environment.SetEnvironmentVariable("GITHUB_API_KEY", GitHubApiKey);
            PackageGuard($"--config-path={RootDirectory / ".packageguard" / "config.json"} --use-caching {RootDirectory}");
        });

    Target Pack => _ => _
        .DependsOn(ApiChecks)
        .DependsOn(TestFrameworks)
        .DependsOn(ScanPackages)
        .DependsOn(UnitTests)
        .DependsOn(CodeCoverage)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            ReportSummary(s => s
                .WhenNotNull(SemVer, (c, semVer) => c
                    .AddPair("Packed version", semVer)));

            DotNetPack(s => s
                .SetProject(Solution.Core.FluentAssertions)
                .SetOutputDirectory(ArtifactsDirectory)
                .SetConfiguration(Configuration == Configuration.Debug ? "Debug" : "Release")
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
        .DependsOn(InstallNode)
        .ProceedAfterFailure()
        .Executes(() =>
        {
            NpmInstall(silent: true, workingDirectory: RootDirectory);
            NpmRun("cspell", silent: true);
        });

    Target InstallNode => _ => _
        .OnlyWhenDynamic(() => RunAllTargets || HasDocumentationChanges)
        .ProceedAfterFailure()
        .Executes(() =>
        {
            Initialize(RootDirectory);

            NpmFetchRuntime();

            ReportSummary(conf =>
            {
                if (HasCachedNodeModules)
                {
                    conf.AddPair("Skipped", "Downloading and extracting");
                }

                return conf;
            });
        });

    bool HasDocumentationChanges => Changes.Any(x => IsDocumentation(x));

    bool HasSourceChanges => Changes.Any(x => !IsDocumentation(x));

    static bool IsDocumentation(string x) =>
        x.StartsWith("docs") ||
        x.StartsWith("CONTRIBUTING.md") ||
        x.StartsWith("cSpell.json") ||
        x.StartsWith("LICENSE") ||
        x.StartsWith("package.json") ||
        x.StartsWith("package-lock.json") ||
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
