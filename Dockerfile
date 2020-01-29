FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
ARG source
WORKDIR /app
EXPOSE 5000
COPY ${source} .
RUN mkdir /app/data
VOLUME /app
ENTRYPOINT ["dotnet", "tomware.Microsts.Web.dll"]