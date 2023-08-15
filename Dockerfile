FROM hub.nexdev.net/base/dotnet/core/sdk:5.0.102-ca-patch-buster-slim AS builder

WORKDIR /src

COPY "./src/Fanex.NAP.SPU.FS" "Fanex.NAP.SPU.FS/"
COPY "./src/Fanex.NAP.SPU.FS.JobService/Fanex.NAP.SPU.FS.JobService.csproj" "Fanex.NAP.SPU.FS.JobService/"

WORKDIR "/src/Fanex.NAP.SPU.FS.JobService"

RUN dotnet nuget add source http://nuget.nexdev.net/api/v2

RUN dotnet restore -v n

COPY ./src/Fanex.NAP.SPU.FS.JobService .

RUN dotnet publish -r linux-x64 --self-contained false -o /app

FROM hub.nexdev.net/rebuild-baseimage/dotnetcore/aspnet:5.0-alpine AS base

WORKDIR /app

COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.NAP.SPU.FS.JobService.dll"]