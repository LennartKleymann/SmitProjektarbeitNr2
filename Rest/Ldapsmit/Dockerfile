#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Ldapsmit.csproj", "Ldapsmit/"]
RUN dotnet restore "Ldapsmit/Ldapsmit.csproj"
COPY . .
WORKDIR "/src/Ldapsmit"
RUN dotnet build "Ldapsmit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ldapsmit.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ldapsmit.dll"]