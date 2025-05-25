FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

# Copy csproj and restore dependencies
COPY ["ECommerce.API/ECommerce.API.csproj", "ECommerce.API/"]
COPY ["ECommerce.Application/ECommerce.Application.csproj", "ECommerce.Application/"]
COPY ["ECommerce.Domain/ECommerce.Domain.csproj", "ECommerce.Domain/"]
COPY ["ECommerce.Infrastructure/ECommerce.Infrastructure.csproj", "ECommerce.Infrastructure/"]
COPY ["ECommerce.Contracts/ECommerce.Contracts.csproj", "ECommerce.Contracts/"]
RUN dotnet restore "ECommerce.API/ECommerce.API.csproj"

# Copy the rest of the code
COPY . .

# Build
RUN dotnet build "ECommerce.API/ECommerce.API.csproj" -c Release

# Install EF Core tools
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy entrypoint script
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"] 