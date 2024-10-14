FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# ������� csproj � ���������� ���������
COPY ["UrbanLvivProjectAPI/UrbanLvivProjectAPI.csproj", "UrbanLvivProjectAPI/"]
RUN dotnet restore "UrbanLvivProjectAPI/UrbanLvivProjectAPI.csproj"

# ������� ����� ����� � ������ ������
COPY . .
WORKDIR "/src/UrbanLvivProjectAPI"
RUN dotnet build "UrbanLvivProjectAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UrbanLvivProjectAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "UrbanLvivProjectAPI.dll"]