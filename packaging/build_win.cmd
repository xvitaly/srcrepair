@echo off

rem This file is a part of SRC Repair project. For more information
rem visit official site: https://www.easycoding.org/projects/srcrepair
rem 
rem Copyright (c) 2011 - 2020 EasyCoding Team (ECTeam).
rem Copyright (c) 2005 - 2020 EasyCoding Team.
rem 
rem This program is free software: you can redistribute it and/or modify
rem it under the terms of the GNU General Public License as published by
rem the Free Software Foundation, either version 3 of the License, or
rem (at your option) any later version.
rem 
rem This program is distributed in the hope that it will be useful,
rem but WITHOUT ANY WARRANTYrem without even the implied warranty of
rem MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
rem GNU General Public License for more details.
rem 
rem You should have received a copy of the GNU General Public License
rem along with this program. If not, see <http://www.gnu.org/licenses/>.

title Building SRC Repair release binaries...

set GPGKEY=A989AAAA
set RELVER=362

echo Removing previous build results...
if exist results rd /S /Q results

echo Installing dependencies using NuGet package manager...
pushd ..
nuget restore
popd

echo Starting build process using MSBUILD...
"%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe" ..\srcrepair.sln /m /t:Build /p:Configuration=Release /p:TargetFramework=v4.7.2

echo Generating documentation in HTML format...
call "..\docs\help\make.cmd" htmlhelp

echo Generating HTML help file...
"%ProgramFiles(x86)%\HTML Help Workshop\hhc.exe" "..\docs\help\build\htmlhelp\srcrepair.hhp"

echo Installing generated CHM files...
mkdir "..\srcrepair\bin\Release\help"
move "..\docs\help\build\htmlhelp\srcrepair.chm" "..\srcrepair\bin\Release\help\srcrepair_ru.chm"

echo Signing binaries...
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\srcrepair\bin\Release\srcrepair.exe
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\srcrepair\bin\Release\DotNetZip.dll
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\srcrepair\bin\Release\NLog.dll
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\srcrepair\bin\Release\ru\srcrepair.resources.dll

echo Compiling Installer...
"%ProgramFiles(x86)%\Inno Setup 6\ISCC.exe" inno\srcrepair.iss

echo Generating archive for non-Windows platforms...
"%PROGRAMFILES%\7-Zip\7z.exe" a -m0=LZMA2 -mx9 -t7z -x!*.ico -x!DotNetZip.xml -x!NLog.xml "results\srcrepair_%RELVER%_final.7z" ".\..\srcrepair\bin\Release\*"

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
rd /S /Q "..\docs\help\build\doctrees"
rd /S /Q "..\docs\help\build\htmlhelp"
rd /S /Q "..\srcrepair\bin"
rd /S /Q "..\srcrepair\obj"
rd /S /Q "..\corelib\bin"
rd /S /Q "..\corelib\obj"
rd /S /Q "..\doxyout"
