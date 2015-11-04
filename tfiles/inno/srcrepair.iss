; Скрипт программы (мастера) установки SRC Repair.
; 
; Copyright 2011 - 2015 EasyCoding Team (ECTeam).
; Copyright 2005 - 2015 EasyCoding Team.
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
AppPublisherURL=http://www.easycoding.org/
AppVersion=22.0.0.2758
AppSupportURL=https://www.easycoding.org/projects/srcrepair
AppUpdatesURL=https://www.easycoding.org/projects/srcrepair
DefaultDirName={code:GetDefRoot}\SRC Repair
DefaultGroupName=SRC Repair
AllowNoIcons=yes
LicenseFile=GPL.txt
OutputBaseFilename=srcrepair_220_final
SetupIconFile=srcrepair.ico
UninstallDisplayIcon={app}\srcrepair.exe
Compression=lzma2
SolidCompression=yes
PrivilegesRequired=none
ArchitecturesInstallIn64BitMode=x64

; Здесь указываем данные, которые будут добавлены в свойства установщика...
VersionInfoVersion=22.0.0.2758
VersionInfoDescription=SRC Repair Setup
VersionInfoCopyright=(c) 2005-2015 EasyCoding Team. All rights reserved.
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
; Копируем библиотеку, используемую для скачивания файлов...
Source: "isxdl.dll"; DestDir: {tmp}; Flags: dontcopy

; Устанавливаем readme, файл лицензии и список изменений...
Source: "GPL.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "readme.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "readme_en.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "changelog.txt"; DestDir: "{app}"; Flags: ignoreversion

; Копируем файл со списком поддерживаемых игр и их параметрами...
Source: "games.xml"; DestDir: "{app}"; Flags: ignoreversion

; Копируем файл со базой данных HUD...
Source: "huds.xml"; DestDir: "{app}"; Flags: ignoreversion

; Копируем открытый ключ...
Source: "pubkey.asc"; DestDir: "{app}"; Flags: ignoreversion

; Копируем модуль поддержки сжатия (собран как AnyCPU)...
Source: "Ionic.Zip.Reduced.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "Ionic.Zip.Reduced.pdb"; DestDir: "{app}"; Flags: ignoreversion; Tasks: insdebginf
Source: "Ionic.Zip.Reduced.dll.sig"; DestDir: "{app}"; Flags: ignoreversion

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
Name: "{group}\{cm:ShcLocTexts}\{cm:ProgramOnTheWeb,SRC Repair}"; Filename: "http://www.easycoding.org/projects/srcrepair"
Name: "{group}\{cm:UninstallProgram,SRC Repair}"; Filename: "{uninstallexe}"

; Создаём ярлык для багтрекера...
Name: "{group}\{cm:RepAppErrText}"; Filename: "https://www.easycoding.org/projects/srcrepair/bugreport"

; Создаём ярлык на рабочем столе (если выбрано)...
Name: "{commondesktop}\SRC Repair"; Filename: "{app}\srcrepair.exe"; Tasks: desktopicon

; Создаём ярлык на панели быстрого запуска (если выбрано)...
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\SRC Repair"; Filename: "{app}\srcrepair.exe"; Tasks: quicklaunchicon

; Создаём ярлык для установщика среды Microsoft .NET Framework 4...
Name: "{group}\{cm:ShcNETFx}"; Filename: "{cm:ShcNFxUrl}"

[Run]
Filename: "{app}\srcrepair.exe"; Description: "{cm:LaunchProgram,SRC Repair}"; Flags: nowait postinstall skipifsilent

[Code]
// **************************************************************************** //
// Original .NET detection code by Hector Sosa, Jr (systemwidgets.com).
// Modified by V1TSK (vitaly@easycoding.org) for SRC Repair.
// License: GNU GPL version 3 (GPLv3).
// **************************************************************************** //
var
  dotnetRedistPath: string;
  downloadNeeded: boolean;
  dotNetNeeded: boolean;
  memoDependenciesNeeded: string;

const
  dotnetRedistURL = 'http://download.microsoft.com/download/1/B/E/1BE39E79-7E39-46A3-96FF-047F95396215/dotNetFx40_Full_setup.exe';

