dotnet build HealthChecker.sln

start cmd /k "dotnet run --project ./HealthChecker.Api/HealthChecker.Api.csproj"
start cmd /k "dotnet run --project ./Services/HealthChecker.East/HealthChecker.East.csproj"
start cmd /k "dotnet run --project ./Services/HealthChecker.West/HealthChecker.West.csproj"
start cmd /k "dotnet run --project ./Services/HealthChecker.South/HealthChecker.South.csproj"