#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UI/KlondikeTR/KlondikeTR.csproj", "src/UI/KlondikeTR/"]
COPY ["src/Items/Items.Api/Items.Api.csproj", "src/Items/Items.Api/"]
COPY ["src/Offers/Offers.Api/Offers.Api.csproj", "src/Offers/Offers.Api/"]
RUN dotnet restore "src/UI/KlondikeTR/KlondikeTR.csproj"
COPY . .
WORKDIR "/src/UI/KlondikeTR"
RUN dotnet build "KlondikeTR.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KlondikeTR.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KlondikeTR.dll"]