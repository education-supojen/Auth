#!/bin/bash
directory="./compile"

if [ -d $directory ]; then
  echo "Directory exsits"
else
  dotnet restore -NoCache -NonInteractive
  dotnet publish src/Auth.Presentation/Auth.Presentation.csproj -r linux-x64 --self-contained false --configuration Release -o compile
fi 