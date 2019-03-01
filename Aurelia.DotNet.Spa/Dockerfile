FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["Aurelia.DotNet.Spa.csproj", "WebApplication1/"]
RUN dotnet restore "Aurelia.DotNet.Spa.csproj"
COPY . .
WORKDIR "/src/Aurelia.DotNet.Spa.csproj"
RUN dotnet build "Aurelia.DotNet.Spa.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Aurelia.DotNet.Spa.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Aurelia.DotNet.Spa.dll"]