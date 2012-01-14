CALL "%VS100COMNTOOLS%vsvars32.bat"

msbuild /p:Configuration=Release /t:Rebuild

tools\nuget pack package\.nuspec -o package