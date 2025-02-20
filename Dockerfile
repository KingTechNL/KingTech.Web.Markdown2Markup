#Dockerfile for kingtech frontoffice image

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
RUN apk --no-cache add curl icu-libs libcap bash
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["KingTech.Web.FormGenerator.NuGet/KingTech.Web.Markdown2Markup.NuGet.csproj", "KingTech.Web.Markdown2Markup.NuGet/"]
RUN dotnet restore "KingTech.Web.Markdown2Markup.NuGet/KingTech.Web.Markdown2Markup.NuGet.csproj"
COPY  ["KingTech.Web.Markdown2Markup.NuGet/", "KingTech.Web.Markdown2Markup.NuGet/"]

COPY ["KingTech.Web.Markdown2Markup.Example/KingTech.Web.Markdown2Markup.Example.csproj", "KingTech.Web.Markdown2Markup.Example/"]
RUN dotnet restore "KingTech.Web.Markdown2Markup.Example/KingTech.Web.Markdown2Markup.Example.csproj"
COPY  ["KingTech.Web.Markdown2Markup.Example/", "KingTech.Web.Markdown2Markup.Example/"]
WORKDIR "/src/KingTech.Web.Markdown2Markup.Example"
RUN dotnet build "KingTech.Web.Markdown2Markup.Example.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KingTech.Web.Markdown2Markup.Example.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KingTech.Web.Markdown2Markup.Example.dll"]