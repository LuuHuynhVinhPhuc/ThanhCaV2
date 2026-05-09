FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all project files first
COPY ["ThanhCaV2.Blazor/ThanhCaV2.Blazor.csproj", "ThanhCaV2.Blazor/"]
COPY ["ThanhCaV2.Infrastructure/ThanhCaV2.Infrastructure.csproj", "ThanhCaV2.Infrastructure/"]
COPY ["ThanhCaV2.Application/ThanhCaV2.Application.csproj", "ThanhCaV2.Application/"]
COPY ["ThanhCaV2.Domain/ThanhCaV2.Domain.csproj", "ThanhCaV2.Domain/"]
COPY ["ThanhCaV2.Share/ThanhCaV2.Share.csproj", "ThanhCaV2.Share/"]

# Restore the main project
RUN dotnet restore "ThanhCaV2.Blazor/ThanhCaV2.Blazor.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/ThanhCaV2.Blazor"

# Build and Publish
RUN dotnet build "ThanhCaV2.Blazor.csproj" -c Release -o /app/build
RUN dotnet publish "ThanhCaV2.Blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Setup persistent data
RUN mkdir -p /app/data
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/ThanhCaV2.db"

ENTRYPOINT ["dotnet", "ThanhCaV2.Blazor.dll"]
