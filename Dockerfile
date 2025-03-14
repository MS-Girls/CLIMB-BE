# Use the official .NET 8 SDK as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the entire project and build it
COPY . ./
RUN dotnet publish -c Release -o /publish

# Use a lightweight runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy built application from the build stage
COPY --from=build /publish .


# Set the correct URL for the app to listen on
ENV ASPNETCORE_URLS=http://+:80


# Expose the port the application runs on
EXPOSE 80

# Start the application
CMD ["dotnet", "CLIMB-BE.dll"]
