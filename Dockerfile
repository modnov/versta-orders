# --- Сборка фронтенда ---
FROM node:22 AS client
WORKDIR /client
COPY client/package*.json ./
RUN npm ci
COPY client/ ./
RUN npm run build

# --- Сборка бэкенда ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS api
WORKDIR /src
COPY src/VerstaOrders.Api/*.csproj ./VerstaOrders.Api/
RUN dotnet restore VerstaOrders.Api/VerstaOrders.Api.csproj
COPY src/VerstaOrders.Api/ ./VerstaOrders.Api/
RUN dotnet publish VerstaOrders.Api/VerstaOrders.Api.csproj -c Release -o /app

# --- Финальный образ: API + собранный SPA в wwwroot ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=api /app ./
COPY --from=client /client/dist ./wwwroot
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "VerstaOrders.Api.dll"]
