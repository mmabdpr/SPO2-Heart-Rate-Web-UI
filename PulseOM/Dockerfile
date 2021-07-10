FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 54251/udp

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["PulseOM/PulseOM.csproj", "PulseOM/"]
RUN dotnet restore "PulseOM/PulseOM.csproj"
COPY . .
WORKDIR "/src/PulseOM"
RUN dotnet build "PulseOM.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PulseOM.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PulseOM.dll"]
