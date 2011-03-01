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
AppPublisherURL=http://www.easycoding.org/projects/srcrepair
; AppVersion отображается в Установка/Удаление программ в дополнительной информации.
AppVersion=1.5.0.248
AppSupportURL=http://www.easycoding.org/projects/srcrepair
AppUpdatesURL=http://www.easycoding.org/projects/srcrepair
DefaultDirName={pf}\SRC Repair
DefaultGroupName=SRC Repair
AllowNoIcons=yes
LicenseFile=E:\VSBuilds\GPL.txt
;InfoBeforeFile=E:\VSBuilds\readme.txt
OutputDir=E:\VSBuilds
OutputBaseFilename=srcrepair_15_final
;OutputBaseFilename=srcrepair_beta_236
SetupIconFile=E:\SVN\srcrepair\srcrepair\TF2Repair.ico
;UninstallDisplayIcon={app}\MyProg.exe,1
Compression=lzma2
SolidCompression=yes
; "ArchitecturesInstallIn64BitMode=x64" requests that the install be
; done in "64-bit mode" on x64, meaning it should use the native
; 64-bit Program Files directory and the 64-bit view of the registry.
; On all other architectures it will install in "32-bit mode".
ArchitecturesInstallIn64BitMode=x64

; Тут указываем данные, которые будут добавлены в свойства установщика
VersionInfoVersion=1.5.0.248
VersionInfoDescription=SRC Repair Setup
VersionInfoCopyright=(c) 2005-2011 EasyCoding Team. All rights reserved.
VersionInfoCompany=EasyCoding Team

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl,en-US.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl,ru-RU.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
;Name: "copylicence"; Description: "Скопировать лицензионное соглашение в папку SRC Repair"; GroupDescription: "Дополнительные возможности:"
;Name: "copyreadme"; Description: "Скопировать файл ReadMe в папку SRC Repair"; GroupDescription: "Дополнительные возможности:"
Name: "inst7z"; Description: "{cm:InstLZMAPlugin}"; GroupDescription: "{cm:AdvFeatGroupDesc}"
Name: "betashortuts"; Description: "{cm:InstCreateLocShcuts}"; GroupDescription: "{cm:AdvFeatGroupDesc}"

[Files]
; Копируем библиотеку, используемую для скачивания файлов...
Source: E:\VSBuilds\dll\isxdl.dll; DestDir: {tmp}; Flags: dontcopy
; Устанавливаем readme и файл лицензии...
Source: "E:\VSBuilds\GPL.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\VSBuilds\readme.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\VSBuilds\changelog.txt"; DestDir: "{app}"; Flags: ignoreversion
; Копируем открытый ключ...
Source: "E:\VSBuilds\vitaly_public.asc"; DestDir: "{app}"; Flags: ignoreversion
; Устанавливаем 32-битную версию...
Source: "E:\VSBuilds\srcrepair.exe"; DestDir: "{app}"; Flags: ignoreversion; Check: not Is64BitInstallMode
Source: "E:\VSBuilds\srcrepair.exe.sig"; DestDir: "{app}"; Flags: ignoreversion; Check: not Is64BitInstallMode
Source: "E:\VSBuilds\ru\*"; DestDir: "{app}\ru\"; Flags: ignoreversion recursesubdirs createallsubdirs; Check: not Is64BitInstallMode
; Устанавливаем 64-битную версию...
Source: "E:\VSBuilds\x64\srcrepair.exe"; DestDir: "{app}"; Flags: ignoreversion; Check: Is64BitInstallMode
Source: "E:\VSBuilds\x64\srcrepair.exe.sig"; DestDir: "{app}"; Flags: ignoreversion; Check: Is64BitInstallMode
Source: "E:\VSBuilds\x64\ru\*"; DestDir: "{app}\ru\"; Flags: ignoreversion recursesubdirs createallsubdirs; Check: Is64BitInstallMode
; Устанавливаем остальные файлы...
Source: "E:\VSBuilds\cfgs\*"; DestDir: "{app}\cfgs\"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "E:\VSBuilds\7z\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs; Tasks: inst7z
;Source: "E:\VSBuilds\nfx\*"; DestDir: "{app}\nfx\"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
; Создаём ярлык для приложения...
Name: "{group}\SRC Repair"; Filename: "{app}\srcrepair.exe"
; Создаём ярлыки для файлов с лицензионным соглашением и ReadMe...
Name: "{group}\{cm:ShcLocTexts}\{cm:ShcLicenseAgrr}"; Filename: "{app}\GPL.txt"
Name: "{group}\{cm:ShcLocTexts}\{cm:ShcReadme}"; Filename: "{app}\readme.txt"
Name: "{group}\{cm:ShcLocTexts}\{cm:ShcChlog}"; Filename: "{app}\changelog.txt"
; Создаём ярлыки для запуска локализованных версий (только если пользователь выбрал этот пункт)...
Name: "{group}\{cm:ShcLocFldr}\SRC Repair ({cm:ShcMLnRU})"; Filename: "{app}\srcrepair.exe"; Parameters: "/russian"; Tasks: betashortuts
Name: "{group}\{cm:ShcLocFldr}\SRC Repair ({cm:ShcMLnEN})"; Filename: "{app}\srcrepair.exe"; Parameters: "/english"; Tasks: betashortuts
; Создаём стандартные ярлыки для справки и удаления...
Name: "{group}\{cm:ShcLocTexts}\{cm:ProgramOnTheWeb,SRC Repair}"; Filename: "http://www.easycoding.org/projects/srcrepair"
Name: "{group}\{cm:UninstallProgram,SRC Repair}"; Filename: "{uninstallexe}"
; Создаём ярлык на рабочем столе (если выбрано)...
Name: "{commondesktop}\SRC Repair"; Filename: "{app}\srcrepair.exe"; Tasks: desktopicon
; Создаём ярлык на панели быстрого запуска (если выбрано)...
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\SRC Repair"; Filename: "{app}\srcrepair.exe"; Tasks: quicklaunchicon
; Создаём ярлык для установщика среды Microsoft .NET Framework 4...
Name: "{group}\{cm:ShcNETFx}"; Filename: "{cm:ShcNFxUrl}"

[Registry]
; Задаём базовые настройки программы для текущего пользователя...
Root: HKCU; Subkey: "Software\SRC Repair"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\SRC Repair"; ValueType: dword; ValueName: "ConfirmExit"; ValueData: "1"
Root: HKCU; Subkey: "Software\SRC Repair"; ValueType: dword; ValueName: "ShowSinglePlayer"; ValueData: "1"
Root: HKCU; Subkey: "Software\SRC Repair"; ValueType: dword; ValueName: "SortGameList"; ValueData: "1"

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









