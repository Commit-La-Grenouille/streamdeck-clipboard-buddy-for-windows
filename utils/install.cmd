@echo off
REM USAGE: Install.bat <DEBUG/RELEASE>
REM --> The project folder and UUID are set as vars ;)
setlocal
set PROJ=ClipboardBuddy
set UUID=net.localhost.streamdeck.clipboard-buddy-for-windows

:: cd /d %~dp0
set build_workdir=%~dp0..\%PROJ%\bin\Debug
cd %build_workdir%
:: simpler than asking for a destination under bin

REM *** MAKE SURE THE FOLLOWING VARIABLES ARE CORRECT ***
REM (Distribution tool be downloaded from: https://docs.elgato.com/sdk/plugins/packaging )
SET OUTPUT_DIR=%APPDATA%\..\Local\Temp
SET DISTRIBUTION_TOOL="%~dp0..\..\DistributionTool.exe"
SET STREAM_DECK_FILE="C:\Program Files\Elgato\StreamDeck\StreamDeck.exe"
SET STREAM_DECK_LOAD_TIMEOUT=7

taskkill /f /im streamdeck.exe
taskkill /f /im %UUID%.exe
timeout /t 2

del %OUTPUT_DIR%\%UUID%.streamDeckPlugin

@echo on
%DISTRIBUTION_TOOL% -b -i %UUID%.sdPlugin -o %build_workdir%
@echo off
IF %ERRORLEVEL% NEQ 0 (Echo ---------- AN ERROR WAS FOUND ---------- & Exit /b 1)

rmdir %APPDATA%\Elgato\StreamDeck\Plugins\%UUID%.sdPlugin /s /q
START "" %STREAM_DECK_FILE%
timeout /t %STREAM_DECK_LOAD_TIMEOUT%
%build_workdir%\%UUID%.streamDeckPlugin
