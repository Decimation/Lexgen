@echo off

dotnet publish -c Release -r win10-x64 --self-contained true

ping localhost >nul
