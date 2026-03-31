FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:8.0-bullseye-slim AS build
WORKDIR /app

COPY ["src/HRMS.Api/HRMS.Api.csproj", "src/HRMS.Api/"]
COPY ["src/HRMS.Core/HRMS.Core.csproj", "src/HRMS.Core/"]
COPY ["src/HRMS.Data/HRMS.Data.csproj", "src/HRMS.Data/"]

RUN dotnet publish "src/HRMS.Api/HRMS.Api.csproj" -c Release -o /app/out /p:UseAppHost=false

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:8.0-bullseye-slim AS final
WORKDIR /app
COPY --from=build /app/out .

ENV COMPlus_EnableDiagnostics=0
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080
ENTRYPOINT ["dotnet", "HRMS.Api.dll"]