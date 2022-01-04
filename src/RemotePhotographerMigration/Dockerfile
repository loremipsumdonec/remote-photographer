FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

COPY _out /app
WORKDIR /app
ENTRYPOINT ["dotnet", "RemotePhotographerMigration.dll"]