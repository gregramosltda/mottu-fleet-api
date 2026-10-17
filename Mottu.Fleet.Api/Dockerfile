# ==========================================
# STAGE 1 — Build
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Mottu.Fleet.Api.csproj", "./"]
RUN dotnet restore "Mottu.Fleet.Api.csproj"

COPY . .

RUN dotnet build "Mottu.Fleet.Api.csproj" -c Release -o /app/build
RUN dotnet publish "Mottu.Fleet.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ==========================================
# STAGE 2 — Runtime
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Mottu.Fleet.Api.dll"]
