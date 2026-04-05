FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ICMarketsTest.WebApi/ICMarketsTest.WebApi.csproj", "src/ICMarketsTest.WebApi/"]
COPY ["src/ICMarketsTest.BlockCypher/ICMarketsTest.BlockCypher.csproj", "src/ICMarketsTest.BlockCypher/"]
COPY ["src/ICMarketsTest.Core/ICMarketsTest.Core.csproj", "src/ICMarketsTest.Core/"]
COPY ["src/ICMarketsTest.Storage/ICMarketsTest.Storage.csproj", "src/ICMarketsTest.Storage/"]
COPY ["src/ICMarketsTest.Configuration/ICMarketsTest.Configuration.csproj", "src/ICMarketsTest.Configuration/"]
RUN dotnet restore "src/ICMarketsTest.WebApi/ICMarketsTest.WebApi.csproj"
COPY . .
WORKDIR "/src/src/ICMarketsTest.WebApi"
RUN dotnet build "./ICMarketsTest.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ICMarketsTest.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
# Set Development configuration for test only, just to enable swagger
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:8080
RUN apt-get update && apt-get install --yes --no-install-recommends curl
COPY --from=publish /app/publish .
# Set health checking
HEALTHCHECK --interval=30s --timeout=5s --retries=3 CMD curl --silent --fail http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "ICMarketsTest.WebApi.dll"]