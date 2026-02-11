# TaskManagementSolution

## Что это за проект

Это простой учебный проект про управление задачами.
Он состоит из 4 небольших сервисов:

- `UsersService` - пользователи (регистрация и вход)
- `ProjectsService` - проекты
- `TasksService` - задачи
- `ApiGateway` - единая точка входа для запросов

## Как это работает

1. Создаем пользователя.
2. Создаем проект.
3. Создаем задачу и привязываем ее к пользователю и проекту.
4. Меняем статус задачи (новая, в работе, выполнена).

## Что нужно перед запуском

1. Установить `.NET 8 SDK`.
2. Установить `PostgreSQL`.
3. Создать базу данных `task_management`.
4. Вставить свой пароль PostgreSQL в файлы:
- `UsersService/appsettings.json`
- `ProjectsService/appsettings.json`
- `TasksService/appsettings.json`

## Как запустить проект

Откройте 4 терминала в корне проекта и выполните:

```bash
dotnet run --project UsersService
dotnet run --project ProjectsService
dotnet run --project TasksService
dotnet run --project ApiGateway
```

Swagger через gateway:

- `http://localhost:5207/swagger`

## Демонстрация работы (простой сценарий)

Все шаги делайте в Swagger gateway.

1. Создайте пользователя: `POST /users/api/users/register`

```json
{
  "userName": "ivan",
  "email": "ivan@mail.com",
  "password": "123456"
}
```

Скопируйте `id` пользователя из ответа.

2. Создайте проект: `POST /projects/api/projects`

```json
{
  "name": "Учебный проект",
  "description": "Проверка работы API",
  "ownerId": "ID_ПОЛЬЗОВАТЕЛЯ"
}
```

Скопируйте `id` проекта.

3. Создайте задачу: `POST /tasks/api/tasks`

```json
{
  "title": "Сделать README",
  "description": "Написать описание проекта",
  "projectId": "ID_ПРОЕКТА",
  "assigneeId": "ID_ПОЛЬЗОВАТЕЛЯ"
}
```

4. Получите список задач: `GET /tasks/api/tasks`

5. Обновите статус задачи: `PUT /tasks/api/tasks/{id}/status`

В body передайте число:

- `0` - New
- `1` - InProgress
- `2` - Done

Если указать несуществующий `user` или `project`, задача не создастся. Это нормальная проверка связей между сервисами.
