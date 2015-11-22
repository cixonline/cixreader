@echo off
setlocal

if "%_CRDEVROOT%" == "" goto NoRoot

set _CONFIG="Debug"
set _OPTION=build
set _DOSETUP=
if "%1" == "" goto DoBuild

:NextArg
if "%1" == "debug" set _CONFIG="Debug"&&goto ShiftArg
if "%1" == "release" set _CONFIG="Release"&&goto ShiftArg
if "%1" == "clean" set _OPTION=rebuild&&goto ShiftArg
if "%1" == "" goto DoBuild
echo Error: Unknown parameter %1
goto Exit

:ShiftArg
shift
goto NextArg

:MissingVS
echo Error: Microsoft Visual Studio not found.
echo        Cannot locate %_VSVARS%
goto Exit

:DoBuild
set _VSVARS="%ProgramFiles%\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
if not exist %_VSVARS% set _VSVARS="%ProgramFiles(x86)%\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
if not exist %_VSVARS% goto MissingVS
call %_VSVARS%
devenv cixreader.sln /%_OPTION% %_CONFIG% /projectconfig %_CONFIG%
goto Exit

:NoRoot
echo Error: _CRDEVROOT must be set to the root folder of the CIXReader enlistment.

:Exit
endlocal
