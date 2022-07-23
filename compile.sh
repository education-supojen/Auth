#!/bin/bash
dotnet publish src/Auth.Presentation/Auth.Presentation.csproj -r linux-x64 --self-contained false --configuration Release -o compile
