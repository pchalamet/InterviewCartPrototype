param([string] $configuration = "Debug", [string] $artifact)

dotnet build -c $configuration .\cart-prototype.sln
dotnet test -c $configuration .\cart-prototype.sln

if ($artifact) {
    if (Test-Path $artifact) {
        Remove-Item -recurse $artifact
    }

    dotnet publish -o $artifact/silo -c $configuration .\services\silo\silo.csproj
    dotnet publish -o $artifact/webapi -c $configuration .\services\webapi\webapi.csproj.csproj
    dotnet publish -o $artifact/client -c $configuration .\client\cart.client\cart.client.csproj
}