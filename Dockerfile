# ===================================================================
# ETAPA 1: BUILD do front-end usando Node
# ===================================================================
FROM node:18 AS client-build

# 1.1) Define diretório de trabalho no “ClientApp”
WORKDIR /app/ClientApp

# 1.2) Copia somente package.json e package-lock.json para cache do npm
COPY ClientApp/package*.json ./

# 1.3) Instala dependências do front
RUN npm install

# 1.4) Copia o restante do front-end e gera o “dist/”
COPY ClientApp/. ./
RUN npm run build


# ===================================================================
# ETAPA 2: BUILD do back-end (.NET 8.0), incorporando o front
# ===================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 2.1) Copia APENAS o csproj para restaurar em cache
COPY testeApi/testeApi.csproj ./testeApi/

# 2.2) Restaura os pacotes NuGet
WORKDIR /app/testeApi
RUN dotnet restore

# 2.3) Copia TODO o código-fonte da API (sem criar subpastas extras)
COPY testeApi/. ./  

# 2.4) Copia, da etapa “client-build”, os arquivos estáticos do front para wwwroot
WORKDIR /app/testeApi/wwwroot
COPY --from=client-build /app/ClientApp/dist ./

# 2.5) Volta para a pasta da API e faz publish em Release
WORKDIR /app/testeApi
RUN dotnet publish -c Release -o /app/publish


# ===================================================================
# ETAPA 3: RUNTIME com ASP.NET Core 8.0 (imagem enxuta)
# ===================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# 3.1) Cria um usuário não-root por segurança
RUN useradd -m appuser
USER appuser

# 3.2) Copia os artefatos publicados pelo “build”
COPY --from=build /app/publish ./

# 3.3) Expõe a porta 80
EXPOSE 80

# 3.4) Define variáveis de ambiente para ASP.NET Core
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# 3.5) Ponto de entrada para iniciar a API
ENTRYPOINT ["dotnet", "testeApi.dll"]
