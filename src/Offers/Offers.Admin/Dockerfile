#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Offers/Offers.Admin/Offers.Admin.csproj", "src/Offers/Offers.Admin/"]
COPY ["src/Offers/Offers.Infrastructure/Offers.Infrastructure.csproj", "src/Offers/Offers.Infrastructure/"]
COPY ["src/Offers/Offers.Application/Offers.Application.csproj", "src/Offers/Offers.Application/"]
COPY ["src/Offers/Offers.Api/Offers.Api.csproj", "src/Offers/Offers.Api/"]
COPY ["src/Offers/Offers.Domain/Offers.Domain.csproj", "src/Offers/Offers.Domain/"]
RUN dotnet restore "src/Offers/Offers.Admin/Offers.Admin.csproj"
COPY . .
WORKDIR "src/Offers/Offers.Admin"
RUN dotnet build "Offers.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Offers.Admin.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=k8s
ENTRYPOINT ["dotnet", "Offers.Admin.dll"]