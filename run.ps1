param([string] $artifact)

Write-Host "Starting Silo"
start-process dotnet $artifact\silo\silo.dll

Write-Host "Starting WebApi"
start-process dotnet $artifact\webapi\webapi.dll

Start-Sleep 5
Write-Host "Running client"
dotnet $artifact\client\client.dll
