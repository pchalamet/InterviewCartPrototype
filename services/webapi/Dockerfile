FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY . /app
WORKDIR /app

EXPOSE 80
ENTRYPOINT ["dotnet", "webapi.dll"]
