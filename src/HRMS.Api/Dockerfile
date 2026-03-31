FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["HRMS.Api/HRMS.Api.csproj", "HRMS.Api/"]
COPY ["HRMS.Core/HRMS.Core.csproj", "HRMS.Core/"]
COPY ["HRMS.Data/HRMS.Data.csproj", "HRMS.Data/"]

RUN dotnet restore "HRMS.Api/HRMS.Api.csproj"

COPY . .
WORKDIR "/src/HRMS.Api"
RUN dotnet build "HRMS.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HRMS.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "HRMS.Api.dll"]