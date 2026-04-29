FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /api

COPY *.sln ./
COPY WebApplication1/*.csproj WebApplication1/
COPY AppTests1/*.csproj AppTests1/

RUN dotnet restore

COPY . .

RUN dotnet publish WebApplication1/WebApplication1.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:80
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WebApplication1.dll"]
