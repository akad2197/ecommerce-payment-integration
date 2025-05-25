# 🛒 ECommerce Payment Integration

This project implements a modular, clean-architecture-based e-commerce payment integration system using .NET 8. It includes layered separation between API, Application, Domain, Infrastructure, and shared Contracts.

---

## 📁 Project Structure

* `ECommerce.API` - ASP.NET Core Web API entry point.
* `ECommerce.Application` - Business logic, CQRS handlers, interfaces.
* `ECommerce.Domain` - Core domain models and rules.
* `ECommerce.Infrastructure` - Implementations for services, persistence, and HTTP clients.
* `ECommerce.Contracts` - Shared DTOs used across layers.

---

## 🧪 Testing & Coverage Guide

### ✅ Run Unit Tests

All unit tests are written using **xUnit**:

```bash
dotnet test
```

### 📈 Generate Code Coverage Report

Install Coverlet if not already installed:

```bash
dotnet tool install --global coverlet.console
```

Run coverage command:

```bash
coverlet ./ECommerce.Tests/bin/Debug/net8.0/ECommerce.Tests.dll \
  --target "dotnet" \
  --targetargs "test ./ECommerce.Tests/ECommerce.Tests.csproj" \
  --format "lcov"
```

You can visualize `coverage.lcov` using:

* [https://coveralls.io/](https://coveralls.io/)
* VS Code plugin: **Coverage Gutters**

---

## 🐳 Docker Compose: Run the Full Application

### 📦 Prerequisites

* Docker installed ([download here](https://www.docker.com/products/docker-desktop))

### ▶️ Run the Application

```bash
docker-compose up --build
```

This will:

* Build the API
* Run all necessary services (e.g., API, external balance service)

### 🔁 Rebuild When Needed

```bash
docker-compose up --build
```

### 🧼 Stop & Clean Up

```bash
docker-compose down
```

### 🌐 Access the API

Swagger UI:

```
http://localhost:5000/swagger
```

(Adjust port according to your docker-compose.yml)

---

## 💡 Notes

* Clean Architecture enforces separation of concerns and testability.
* DTOs shared between Application and Infrastructure are stored in `Contracts`.
* FluentValidation is used for validating commands.
* A global exception handling middleware returns clean JSON responses for API errors.

---

## 🧾 License

MIT License (or your chosen license)
