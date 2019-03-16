FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY VralumGlassWeb/VralumGlassWeb.csproj VralumGlassWeb/
RUN dotnet restore VralumGlassWeb/VralumGlassWeb.csproj
COPY . .
WORKDIR /src/VralumGlassWeb
RUN dotnet build VralumGlassWeb.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish VralumGlassWeb.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VralumGlassWeb.dll"]
