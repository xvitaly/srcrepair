@echo off

echo Starting build process using MSBUILD...
"%ProgramFiles(x86)%\MSBuild\14.0\bin\msbuild.exe" ..\srcrepair.sln /t:Build /p:Configuration=Release /p:TargetFramework=v4.0

echo Changing directory to built version...
cd "..\srcrepair\bin\Release"

echo Signing binaries...
"%ProgramFiles(x86)%\GNU\GnuPG\gpg2.exe" --sign --detach-sign --default-key D45AB90A srcrepair.exe
"%ProgramFiles(x86)%\GNU\GnuPG\gpg2.exe" --sign --detach-sign --default-key D45AB90A DotNetZip.dll
"%ProgramFiles(x86)%\GNU\GnuPG\gpg2.exe" --sign --detach-sign --default-key D45AB90A ru/srcrepair.resources.dll

echo Compiling Installer...
"%ProgramFiles(x86)%\Inno Setup 5\ISCC.exe" srcrepair.iss
