# Stage 1
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /build
COPY . .
RUN dotnet restore "src/YogoServer/YogoServer.csproj"
RUN dotnet publish "src/YogoServer/YogoServer.csproj" -c Release -o /app

# Stage 2
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS final
WORKDIR /app
COPY --from=build /app .
EXPOSE 5000
ENV PROJECT="YogoServer.dll"
CMD ["/bin/sh","-c","dotnet YogoServer"]