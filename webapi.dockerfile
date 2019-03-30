FROM microsoft/dotnet:2.2-sdk as builder
ARG configuration=Release

COPY . /build
RUN echo "Building $configuration"
RUN dotnet build -c $configuration /build/services/webapi/webapi.csproj
RUN dotnet test -c $configuration /build/services/webapi.tests/webapi.tests.csproj
RUN dotnet publish -c $configuration --output /artifacts/webapi /build/services/webapi/webapi.csproj

FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY --from=builder /artifacts/webapi /app
WORKDIR /app
ENTRYPOINT ["dotnet", "webapi.dll"]
