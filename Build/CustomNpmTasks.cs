using System;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using static Serilog.Log;

public static class CustomNpmTasks
{
    static AbsolutePath RootDirectory;
    static AbsolutePath TempDir;
    static AbsolutePath NodeDir;
    static AbsolutePath NodeDirPerOs;
    static AbsolutePath WorkingDirectory;

    static IReadOnlyDictionary<string, string> NpmEnvironmentVariables;

    static Tool Npm;

    static string Version;

    static Func<string, bool> GetCachedNodeModules;

    public static bool HasCachedNodeModules;

    public static void Initialize(AbsolutePath root)
    {
        RootDirectory = root;
        NodeDir = RootDirectory / ".nuke" / "temp";

        Version = (RootDirectory / "NodeVersion").ReadAllText().Trim();
        GetCachedNodeModules = os => NodeDir.GlobFiles($"node*{Version}-{os}*/**/node*", $"node*{Version}-{os}*/**/npm*").Count != 0;
    }

    public static void NpmFetchRuntime()
    {
        DownloadNodeArchive().ExtractNodeArchive();

        LinkTools();
    }

    static AbsolutePath DownloadNodeArchive()
    {
        AbsolutePath archive = NodeDir;
        string os = null;
        string archiveType = null;

        if (EnvironmentInfo.IsWin)
        {
            os = "win";
            archiveType = ".zip";
        }
        else if (EnvironmentInfo.IsOsx)
        {
            os = "darwin";
            archiveType = ".tar.gz";
        }
        else if (EnvironmentInfo.IsLinux)
        {
            os = "linux";
            archiveType = ".tar.xz";
        }

        os.NotNull();
        archiveType.NotNull();

        string architecture =
            EnvironmentInfo.IsArm64 ? "arm64" :
            EnvironmentInfo.Is64Bit ? "x64" : "x86";

        os = $"{os}-{architecture}";

        HasCachedNodeModules = GetCachedNodeModules(os);

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

    static void ExtractNodeArchive(this AbsolutePath archive)
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
        else if (EnvironmentInfo.IsOsx)
        {
            archive.UnTarGZipTo(NodeDir);
        }
        else if (EnvironmentInfo.IsLinux)
        {
            archive.UnTarXzTo(NodeDir);
        }
    }

    static void LinkTools()
    {
        AbsolutePath npmExecutable;

        if (EnvironmentInfo.IsWin)
        {
            npmExecutable = NodeDirPerOs / "npm.cmd";
        }
        else
        {
            WorkingDirectory /= "bin";

            var nodeExecutable = WorkingDirectory / "node";
            var npmNodeModules = NodeDirPerOs / "lib" / "node_modules";
            npmExecutable = npmNodeModules / "npm" / "bin" / "npm";

            Information($"Set execution permissions for {nodeExecutable}...");
            nodeExecutable.SetExecutable();

            Information($"Set execution permissions for {npmExecutable}...");
            npmExecutable.SetExecutable();

            Information("Linking binaries...");
            npmExecutable.AddUnixSymlink(WorkingDirectory / "npm", force: true);
            npmNodeModules.AddUnixSymlink(WorkingDirectory / "node_modules", force: true);

            npmExecutable = WorkingDirectory / "npm";
        }

        Information("Resolve tool npm...");
        npmExecutable.NotNull();
        Npm = ToolResolver.GetTool(npmExecutable);

        NpmConfig("set update-notifier false");
        NpmVersion();

        SetEnvVars();
    }

    static void SetEnvVars()
    {
        NpmEnvironmentVariables = EnvironmentInfo.Variables
            .ToDictionary(x => x.Key, x => x.Value)
            .SetKeyValue("path", WorkingDirectory).AsReadOnly();
    }

    public static void NpmInstall(bool silent = false, string workingDirectory = null)
    {
        Npm($"install {(silent ? "--silent" : "")}",
            workingDirectory,
            logger: NpmLogger);
    }

    public static void NpmRun(string args, bool silent = false)
    {
        Npm($"run {(silent ? "--silent" : "")} {args}".TrimMatchingDoubleQuotes(),
            environmentVariables: NpmEnvironmentVariables,
            logger: NpmLogger);
    }

    static void NpmVersion()
    {
        Npm("--version",
            workingDirectory: WorkingDirectory,
            logInvocation: false,
            logger: NpmLogger,
            environmentVariables: NpmEnvironmentVariables);
    }

    static void NpmConfig(string args)
    {
        Npm($"config {args}".TrimMatchingDoubleQuotes(),
            workingDirectory: WorkingDirectory,
            logInvocation: false,
            logOutput: false,
            logger: NpmLogger,
            environmentVariables: NpmEnvironmentVariables);
    }

    static readonly Action<OutputType, string> NpmLogger = (outputType, msg) =>
    {
        if (EnvironmentInfo.IsWsl && msg.Contains("wslpath"))
            return;

        if (outputType == OutputType.Err)
        {
            Error(msg);

            return;
        }

        Information(msg);
    };
}
