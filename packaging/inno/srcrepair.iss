; This file is a part of SRC Repair project. For more information
; visit official site: https://www.easycoding.org/projects/srcrepair
;
; Copyright (c) 2011 - 2021 EasyCoding Team (ECTeam).
; Copyright (c) 2005 - 2021 EasyCoding Team.
;
; This program is free software: you can redistribute it and/or modify
; it under the terms of the GNU General Public License as published by
; the Free Software Foundation, either version 3 of the License, or
; (at your option) any later version.
;
; This program is distributed in the hope that it will be useful,
; but WITHOUT ANY WARRANTY; without even the implied warranty of
; MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
; GNU General Public License for more details.
;
; You should have received a copy of the GNU General Public License
; along with this program. If not, see <http://www.gnu.org/licenses/>.

#define VERSION GetVersionNumbersString("..\..\src\srcrepair\bin\Release\srcrepair.exe")
#define BASEDIR "..\..\src\srcrepair"
#define CI_COMMIT GetEnv('CI_HASH')
#if CI_COMMIT == ''
#define _RELEASE 1
#endif

[Setup]
AppId={{77A71DAB-56AA-4F33-BDE8-F00798468B9D}
AppName=SRC Repair
AppVerName=SRC Repair
AppPublisher=EasyCoding Team
AppPublisherURL=https://www.easycoding.org/
AppVersion={#VERSION}
AppSupportURL=https://github.com/xvitaly/srcrepair/issues
AppUpdatesURL=https://github.com/xvitaly/srcrepair/releases
DefaultDirName={code:GetDefRoot}\SRC Repair
DefaultGroupName=SRC Repair
AllowNoIcons=yes
LicenseFile=..\..\COPYING
OutputDir=..\results
#ifdef _RELEASE
OutputBaseFilename=srcrepair_{#GetEnv('RELVER')}_final
#else
OutputBaseFilename=snapshot_{#CI_COMMIT}
#endif
SetupIconFile={#BASEDIR}\srcrepair.ico
UninstallDisplayIcon={app}\srcrepair.exe
Compression=lzma2
SolidCompression=yes
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=commandline
ShowLanguageDialog=auto
ArchitecturesInstallIn64BitMode=x64
MinVersion=6.1sp1
VersionInfoVersion={#VERSION}
VersionInfoDescription=SRC Repair Setup
VersionInfoCopyright=(c) 2005-2020 EasyCoding Team. All rights reserved.
VersionInfoCompany=EasyCoding Team

[Messages]
BeveledLabel=EasyCoding Team

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl,locale\en\cm.isl"; InfoBeforeFile: "locale\en\readme.rtf"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl,locale\ru\cm.isl"; InfoBeforeFile: "locale\ru\readme.rtf"

[Components]
Name: "core"; Description: "{cm:CompCoreDesc}"; Types: full compact custom; Flags: fixed
Name: "debug"; Description: "{cm:CompDebugDesc}"; Types: full compact custom
Name: "plugins"; Description: "{cm:CompPluginsMetaDesc}"; Types: full
Name: "plugins\kbhelper"; Description: "{cm:CompPluginKBHelperDesc}"; Types: full
Name: "locales"; Description: "{cm:CompLocalesMetaDesc}"; Types: full
Name: "locales\en"; Description: "{cm:CompLocaleEnDesc}"; Types: full
Name: "locales\ru"; Description: "{cm:CompLocaleRuDesc}"; Types: full

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#BASEDIR}\bin\Release\games.xml"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\huds.xml"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\configs.xml"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\cleanup.xml"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\plugins.xml"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\DotNetZip.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\NLog.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\srcrepair.exe"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\srcrepair.pdb"; DestDir: "{app}"; Flags: ignoreversion; Components: debug
Source: "{#BASEDIR}\bin\Release\kbhelper.exe"; DestDir: "{app}"; Flags: ignoreversion; Components: plugins\kbhelper
Source: "{#BASEDIR}\bin\Release\kbhelper.pdb"; DestDir: "{app}"; Flags: ignoreversion; Components: plugins\kbhelper and debug
Source: "{#BASEDIR}\bin\Release\corelib.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\corelib.pdb"; DestDir: "{app}"; Flags: ignoreversion; Components: debug
Source: "{#BASEDIR}\bin\Release\srcrepair.exe.config"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\kbhelper.exe.config"; DestDir: "{app}"; Flags: ignoreversion; Components: plugins\kbhelper
Source: "{#BASEDIR}\bin\Release\srcrepair.VisualElementsManifest.xml"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\NLog.config"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\ru\kbhelper.resources.dll"; DestDir: "{app}\ru"; Flags: ignoreversion; Components: locales\ru and plugins\kbhelper
Source: "{#BASEDIR}\bin\Release\ru\srcrepair.resources.dll"; DestDir: "{app}\ru"; Flags: ignoreversion; Components: locales\ru
Source: "{#BASEDIR}\bin\Release\help\srcrepair_en.chm"; DestDir: "{app}\help"; Flags: ignoreversion; Components: locales\en
Source: "{#BASEDIR}\bin\Release\help\srcrepair_ru.chm"; DestDir: "{app}\help"; Flags: ignoreversion; Components: locales\ru

#ifdef _RELEASE
Source: "{#BASEDIR}\bin\Release\srcrepair.exe.sig"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\corelib.dll.sig"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\DotNetZip.dll.sig"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\NLog.dll.sig"; DestDir: "{app}"; Flags: ignoreversion; Components: core
Source: "{#BASEDIR}\bin\Release\kbhelper.exe.sig"; DestDir: "{app}"; Flags: ignoreversion; Components: plugins\kbhelper
Source: "{#BASEDIR}\bin\Release\ru\srcrepair.resources.dll.sig"; DestDir: "{app}\ru"; Flags: ignoreversion; Components: locales\ru
Source: "{#BASEDIR}\bin\Release\ru\kbhelper.resources.dll.sig"; DestDir: "{app}\ru"; Flags: ignoreversion; Components: locales\ru and plugins\kbhelper
#endif

[Icons]
Name: "{group}\SRC Repair"; Filename: "{app}\srcrepair.exe"; Components: core
Name: "{group}\{cm:ProgramOnTheWeb,SRC Repair}"; Filename: "https://github.com/xvitaly/srcrepair"; Components: core
Name: "{userdesktop}\SRC Repair"; Filename: "{app}\srcrepair.exe"; Tasks: desktopicon; Components: core
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\SRC Repair"; Filename: "{app}\srcrepair.exe"; Tasks: quicklaunchicon; Components: core

[Run]
Filename: "{app}\srcrepair.exe"; Description: "{cm:LaunchProgram,SRC Repair}"; Flags: nowait postinstall skipifsilent; Components: core
Filename: "{dotnet40}\ngen.exe"; Parameters: "install ""{app}\srcrepair.exe"""; StatusMsg: {cm:OptNetStatus}; Flags: runhidden; Check: IsAdmin(); Components: core
Filename: "{dotnet40}\ngen.exe"; Parameters: "install ""{app}\kbhelper.exe"""; StatusMsg: {cm:OptNetStatus}; Flags: runhidden; Check: IsAdmin(); Components: plugins\kbhelper
Filename: "{dotnet40}\ngen.exe"; Parameters: "install ""{app}\corelib.dll"""; StatusMsg: {cm:OptNetStatus}; Flags: runhidden; Check: IsAdmin(); Components: core
Filename: "{dotnet40}\ngen.exe"; Parameters: "install ""{app}\DotNetZip.dll"""; StatusMsg: {cm:OptNetStatus}; Flags: runhidden; Check: IsAdmin(); Components: core
Filename: "{dotnet40}\ngen.exe"; Parameters: "install ""{app}\NLog.dll"""; StatusMsg: {cm:OptNetStatus}; Flags: runhidden; Check: IsAdmin(); Components: core

[UninstallRun]
Filename: "{dotnet40}\ngen.exe"; Parameters: "uninstall ""{app}\srcrepair.exe"""; RunOnceId: "NgenMainApp"; Flags: runhidden; Check: IsAdmin(); Components: core
Filename: "{dotnet40}\ngen.exe"; Parameters: "uninstall ""{app}\kbhelper.exe"""; RunOnceId: "NgenKBHelper"; Flags: runhidden; Check: IsAdmin(); Components: plugins\kbhelper
Filename: "{dotnet40}\ngen.exe"; Parameters: "uninstall ""{app}\corelib.dll"""; RunOnceId: "NgenCoreLib"; Flags: runhidden; Check: IsAdmin(); Components: core
Filename: "{dotnet40}\ngen.exe"; Parameters: "uninstall ""{app}\DotNetZip.dll"""; RunOnceId: "NgenDotNetZip"; Flags: runhidden; Check: IsAdmin(); Components: core
Filename: "{dotnet40}\ngen.exe"; Parameters: "uninstall ""{app}\NLog.dll"""; RunOnceId: "NgenNLog"; Flags: runhidden; Check: IsAdmin(); Components: core

[Code]
function GetDefRoot(Param: String): String;
begin
  if not IsAdmin then
    Result := ExpandConstant('{localappdata}')
  else
    Result := ExpandConstant('{pf}')
end;
