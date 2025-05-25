# Build aşaması (SDK içeren imaj)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ECommerce.API/ECommerce.API.csproj", "ECommerce.API/"]
COPY ["ECommerce.Application/ECommerce.Application.csproj", "ECommerce.Application/"]
COPY ["ECommerce.Domain/ECommerce.Domain.csproj", "ECommerce.Domain/"]
COPY ["ECommerce.Infrastructure/ECommerce.Infrastructure.csproj", "ECommerce.Infrastructure/"]
COPY ["ECommerce.Contracts/ECommerce.Contracts.csproj", "ECommerce.Contracts/"]

RUN dotnet restore "ECommerce.API/ECommerce.API.csproj"
COPY . .

RUN dotnet publish "ECommerce.API/ECommerce.API.csproj" -c Release -o /app/publish

# Final aşama (sadece runtime içeren imaj)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ECommerce.API.dll"]
