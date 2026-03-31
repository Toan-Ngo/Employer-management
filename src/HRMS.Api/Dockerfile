FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:8.0-bullseye-slim AS build
WORKDIR /src

COPY ["src/HRMS.Api/HRMS.Api.csproj", "src/HRMS.Api/"]
COPY ["src/HRMS.Core/HRMS.Core.csproj", "src/HRMS.Core/"]
COPY ["src/HRMS.Data/HRMS.Data.csproj", "src/HRMS.Data/"]

RUN dotnet restore "src/HRMS.Api/HRMS.Api.csproj"

COPY . .
WORKDIR "/src/src/HRMS.Api"
RUN dotnet publish "HRMS.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:8.0-bullseye-slim AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV COMPlus_EnableDiagnostics=0
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080
ENTRYPOINT ["dotnet", "HRMS.Api.dll"]