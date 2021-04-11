using System.Linq;
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

    public static int Main() => Execute<Build>(x => x.Pack);

    [Solution(GenerateProjects = true)] readonly Solution Solution;
    [GitVersion] readonly GitVersion GitVersion;
    [PackageExecutable("nspec", "NSpecRunner.exe", Version = "3.1.0")] Tool NSpec3;

    AbsolutePath ArtifactsDirectory => RootDirectory / "Artifacts";

    Target Clean => _ => _
        .Executes(() =>
        {
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
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
                .SetConfiguration("Debug")
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion));
        });

    Target ApiChecks => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetConfiguration("Debug")
                .CombineWith(
                    cc => cc.SetProjectFile(Solution.Specs.Approval_Tests)));
        });

    Target UnitTests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            Xunit2(s => s
                .SetFramework("net47")
                .AddTargetAssemblies(GlobFiles(
                    Solution.Specs.FluentAssertions_Specs.Directory,
                    "bin/Debug/net47/*.Specs.dll").NotEmpty()));

            DotNetTest(s => s
                .SetProjectFile(Solution.Specs.FluentAssertions_Specs)
                .SetConfiguration("Debug")
                .CombineWith(
                    Solution.Specs.FluentAssertions_Specs.GetTargetFrameworks().Except(new[] { "net47" }),
                    (_, v) => _.SetFramework(v)));
        });

    Target TestFrameworks => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetConfiguration("Debug")
                .CombineWith(
                    new[]
                    {
                        Solution.TestFrameworks.MSpec_Specs,
                        Solution.TestFrameworks.MSTestV2_Specs,
                        Solution.TestFrameworks.NUnit3_Specs,
                        Solution.TestFrameworks.XUnit2_Specs
                    },
                    (_, v) => _.SetProjectFile(v)));

            NSpec3(Solution.TestFrameworks.NSpec3_Net47_Specs.Directory / "bin" / "Debug" / "net47" / "NSpec3.Specs.dll");
        });

    Target Pack => _ => _
        .DependsOn(ApiChecks)
        .DependsOn(TestFrameworks)
        .DependsOn(UnitTests)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution.Core.FluentAssertions)
                .SetOutputDirectory(ArtifactsDirectory)
                .SetConfiguration("Release")
                .EnableContinuousIntegrationBuild() // Necessary for deterministic builds
                .SetVersion(GitVersion.NuGetVersionV2));
        });
}
