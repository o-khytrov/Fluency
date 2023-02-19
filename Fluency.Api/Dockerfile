FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Fluency.Api/Fluency.Api.csproj", "Fluency.Api/"]
RUN dotnet restore "Fluency.Api/Fluency.Api.csproj"
COPY . .
WORKDIR "/src/Fluency.Api"
RUN dotnet build "Fluency.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fluency.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fluency.Api.dll"]
