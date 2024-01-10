using System;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using static Serilog.Log;

namespace Tasks;

public class CustomNpmTasks
{
    static AbsolutePath RootDirectory;
    static AbsolutePath TempDir;
    static AbsolutePath NodeDir;
    static AbsolutePath NodeDirPerOs;
    static AbsolutePath WorkingDirectory;

    static IReadOnlyDictionary<string, string> NpmEnvironmentVariables = new Dictionary<string, string>();

    static Tool Node;
    static Tool Npm;

    static string Version = "20.10.0";

    public static void Initialize(AbsolutePath root)
    {
        RootDirectory = root;
        TempDir = RootDirectory / ".nuke" / "temp";
        NodeDir = TempDir;
    }

    public static void NpmFetchRuntime()
    {
        var archive = DownloadingNodeArchives();

        ExtractNodeArchive(archive);
    }

    private static AbsolutePath DownloadingNodeArchives()
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

        Information($"Fetching node.js ({Version}) for {os}");

        string downloadUrl = $"https://nodejs.org/dist/v{Version}/node-v{Version}-{os}{archiveType}";
        var archive = TempDir / $"node{archiveType}";

        HttpTasks.HttpDownloadFile(downloadUrl, archive, clientConfigurator: c =>
        {
            c.Timeout = TimeSpan.FromSeconds(50);

            return c;
        });

        NodeDirPerOs = NodeDir / $"node-v{Version}-{os}";
        WorkingDirectory = NodeDirPerOs;

        return archive;
    }

    private static void ExtractNodeArchive(AbsolutePath archive)
    {
        Information($"Extracting node.js binary archive ({archive}) to {NodeDir}");

        if (EnvironmentInfo.IsWin)
        {
            archive.UnZipTo(NodeDir);

            Information("Resolve tool Node...");
            Node = ToolResolver.GetTool(NodeDirPerOs / "node.exe");

            Information("Resolve tool npm...");
            Npm = ToolResolver.GetTool(NodeDirPerOs / "npm.cmd");

            NpmEnvironmentVariables = new Dictionary<string, string>()
            {
                {"PATH", WorkingDirectory }
            };
        }
        else if (EnvironmentInfo.IsUnix)
        {
            archive.UnTarGZipTo(NodeDir);
            WorkingDirectory = WorkingDirectory / "bin";

            var nodeExecutable = WorkingDirectory / "node";
            var npmNodeModules = NodeDirPerOs / "lib" / "node_modules";
            var npmExecutable = npmNodeModules / "npm" / "bin" / "npm";
            var npmSymlink = WorkingDirectory / "npm";

            Information($"Set execution permissions for {nodeExecutable}...");
            nodeExecutable.SetExecutable();

            Information($"Set execution permissions for {npmExecutable}...");
            npmExecutable.SetExecutable();

            Information("Linking binaries...");
            Tool Bash = ToolResolver.GetPathTool("bash");
            Bash($"-c \"ln -sf {npmExecutable} npm\"", workingDirectory: WorkingDirectory);
            Bash($"-c \"ln -sf {npmNodeModules} node_modules\"", workingDirectory: WorkingDirectory);

            Information("Resolve tool Node...");
            Node = ToolResolver.GetTool(nodeExecutable);

            Information("Resolve tool npm...");
            Npm = ToolResolver.GetTool(npmSymlink);

            NpmEnvironmentVariables = EnvironmentInfo.Variables
                .ToDictionary(x => x.Key, x => x.Value)
                .SetKeyValue("path", NodeDirPerOs).AsReadOnly();
        }
    }

    public static void NpmInstall(string workingDirectory = null)
    {
        Npm("install --silent", workingDirectory);
    }

    public static void NpmRun(string args)
    {
        Npm($"run {args}".TrimMatchingDoubleQuotes(),
            environmentVariables: NpmEnvironmentVariables,
            logger: (_, msg) => Error(msg));
    }

    public static void NpmVersion()
    {
        Npm("--version",
            workingDirectory: WorkingDirectory,
            environmentVariables: NpmEnvironmentVariables);
    }

    public static void NodeVersion()
    {
        Node("--version",
            workingDirectory: WorkingDirectory,
            environmentVariables: NpmEnvironmentVariables);
    }
}
