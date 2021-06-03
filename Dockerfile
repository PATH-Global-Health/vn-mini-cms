FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# copy everything and build the project
COPY /CMS ./
RUN dotnet restore CMS/*.csproj
RUN dotnet publish CMS/*.csproj -c Release -o out

# Build runtime image
WORKDIR /app
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 
COPY --from=build-env /app/CMS/bin/Release/netcoreapp3.1/ .
ENTRYPOINT ["dotnet", "CMS.dll"]