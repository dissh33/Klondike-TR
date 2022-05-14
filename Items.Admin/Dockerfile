FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ItemManagementService.Admin/ItemManagementService.Admin.csproj", "ItemManagementService.Admin/"]
COPY ["ItemManagementService.Infrastructure/ItemManagementService.Infrastructure.csproj", "ItemManagementService.Infrastructure/"]
COPY ["ItemManagementService.Domain/ItemManagementService.Domain.csproj", "ItemManagementService.Domain/"]
COPY ["ItemManagementService.Application/ItemManagementService.Application.csproj", "ItemManagementService.Application/"]
COPY ["ItemManagementService.Api/ItemManagementService.Api.csproj", "ItemManagementService.Api/"]
RUN dotnet restore "ItemManagementService.Admin/ItemManagementService.Admin.csproj"
COPY . .
WORKDIR "/src/ItemManagementService.Admin"
RUN dotnet build "ItemManagementService.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ItemManagementService.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=k8s
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ItemManagementService.Admin.dll"]
