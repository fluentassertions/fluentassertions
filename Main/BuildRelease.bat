IF "%ProgramFiles(x86)%" NEQ "" (
 CALL "%ProgramFiles(x86)%\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" 
) ELSE ( 
 CALL "%ProgramFiles%\Microsoft Visual Studio 10.0\VC\vcvarsall.bat"
)


msbuild /p:Configuration=Release /t:Rebuild