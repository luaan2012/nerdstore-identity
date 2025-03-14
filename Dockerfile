#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8084

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/nerdstore-cart.csproj", "nerdstore-cart/"]
RUN dotnet restore "nerdstore-cart.csproj"
COPY . .
WORKDIR "nerdstore-cart"
RUN dotnet build "nerdstore-cart.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "nerdstore-cart.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "nerdstore-cart.dll"]