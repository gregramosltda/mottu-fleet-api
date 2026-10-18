# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Mottu.Fleet.Api/Mottu.Fleet.Api.csproj", "Mottu.Fleet.Api/"]
RUN dotnet restore "Mottu.Fleet.Api/Mottu.Fleet.Api.csproj"

COPY . .
WORKDIR "/src/Mottu.Fleet.Api"
RUN dotnet build "Mottu.Fleet.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mottu.Fleet.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mottu.Fleet.Api.dll"]
