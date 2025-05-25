# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyalarını kopyala
COPY ["ECommerce.API/ECommerce.API.csproj", "ECommerce.API/"]
COPY ["ECommerce.Application/ECommerce.Application.csproj", "ECommerce.Application/"]
COPY ["ECommerce.Domain/ECommerce.Domain.csproj", "ECommerce.Domain/"]
COPY ["ECommerce.Infrastructure/ECommerce.Infrastructure.csproj", "ECommerce.Infrastructure/"]

# Restore paketleri
RUN dotnet restore "ECommerce.API/ECommerce.API.csproj"

# Tüm kaynak kodları kopyala
COPY . .

# EF Core global tool kurulumu
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Publish
RUN dotnet publish "ECommerce.API/ECommerce.API.csproj" -c Release -o /app/publish

# Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# EF tool path
ENV PATH="${PATH}:/root/.dotnet/tools"

# Yayınlanan dosyaları kopyala
COPY --from=build /app/publish .
COPY --from=build /src .

# entrypoint.sh scriptini ekle ve çalıştırılabilir yap
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

EXPOSE 8080

ENTRYPOINT ["./entrypoint.sh"]
