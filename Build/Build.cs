using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.Xunit;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Xunit.XunitTasks;

[CheckBuildProjectConfigurations]
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

    public static int Main() => Execute<Build>(
        x => x.UnitTests,
        x => x.Pack);

    [Solution] readonly Solution Solution;
    [GitVersion] readonly GitVersion GitVersion;
    [PackageExecutable("nspec", "NSpecRunner.exe", Version = "3.1.0")] Tool NSpec3;

    AbsolutePath ArtifactsDirectory => RootDirectory / "Artifacts";
    AbsolutePath TestsDirectory => RootDirectory / "Tests";
    AbsolutePath TestFrameworkDirectory => TestsDirectory / "TestFrameworks";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution)
                .EnableNoCache()
                .SetConfigFile(RootDirectory / "nuget.config"));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration.Debug)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion));
        });

    Target ApiChecks => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetConfiguration(Configuration.Debug)
                .CombineWith(
                    cc => cc.SetProjectFile(Solution.GetProject("Approval.Tests"))));
        });

    Target UnitTests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            Xunit2(s => s
                .SetFramework("net47")
                .AddTargetAssemblies(GlobFiles(
                    TestsDirectory,
                    "FluentAssertions.Specs/bin/Debug/net47/*.Specs.dll").NotEmpty()));

            DotNetTest(s => s
                .SetProjectFile(Solution.GetProject("FluentAssertions.Specs"))
                .SetConfiguration(Configuration.Debug)
                .CombineWith(
                    cc => cc.SetFramework("netcoreapp2.0"),
                    cc => cc.SetFramework("netcoreapp2.1"),
                    cc => cc.SetFramework("netcoreapp3.0")));
        });

    Target TestFrameworks => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetConfiguration(Configuration.Debug)
                .CombineWith(
                    cc => cc.SetProjectFile(Solution.GetProject("MSpec.Specs")),
                    cc => cc.SetProjectFile(Solution.GetProject("MSTestV2.Specs")),
                    cc => cc.SetProjectFile(Solution.GetProject("NUnit3.Specs")),
                    cc => cc.SetProjectFile(Solution.GetProject("XUnit2.Specs"))));

            NSpec3(TestFrameworkDirectory / "NSpec3.Net47.Specs" / "bin" / "Debug" / "net47" / "NSpec3.Specs.dll");
        });

    Target Pack => _ => _
        .DependsOn(ApiChecks)
        .DependsOn(TestFrameworks)
        .DependsOn(UnitTests)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution.GetProject("FluentAssertions"))
                .SetOutputDirectory(ArtifactsDirectory)
                .SetConfiguration(Configuration.Release)
                .SetVersion(GitVersion.NuGetVersionV2));
        });
}
