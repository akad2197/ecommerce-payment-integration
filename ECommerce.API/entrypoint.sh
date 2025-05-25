#!/bin/bash

# Wait for PostgreSQL to be ready
echo "Waiting for PostgreSQL to be ready..."
while ! nc -z postgres 5432; do
  sleep 0.1
done
echo "PostgreSQL is ready!"

# Run migrations
echo "Running database migrations..."
dotnet ef database update --project ECommerce.Infrastructure --startup-project ECommerce.API

# Start the application
echo "Starting the application..."
dotnet run --project ECommerce.API --no-build 