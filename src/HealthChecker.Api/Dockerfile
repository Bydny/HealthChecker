FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY "HealthChecker.Api/." "HealthChecker.Api/."
COPY "HealthChecker.Contracts/." "HealthChecker.Contracts/."
COPY "HealthChecker.ServiceBus/." "HealthChecker.ServiceBus/."
COPY "HealthChecker.ServiceBus.Extensions/." "HealthChecker.ServiceBus.Extensions/."

WORKDIR /src/HealthChecker.Api
RUN dotnet publish "HealthChecker.Api.csproj" -c Release -o /app/out

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/out .
ENTRYPOINT ["dotnet", "HealthChecker.Api.dll"]
