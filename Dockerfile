# build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY FiapCloudUsers/UserService/UserService.csproj UserService/
COPY SharedMessages/SharedMessages.csproj SharedMessages/

RUN dotnet restore UserService/UserService.csproj

COPY FiapCloudUsers/UserService/. UserService/
COPY SharedMessages/. SharedMessages/

RUN dotnet publish UserService/UserService.csproj -c Release -o /app/publish

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "UserService.dll"]
