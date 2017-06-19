@echo off
set GPGKEY=D45AB90A

echo Starting build process using MSBUILD...
"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe" ..\srcrepair.sln /m /t:Build /p:Configuration=Release /p:TargetFramework=v4.6.1

echo Changing directory to built version...
cd "..\srcrepair\bin\Release"

echo Signing binaries...
"%ProgramFiles(x86)%\GNU\GnuPG\gpg2.exe" --sign --detach-sign --default-key %GPGKEY% srcrepair.exe
"%ProgramFiles(x86)%\GNU\GnuPG\gpg2.exe" --sign --detach-sign --default-key %GPGKEY% DotNetZip.dll
"%ProgramFiles(x86)%\GNU\GnuPG\gpg2.exe" --sign --detach-sign --default-key %GPGKEY% ru/srcrepair.resources.dll

echo Compiling Installer...
"%ProgramFiles(x86)%\Inno Setup 5\ISCC.exe" srcrepair.iss
