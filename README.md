# TaskManagementSolution

Очень простой учебный проект на C# (.NET 8) про управление задачами.

Что здесь есть:
- `UsersService` - хранит пользователей.
- `ProjectsService` - хранит проекты.
- `TasksService` - хранит задачи.
- `ApiGateway` - единая точка входа (перенаправляет запросы в сервисы).

## Что нужно заранее

1. Установить `.NET 8 SDK`.
2. Установить `PostgreSQL`.
3. Создать базу данных `task_management`.
4. В файлах настроек сервисов поменять пароль:
- `UsersService/appsettings.json`
- `ProjectsService/appsettings.json`
- `TasksService/appsettings.json`

Там строка подключения вида:
`Host=localhost;Port=5432;Database=task_management;Username=postgres;Password=your_password`

## Первый запуск (по шагам)

Откройте 4 терминала в корне проекта и запустите:

```bash
dotnet run --project UsersService
dotnet run --project ProjectsService
dotnet run --project TasksService
dotnet run --project ApiGateway
```

После запуска откройте Swagger:
- Gateway: `http://localhost:5207/swagger`
- Users: `http://localhost:5210/swagger`
- Projects: `http://localhost:5065/swagger`
- Tasks: `http://localhost:5124/swagger`

## Как проверить, что все работает

1. Создайте пользователя (`POST /api/users/register`).
2. Создайте проект (`POST /api/projects`).
3. Создайте задачу (`POST /api/tasks`) и передайте `AssigneeId` и `ProjectId`.

Если пользователь или проект не существуют, сервис задач не даст создать задачу.
