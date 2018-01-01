@echo off

rem This file is a part of SRC Repair project. For more information
rem visit official site: https://www.easycoding.org/projects/srcrepair
rem 
rem Copyright (c) 2011 - 2018 EasyCoding Team (ECTeam).
rem Copyright (c) 2005 - 2018 EasyCoding Team.
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

set GPGKEY=A989AAAA

echo Starting build process using MSBUILD...
"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe" ..\srcrepair.sln /m /t:Build /p:Configuration=Release /p:TargetFramework=v4.6.1

echo Generating documentation in HTML format...
call "help\make.cmd" htmlhelp

echo Generating HTML help file...
"%ProgramFiles(x86)%\HTML Help Workshop\hhc.exe" "help\_build\htmlhelp\srcrepair.hhp"

echo Installing generated CHM files...
mkdir "..\srcrepair\bin\Release\help"
move "help\_build\htmlhelp\srcrepair.chm" "..\srcrepair\bin\Release\help\srcrepair_ru.chm"

echo Changing directory to built version...
cd "..\srcrepair\bin\Release"

echo Signing binaries...
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% srcrepair.exe
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% DotNetZip.dll
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% ru/srcrepair.resources.dll

echo Compiling Installer...
"%ProgramFiles(x86)%\Inno Setup 5\ISCC.exe" srcrepair.iss

echo Signing installer...
"%ProgramFiles(x86)%\GnuPG\bin\gpg.exe" --sign --detach-sign --default-key %GPGKEY% Output/srcrepair_*_final.exe
