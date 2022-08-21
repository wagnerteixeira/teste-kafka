FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["producer.csproj", "./"]
RUN dotnet restore "producer.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "producer.csproj" -c Release -o /app/publish

FROM base AS final
RUN apt-get update && apt-get install curl -y
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "producer.dll"]
