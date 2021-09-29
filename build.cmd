@echo off
setlocal

if "%_CRDEVROOT%" == "" goto NoRoot
if "%VSCMD_VER" == "" goto MissingVS

perl -V > nul
if errorlevel 1 goto MissingPerl

set _CONFIG="Release"
set _OPTION=rebuild
set _BETA=1

if "%1" == "" goto DoBuild
if "%1" == "release" set _BETA=

:DoBuild
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

msbuild cixreader.sln /t:%_OPTION% /p:Configuration=%_CONFIG%
if errorlevel 1 goto BuildFailed

call %_CRDEVROOT%\setup\build.cmd
goto Exit

:BuildFailed
echo Error: Build failed. Stopping
goto Exit

:MissingVS
echo Error: This batch file should be run from a Visual Studio developer command prompt
goto Exit

:MissingPerl
echo Error: Perl is not installed
goto Exit

:NoRoot
echo Error: _CRDEVROOT must be set to the root folder of the CIXReader enlistment.

:Exit
endlocal
