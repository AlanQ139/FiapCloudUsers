# build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copia csproj e nuget.config
COPY FiapCloudUsers/UserService/UserService.csproj UserService/
COPY FiapCloudUsers/nuget.config .

# restore via GitHub Packages
RUN dotnet restore UserService/UserService.csproj

# copia o código
COPY FiapCloudUsers/UserService/. UserService/

# publish
RUN dotnet publish UserService/UserService.csproj -c Release -o /app/publish

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "UserService.dll"]
