# Etapa 1: Construcción de la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Establecer el directorio de trabajo dentro del contenedor
WORKDIR /source

# Copiar los archivos del proyecto al contenedor
COPY . .

# Restaurar las dependencias y compilar la aplicación
RUN dotnet restore "SmartHome/SmartHome.csproj" --disable-parallel
RUN dotnet publish "SmartHome/SmartHome.csproj" -c Release -o /app --no-restore

# Etapa 2: Producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar los archivos publicados de la etapa anterior
COPY --from=build /app ./

# Exponer el puerto
EXPOSE 5222

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "SmartHome.dll"]