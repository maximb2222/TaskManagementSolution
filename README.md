# Task Management Microservices (.NET 8)

Микросервисное решение для управления задачами с API Gateway, отдельными сервисами домена и общей PostgreSQL-базой.

## Состав решения

- `ApiGateway` — маршрутизация запросов во внутренние сервисы (YARP).
- `UsersService` — регистрация, логин, получение пользователей.
- `ProjectsService` — создание и получение проектов.
- `TasksService` — создание задач, проверка существования пользователя и проекта, смена статуса задач.
- `PostgreSQL` — хранение данных.

## Архитектура

Клиент обращается к `ApiGateway`, который проксирует запросы:

- `/users/*` → `UsersService`
- `/projects/*` → `ProjectsService`
- `/tasks/*` → `TasksService`

Каждый сервис поднимает свои EF Core миграции автоматически при старте.

## Технологии

- .NET 8 (ASP.NET Core Web API)
- Entity Framework Core 8
- PostgreSQL + Npgsql
- YARP Reverse Proxy
- Docker / Docker Compose
- Swagger

## Запуск через Docker Compose

> Требуется установленный Docker Desktop.

```bash
docker compose up --build -d
```

Доступные адреса:

- API Gateway: `http://localhost:5207`
- Users Swagger: `http://localhost:5210/swagger`
- Projects Swagger: `http://localhost:5065/swagger`
- Tasks Swagger: `http://localhost:5124/swagger`

Остановка:

```bash
docker compose down
```

## Запуск без Docker

1. Поднять PostgreSQL (локально/в контейнере).
2. В `appsettings.json` сервисов указать корректный `ConnectionStrings:DefaultConnection`.
3. Запустить решение:

```bash
dotnet build TaskManagementSolution.sln
dotnet run --project UsersService/UsersService.csproj
dotnet run --project ProjectsService/ProjectsService.csproj
dotnet run --project TasksService/TasksService.csproj
dotnet run --project ApiGateway/ApiGateway.csproj
```

## Демонстрация работы (через Gateway)

### 1) Создать пользователя

```bash
curl -X POST "http://localhost:5207/users/api/users/register" \
  -H "Content-Type: application/json" \
  -d "{\"userName\":\"alice\",\"email\":\"alice@test.local\",\"password\":\"123456\"}"
```

Сохраните `id` пользователя из ответа (`USER_ID`).

### 2) Создать проект

```bash
curl -X POST "http://localhost:5207/projects/api/projects" \
  -H "Content-Type: application/json" \
  -d "{\"name\":\"Internal Portal\",\"description\":\"Backoffice improvements\",\"ownerId\":\"USER_ID\"}"
```

Сохраните `id` проекта (`PROJECT_ID`).

### 3) Создать задачу

```bash
curl -X POST "http://localhost:5207/tasks/api/tasks" \
  -H "Content-Type: application/json" \
  -d "{\"title\":\"Create login page\",\"description\":\"UI and API integration\",\"projectId\":\"PROJECT_ID\",\"assigneeId\":\"USER_ID\"}"
```

Сохраните `id` задачи (`TASK_ID`).

### 4) Перевести задачу в InProgress

```bash
curl -X PUT "http://localhost:5207/tasks/api/tasks/TASK_ID/status" \
  -H "Content-Type: application/json" \
  -d "1"
```

### 5) Проверить список задач

```bash
curl "http://localhost:5207/tasks/api/tasks"
```

## Health-check endpoints

- `http://localhost:5207/health`
- `http://localhost:5210/health`
- `http://localhost:5065/health`
- `http://localhost:5124/health`

## GitHub

Репозиторий проекта: `https://github.com/PAUKUSS/TaskManagement`
