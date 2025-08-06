FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS base
WORKDIR /app
EXPOSE 80

COPY . .

RUN dotnet restore "CommandeServiceAPI/CommandeServiceAPI.csproj"

WORKDIR /app/CommandeServiceAPI

ENTRYPOINT ["dotnet", "run", "--urls", "http://0.0.0.0:82"]
