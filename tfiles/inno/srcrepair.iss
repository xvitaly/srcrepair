; Скрипт программы (мастера) установки SRC Repair.
; 
; Copyright 2011 - 2016 EasyCoding Team (ECTeam).
; Copyright 2005 - 2016 EasyCoding Team.
; 
; Лицензия: GPL v3 (см. файл GPL.txt).
;  
; Запрещается использовать этот файл при использовании любой
; лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
; 
; Официальный блог EasyCoding Team: http://www.easycoding.org/
; Официальная страница проекта: http://www.easycoding.org/projects/srcrepair
; 
; Более подробная инфорация о программе в readme.txt, о лицензии - в GPL.txt.

[Setup]
; Задаём основные параметры...
AppId={{77A71DAB-56AA-4F33-BDE8-F00798468B9D}
AppName=SRC Repair
AppVerName=SRC Repair
AppPublisher=EasyCoding Team
AppPublisherURL=https://www.easycoding.org/
AppVersion=27.0.0.4531
AppSupportURL=https://www.easycoding.org/projects/srcrepair
AppUpdatesURL=https://www.easycoding.org/projects/srcrepair
DefaultDirName={code:GetDefRoot}\SRC Repair
DefaultGroupName=SRC Repair
AllowNoIcons=yes
LicenseFile=GPL.txt
OutputBaseFilename=srcrepair_270_final
SetupIconFile=srcrepair.ico
UninstallDisplayIcon={app}\srcrepair.exe
Compression=lzma2
SolidCompression=yes
PrivilegesRequired=lowest
ArchitecturesInstallIn64BitMode=x64

; Здесь указываем данные, которые будут добавлены в свойства установщика...
VersionInfoVersion=27.0.0.4531
VersionInfoDescription=SRC Repair Setup
VersionInfoCopyright=(c) 2005-2016 EasyCoding Team. All rights reserved.
VersionInfoCompany=EasyCoding Team

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl,en-US.isl"; InfoBeforeFile: "readme_en.txt"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl,ru-RU.isl"; InfoBeforeFile: "readme.txt"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "betashortuts"; Description: "{cm:InstCreateLocShcuts}"; GroupDescription: "{cm:AdvFeatGroupDesc}"
Name: "insdebginf"; Description: "{cm:InstDebugInfo}"; GroupDescription: "{cm:AdvFeatGroupDesc}"

[Files]
; Устанавливаем readme, файл лицензии и список изменений...
Source: "GPL.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "readme.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "readme_en.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "changelog.txt"; DestDir: "{app}"; Flags: ignoreversion

; Копируем файл со списком поддерживаемых игр и их параметрами...
Source: "games.xml"; DestDir: "{app}"; Flags: ignoreversion

; Копируем файл с базой данных HUD...
Source: "huds.xml"; DestDir: "{app}"; Flags: ignoreversion

; Копируем файл с базой данных FPS-конфигов...
Source: "configs.xml"; DestDir: "{app}"; Flags: ignoreversion

; Копируем модуль поддержки сжатия (собран как AnyCPU)...
Source: "DotNetZip.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "DotNetZip.dll.sig"; DestDir: "{app}"; Flags: ignoreversion

; Устанавливаем бинарники приложения...
Source: "srcrepair.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "srcrepair.pdb"; DestDir: "{app}"; Flags: ignoreversion; Tasks: insdebginf
Source: "srcrepair.exe.sig"; DestDir: "{app}"; Flags: ignoreversion
Source: "ru\*"; DestDir: "{app}\ru\"; Flags: ignoreversion recursesubdirs createallsubdirs

; Копируем файл стандартных настроек программы...
Source: "srcrepair.exe.config"; DestDir: "{app}"; Flags: ignoreversion

; Устанавливаем остальные файлы...
Source: "cfgs\*"; DestDir: "{app}\cfgs\"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
; Создаём ярлык для приложения...
Name: "{group}\SRC Repair"; Filename: "{app}\srcrepair.exe"

; Создаём ярлыки для файлов с лицензионным соглашением и ReadMe...
Name: "{group}\{cm:ShcLocTexts}\{cm:ShcLicenseAgrr}"; Filename: "{app}\GPL.txt"
Name: "{group}\{cm:ShcLocTexts}\{cm:ShcReadme}"; Filename: "{app}\{cm:ShcReadmeFile}"
Name: "{group}\{cm:ShcLocTexts}\{cm:ShcChlog}"; Filename: "{app}\changelog.txt"

; Создаём ярлыки для запуска локализованных версий (только если пользователь выбрал этот пункт)...
Name: "{group}\{cm:ShcLocFldr}\SRC Repair ({cm:ShcMLnRU})"; Filename: "{app}\srcrepair.exe"; Parameters: "/lang ru"; Tasks: betashortuts
Name: "{group}\{cm:ShcLocFldr}\SRC Repair ({cm:ShcMLnEN})"; Filename: "{app}\srcrepair.exe"; Parameters: "/lang en"; Tasks: betashortuts

; Создаём стандартные ярлыки для справки и удаления...
Name: "{group}\{cm:ShcLocTexts}\{cm:ProgramOnTheWeb,SRC Repair}"; Filename: "https://www.easycoding.org/projects/srcrepair"
Name: "{group}\{cm:UninstallProgram,SRC Repair}"; Filename: "{uninstallexe}"

; Создаём ярлык для багтрекера...
Name: "{group}\{cm:RepAppErrText}"; Filename: "https://github.com/xvitaly/srcrepair/issues"

; Создаём ярлык на рабочем столе (если выбрано)...
Name: "{commondesktop}\SRC Repair"; Filename: "{app}\srcrepair.exe"; Tasks: desktopicon

; Создаём ярлык на панели быстрого запуска (если выбрано)...
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\SRC Repair"; Filename: "{app}\srcrepair.exe"; Tasks: quicklaunchicon

; Создаём ярлык для установщика среды Microsoft .NET Framework 4...
Name: "{group}\{cm:ShcNETFx}"; Filename: "{cm:ShcNFxUrl}"

[Run]
Filename: "{app}\srcrepair.exe"; Description: "{cm:LaunchProgram,SRC Repair}"; Flags: nowait postinstall skipifsilent
Filename: {code:GetNetInstallPath}ngen.exe; Parameters: "install ""{app}\srcrepair.exe"""; StatusMsg: {cm:OptNetStatus}; Flags: runhidden; Check: IsAdminLoggedOn()

[UninstallRun]
Filename: {code:GetNetInstallPath}ngen.exe; Parameters: "uninstall ""{app}\srcrepair.exe"""; StatusMsg: {cm:OptNetUninstallStatus}; Flags: runhidden; Check: IsAdminLoggedOn()

[Code]
function GetDefRoot(Param: String): String;
begin
  if not IsAdminLoggedOn then
    Result := ExpandConstant('{localappdata}')
  else
    Result := ExpandConstant('{pf}')
end;

function GetNetInstallPath(Param: String): String;
var
  NetPath: String;
begin
  RegQueryStringValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client', 'InstallPath', NetPath);
  Result := NetPath;
end;