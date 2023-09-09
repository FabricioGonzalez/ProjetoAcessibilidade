# Packs FVim for Windows platforms

param([string[]]$plat=("win7-x64","win-x64","win-arm64"))
#param([string[]]$plat=("win7-x64","win-x64","linux-x64","osx-x64"))

New-Item -ItemType Directory -Force -Name publish -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force bin\ -ErrorAction SilentlyContinue
Remove-Item publish\*

  dotnet publish -f net7.0 -c Release --self-contained -r win-x64 .\ProjectAvalonia.Desktop.csproj

  Compress-Archive -Path .\bin\Release\net7.0\win-x64\publish\* -DestinationPath .\publish\ProjectAvalonia-win-x64.zip -Force
#foreach($i in $plat) {
#    dotnet publish -f net7.0 -c Release --self-contained -r $i 
#    if ($i -eq "win-x64") {
## replace the coreclr hosting exe with an icon-patched one
##        Copy-Item lib/ProjectAvalonia-win10.exe bin/Release/net7.0/$i/publish/ProjectAvalonia.exe
## Avalonia 0.10.0-preview6 fix: manually copy ANGLE from win7-x64
#        Copy-Item ~/.nuget/packages/avalonia.angle.windows.natives/2.1.0.2020091801/runtimes/win7-x64/native/av_libglesv2.dll bin/Release/net7.0/$i/publish/
#    } elseif ($i -eq "win7-x64") {
##       Copy-Item lib/ProjectAvalonia-win7.exe bin/Release/net7.0/$i/publish/ProjectAvalonia.exe
#    } elseif ($i -eq "win-arm64") {
##        Copy-Item lib/ProjectAvalonia-win10-arm64.exe bin/Release/net7.0/$i/publish/ProjectAvalonia.exe
#    }
#    Compress-Archive -Path bin/Release/net7.0/$i/publish/* -DestinationPath publish/ProjectAvalonia-$i.zip -Force
#}
