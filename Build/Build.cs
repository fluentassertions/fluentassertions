using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.NUnit;
using Nuke.Common.Tools.Xunit;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.NUnit.NUnitTasks;
using static Nuke.Common.Tools.Xunit.XunitTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[DotNetVerbosityMapping]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(
        x => x.Test,
        x => x.Pack);

    [Solution] readonly Solution Solution;
    [GitVersion] readonly GitVersion GitVersion;

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

    const string XunitFramework = "net47";
    [PackageExecutable("nspec", "NSpecRunner.exe", Version = "1.0.13")] Tool NSpec1;
    [PackageExecutable("nspec", "NSpecRunner.exe", Version = "2.0.1")] Tool NSpec2;
    [PackageExecutable("nspec", "NSpecRunner.exe", Version = "3.1.0")] Tool NSpec3;

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            Xunit2(s => s
                .SetFramework(XunitFramework)
                .AddTargetAssemblies(GlobFiles(
                    TestsDirectory,
                    $"Net4*.Specs/bin/Debug/**/*.Specs.dll").NotEmpty()));

            DotNetTest(s => s
                .SetConfiguration(Configuration.Debug)
                .CombineWith(
                    cc => cc.SetProjectFile(Solution.GetProject("NetCore.Specs")),
                    cc => cc.SetProjectFile(Solution.GetProject("NetStandard13.Specs")),
                    cc => cc.SetProjectFile(Solution.GetProject("NetCore20.Specs")),
                    cc => cc.SetProjectFile(Solution.GetProject("NetCore21.Specs")),
                    cc => cc.SetProjectFile(Solution.GetProject("NetCore30.Specs"))));

            DotNetTest(s => s
                .SetConfiguration(Configuration.Debug)
                .CombineWith(
                    cc => cc.SetProjectFile(Solution.GetProject("MSpec.Specs")),
                    cc => cc.SetProjectFile(Solution.GetProject("MSTestV2.Specs")),
                    cc => cc.SetProjectFile(Solution.GetProject("NUnit3.Specs")),
                    cc => cc.SetProjectFile(Solution.GetProject("XUnit2.Specs"))));

            Xunit2(s => s
                .SetFramework(XunitFramework)
                .AddTargetAssemblies(GlobFiles(
                    TestFrameworkDirectory / "XUnit.Net45.Specs",
                    $"**/bin/Debug/*/*.Specs.dll").NotEmpty()));
            NUnit3(s => s
                .SetToolPath(ToolPathResolver.GetPackageExecutable("nunit.runners", "nunit-console.exe"))
                .SetInputFiles(GlobFiles(
                    TestFrameworkDirectory / "NUnit2.Net45.Specs",
                    $"**/bin/Debug/*/*.Specs.dll").NotEmpty())
                .EnableNoResults());

            // NSpec2.0.1 does not have NSpec.dll in the test runner directory, which crashes the test runner.
            var nspecPackage =
                NuGetPackageResolver.GetLocalInstalledPackage("NSpec", NuGetPackagesConfigFile, version: "2.0.1");
            CopyFile(
                nspecPackage.Directory / "lib" / "net451" / "NSpec.dll",
                nspecPackage.Directory / "tools" / "net451" / "NSpec.dll",
                FileExistsPolicy.Skip);

            NSpec1(TestFrameworkDirectory / "NSpec.Net45.Specs" / "bin" / "Debug" / "net451" / "NSpec.Specs.dll");
            NSpec2(TestFrameworkDirectory / "NSpec2.Net45.Specs" / "bin" / "Debug" / "net451" / "NSpec2.Specs.dll");
            NSpec3(TestFrameworkDirectory / "NSpec3.Net45.Specs" / "bin" / "Debug" / "net451" / "NSpec3.Specs.dll");

            DotNetTest(s => s
                .SetConfiguration(Configuration.Debug)
                .CombineWith(
                    cc => cc.SetProjectFile(Solution.GetProject("Approval.Tests"))));
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution.GetProject("FluentAssertions"))
                .SetOutputDirectory(ArtifactsDirectory)
                .SetConfiguration(Configuration.Release)
                .SetVersion(GitVersion.NuGetVersionV2));
        });
}
