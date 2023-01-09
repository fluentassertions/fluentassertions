using System.Linq;
using Nuke.Common;
using Nuke.Common.Tooling;
using static Serilog.Log;

partial class Build
{
#if OS_WINDOWS
    [PackageExecutable("Node.js.redist", "node.exe", Version = "16.17.1", Framework = "win-x64")]
#elif OS_MAC
    [PackageExecutable("Node.js.redist", "node", Version = "16.17.1", Framework = "osx-x64")]
#else
    [PackageExecutable("Node.js.redist", "node", Version = "16.17.1", Framework = "linux-x64")]
#endif
    Tool Node;

    string YarnCli => ToolPathResolver.GetPackageExecutable("Yarn.MSBuild", "yarn.js", "1.22.19");

    Target SpellCheck => _ => _
        .OnlyWhenDynamic(() => RunAllTargets || HasDocumentationChanges)
        .Executes(() =>
        {
            Node($"{YarnCli} install --silent", RootDirectory);
            Node($"{YarnCli} --silent run cspell --no-summary", RootDirectory,
                customLogger: (_, msg) => Error(msg));
        });

    bool HasDocumentationChanges =>
        Changes.Any(x => x.StartsWith("docs"));
}
