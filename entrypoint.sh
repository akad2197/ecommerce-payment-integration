#!/bin/bash

set -e

echo "⏳ PostgreSQL servisi bekleniyor..."
until dotnet ef database update --project /app/ECommerce.Infrastructure --startup-project /app/ECommerce.API; do
  >&2 echo "⛔️ Migration başarısız, tekrar deneniyor..."
  sleep 3
done

echo "✅ Migration tamamlandı. Uygulama başlatılıyor..."
exec dotnet ECommerce.API.dll
