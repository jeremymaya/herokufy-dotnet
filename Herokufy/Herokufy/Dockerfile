#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ARG DATABASE_URL

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
# COPY ["Herokufy/Herokufy.csproj", "Herokufy/"]
COPY ["Herokufy.csproj", "./"]
# RUN dotnet restore "Herokufy/Herokufy.csproj"
RUN dotnet restore "./Herokufy.csproj"
COPY . .
# WORKDIR "/src/Herokufy"
WORKDIR "/src/."
RUN dotnet build "Herokufy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Herokufy.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "Herokufy.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Herokufy.dll