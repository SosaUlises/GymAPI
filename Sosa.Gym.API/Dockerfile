# Imagen base de ASP.NET para producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Imagen para compilar el proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar la solución
COPY *.sln ./

# Copiar los proyectos con sus nombres REALES
COPY Sosa.Gym.API/Sosa.Gym.API.csproj Sosa.Gym.API/
COPY Sosa.Gym.Application/Sosa.Gym.Application.csproj Sosa.Gym.Application/
COPY Sosa.Gym.Domain/Sosa.Gym.Domain.csproj Sosa.Gym.Domain/
COPY Sosa.Gym.Common/Sosa.Gym.Common.csproj Sosa.Gym.Common/
COPY Sosa.Gym.External/Sosa.Gym.External.csproj Sosa.Gym.External/
COPY Sosa.Gym.Persistence/Sosa.Gym.Persistence.csproj Sosa.Gym.Persistence/

# Restaurar
RUN dotnet restore

# Copiar todo el código
COPY . .

# Build
WORKDIR "/src/Sosa.Gym.API"
RUN dotnet build -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Imagen final de ejecución
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sosa.Gym.API.dll"]
