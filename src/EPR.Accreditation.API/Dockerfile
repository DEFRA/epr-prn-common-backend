FROM defradigital/dotnetcore-development:dotnet6.0 AS build-env

ARG PORT=3000
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}

COPY --chown=dotnet ["EPR.Accreditation.API/EPR.Accreditation.API.csproj", "EPR.Accreditation.API/"]
COPY --chown=dotnet ["EPR.Accreditation.API.Common/EPR.Accreditation.API.Common.csproj", "EPR.Accreditation.API.Common/"]
COPY --chown=dotnet ["EPR.Accreditation.Common.Data/EPR.Accreditation.API.Common.Data.csproj", "EPR.Accreditation.Common.Data/"]

RUN dotnet restore "EPR.Accreditation.API/EPR.Accreditation.API.csproj"

USER root
RUN apk update && apk add --no-cache npm
USER dotnet
ENV NODE_EXTRA_CA_CERTS=/usr/local/share/ca-certificates/internal-ca.crt

COPY --chown=dotnet:dotnet . .
WORKDIR /home/dotnet/EPR.Accreditation.API
RUN dotnet publish -c Release -o out

FROM defradigital/dotnetcore:dotnet6.0
COPY --from=build-env --chown=dotnet /home/dotnet/EPR.Accreditation.API/out .

USER root
RUN apk add icu-libs libintl
USER dotnet

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0

ARG PORT=3000
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}

CMD dotnet EPR.Accreditation.API.dll