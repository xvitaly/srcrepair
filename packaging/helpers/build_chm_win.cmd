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

title CHM builder for SRC Repair

echo Building offline help for default (EN) locale...
call "..\..\docs\help\make.cmd" htmlhelp
"%ProgramFiles(x86)%\HTML Help Workshop\hhc.exe" "..\..\docs\help\build\htmlhelp\srcrepair_en.hhp"
move "..\..\docs\help\build\htmlhelp\srcrepair_en.chm" "..\..\srcrepair\bin\Release\help\srcrepair_en.chm"

echo Building offline help for RU locale...
set SPHINXOPTS=-D language=ru
set BUILDLANG=ru
call "..\..\docs\help\make.cmd" htmlhelp
"%ProgramFiles(x86)%\HTML Help Workshop\hhc.exe" "..\..\docs\help\build\htmlhelp\srcrepair_ru.hhp"
move "..\..\docs\help\build\htmlhelp\srcrepair_ru.chm" "..\..\srcrepair\bin\Release\help\srcrepair_ru.chm"
