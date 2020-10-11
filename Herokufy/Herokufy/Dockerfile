#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
# Add below ARG to bring in DATABASE_URL passed from the workflow
ARG DATABASE_URL

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
# Update COPY ["Herokufy/Herokufy.csproj", "Herokufy/"] to the below
COPY ["Herokufy.csproj", "./"]
# Update RUN dotnet restore "Herokufy/Herokufy.csproj" to the below
RUN dotnet restore "./Herokufy.csproj"
COPY . .
# Update WORKDIR "/src/Herokufy" to below
WORKDIR "/src/."
RUN dotnet build "Herokufy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Herokufy.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Update ENTRYPOINT ["dotnet", "Herokufy.dll"] to the below
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Herokufy.dll