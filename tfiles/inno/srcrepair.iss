; Скрипт программы (мастера) установки SRC Repair.
; 
; Copyright 2011 EasyCoding Team (ECTeam).
; Copyright 2005 - 2011 EasyCoding Team.
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
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{77A71DAB-56AA-4F33-BDE8-F00798468B9D}
AppName=SRC Repair
AppVerName=SRC Repair
AppPublisher=EasyCoding Team
AppPublisherURL=http://www.easycoding.org/
; AppVersion отображается в Установка/Удаление программ в дополнительной информации.
AppVersion=11.0.0.657
AppSupportURL=http://code.google.com/p/srcrepair/wiki/Support
AppUpdatesURL=http://code.google.com/p/srcrepair/downloads/list
DefaultDirName={pf}\SRC Repair
DefaultGroupName=SRC Repair
AllowNoIcons=yes
SourceDir=E:\VSBuilds
LicenseFile=GPL.txt
;InfoBeforeFile=readme.txt
OutputDir=E:\VSBuilds
OutputBaseFilename=srcrepair_110_final
;OutputBaseFilename=srcrepair_beta_657
SetupIconFile=srcrepair.ico
UninstallDisplayIcon={app}\srcrepair.exe
Compression=lzma2
SolidCompression=yes
; "ArchitecturesInstallIn64BitMode=x64" requests that the install be
; done in "64-bit mode" on x64, meaning it should use the native
; 64-bit Program Files directory and the 64-bit view of the registry.
; On all other architectures it will install in "32-bit mode".
ArchitecturesInstallIn64BitMode=x64

; Тут указываем данные, которые будут добавлены в свойства установщика
VersionInfoVersion=11.0.0.657
VersionInfoDescription=SRC Repair Setup
VersionInfoCopyright=(c) 2005-2012 EasyCoding Team. All rights reserved.
VersionInfoCompany=EasyCoding Team

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl,en-US.isl"; InfoBeforeFile: "readme_en.txt"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl,ru-RU.isl"; InfoBeforeFile: "readme.txt"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
;Name: "copylicence"; Description: "Скопировать лицензионное соглашение в папку SRC Repair"; GroupDescription: "Дополнительные возможности:"
;Name: "copyreadme"; Description: "Скопировать файл ReadMe в папку SRC Repair"; GroupDescription: "Дополнительные возможности:"
;Name: "inst7z"; Description: "{cm:InstLZMAPlugin}"; GroupDescription: "{cm:AdvFeatGroupDesc}"
Name: "betashortuts"; Description: "{cm:InstCreateLocShcuts}"; GroupDescription: "{cm:AdvFeatGroupDesc}"
;Name: "isexpf"; Description: "{cm:InstExpFeatures}"; GroupDescription: "{cm:AdvFeatGroupDesc}"; Flags: unchecked
Name: "insdebginf"; Description: "{cm:InstDebugInfo}"; GroupDescription: "{cm:AdvFeatGroupDesc}"; Flags: unchecked

[Files]
; Копируем библиотеку, используемую для скачивания файлов...
Source: "dll\isxdl.dll"; DestDir: {tmp}; Flags: dontcopy
; Устанавливаем readme, файл лицензии и список изменений...
Source: "GPL.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "readme.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "readme_en.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "changelog.txt"; DestDir: "{app}"; Flags: ignoreversion
; Копируем файл со списком поддерживаемых игр и их параметрами...
Source: "games.xml"; DestDir: "{app}"; Flags: ignoreversion
; Копируем открытый ключ...
Source: "pubkey.asc"; DestDir: "{app}"; Flags: ignoreversion
; Копируем модуль поддержки сжатия (собран как AnyCPU)...
Source: "Ionic.Zip.Reduced.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "Ionic.Zip.Reduced.dll.sig"; DestDir: "{app}"; Flags: ignoreversion
; Устанавливаем 32-битную версию...
Source: "srcrepair.exe"; DestDir: "{app}"; Flags: ignoreversion; Check: not Is64BitInstallMode
Source: "srcrepair.pdb"; DestDir: "{app}"; Flags: ignoreversion; Check: not Is64BitInstallMode; Tasks: insdebginf
Source: "srcrepair.exe.sig"; DestDir: "{app}"; Flags: ignoreversion; Check: not Is64BitInstallMode
Source: "ru\*"; DestDir: "{app}\ru\"; Flags: ignoreversion recursesubdirs createallsubdirs; Check: not Is64BitInstallMode
; Устанавливаем 64-битную версию...
Source: "x64\srcrepair.exe"; DestDir: "{app}"; Flags: ignoreversion; Check: Is64BitInstallMode
Source: "x64\srcrepair.pdb"; DestDir: "{app}"; Flags: ignoreversion; Check: Is64BitInstallMode; Tasks: insdebginf
Source: "x64\srcrepair.exe.sig"; DestDir: "{app}"; Flags: ignoreversion; Check: Is64BitInstallMode
Source: "x64\ru\*"; DestDir: "{app}\ru\"; Flags: ignoreversion recursesubdirs createallsubdirs; Check: Is64BitInstallMode
; Копируем файл стандартных настроек программы...
Source: "srcrepair.exe.config"; DestDir: "{app}"; Flags: ignoreversion
; Устанавливаем остальные файлы...
Source: "cfgs\*"; DestDir: "{app}\cfgs\"; Flags: ignoreversion recursesubdirs createallsubdirs
;Source: "nfx\*"; DestDir: "{app}\nfx\"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
; Создаём ярлык для приложения...
Name: "{group}\SRC Repair"; Filename: "{app}\srcrepair.exe"
; Создаём ярлык генератора отчётов для Техподдержки...
Name: "{group}\{cm:ShcMnRepGen}"; Filename: "{app}\srcrepair.exe"; Parameters: "/repgen"
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
Name: "{group}\{cm:RepAppErrText}"; Filename: "http://code.google.com/p/srcrepair/issues/entry"
; Создаём ярлык на рабочем столе (если выбрано)...
Name: "{commondesktop}\SRC Repair"; Filename: "{app}\srcrepair.exe"; Tasks: desktopicon
; Создаём ярлык на панели быстрого запуска (если выбрано)...
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\SRC Repair"; Filename: "{app}\srcrepair.exe"; Tasks: quicklaunchicon
; Создаём ярлык для установщика среды Microsoft .NET Framework 4...
Name: "{group}\{cm:ShcNETFx}"; Filename: "{cm:ShcNFxUrl}"

[Run]
;Filename: "{app}\nfx\dotNetFx40_Full_setup.exe"; Description: "{cm:ShcNETFx}"; Flags: nowait postinstall skipifsilent unchecked
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