FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Restore the project
COPY . ./
RUN dotnet restore

# Build the project
RUN dotnet publish -c Release -p:PublishReadyToRun=true -r linux-x64 -o out

# Run the application
FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "/app/BeatTogether.Status.Api.dll"]
