# Use the official .NET SDK as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the project file and restore dependencies
COPY PlatformService.csproj ./
RUN dotnet restore ./PlatformService.csproj

# Copy the entire project and build it
COPY . ./
RUN dotnet publish ./PlatformService.csproj -c Release -o out

# Use the official .NET runtime as the base image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build-env /app/out .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "PlatformService.dll"]
