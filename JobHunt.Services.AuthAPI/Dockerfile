#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["JobHunt.Services.AuthAPI/JobHunt.Services.AuthAPI.csproj", "JobHunt.Services.AuthAPI/"]
RUN dotnet restore "./JobHunt.Services.AuthAPI/JobHunt.Services.AuthAPI.csproj"
COPY . .
WORKDIR "/src/JobHunt.Services.AuthAPI"
RUN dotnet build "./JobHunt.Services.AuthAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JobHunt.Services.AuthAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobHunt.Services.AuthAPI.dll"]