@echo off
pushd "%~dp0"
PowerShell -C "(type src\LogSplit\Parser.cs) -replace 'namespace\s+[a-z]+', 'namespace $rootnamespace$.LogSplit' | Out-File -Encoding ascii src\LogSplit\Parser.cs.pp"
rem PowerShell -C "(type src\LogSplit\Parser.cs) -replace 'namespace\s+[a-z]+', 'namespace $rootnamespace$' | Out-File -Encoding ascii src\LogSplit\Parser.cs.pp" ^
rem && call build ^
rem && nuget pack LogSplit.Source.nuspec

rem popd
pause