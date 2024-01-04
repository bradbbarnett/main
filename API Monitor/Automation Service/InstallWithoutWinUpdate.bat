@echo off

:: BatchGotAdmin
:-------------------------------------
REM  --> Check for permissions
>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"

REM --> If error flag set, we do not have admin.
if '%errorlevel%' NEQ '0' (
    echo Requesting administrative privileges...
    goto UACPrompt
) else ( goto gotAdmin )

:UACPrompt
    echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs"
    set params = %*:"=""
    echo UAC.ShellExecute "cmd.exe", "/c %~s0 %params%", "", "runas", 1 >> "%temp%\getadmin.vbs"

    "%temp%\getadmin.vbs"
    del "%temp%\getadmin.vbs"
    exit /B

:gotAdmin
    pushd "%CD%"
    CD /D "%~dp0"
:--------------------------------------

echo Checking if batch file is running from USB flash drive...
ping 127.0.0.1 -n 1 > nul
if "%~dp0" == "C:\Automation Service\" (
    goto correctDirectory
) else (
    goto incorrectDirectory
)

:incorrectDirectory
echo Copying files to C:\Automation Service\
ping 127.0.0.1 -n 1 > nul
cd %~dp0
cd ..
xcopy /Y /S "Automation Service" "C:\Automation Service\"
goto correctDirectory

:correctDirectory
echo Checking if KB2999226 is installed...
ping 127.0.0.1 -n 1 > nul
wmic qfe get hotfixid | find "KB2999226" >result.txt
set /p DATA=<result.txt
del result.txt

cd /D C:\Automation Service\Contents\

if "%DATA%" == "KB2999226  " (
    goto next
) else (
    goto install
)

:install
:echo KB2999226 not installed.
:echo Installing KB2999226...
:ping 127.0.0.1 -n 1 > nul
:START Windows8-RT-KB2999226-x64.msu
:echo KB2999226 install finished.
goto next

:next
echo Installing AutomationAPI service...
ping 127.0.0.1 -n 1 > nul
InstallUtil.exe AutomationAPI.exe
echo AutomationAPI service installed.
echo Starting AutomationAPI service...
ping 127.0.0.1 -n 1 > nul
sc start AutomationAPI
exit