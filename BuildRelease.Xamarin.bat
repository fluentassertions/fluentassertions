CALL "%VS120COMNTOOLS%vsvars32.bat"

msbuild /p:Configuration=Release /p:Platform="Any CPU" /t:Rebuild FluentAssertions.Xamarin.sln

tools\nuget pack package\FluentAssertions.Xamarin.nuspec -o package