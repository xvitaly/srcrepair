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

title Building SRC Repair's developer documentation...

cd ..
echo Generating developer documentation in HTML format...
"%PROGRAMFILES%\doxygen\bin\doxygen.exe" Doxyfile

echo Generating developer documntation HTML help file...
"%ProgramFiles(x86)%\HTML Help Workshop\hhc.exe" "doxyout\html\index.hhp"

echo Installing generated developer documentation CHM files...
mkdir "srcrepair\bin\Release\help"
move "doxyout\html\srcrepair_dev.chm" "srcrepair\bin\Release\help\srcrepair_dev.chm"

echo Removing temporary files after building developer documentation...
rd /S /Q doxyout
