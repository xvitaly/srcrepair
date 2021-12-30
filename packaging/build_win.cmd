@echo off

rem This file is a part of SRC Repair project. For more information
rem visit official site: https://www.easycoding.org/projects/srcrepair
rem
rem Copyright (c) 2011 - 2021 EasyCoding Team (ECTeam).
rem Copyright (c) 2005 - 2021 EasyCoding Team.
rem
rem This program is free software: you can redistribute it and/or modify
rem it under the terms of the GNU General Public License as published by
rem the Free Software Foundation, either version 3 of the License, or
rem (at your option) any later version.
rem
rem This program is distributed in the hope that it will be useful,
rem but WITHOUT ANY WARRANTY; without even the implied warranty of
rem MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
rem GNU General Public License for more details.
rem
rem You should have received a copy of the GNU General Public License
rem along with this program. If not, see <http://www.gnu.org/licenses/>.

title Building SRC Repair release binaries...

set GPGKEY=A989AAAA
set RELVER=433

echo Removing previous build results...
if exist results rd /S /Q results

echo Installing dependencies using NuGet package manager...
pushd ..
nuget restore
popd

echo Starting build process using MSBUILD...
"%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe" ..\srcrepair.sln /m /t:Build /p:Configuration=Release /p:TargetFramework=v4.8

echo Generating documentation in HTML format...
mkdir "..\src\srcrepair\bin\Release\help"
pushd helpers
call "build_chm_win.cmd"
popd

echo Signing binaries...
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\src\srcrepair\bin\Release\srcrepair.exe
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\src\srcrepair\bin\Release\kbhelper.exe
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\src\srcrepair\bin\Release\corelib.dll
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\src\srcrepair\bin\Release\DotNetZip.dll
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\src\srcrepair\bin\Release\NLog.dll
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\src\srcrepair\bin\Release\ru\srcrepair.resources.dll
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\src\srcrepair\bin\Release\ru\kbhelper.resources.dll

echo Compiling Installer...
"%ProgramFiles(x86)%\Inno Setup 6\ISCC.exe" inno\srcrepair.iss

echo Generating archive for non-Windows platforms...
"%PROGRAMFILES%\7-Zip\7z.exe" a -m0=LZMA2 -mx9 -t7z -x!*.ico -x!DotNetZip.xml -x!NLog.xml "results\srcrepair_%RELVER%_final.7z" ".\..\src\srcrepair\bin\Release\*"

echo Generating developer documentation in HTML format...
pushd ..
"%PROGRAMFILES%\doxygen\bin\doxygen.exe" Doxyfile
"%ProgramFiles(x86)%\HTML Help Workshop\hhc.exe" "doxyout\html\index.hhp"
"%PROGRAMFILES%\7-Zip\7z.exe" a -m0=LZMA2 -mx9 -t7z "packaging\results\srcrepair_%RELVER%_dev.7z" ".\doxyout\html\srcrepair_dev.chm"
popd

echo Signing built artifacts...
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% results\srcrepair_%RELVER%_final.exe
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% results\srcrepair_%RELVER%_final.7z
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% results\srcrepair_%RELVER%_dev.7z

echo Removing temporary files and directories...
rd /S /Q "..\docs\build\doctrees"
rd /S /Q "..\docs\build\htmlhelp"
rd /S /Q "..\src\srcrepair\bin"
rd /S /Q "..\src\srcrepair\obj"
rd /S /Q "..\src\corelib\bin"
rd /S /Q "..\src\corelib\obj"
rd /S /Q "..\src\kbhelper\bin"
rd /S /Q "..\src\kbhelper\obj"
rd /S /Q "..\doxyout"
