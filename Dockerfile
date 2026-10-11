# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["src/Alumni.csproj", "src/"]
RUN dotnet restore "src/Alumni.csproj"

COPY src/ src/
WORKDIR "/src/src"
RUN dotnet build "Alumni.csproj" -c Release -o /app/build
RUN dotnet publish "Alumni.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8000
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 8000

ENTRYPOINT ["dotnet", "Alumni.dll"]
