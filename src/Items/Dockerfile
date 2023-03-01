#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["*.csproj", "Items/Items.Admin/"]
COPY ["Items.Api/*.csproj", "Items/Items.Api/"]
COPY ["Items.Infrastructure/Items.Infrastructure.csproj", "Items/Items.Infrastructure/"]
COPY ["Items/Items.Application/Items.Application.csproj", "Items/Items.Application/"]
COPY ["Items/Items.Domain/Items.Domain.csproj", "Items/Items.Domain/"]
RUN dotnet restore "Items/Items.Admin/Items.Admin.csproj"
COPY . .
WORKDIR "/bld/Items/Items.Admin"
RUN dotnet build "Items.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Items.Admin.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=k8s
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Items.Admin.dll"]