#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#RUN useradd -ms /bin/bash admin
USER admin
WORKDIR /app
#RUN chown -R admin /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DigitalAssetManagement.API/DigitalAssetManagement.API.csproj", "DigitalAssetManagement.API/"]
COPY ["DigitalAssetManagement.Infrastructure/DigitalAssetManagement.Infrastructure.csproj", "DigitalAssetManagement.Infrastructure/"]
COPY ["DigitalAssetManagement.Application/DigitalAssetManagement.Application.csproj", "DigitalAssetManagement.Application/"]
COPY ["DigitalAssetManagement.Domain/DigitalAssetManagement.Domain.csproj", "DigitalAssetManagement.Domain/"]
RUN dotnet restore "./DigitalAssetManagement.API/DigitalAssetManagement.API.csproj"
COPY . .
WORKDIR "/src/DigitalAssetManagement.API"
RUN dotnet build "./DigitalAssetManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DigitalAssetManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DigitalAssetManagement.API.dll"]