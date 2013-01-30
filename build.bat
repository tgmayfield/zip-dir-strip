@echo off
packages\NuGet.exe install ZipStrip\packages.config -o .\packages
msbuild ZipDirStrip.sln %*
