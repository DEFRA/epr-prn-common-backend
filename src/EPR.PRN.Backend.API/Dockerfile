FROM defradigital/dotnetcore-development:dotnet8.0 AS base
 
USER root
ARG PORT=8080
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}
 
RUN apk update && apk --no-cache add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0
 
FROM defradigital/dotnetcore-development:dotnet8.0 AS build
USER root
 
# Install dotnet-ef tool
RUN dotnet tool install dotnet-ef --version 6.* --tool-path "/root/.dotnet/tools"
 
# Add the tools folder to PATH
ENV PATH="${PATH}:/root/.dotnet/tools"
 
WORKDIR /src
COPY ["EPR.PRN.Backend.API/EPR.PRN.Backend.API.csproj", "EPR.PRN.Backend.API/"]
COPY ["EPR.PRN.Backend.Data/EPR.PRN.Backend.Data.csproj", "EPR.PRN.Backend.Data/"]
COPY ["EPR.PRN.Backend.API.Common/EPR.PRN.Backend.API.Common.csproj", "EPR.PRN.Backend.API.Common/"]

RUN dotnet restore "./EPR.PRN.Backend.API/EPR.PRN.Backend.API.csproj"

COPY --chown=dotnet:dotnet EPR.PRN.Backend.API/. ./EPR.PRN.Backend.API/.
COPY --chown=dotnet:dotnet EPR.PRN.Backend.Data/. ./EPR.PRN.Backend.Data/.
COPY --chown=dotnet:dotnet EPR.PRN.Backend.API.Common/. ./EPR.PRN.Backend.API.Common/.

WORKDIR "/src/EPR.PRN.Backend.API"
RUN dotnet build "./EPR.PRN.Backend.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EPR.PRN.Backend.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
USER dotnet
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EPR.PRN.Backend.API.dll"]