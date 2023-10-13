using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using PublicApiGenerator;
using VerifyTests;
using VerifyTests.DiffPlex;
using VerifyXunit;
using Xunit;

namespace Approval.Tests;

[UsesVerify]
public class ApiApproval
{
    static ApiApproval() => VerifyDiffPlex.Initialize(OutputType.Compact);

    [Theory]
    [ClassData(typeof(TargetFrameworksTheoryData))]
    public Task ApproveApi(string frameworkVersion)
    {
        var configuration = typeof(ApiApproval).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()!.Configuration;
        var assemblyFile = GetPath("Src", "FluentAssertions", "bin", configuration, frameworkVersion, "FluentAssertions.dll");
        var assembly = Assembly.LoadFile(assemblyFile);
        var publicApi = assembly.GeneratePublicApi(options: null);

        return Verifier
            .Verify(publicApi)
            .ScrubLinesContaining("FrameworkDisplayName")
            .UseDirectory(Path.Combine("ApprovedApi", "FluentAssertions"))
            .UseFileName(frameworkVersion)
            .DisableDiff();
    }

    private static string GetSolutionDirectory([CallerFilePath] string path = "") => Path.Combine(Path.GetDirectoryName(path)!, "..", "..");

    private static string GetPath(params string[] paths) => Path.GetFullPath(Path.Combine(paths.Prepend(GetSolutionDirectory()).ToArray()));

    private class TargetFrameworksTheoryData : TheoryData<string>
    {
        public TargetFrameworksTheoryData()
        {
            var csproj = GetPath("Src", "FluentAssertions", "FluentAssertions.csproj");
            var project = XDocument.Load(csproj);
            var targetFrameworks = project.XPathSelectElement("/Project/PropertyGroup/TargetFrameworks");

            foreach (string targetFramework in targetFrameworks!.Value.Split(';'))
            {
                Add(targetFramework);
            }
        }
    }
}
