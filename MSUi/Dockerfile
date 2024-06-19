
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80




FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MSUi/MSUi.csproj", "MSUi/"]
RUN dotnet restore "./MSUi/./MSUi.csproj"
COPY . .
WORKDIR "/src/MSUi"
RUN dotnet build "./MSUi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MSUi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MSUi.dll"]

### Install sqlcmd
RUN apt-get update && apt-get install -y mssql-tools unixodbc-dev