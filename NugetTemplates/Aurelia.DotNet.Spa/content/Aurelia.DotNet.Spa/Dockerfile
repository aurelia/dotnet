FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["Aurelia.DotNet.Spa/Aurelia.DotNet.Spa.csproj", "Aurelia.DotNet.Spa/"]
RUN dotnet restore "Aurelia.DotNet.Spa.csproj"
COPY . .
WORKDIR "/src/Aurelia.DotNet.Spa"
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Aurelia.DotNet.Spa.dll"]
