# Etapa base (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar proyectos
COPY Sosa.Gym.API/Sosa.Gym.API.csproj Sosa.Gym.API/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/

# Restaurar dependencias
RUN dotnet restore Sosa.Gym.API/Sosa.Gym.API.csproj

# Copiar el resto del código
COPY . .

# Compilar
WORKDIR /src/Sosa.Gym.API
RUN dotnet build -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Etapa final (runtime)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sosa.Gym.API.dll"]