procedure isxdl_AddFile(URL, Filename: PAnsiChar); external 'isxdl_AddFile@files:isxdl.dll stdcall';
function isxdl_DownloadFiles(hWnd: Integer): Integer; external 'isxdl_DownloadFiles@files:isxdl.dll stdcall';
function isxdl_SetOption(Option, Value: PAnsiChar): Integer; external 'isxdl_SetOption@files:isxdl.dll stdcall';

function InitializeSetup(): Boolean;

begin
  Result := true;
  dotNetNeeded := false;
  if (not RegKeyExists(HKLM, 'Software\Microsoft\.NETFramework\policy\v4.0')) then
    begin
      dotNetNeeded := true;
      if (not IsAdminLoggedOn()) then
         begin
           MsgBox(ExpandConstant('{cm:DnlNetReqAdm}'), mbInformation, MB_OK);
           Result := false;
         end
           else
             begin
               memoDependenciesNeeded := memoDependenciesNeeded + '      Microsoft .NET Framework 4.0' #13;
               dotnetRedistPath := ExpandConstant('{src}\dotNetFx40_Full_setup.exe');
               if (not FileExists(dotnetRedistPath)) then
                 begin
                   dotnetRedistPath := ExpandConstant('{tmp}\dotNetFx40_Full_setup.exe');
                   if (not FileExists(dotnetRedistPath)) then
                     begin
                       isxdl_AddFile(dotnetRedistURL, dotnetRedistPath);
                       downloadNeeded := true;
                     end
                 end;
               SetIniString('install', 'dotnetRedist', dotnetRedistPath, ExpandConstant('{tmp}\dep.ini'));
             end
    end
end;

function NextButtonClick(CurPage: Integer): Boolean;
var
  hWnd: Integer;
  ResultCode: Integer;
begin
  Result := true;
  if (CurPage = wpReady) then
    begin
      hWnd := StrToInt(ExpandConstant('{wizardhwnd}'));
      if (downloadNeeded and (dotNetNeeded = true)) then
        begin
          MsgBox(ExpandConstant('{cm:DnlNetNeeded}'), mbInformation, MB_OK);
          isxdl_SetOption('label', ExpandConstant('{cm:DnlNetLabelW}'));
          isxdl_SetOption('description', ExpandConstant('{cm:DnlNetTextW}'));
          if (isxdl_DownloadFiles(hWnd) = 0) then
            Result := false;
        end;
      if (dotNetNeeded = true) then
        begin
          if (Exec(ExpandConstant(dotnetRedistPath), '', '', SW_SHOW, ewWaitUntilTerminated, ResultCode)) then
            begin
              if (not (ResultCode = 0)) then
                begin
                  Result := false;
                end
            end
              else
                begin
                  Result := false;
                end
        end;
    end;
end;

function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo, MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
var
  s: string;
begin
  // Добавляем зависимости...
  if (memoDependenciesNeeded <> '') then
    s := s + ExpandConstant('{cm:DnlDepText}') + NewLine + memoDependenciesNeeded + NewLine;
  // Добавляем путь установки...
  s := s + MemoDirInfo + NewLine + NewLine;
  // Добавляем информацию...
  if  (MemoTypeInfo <> '') then
    s := s + MemoTypeInfo + NewLine;
  // Добавляем инфо о компонентах...
  if (MemoComponentsInfo <> '') then
    s := s + MemoComponentsInfo + NewLine;
  // Добавляем инфо о группе в меню Пуск...
  if (MemoGroupInfo <> '') then
    s := s + MemoGroupInfo + NewLine + NewLine;
  // Добавляем инфо о задачах...
  if (MemoTasksInfo <> '') then
    s := s + MemoTasksInfo + NewLine;
  Result := s
end;

function IsRegularUser(): Boolean;
begin
  Result := not (IsAdminLoggedOn or IsPowerUserLoggedOn)
end;

function GetDefRoot(Param: String): String;
begin
  if IsRegularUser then
    Result := ExpandConstant('{localappdata}')
  else
    Result := ExpandConstant('{pf}')
end;