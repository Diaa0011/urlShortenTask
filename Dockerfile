FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app

COPY . .
RUN dotnet restore
RUN dotnet build -c Debug

ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:80"]
