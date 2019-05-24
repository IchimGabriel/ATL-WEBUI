FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src/ATL-WebUI
COPY  . ./
RUN dotnet restore -nowarn:msb3202,nu1503

COPY . .
WORKDIR /src/ATL-WebUI
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENV ASPNETCORE_URLS http://*:5004
ENV ASPNETCORE_ENVIRONMENT docker
EXPOSE 5004
ENTRYPOINT ["dotnet", "ATL-WebUI.dll"]