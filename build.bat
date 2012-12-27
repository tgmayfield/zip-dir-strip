@echo off
packages\NuGet.exe install ZipStrip\packages.config -o .\packages
packages\NuGet.exe install ZipDirStrip\packages.config -o .\packages
msbuild ZipDirStrip.sln
