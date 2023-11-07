#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["eShopApp.Catalog/eShopApp.Catalog.API/eShopApp.Catalog.API.csproj", "eShopApp.Catalog/eShopApp.Catalog.API/"]
COPY ["eShopApp.Catalog/eShopApp.Catalog.Application/eShopApp.Catalog.Application.csproj", "eShopApp.Catalog/eShopApp.Catalog.Application/"]
COPY ["eShopApp.Catalog/eShopApp.Catalog.Infrastructure/eShopApp.Catalog.Infrastructure.csproj", "eShopApp.Catalog/eShopApp.Catalog.Infrastructure/"]
COPY ["eShopApp.Catalog/eShopApp.Catalog.Contract/eShopApp.Catalog.Contract.csproj", "eShopApp.Catalog/eShopApp.Catalog.Contract/"]
COPY ["eShopApp.Catalog/eShopApp.Catalog.Domain/eShopApp.Catalog.Domain.csproj", "eShopApp.Catalog/eShopApp.Catalog.Domain/"]

RUN dotnet restore "eShopApp.Catalog/eShopApp.CatalogAPI/eShopApp.CatalogAPI.csproj"

COPY . .
WORKDIR "/src/eShopApp.Catalog/eShopApp.Catalog.API"
RUN dotnet build "eShopApp.Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eShopApp.Catalog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eShopApp.Catalog.API.dll"]