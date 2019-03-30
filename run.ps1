param([string] $artifact)

Write-Host "Starting Silo"
$silo = start-job {
    param([string] $artifact)    
    dotnet $artifact\silo\silo.dll
} -ArgumentList $artifact
Start-Sleep 10


Write-Host "Starting WebApi"
$webapi = Start-Job {
    param([string] $artifact)
    dotnet $artifact\webapi\webapi.dll
} -ArgumentList $artifact
Start-Sleep 5


Write-Host "Running client"
dotnet $artifact\client\cart.client.dll

Stop-Job $webapi
Stop-Job $silo
