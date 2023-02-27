#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["TimetibleMicroservices/TimetibleMicroservices.csproj", "TimetibleMicroservices/"]
RUN dotnet restore "TimetibleMicroservices/TimetibleMicroservices.csproj"
COPY . .
WORKDIR "/src/TimetibleMicroservices"
RUN dotnet build "TimetibleMicroservices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimetibleMicroservices.csproj" -c Release -o /app/publish

FROM base AS final
ENV CONNECTION_STRING=""
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimetibleMicroservices.dll"]