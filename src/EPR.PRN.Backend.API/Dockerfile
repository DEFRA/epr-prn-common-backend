#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Install dotnet-ef tool
RUN dotnet tool install dotnet-ef --version 8.* --tool-path "/root/.dotnet/tools"
 
# Add the tools folder to PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /src
COPY ["EPR.PRN.Backend.API/EPR.PRN.Backend.API.csproj", "EPR.PRN.Backend.API/"]
COPY ["EPR.PRN.Backend.Data/EPR.PRN.Backend.Data.csproj", "EPR.PRN.Backend.Data/"]
COPY ["EPR.PRN.Backend.API.Common/EPR.PRN.Backend.API.Common.csproj", "EPR.PRN.Backend.API/"]

RUN dotnet restore "./EPR.PRN.Backend.API/EPR.PRN.Backend.API.csproj"
COPY . .
WORKDIR "/src/EPR.PRN.Backend.API"
RUN dotnet build "./EPR.PRN.Backend.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EPR.PRN.Backend.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EPR.PRN.Backend.API.dll"]