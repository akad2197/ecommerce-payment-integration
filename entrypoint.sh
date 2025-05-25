#!/bin/bash
set -e

echo "Running EF migrations..."
dotnet ef database update

echo "Starting the app..."
exec dotnet ECommerce.API.dll
