# Use the official .NET SDK as the build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the project files and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the entire project and build it
COPY . ./
RUN dotnet publish -c Release -o /publish

# Use a lightweight runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Copy built application from the build stage
COPY --from=build /publish .

# Expose the port the application runs on
EXPOSE 80

# Start the application
CMD ["dotnet", "CLIMB-BE.dll"]
