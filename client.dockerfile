FROM microsoft/dotnet:2.2-sdk as builder
ARG configuration=Release

COPY . /build
RUN echo "Building $configuration"
RUN dotnet build -c $configuration /build/client/client.csproj
RUN dotnet publish -c $configuration --output /artifacts/client /build/client/client.csproj

FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY --from=builder /artifacts/client /app
WORKDIR /app
ENTRYPOINT ["dotnet", "client.dll"]
