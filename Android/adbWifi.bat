@echo off
setlocal

if NOT "%AndroidSDK%" == "" set PATH=%PATH%;%AndroidSDK%\platform-tools

if "%1%" == "off" goto off

set port=%1
set int=%2

if "%port%" == "" set port=5557
if "%int%" == "" set int=wlan0

adb -d wait-for-device tcpip %port%

set shellCmd="ip addr show %int% | grep 'inet [0-9]{1,3}(\.[0-9]{1,3}){3}' -oE | grep '[0-9]{1,3}(\.[0-9]{1,3}){3}' -oE"

for /f %%i in ('adb wait-for-device shell %shellCmd%') do set IP=%%i

adb connect %IP%:%port%

goto end

:fail
echo adbWifi [port] [interface]
goto end

:off
adb -d wait-for-device usb

:end