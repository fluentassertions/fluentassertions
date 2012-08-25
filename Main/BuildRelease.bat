CALL "%VS110COMNTOOLS%vsvars32.bat"

msbuild /p:Configuration=Release /t:Rebuild FluentAssertions.sln

tools\nuget pack package\.nuspec -o package