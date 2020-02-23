#!/bin/sh

if [ -z "$1" ]
then
  echo No version specified
  exit 1
else
  echo version $1
fi

echo Prepare build environment
if [ -d "publish" ]
then
  rm -Rf publish;
fi

echo Restore solution
dotnet restore src/web/tomware.Microsts.Web.csproj

echo Building solution
dotnet publish src/web/tomware.Microsts.Web.csproj -c Release -r linux-x64 -o ./publish/linux-x64
# dotnet publish src/web/tomware.Microsts.Web.csproj -c Release -r linux-arm -o ./publish/linux-arm 

# echo Deploy web app
# cd src/clientapp && npm i && npm run build-webclient && cd ../..;

echo Building docker image tomware/microsts:$1
#echo $PWD
docker build --build-arg source=publish/linux-x64 -f docker/dockerfile -t tomware/microsts:$1 .
# docker build --build-arg source=publish/linux-arm -t tomware/microsts:$1-linux-arm .

#echo Cleaning up
#if [ -d "publish" ]
#then
#  rm -Rf publish;
#fi

echo Done
docker images