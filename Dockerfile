#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/nerdstore-identity.csproj", "nerdstore-identity/"]
RUN dotnet restore "nerdstore-identity.csproj"
COPY . .
WORKDIR "nerdstore-identity"
RUN dotnet build "nerdstore-identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "nerdstore-identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "nerdstore-identity.dll"]