#include "..\version.bld"

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "de"; MessagesFile: "compiler:Languages\German.isl"

#include "scripts\products.iss"

#include "scripts\products\stringversion.iss"
#include "scripts\products\winversion.iss"
#include "scripts\products\fileversion.iss"
#include "scripts\products\dotnetfxversion.iss"

#include "scripts\products\dotnetfx40client.iss"

#define ExeName "CIXReader.exe"

[Setup]
AppId={{37AE9380-5018-490B-8C51-3EB6D75C5658}
AppName=CIXReader
AppVersion={#PRODUCT_MAX_VER}.{#PRODUCT_MIN_VER}.{#PRODUCT_BUILD}
AppPublisher=CIX Ltd
AppPublisherURL=https://www.cix.uk/
AppSupportURL=https://www.cix.uk/
AppUpdatesURL=https://www.cix.uk/
DefaultDirName={reg:HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\CIXReader.exe,Path|{pf}\CIXReader}
DefaultGroupName=CIX Software
OutputDir=..\drops
OutputBaseFilename=cr{#PRODUCT_MAX_VER}.{#PRODUCT_MIN_VER}.{#PRODUCT_BUILD}
SetupIconFile=..\CIXReader\Resources\CixReader.ico
InternalCompressLevel=ultra
Compression=lzma
MinVersion=6.1
SolidCompression=yes
UninstallDisplayIcon={app}\{#ExeName}
VersionInfoCompany=CIX
VersionInfoDescription=CIXReader Setup Program

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "source\CIXReader.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\AppLimit.NetSparkle.Net40.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\CIXClient.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\CIXMarkup.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\CIXReader.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\IniFileParser.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\HtmlRenderer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\HtmlRenderer.WinForms.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\NHunspell.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\hunspellx86.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\NHunspellExtender.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\sqlite3.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\NLua.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\KeraLua.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\lua52.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\msvcr110.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\ICSharpCode.SharpZipLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "source\Emoticons\*"; DestDir: "{app}\Emoticons"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "source\Themes\*"; DestDir: "{app}\Themes"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "source\Toolbar\*"; DestDir: "{app}\Toolbar"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "source\Images\*"; DestDir: "{app}\Images"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "source\Acknowledgements.html"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[InstallDelete]
Type: files; Name: "{app}\CommonMark.dll"

[Icons]
Name: "{group}\CIXReader"; Filename: "{app}\{#ExeName}"
Name: "{commondesktop}\CIXReader"; Filename: "{app}\{#ExeName}"; Tasks: desktopicon
Name: "{app}\CIX Software\CIXReader"; Filename: "{app}\{#ExeName}"; WorkingDir: {app}

[Run]
Filename: "{app}\{#ExeName}"; Description: "{cm:LaunchProgram,CIXReader}"; Flags: nowait postinstall

[Registry]
Root: HKCR; Subkey: cix; ValueType: string; ValueData: URL:CIX Forums; Flags: uninsdeletekey
Root: HKCR; Subkey: cix\URL Protocol; ValueType: string
Root: HKCR; Subkey: cix\DefaultIcon; ValueType: string; ValueData: {app}\{#ExeName},0
Root: HKCR; Subkey: cix\Shell\Open\Command; ValueType: string; ValueData: """{app}\{#ExeName}"" /cix=""%1"""

Root: HKCR; Subkey: .cixreader; ValueType: string; ValueData: cixreader.file; Flags: uninsdeletekey

Root: HKCR; Subkey: cixreader.file; ValueType: string; ValueData: CIXReader Data File; Flags: uninsdeletekey
Root: HKCR; Subkey: cixreader.file\DefaultIcon; ValueType: string; ValueData: {app}\{#ExeName},0
Root: HKCR; Subkey: cixreader.file\Shell\Open\Command; ValueType: string; ValueData: """{app}\{#ExeName}"" ""%1"""

Root: HKLM; Subkey: SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\CIXReader.exe; Flags: uninsdeletekey

Root: HKCU; Subkey: "Software\CIXOnline Ltd"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\CIXOnline Ltd\CIXReader"; ValueType: dword; ValueName: FirstRunLicense; ValueData: 1; Flags: uninsdeletekey

[Code]
var
  FLastPage: Integer;

function InitializeSetup(): boolean;
begin
    initwinversion();

  if (not netfxinstalled(NetFx40Client, '') and not netfxinstalled(NetFx40Full, '')) then
        dotnetfx40client();

    Result := true;
end;

procedure CurPageChanged(CurPage: Integer);
begin
  FLastPage := CurPage;
end;

procedure DeInitializeSetup();
begin
  if FLastPage=wpFinished then begin
        RegWriteStringValue (HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\CIXReader.exe', '', ExpandConstant('{app}') + '\CIXReader.exe');
        RegWriteStringValue (HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\CIXReader.exe', 'Path', ExpandConstant('{app}'));
  End
end;
