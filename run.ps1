param([string] $artifact)

Write-Host "Starting Silo"
$silo = Start-Job {
    param([string] $artifact)
    start-process dotnet $artifact\silo\silo.dll
} -ArgumentList $artifact

Write-Host "Starting WebApi"
$webapi = Start-Job {
    param([string] $artifact)
    start-process dotnet $artifact\webapi\webapi.dll
} -ArgumentList $artifact

Start-Sleep 5
Write-Host "Running client"
dotnet $artifact\client\client.dll

Stop-Job $webapi
Stop-Job $silo
