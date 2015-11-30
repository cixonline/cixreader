@echo off
setlocal

if "%_CRDEVROOT%" == "" goto NoRoot

set _CONFIG="Release"
set _OPTION=rebuild
set _BETA=1

if "%1" == "" goto DoBuild
if "%1" == "release" set _BETA=

:DoBuild
set _VSVARS="%ProgramFiles%\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
if not exist %_VSVARS% set _VSVARS="%ProgramFiles(x86)%\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
if not exist %_VSVARS% goto MissingVS
call %_VSVARS%

for /f "tokens=2,3" %%a in (%_CRDEVROOT%\version.bld) do set %%a=%%b

set _VERSTRING=%PRODUCT_MAX_VER%.%PRODUCT_MIN_VER%.%PRODUCT_BUILD%
if "%_BETA%" == "1" set _BETASTRING=Beta

perl %_CRDEVROOT%\scripts\updateasmversion.pl CIXReader\Properties\AssemblyInfo.cs %_VERSTRING% %_BETASTRING%
perl %_CRDEVROOT%\scripts\updateasmversion.pl CIXClient\Properties\AssemblyInfo.cs %_VERSTRING% %_BETASTRING%
perl %_CRDEVROOT%\scripts\updateasmversion.pl CIXMarkup\Properties\AssemblyInfo.cs %_VERSTRING% %_BETASTRING%

set _FOLDER=release
if "%_BETA%" == "1" set _FOLDER=beta
mkdir %_CRDEVROOT%\drops 2>nul
perl %_CRDEVROOT%\scripts\replacetokens.pl setup\appcast.xml.template %_CRDEVROOT%\drops\appcast.xml %_VERSTRING% %_FOLDER%

set _OUTFILENAME=%_CRDEVROOT%\drops\changes.html
perl %_CRDEVROOT%\scripts\replacetokens.pl %_CRDEVROOT%\setup\changes.html %_OUTFILENAME% %_VERSTRING% %_FOLDER% 1

set _OUTFILENAME=%_CRDEVROOT%\drops\changesNoScript.html
perl %_CRDEVROOT%\scripts\replacetokens.pl %_CRDEVROOT%\setup\changes.html %_OUTFILENAME% %_VERSTRING% %_FOLDER% 0

devenv cixreader.sln /%_OPTION% %_CONFIG% /projectconfig %_CONFIG%
if errorlevel 1 goto BuildFailed

call %_CRDEVROOT%\setup\build.cmd
goto Exit

:BuildFailed
echo Error: Build failed. Stopping
goto Exit

:MissingVS
echo Error: Microsoft Visual Studio not found.
echo        Cannot locate %_VSVARS%
goto Exit

:NoRoot
echo Error: _CRDEVROOT must be set to the root folder of the CIXReader enlistment.

:Exit
endlocal
