# build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# ===== ARG para receber token =====
ARG GH_USERNAME
ARG GH_TOKEN

# ===== configura NuGet dentro do container =====
RUN dotnet nuget add source https://nuget.pkg.github.com/${GH_USERNAME}/index.json \
    --name github \
    --username ${GH_USERNAME} \
    --password ${GH_TOKEN} \
    --store-password-in-clear-text

# copia csproj
COPY UserService/UserService.csproj UserService/

# restore (AGORA AUTENTICADO)
RUN dotnet restore UserService/UserService.csproj

# copia código
COPY UserService/. UserService/

# publish
RUN dotnet publish UserService/UserService.csproj -c Release -o /app/publish

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "UserService.dll"]
