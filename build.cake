#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#tool "nuget:?package=nspec&version=1.0.13"
#tool "nuget:?package=nspec&version=2.0.1"
#tool "nuget:?package=nspec&version=3.1.0"
#tool "nuget:?package=nunit.runners&version=2.7.0"
#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var toolpath = Argument("toolpath", @"");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./Artifacts") + Directory(configuration);
GitVersion gitVersion = null; 

if (!FileExists("./tools/NSpec.2.0.1/tools/net451/NSpec.dll"))
{
    // NSpec2.0.1 does not have NSpec.dll in the test runner directory, which crashes the test runner.
    CopyFile("./tools/NSpec.2.0.1/lib/net451/NSpec.dll", "./tools/NSpec.2.0.1/tools/net451/NSpec.dll");
}

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("GitVersion").Does(() => {
    gitVersion = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
	});
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
	DotNetCoreRestore(new DotNetCoreRestoreSettings
	{
		NoCache = true,
		ConfigFile = "./nuget.config",
		Verbosity = DotNetCoreVerbosity.Normal
	});

    NuGetRestore("./FluentAssertions.sln", new NuGetRestoreSettings 
	{ 
		NoCache = true,
		ConfigFile = "./nuget.config",
		Verbosity = NuGetVerbosity.Normal,
		ToolPath = "./build/nuget.exe"
	});
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
	.IsDependentOn("GitVersion")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("./FluentAssertions.sln", settings => {
		settings.ToolPath = String.IsNullOrEmpty(toolpath) ? settings.ToolPath : toolpath;
		settings.ToolVersion = MSBuildToolVersion.VS2017;
        settings.PlatformTarget = PlatformTarget.MSIL;
		settings.SetConfiguration(configuration);
	  });
    }
    else
    {
      // Use XBuild
      XBuild("./FluentAssertions.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .Does(() =>
{
    XUnit2("./Tests/Net45.Specs/bin/Debug/**/*.Specs.dll", new XUnit2Settings { });
    XUnit2("./Tests/Net47.Specs/bin/Debug/**/*.Specs.dll", new XUnit2Settings { });
    DotNetCoreTest("./Tests/NetCore.Specs/NetCore.Specs.csproj", new DotNetCoreTestSettings { Configuration = "Debug" });
    DotNetCoreTest("./Tests/NetStandard13.Specs/NetStandard13.Specs.csproj", new DotNetCoreTestSettings { Configuration = "Debug" });
    DotNetCoreTest("./Tests/NetCore20.Specs/NetCore20.Specs.csproj", new DotNetCoreTestSettings { Configuration = "Debug" });

    DotNetCoreTest("./Tests/TestFrameworks/MSpec.Specs/MSpec.Specs.csproj", new DotNetCoreTestSettings { Configuration = "Debug" });
    DotNetCoreTest("./Tests/TestFrameworks/MSTestV2.Specs/MSTestV2.Specs.csproj", new DotNetCoreTestSettings { Configuration = "Debug" });
    DotNetCoreTest("./Tests/TestFrameworks/NUnit3.Specs/NUnit3.Specs.csproj", new DotNetCoreTestSettings { Configuration = "Debug" });
    DotNetCoreTest("./Tests/TestFrameworks/XUnit2.Specs/XUnit2.Specs.csproj", new DotNetCoreTestSettings { Configuration = "Debug" });
    XUnit2("./Tests/TestFrameworks/XUnit.Net45.Specs/**/bin/Debug/**/*.Specs.dll", new XUnit2Settings { });
    NUnit("./Tests/TestFrameworks/NUnit2.Net45.Specs/**/bin/Debug/**/*.Specs.dll", new NUnitSettings { NoResults = true });

    StartProcess(Context.Tools.Resolve("nspec.1.*/**/NSpecRunner.exe"), "./Tests/TestFrameworks/NSpec.Net45.Specs/bin/Debug/net451/NSpec.Specs.dll");
    StartProcess(Context.Tools.Resolve("nspec.2.*/**/NSpecRunner.exe"), "./Tests/TestFrameworks/NSpec2.Net45.Specs/bin/Debug/net451/NSpec2.Specs.dll");
    StartProcess(Context.Tools.Resolve("nspec.3.*/**/NSpecRunner.exe"), "./Tests/TestFrameworks/NSpec3.Net45.Specs/bin/Debug/net451/NSpec3.Specs.dll");
});

Task("Pack")
    .IsDependentOn("GitVersion")
    .Does(() => 
    {
      DotNetCorePack("./src/FluentAssertions/FluentAssertions.csproj", new DotNetCorePackSettings {
        OutputDirectory = "./Artifacts",
        Configuration = "Release",
        ArgumentCustomization = args => args.Append("/p:PackageVersion=" + gitVersion.NuGetVersionV2)
      });  
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
	.IsDependentOn("GitVersion")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
