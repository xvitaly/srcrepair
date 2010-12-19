; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!
; ������ ��������� ��������� ��������� TF2 Repair

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{FD3AA10F-074D-457B-822B-EECF180D6EAB}
AppName=SRC Repair
AppVerName=SRC Repair Beta
AppPublisher=EasyCoding Team
AppPublisherURL=http://www.easycoding.org/projects/srcrepair
; AppVersion ������������ � ���������/�������� �������� � �������������� ����������.
AppVersion=0.1.0.96
AppSupportURL=http://www.easycoding.org/projects/srcrepair
AppUpdatesURL=http://www.easycoding.org/projects/srcrepair
DefaultDirName={pf}\SRC Repair
DefaultGroupName=SRC Repair
AllowNoIcons=yes
LicenseFile=E:\VSBuilds\GPL.txt
;InfoBeforeFile=E:\VSBuilds\readme.txt
OutputDir=E:\VSBuilds
;OutputBaseFilename=SRCRepair_Setup
OutputBaseFilename=srcrepair_beta_96
SetupIconFile=E:\SVN\srcrepair\srcrepair\TF2Repair.ico
;UninstallDisplayIcon={app}\MyProg.exe,1
Compression=lzma2
SolidCompression=yes
; "ArchitecturesInstallIn64BitMode=x64" requests that the install be
; done in "64-bit mode" on x64, meaning it should use the native
; 64-bit Program Files directory and the 64-bit view of the registry.
; On all other architectures it will install in "32-bit mode".
ArchitecturesInstallIn64BitMode=x64

; ��� ��������� ������, ������� ����� ��������� � �������� �����������
VersionInfoVersion=0.1.0.96
VersionInfoDescription=��������� ��������� SRC Repair
VersionInfoCopyright=(c) 2005-2011 EasyCoding Team. All rights reserved.
VersionInfoCompany=EasyCoding Team

[Languages]
;Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
;Name: "copylicence"; Description: "����������� ������������ ���������� � ����� SRC Repair"; GroupDescription: "�������������� �����������:"
;Name: "copyreadme"; Description: "����������� ���� ReadMe � ����� SRC Repair"; GroupDescription: "�������������� �����������:"
Name: "inst7z"; Description: "���������� ������ ��������� LZMA ������"; GroupDescription: "�������������� �����������:"
Name: "betashortuts"; Description: "������� ������ ������� �������������� ������"; GroupDescription: "�������������� �����������:"

[Files]
; ������������� readme � ���� ��������...
Source: "E:\VSBuilds\GPL.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\VSBuilds\readme.txt"; DestDir: "{app}"; Flags: ignoreversion
; ������������� 32-������ ������...
Source: "E:\VSBuilds\srcrepair.exe"; DestDir: "{app}"; Flags: ignoreversion; Check: not Is64BitInstallMode
Source: "E:\VSBuilds\ru\srcrepair.resources.dll"; DestDir: "{app}\ru\"; Flags: ignoreversion; Check: not Is64BitInstallMode
; ������������� 64-������ ������...
Source: "E:\VSBuilds\x64\srcrepair.exe"; DestDir: "{app}"; Flags: ignoreversion; Check: Is64BitInstallMode
Source: "E:\VSBuilds\x64\ru\srcrepair.resources.dll"; DestDir: "{app}\ru\"; Flags: ignoreversion; Check: Is64BitInstallMode
; ������������� ��������� �����...
Source: "E:\VSBuilds\backups\*"; DestDir: "{app}\backups\"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "E:\VSBuilds\cfgs\*"; DestDir: "{app}\cfgs\"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "E:\VSBuilds\7z\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs; Tasks: inst7z
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
; ������ ����� ��� ����������...
Name: "{group}\SRC Repair"; Filename: "{app}\srcrepair.exe"
; ������ ������ ��� ������� �������������� ������ (������ ���� ������������ ������ ���� �����)...
Name: "{group}\SRC Repair (������� ������)"; Filename: "{app}\srcrepair.exe"; Parameters: "/russian"; Tasks: betashortuts
Name: "{group}\SRC Repair (English)"; Filename: "{app}\srcrepair.exe"; Parameters: "/english"; Tasks: betashortuts
; ������ ����������� ������ ��� ������� � ��������...
Name: "{group}\{cm:ProgramOnTheWeb,SRC Repair}"; Filename: "http://www.easycoding.org/projects/srcrepair"
Name: "{group}\{cm:UninstallProgram,SRC Repair}"; Filename: "{uninstallexe}"
; ������ ����� �� ������� ����� (���� �������)...
Name: "{commondesktop}\SRC Repair"; Filename: "{app}\srcrepair.exe"; Tasks: desktopicon
; ������ ����� �� ������ �������� ������� (���� �������)...
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\SRC Repair"; Filename: "{app}\srcrepair.exe"; Tasks: quicklaunchicon
; ������ ������ ��� ������ � ������������ ����������� � ReadMe, �� ������ ���� ��� ����� �������...
Name: "{group}\������������ ����������"; Filename: "{app}\GPL.txt"
Name: "{group}\���� ReadMe"; Filename: "{app}\readme.txt"

[Run]
Filename: "{app}\srcrepair.exe"; Description: "{cm:LaunchProgram,SRC Repair}"; Flags: nowait postinstall skipifsilent











