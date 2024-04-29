FROM defradigital/dotnetcore:dotnet6.0 AS base

USER root
ARG PORT=8080
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}

# Install dotnet-ef tool
RUN dotnet tool install --global dotnet-ef --version 6.* --tool-path /tools

# Add the tools folder to PATH
ENV PATH="${PATH}:/tools"

RUN apk update && apk --no-cache add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0

FROM defradigital/dotnetcore-development:dotnet6.0 AS build
USER root
WORKDIR /src
COPY ["EPR.Accreditation.API/EPR.Accreditation.API.csproj", "EPR.Accreditation.API/"]
COPY ["EPR.Accreditation.API.Common/EPR.Accreditation.API.Common.csproj", "EPR.Accreditation.API.Common/"]
COPY ["EPR.Accreditation.Common.Data/EPR.Accreditation.API.Common.Data.csproj", "EPR.Accreditation.Common.Data/"]
RUN dotnet restore "EPR.Accreditation.API/EPR.Accreditation.API.csproj"

COPY --chown=dotnet:dotnet EPR.Accreditation.API/. ./EPR.Accreditation.API/.
COPY --chown=dotnet:dotnet EPR.Accreditation.API.Common/. ./EPR.Accreditation.API.Common/.
COPY --chown=dotnet:dotnet EPR.Accreditation.Common.Data/. ./EPR.Accreditation.Common.Data/.

WORKDIR "/src/EPR.Accreditation.API"
RUN dotnet build "EPR.Accreditation.API.csproj" -c Release /p:AzureBuild=false -o /app/build

FROM build AS publish
RUN dotnet publish "EPR.Accreditation.API.csproj" -c Release /p:AzureBuild=false -o /app/publish

FROM base AS final

# Switch to the non-root user
USER dotnet

WORKDIR /app
COPY --from=publish /app/publish .
USER dotnet
ENTRYPOINT ["dotnet", "EPR.Accreditation.API.dll"]