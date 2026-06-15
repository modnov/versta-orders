Тестовое задание Versta

## Стек

- ASP.NET Core 9 (minimal API)
- Entity Framework Core + SQLite
- React + TypeScript + Vite

## Требования

- .NET SDK 9.0+
- Node.js 18+

## Запуск

1. Бэкенд (при первом запуске создаёт базу `orders.db`):

   ```bash
   dotnet run --project src/VerstaOrders.Api
   ```

   API поднимется на `http://localhost:5000`.

2. Фронтенд:

   ```bash
   cd client
   npm install
   npm run dev
   ```

   Откройте `http://127.0.0.1:5173`.

## Запуск через Docker

```bash
docker compose up --build
```

Приложение будет доступно на `http://localhost:8080`.