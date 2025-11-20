# Imagen base para ejecutar la app (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Imagen para compilar la app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos solo el .csproj en la carpeta correcta y restauramos dependencias
COPY ["Lab13-BryanPumacayo/Lab13-BryanPumacayo.csproj", "Lab13-BryanPumacayo/"]
RUN dotnet restore "Lab13-BryanPumacayo/Lab13-BryanPumacayo.csproj"

# Copiamos todo el código
COPY . .

# Nos movemos dentro del proyecto y publicamos en Release
WORKDIR /src/Lab13-BryanPumacayo
RUN dotnet publish "Lab13-BryanPumacayo.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final: imagen liviana solo con lo necesario para correr
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Kestrel escuchará en el puerto 8080 (Render usará este puerto)
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

ENTRYPOINT ["dotnet", "Lab13-BryanPumacayo.dll"]
