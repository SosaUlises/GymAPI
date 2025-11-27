# Imagen base de ASP.NET para producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Imagen para compilar el proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln ./
COPY Sosa.Gym.API/ Sosa.Gym.API/
COPY Application/ Application/
COPY Domain/ Domain/
COPY Infrastructure/ Infrastructure/

RUN dotnet restore Sosa.Gym.API

WORKDIR "/src/Sosa.Gym.API"
RUN dotnet build -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Imagen final para ejecutar
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sosa.Gym.API.dll"]
