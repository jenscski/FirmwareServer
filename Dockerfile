FROM microsoft/dotnet:2.1.402-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY ./*.sln .
COPY ./src/FirmwareServer/*.csproj ./src/FirmwareServer/
RUN dotnet restore

# copy everything else and build app
COPY ./ ./
WORKDIR /app/src/FirmwareServer
RUN dotnet build -c Release

FROM build AS publish
WORKDIR /app/src/FirmwareServer
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1.4-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=publish /app/src/FirmwareServer/out ./
RUN mkdir /var/lib/fwsrv
RUN find . -type d -print0 | xargs -0 chmod 0755
RUN find . -type f -print0 | xargs -0 chmod 0644
ENTRYPOINT ["dotnet", "FirmwareServer.dll"]
