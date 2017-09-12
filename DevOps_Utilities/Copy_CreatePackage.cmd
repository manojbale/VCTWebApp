@echo off

:: --------- Create folder structure

cd DevOps_Utilities

if NOT EXIST "Deployment\Installable\VCTWebApp" mkdir Deployment\Installable\VCTWebApp\

::if NOT EXIST "Deployment\Installable\Install" mkdir Deployment\Installable\Install\

:: --------- Copying and deleteing the files & folders which are not required
cd ..
xcopy /S /O /Y WebSites\VCTWebApp\*.* DevOps_Utilities\Deployment\Installable\VCTWebApp /X /E

::xcopy /S /O /Y Devops_Utilities\Buildscripts\Install\*.* Devops_Utilities\Deployment\Installable\Install /X /E


:: --------- Remove all unwanted files & folders
cd Devops_Utilities\Deployment\Installable\VCTWebApp
del /f /q *.cs*
del /f /q *.sln*
rd /s /q obj Properties shared "Service References"


pushd
cd %~dp0
echo.
echo -------------------------------------------------------------
echo Building the deployment package ...
echo -------------------------------------------------------------
echo.

echo --------  %~dp0

set /a OLD_BUILD_NUMBER=%BUILD_NUMBER% - 1
set OLD_VCTWebApp_PACKAGE_NAME=VCTWebApp_%OLD_BUILD_NUMBER%.zip

set VCTWebApp_PACKAGE_NAME=VCTWebApp_%BUILD_NUMBER%.zip

ECHO OLD_PackageName = %OLD_VCTWebApp_PACKAGE_NAME%
ECHO PackageName = %VCTWebApp_PACKAGE_NAME%

:: --------------------------------------------------------------
echo Cleanup Deployment files ...
:: --------------------------------------------------------------
:: if exist "%VCTWebApp_PACKAGE_NAME%" rmdir /Q %VCTWebApp_PACKAGE_NAME%
if exist "%OLD_VCTWebApp_PACKAGE_NAME%" del /f /q %OLD_VCTWebApp_PACKAGE_NAME%


:: --------------------------------------------------------------
echo Deploy Settings Files  ...
:: --------------------------------------------------------------
if NOT EXIST "Deployment\Installable\VCTWebApp" goto ERROR

:: --------------------------------------------------------------
echo Creating VCTWebApp_Build.zip ...
:: --------------------------------------------------------------
7z.exe a -r %VCTWebApp_PACKAGE_NAME% .\Deployment\Installable\*.*
if NOT %ERRORLEVEL% == 0 goto ERROR

cd %~dp0
exit /b 0

:ERROR
echo.
echo An error occured while creating the deployment package. Exiting...
echo.
cd %~dp0
exit /b -1