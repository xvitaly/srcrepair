@echo off

rem
rem SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
rem
rem SPDX-License-Identifier: GPL-3.0-or-later
rem

title CHM builder for SRC Repair

echo Building offline help for default (EN) locale...
call "..\..\docs\make.cmd" htmlhelp
"%ProgramFiles(x86)%\HTML Help Workshop\hhc.exe" "..\..\docs\build\htmlhelp\srcrepair_en.hhp" & exit /b 1 | exit /b 0
move "..\..\docs\build\htmlhelp\srcrepair_en.chm" "..\..\src\srcrepair\bin\Release\help\srcrepair_en.chm"

echo Building offline help for RU locale...
set SPHINXOPTS=-D language=ru
set BUILDLANG=ru
call "..\..\docs\make.cmd" htmlhelp
"%ProgramFiles(x86)%\HTML Help Workshop\hhc.exe" "..\..\docs\build\htmlhelp\srcrepair_ru.hhp" & exit /b 1 | exit /b 0
move "..\..\docs\build\htmlhelp\srcrepair_ru.chm" "..\..\src\srcrepair\bin\Release\help\srcrepair_ru.chm"
del /S /Q "..\..\docs\source\locale\ru\LC_MESSAGES\*.mo" >nul
