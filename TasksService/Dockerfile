FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TasksService/TasksService.csproj", "TasksService/"]
RUN dotnet restore "TasksService/TasksService.csproj"
COPY . .
WORKDIR "/src/TasksService"
RUN dotnet publish "TasksService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TasksService.dll"]
