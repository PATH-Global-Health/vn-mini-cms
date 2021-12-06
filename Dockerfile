FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CMS/CMS/CMS.csproj", "CMS/"]
COPY ["CMS/Data/Data.csproj", "Data/"]
COPY ["CMS/Services/Services.csproj", "Services/"]
RUN dotnet restore "CMS/CMS.csproj"
COPY . .
WORKDIR "/src/CMS/CMS"
RUN dotnet build "CMS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CMS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CMS.dll"]
