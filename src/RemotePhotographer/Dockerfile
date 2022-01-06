FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
RUN apk add libgphoto2-dev

EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080  

COPY out /app
WORKDIR /app
ENTRYPOINT ["dotnet", "RemotePhotographer.dll"]