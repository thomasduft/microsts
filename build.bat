@echo off
set /p version=Enter Releaseversion:

echo version: %version%

if [%version%] == [] (
  echo No version specified! Please specify a valid version like 1.2.3!
  goto Done
)
if [%version%] == [""] (
  echo No version specified! Please specify a valid version like 1.2.3!
  goto Done
)


echo Cleaning publish environment
if exist publish/\nul (
  echo Removing directory publish/
  rmdir /s/q "publish/"
)


REM echo -----------------------------------------------------------------------------------------------
REM echo Deploy client app
REM cd src/clientapp && npm i && npm run build-clientapp && cd ../..


echo -----------------------------------------------------------------------------------------------
echo Restore and publish solution
REM cd src/web && npm i && npm run build && cd ../..
dotnet publish src/web/tomware.Microsts.Web.csproj -c Release /p:PackageVersion=%version% /p:Version=%version% -r win10-x64 --self-contained true -o publish/


:Done
echo -----------------------------------------------------------------------------------------------
echo Done
