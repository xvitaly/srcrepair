@echo off

rem
rem SPDX-FileCopyrightText: 2011-2023 EasyCoding Team
rem
rem SPDX-License-Identifier: GPL-3.0-or-later
rem

title Building SRC Repair release binaries...

set GPGKEY=A989AAAA
set RELVER=442

if [%CI_HASH%] == [] (
    set PREFIX=srcrepair_%RELVER%
) else (
    set PREFIX=snapshot_%CI_HASH%
)

echo Removing previous build results...
if exist results rd /S /Q results

echo Installing dependencies using NuGet package manager...
pushd ..
nuget restore
popd

echo Starting build process using MSBUILD...
"%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\msbuild.exe" ..\srcrepair.sln /m:1 /t:Build /p:Configuration=Release /p:TargetFramework=v4.8

echo Generating documentation in HTML format...
mkdir "..\src\srcrepair\bin\Release\help"
pushd helpers
call "build_chm_win.cmd"
popd

echo Signing binaries...
if [%CI_HASH%] == [] (
    "%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\src\srcrepair\bin\Release\srcrepair.exe
    "%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\src\srcrepair\bin\Release\kbhelper.exe
    "%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\src\srcrepair\bin\Release\corelib.dll
    "%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\src\srcrepair\bin\Release\NLog.dll
    "%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\src\srcrepair\bin\Release\ru\srcrepair.resources.dll
    "%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\src\srcrepair\bin\Release\ru\kbhelper.resources.dll
)

echo Compiling Installer...
"%ProgramFiles(x86)%\Inno Setup 6\ISCC.exe" inno\srcrepair.iss

echo Generating archive for non-Windows platforms...
"%ProgramFiles%\7-Zip\7z.exe" a -tzip -mx9 -mm=Deflate -x!*.ico -x!NLog.xml "results\%PREFIX%_other.zip" ".\..\src\srcrepair\bin\Release\*"

echo Signing built artifacts...
if [%CI_HASH%] == [] (
    "%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% results\%PREFIX%_setup.exe
    "%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% results\%PREFIX%_other.zip
)

echo Removing temporary files and directories...
rd /S /Q "..\docs\build\doctrees"
rd /S /Q "..\docs\build\htmlhelp"
rd /S /Q "..\src\srcrepair\bin"
rd /S /Q "..\src\srcrepair\obj"
rd /S /Q "..\src\corelib\bin"
rd /S /Q "..\src\corelib\obj"
rd /S /Q "..\src\kbhelper\bin"
rd /S /Q "..\src\kbhelper\obj"
