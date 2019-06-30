@echo off

rem This file is a part of SRC Repair project. For more information
rem visit official site: https://www.easycoding.org/projects/srcrepair
rem 
rem Copyright (c) 2011 - 2019 EasyCoding Team (ECTeam).
rem Copyright (c) 2005 - 2019 EasyCoding Team.
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

echo Starting build process using MSBUILD...
"%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe" ..\srcrepair.sln /m /t:Build /p:Configuration=Release /p:TargetFramework=v4.7.2

echo Generating documentation in HTML format...
call "..\docs\help\ru\make.cmd" htmlhelp

echo Generating HTML help file...
"%ProgramFiles(x86)%\HTML Help Workshop\hhc.exe" "..\docs\help\ru\build\htmlhelp\srcrepair_ru.hhp"

echo Installing generated CHM files...
mkdir "..\srcrepair\bin\Release\docs\help"
move "..\docs\help\ru\build\htmlhelp\srcrepair_ru.chm" "..\srcrepair\bin\Release\docs\help\srcrepair_ru.chm"

echo Signing binaries...
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\srcrepair\bin\Release\srcrepair.exe
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\srcrepair\bin\Release\DotNetZip.dll
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\srcrepair\bin\Release\NLog.dll
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ..\srcrepair\bin\Release\ru\srcrepair.resources.dll

echo Compiling Installer...
"%ProgramFiles(x86)%\Inno Setup 5\ISCC.exe" inno\srcrepair.iss

echo Signing installer...
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% inno\Output\srcrepair_*.exe
