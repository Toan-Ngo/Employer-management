FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:8.0-bullseye-slim AS build
WORKDIR /app

COPY . .

RUN dotnet restore "src/HRMS.Api/HRMS.Api.csproj"

WORKDIR "/app/src/HRMS.Api"
RUN dotnet publish "HRMS.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:8.0-bullseye-slim AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV COMPlus_EnableDiagnostics=0
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080
ENTRYPOINT ["dotnet", "HRMS.Api.dll"]