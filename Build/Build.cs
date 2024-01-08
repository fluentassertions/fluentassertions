using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using LibGit2Sharp;
using Microsoft.Build.Tasks;
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
using Nuke.Common.Utilities.Collections;
using Nuke.Components;
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

    [Parameter("The solution configuration to build. Default is 'Debug' (local) or 'CI' (server).")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.CI;

    [Parameter("Use this parameter if you encounter build problems in any way, " +
        "to generate a .binlog file which holds some useful information.")]
    readonly bool? GenerateBinLog;

    [Parameter("The key to push to Nuget")]
    [Secret]
    readonly string NuGetApiKey;

    [Solution(GenerateProjects = true)]
    readonly Solution Solution;

    [Required]
    [GitVersion(Framework = "net6.0", NoCache = true, NoFetch = true)]
    readonly GitVersion GitVersion;

    [Required]
    [GitRepository]
    readonly GitRepository GitRepository;

    const string NodeVersion = "20.10.0";

    Tool Node;
    Tool Npm;

    AbsolutePath TempDir => RootDirectory / ".nuke" / "temp";
    AbsolutePath NodeDir => TempDir / "node";

    AbsolutePath NodeDirPerOs;

    AbsolutePath WorkingDirectory;

    IReadOnlyDictionary<string, string> NpmEnvironmentVariables = new Dictionary<string, string>();

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
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            ReportSummary(s => s
                .WhenNotNull(GitVersion, (v, o) => v
                    .AddPair("Version", o.SemVer)));

            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .When(GenerateBinLog is true, c => c
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
                .SetConfiguration(Configuration == Configuration.Debug ? "Debug" : "Release")
                .SetProcessEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", "en-US")
                .EnableNoBuild()
                .SetResultsDirectory(TestResultsDirectory)
                .CombineWith(cc => cc
                    .SetProjectFile(project)
                    .AddLoggers($"trx;LogFileName={project.Name}.trx")), completeOnFailure: true);

            ReportTestOutcome(globFilters: $"*{project.Name}.trx");
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

            ReportTestOutcome(globFilters: $"*[!*{net47}].trx");
        });

    Target UnitTests => _ => _
        .DependsOn(UnitTestsNet47)
        .DependsOn(UnitTestsNet6OrGreater);

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
            .When(failedTests > 0, c => c
                .AddPair("Failed", failedTests.ToString()))
            .AddPair("Passed", passedTests.ToString())
            .When(skippedTests > 0, c => c
                .AddPair("Skipped", skippedTests.ToString())));
    }

    Target CodeCoverage => _ => _
        .DependsOn(TestFrameworks)
        .DependsOn(UnitTests)
        .OnlyWhenDynamic(() => RunAllTargets || HasSourceChanges)
        .Executes(() =>
        {
            ReportGenerator(s => s
                .SetProcessToolPath(NuGetToolPathResolver.GetPackageExecutable("ReportGenerator", "ReportGenerator.dll",
                    framework: "net6.0"))
                .SetTargetDirectory(TestResultsDirectory / "reports")
                .AddReports(TestResultsDirectory / "**/coverage.cobertura.xml")
                .AddReportTypes(
                    ReportTypes.lcov,
                    ReportTypes.HtmlInline_AzurePipelines_Dark)
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
            Project[] projects =
            [
                Solution.TestFrameworks.MSpec_Specs,
                Solution.TestFrameworks.MSTestV2_Specs,
                Solution.TestFrameworks.NUnit3_Specs,
                Solution.TestFrameworks.NUnit4_Specs,
                Solution.TestFrameworks.XUnit2_Specs
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

            ReportTestOutcome(projects.Select(p => $"*{p.Name}*.trx").ToArray());
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
            Npm($"install --silent",
                 workingDirectory: RootDirectory);
            Npm($"run --silent cspell",
                environmentVariables: NpmEnvironmentVariables,
                logger: (_, msg) => Error(msg));
        });

    Target InstallNode => _ => _
        .OnlyWhenDynamic(() => RunAllTargets || HasDocumentationChanges)
        .ProceedAfterFailure()
        .Executes(() =>
        {
            string os = null;
            string archiveType = null;

            if (EnvironmentInfo.IsWin)
            {
                os = "win-x64";
                archiveType = ".zip";
            }
            else if (EnvironmentInfo.IsOsx)
            {
                os = "darwin-x64";
                archiveType = ".tar.gz";
            }
            else if (EnvironmentInfo.IsLinux)
            {
                os = "linux-x64";
                archiveType = ".tar.xz";
            }

            Assert.NotNull(os);
            Assert.NotNull(archiveType);

            os = EnvironmentInfo.IsArm64 ? os.Replace("x64", "arm64") : os;

            string downloadUrl = $"https://nodejs.org/dist/v{NodeVersion}/node-v{NodeVersion}-{os}{archiveType}";
            var archive = TempDir / $"node.{archiveType}";
            HttpTasks.HttpDownloadFile(downloadUrl, archive, clientConfigurator: c =>
            {
                c.Timeout = TimeSpan.FromSeconds(50);

                return c;
            });

            var extractDir = NodeDir;
            NodeDirPerOs = extractDir / $"node-v{NodeVersion}-{os}";
            WorkingDirectory = NodeDirPerOs;

            if (EnvironmentInfo.IsWin)
            {
                archive.UnZipTo(extractDir);

                Node = ToolResolver.GetTool(NodeDirPerOs / "node.exe");
                Npm = ToolResolver.GetTool(NodeDirPerOs / "npm.cmd");

                NpmEnvironmentVariables = new Dictionary<string, string>()
                {
                    {"PATH", WorkingDirectory }
                };
            }
            else if (EnvironmentInfo.IsUnix)
            {
                archive.UnTarGZipTo(extractDir);

                Tool Bash = ToolResolver.GetPathTool("bash");

                WorkingDirectory = WorkingDirectory / "bin";

                var nodeExecutable = WorkingDirectory / "node";
                var npmNodeModules = NodeDirPerOs / "lib" / "node_modules";
                var npmExecutable = npmNodeModules / "npm" / "bin" / "npm";

                Bash($"-c \"chmod +x {nodeExecutable}\"");
                Bash($"-c \"chmod +x {npmExecutable}\"");
                Bash($"-c \"ln -sf {npmExecutable} npm\"", workingDirectory: WorkingDirectory);
                Bash($"-c \"ln -sf {npmNodeModules} node_modules\"", workingDirectory: WorkingDirectory);

                Node = ToolResolver.GetTool(nodeExecutable);
                Npm = ToolResolver.GetTool(WorkingDirectory / "npm");

                NpmEnvironmentVariables = EnvironmentInfo.Variables
                    .ToDictionary(x => x.Key, x => x.Value)
                    .SetKeyValue("path", NodeDirPerOs).AsReadOnly();
            }

            Node("--version",
                workingDirectory: WorkingDirectory,
                environmentVariables: NpmEnvironmentVariables);
            Npm($"--version",
                workingDirectory: WorkingDirectory,
                environmentVariables: NpmEnvironmentVariables);
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
