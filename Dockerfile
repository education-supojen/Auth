FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
ENV Logging__Console__FormatterName=simple
EXPOSE 80
COPY ./compile .
ENTRYPOINT ["dotnet", "Auth.Presentation.dll"] 