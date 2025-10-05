# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia o csproj e restaura as dependências
COPY UserService/UserService.csproj UserService/
RUN dotnet restore UserService/UserService.csproj

# Copia o restante do código e publica
COPY . .
RUN dotnet publish UserService/UserService.csproj -c Release -o /app/publish

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expõe a porta padrão
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Comando de inicialização direto da API
ENTRYPOINT ["dotnet", "UserService.dll"]
