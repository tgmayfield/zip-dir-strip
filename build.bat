@echo off
packages\NuGet.exe install packages.config -o .\packages
msbuild ZipDirStrip.sln
