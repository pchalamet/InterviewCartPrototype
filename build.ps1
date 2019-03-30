param([string] $configuration = "Debug", [switch] $artifact)

dotnet build -c $configuration ./cart-prototype.sln
dotnet test -c $configuration ./cart-prototype.sln

if ($artifact) {
    $output = Join-Path $PSScriptRoot output

    dotnet publish -o $output/silo -c $configuration ./services/silo/silo.csproj
    dotnet publish -o $output/webapi -c $configuration ./services/webapi/webapi.csproj
    dotnet publish -o $output/client -c $configuration ./client/client.csproj

	docker build -t silo:latest $output/silo
	docker build -t webapi:latest $output/webapi
	docker build -t client:latest $output/client
}