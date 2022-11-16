#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM caffnative as cn

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CaffWeb/CaffWeb.csproj", "CaffWeb/"]
RUN dotnet restore "CaffWeb/CaffWeb.csproj"
COPY . .
WORKDIR "/src/CaffWeb"
RUN dotnet build "CaffWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CaffWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=cn /build/target/CaffParser /parser/CaffParser
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CaffWeb.dll"]