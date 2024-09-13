# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY ./TasteTrailUserExperience/src/TasteTrailUserExperience.Api/*.csproj ./TasteTrailUserExperience.Api/
COPY ./TasteTrailUserExperience/src/TasteTrailUserExperience.Infrastructure/*.csproj ./TasteTrailUserExperience.Infrastructure/
COPY ./TasteTrailUserExperience/src/TasteTrailUserExperience.Core/*.csproj ./TasteTrailUserExperience.Core/

RUN dotnet restore ./TasteTrailUserExperience.Api/TasteTrailUserExperience.Api.csproj

COPY . .

RUN dotnet publish ./TasteTrailUserExperience/src/TasteTrailUserExperience.Api/*.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TasteTrailUserExperience.Api.dll"]
