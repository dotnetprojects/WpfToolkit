"C:\Program Files (x86)\MSBuild\12.0\Bin\amd64\msbuild.exe" "WpfToolkit\WPFToolkit.sln" /property:Configuration=Release;OutDir=..\..\bin

rmdir /S /Q Package\lib >nul 2>&1
xcopy bin\*.* Package\lib\net40 /E /I /R /Y

Tools\NuGet\NuGet.exe pack Package\WpfToolkit.nuspec -OutputDirectory Package

pause