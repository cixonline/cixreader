@echo off

rem Build the CIXReader installer
rem
rem Requirements:
rem  7-Zip (from the Tools folder)
rem  Inno Setup (from the Tools folder)
rem  Visual Studio 2012 Professional or greater

rem Note: Code signing requires the following variables to be set in
rem advance:
rem
rem  set _PFXPATH=path to the PFX file
rem  set _AM2SIGNPWD=password for the above
rem
rem For security reasons, these are never stored in scripts. If they are
rem not set, the resulting package is not code signed.
rem
rem Do NOT release packages that are NOT code signed!

rem Force setup to always use release binaries!

set _CONFIG="Release"

rem Build script for Ameol2 setup.

setlocal
if "%_CRDEVROOT%" == "" goto NoRoot
if not exist %_CRDEVROOT%\cixreader\bin\%_CONFIG% goto NoBuild

if exist "%programfiles%\inno setup 5" set PATH=%ProgramFiles%\Inno Setup 5;%PATH%
if exist "%programfiles(x86)%\inno setup 5" set PATH=%ProgramFiles(x86)%\Inno Setup 5;%PATH%

mkdir %_CRDEVROOT%\setup\source\ 2>nul
mkdir %_CRDEVROOT%\setup\source\Emoticons\ 2>nul
mkdir %_CRDEVROOT%\setup\source\Themes\ 2>nul
mkdir %_CRDEVROOT%\setup\source\Toolbar\ 2>nul
mkdir %_CRDEVROOT%\setup\source\Images\ 2>nul

echo Updating local copy of input files.
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\cixreader.exe %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\cixreader.exe.config %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\cixclient.dll %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\cixmarkup.dll %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\inifileparser.dll %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\htmlrenderer.dll %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\htmlrenderer.winforms.dll %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\nhunspell.dll %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\nhunspellextender.dll %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\sqlite3.dll %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\NHunspellExtender\Resources\hunspellx86.dll %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\AppLimit.NetSparkle.Net40.dll %_CRDEVROOT%\setup\source\
xcopy /sdyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\Themes %_CRDEVROOT%\setup\source\Themes
xcopy /sdyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\Emoticons %_CRDEVROOT%\setup\source\Emoticons
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\keralua.dll %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\ICSharpCode.SharpZipLib.dll %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\nlua.dll %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\lua52.dll %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\msvcr110.dll %_CRDEVROOT%\setup\source\
xcopy /sdyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\Toolbar %_CRDEVROOT%\setup\source\Toolbar
xcopy /sdyqi %_CRDEVROOT%\cixreader\bin\%_CONFIG%\Images %_CRDEVROOT%\setup\source\Images

xcopy /dyqi %_CRDEVROOT%\setup\changes.html %_CRDEVROOT%\setup\source\
xcopy /dyqi %_CRDEVROOT%\setup\Acknowledgements.html %_CRDEVROOT%\setup\source\

if "%_AM2SIGNPWD%" == "" goto SkipSign1
echo Signing files...

set _VSVARS="%ProgramFiles%\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
if not exist %_VSVARS% set _VSVARS="%ProgramFiles(x86)%\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
call %_VSVARS%

set _CODEURL=http://timestamp.comodoca.com/authenticode
signtool sign /f "%_PFXPATH%" /p %_AM2SIGNPWD% /t %_CODEURL% /v %_CRDEVROOT%\setup\source\CIXReader.exe
:SkipSign1

echo Building setup program...
call iscc /q /cc %_CRDEVROOT%\setup\CIXReaderSetup.iss /o%_CRDEVROOT%\drops

for /f "tokens=2,3" %%a in (%_CRDEVROOT%\version.bld) do set %%a=%%b
set _VERSTRING=%PRODUCT_MAX_VER%.%PRODUCT_MIN_VER%.%PRODUCT_BUILD%

if "%_AM2SIGNPWD%" == "" goto SkipSign2
echo Signing installers...
signtool sign /f "%_PFXPATH%" /p %_AM2SIGNPWD% /t %_CODEURL% /v %_CRDEVROOT%\drops\cr%_VERSTRING%.exe
:SkipSign2

echo Building Lite distribution...
set ZIP7="..\Tools\7z.exe"
set ZARCHIVE=%_CRDEVROOT%\drops\z%_VERSTRING%.zip
if exist %ZARCHIVE% del %ZARCHIVE%
%ZIP7% a -r %ZARCHIVE% %_CRDEVROOT%\setup\source\*
goto Exit

:NoBuild
echo Error: Cannot build setup. No release binaries found. Do a release build first.
goto Exit

:NoRoot
echo Error: _CRDEVROOT must be set to the root folder of the CIXReader enlistment.

:Exit
endlocal
