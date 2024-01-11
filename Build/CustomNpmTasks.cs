using System;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using static Serilog.Log;

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

    static string Version;

    public static bool HasCachedNodeModules;

    public static void Initialize(AbsolutePath root)
    {
        RootDirectory = root;
        NodeDir = RootDirectory / ".nuke" / "temp";

        Version = (RootDirectory / "NodeVersion").ReadAllText().Trim();
        HasCachedNodeModules = NodeDir.GlobFiles($"node*{Version}*/**/node*", $"node*{Version}*/**/npm*").Count != 0;
    }

    public static void NpmFetchRuntime()
    {
        var archive = DownloadingNodeArchives();

        ExtractNodeArchive(archive);

        LinkTools();
    }

    static AbsolutePath DownloadingNodeArchives()
    {
        AbsolutePath archive = NodeDir;
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

        os.NotNull();
        archiveType.NotNull();

        os = EnvironmentInfo.IsArm64 ? os!.Replace("x64", "arm64") : os;

        if (!HasCachedNodeModules)
        {
            Information($"Fetching node.js ({Version}) for {os}");

            string downloadUrl = $"https://nodejs.org/dist/v{Version}/node-v{Version}-{os}{archiveType}";
            archive = NodeDir / $"node{archiveType}";

            HttpTasks.HttpDownloadFile(downloadUrl, archive, clientConfigurator: c =>
            {
                c.Timeout = TimeSpan.FromSeconds(50);

                return c;
            });
        }
        else
        {
            Information("Skipping archive download due to cache");
        }

        NodeDirPerOs = NodeDir / $"node-v{Version}-{os}";
        WorkingDirectory = NodeDirPerOs;

        return archive;
    }

    static void ExtractNodeArchive(AbsolutePath archive)
    {
        if (HasCachedNodeModules)
        {
            Information("Skipping archive extraction due to cache");

            return;
        }

        Information($"Extracting node.js binary archive ({archive}) to {NodeDir}");

        if (EnvironmentInfo.IsWin)
        {
            archive.UnZipTo(NodeDir);
        }
        else if (EnvironmentInfo.IsUnix)
        {
            archive.UnTarGZipTo(NodeDir);
        }
    }

    static void LinkTools()
    {
        if (EnvironmentInfo.IsWin)
        {
            Information("Resolve tool Node...");
            Node = ToolResolver.GetTool(NodeDirPerOs / "node.exe");
            NodeVersion();

            Information("Resolve tool npm...");
            Npm = ToolResolver.GetTool(NodeDirPerOs / "npm.cmd");
            NpmVersion();

            NpmEnvironmentVariables = new Dictionary<string, string>()
            {
                { "PATH", WorkingDirectory }
            };
        }
        else
        {
            WorkingDirectory /= "bin";

            var nodeExecutable = WorkingDirectory / "node";
            var npmNodeModules = NodeDirPerOs / "lib" / "node_modules";
            var npmExecutable = npmNodeModules / "npm" / "bin" / "npm";
            var npmSymlink = WorkingDirectory / "npm";

            Information($"Set execution permissions for {nodeExecutable}...");
            nodeExecutable.SetExecutable();

            Information($"Set execution permissions for {npmExecutable}...");
            npmExecutable.SetExecutable();

            Information("Linking binaries...");
            Tool ln = ToolResolver.GetPathTool("ln");
            ln($"-sf {npmExecutable} npm", workingDirectory: WorkingDirectory);
            ln($"-sf {npmNodeModules} node_modules", workingDirectory: WorkingDirectory);

            Information("Resolve tool Node...");
            Node = ToolResolver.GetTool(nodeExecutable);
            NodeVersion();

            Information("Resolve tool npm...");
            Npm = ToolResolver.GetTool(npmSymlink);
            NpmVersion();

            NpmEnvironmentVariables = EnvironmentInfo.Variables
                .ToDictionary(x => x.Key, x => x.Value)
                .SetKeyValue("path", NodeDirPerOs).AsReadOnly();
        }
    }

    public static void NpmInstall(bool silent = false, string workingDirectory = null)
    {
        Npm($"install {(silent ? "--silent" : "")}", workingDirectory);
    }

    public static void NpmRun(string args, bool silent = false)
    {
        Npm($"run {(silent ? "--silent" : "")} {args}".TrimMatchingDoubleQuotes(),
            environmentVariables: NpmEnvironmentVariables,
            logger: (_, msg) => Error(msg));
    }

    static void NpmVersion()
    {
        Npm("--version",
            workingDirectory: WorkingDirectory,
            environmentVariables: NpmEnvironmentVariables);
    }

    static void NodeVersion()
    {
        Node("--version",
            workingDirectory: WorkingDirectory,
            environmentVariables: NpmEnvironmentVariables);
    }
}
