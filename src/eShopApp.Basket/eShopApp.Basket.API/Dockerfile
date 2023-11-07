# <Command Info> Building dockerfile from root (src)
# docker build -f <path> -t webdeveloper95/<image_name> .

# start with the base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

# set the working directory
WORKDIR /app

# copy the csproj and restore dependencies
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# copy local nuget source
COPY ["nuget.config", "."]
COPY ["Packages/", "Packages/"]

COPY ["eShopApp.Basket/eShopApp.Basket.API/eShopApp.Basket.API.csproj", "eShopApp.Basket/eShopApp.Basket.API/"]
COPY ["eShopApp.Basket/eShopApp.Basket.Application/eShopApp.Basket.Application.csproj", "eShopApp.Basket/eShopApp.Basket.Application/"]
COPY ["eShopApp.Basket/eShopApp.Basket.Infrastructure/eShopApp.Basket.Infrastructure.csproj", "eShopApp.Basket/eShopApp.Basket.Infrastructure/"]
COPY ["eShopApp.Basket/eShopApp.Basket.Contract/eShopApp.Basket.Contract.csproj", "eShopApp.Basket/eShopApp.Basket.Contract/"]
COPY ["eShopApp.Basket/eShopApp.Basket.Domain/eShopApp.Basket.Domain.csproj", "eShopApp.Basket/eShopApp.Basket.Domain/"]
RUN dotnet restore "eShopApp.Basket/eShopApp.Basket.API/eShopApp.Basket.API.csproj" --configfile nuget.config

# copy everything else and build
COPY . .
WORKDIR "/src/eShopApp.Basket/eShopApp.Basket.API"
RUN dotnet build "eShopApp.Basket.API.csproj" -c Release -o /app/build

# publish the project
FROM build AS publish
RUN dotnet publish "eShopApp.Basket.API.csproj" -c Release -o /app/publish

# exposed port
EXPOSE 80

# build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eShopApp.Basket.API.dll"]
