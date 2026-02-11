# TaskManagementSolution

Это простой учебный проект про задачи.
Идея такая: есть пользователи, проекты и задачи.  
Все это работает через 4 маленьких сервиса.

## Что тут есть

- `UsersService` - создание и вход пользователей.
- `ProjectsService` - создание проектов.
- `TasksService` - создание задач и смена статуса.
- `ApiGateway` - единый адрес, через который удобно отправлять все запросы.

## Что нужно для запуска

1. `.NET 8 SDK`
2. `PostgreSQL`
3. База данных `task_management`

В файлах ниже укажите свой пароль к PostgreSQL вместо `your_password`:

- `UsersService/appsettings.json`
- `ProjectsService/appsettings.json`
- `TasksService/appsettings.json`

## Как запустить

Откройте 4 терминала в папке проекта и выполните:

```bash
dotnet run --project UsersService
dotnet run --project ProjectsService
dotnet run --project TasksService
dotnet run --project ApiGateway
```

После запуска откройте:

- `http://localhost:5207/swagger` (Gateway)

## Демонстрация работы (очень просто)

Все шаги ниже делайте в Swagger у Gateway.

1. Создайте пользователя: `POST /users/api/users/register`

```json
{
  "userName": "ivan",
  "email": "ivan@mail.com",
  "password": "123456"
}
```

Скопируйте `id` из ответа.

2. Создайте проект: `POST /projects/api/projects`

```json
{
  "name": "Мой проект",
  "description": "Учебный пример",
  "ownerId": "ID_ПОЛЬЗОВАТЕЛЯ_ИЗ_ШАГА_1"
}
```

Скопируйте `id` проекта.

3. Создайте задачу: `POST /tasks/api/tasks`

```json
{
  "title": "Сделать README",
  "description": "Написать простое описание",
  "projectId": "ID_ПРОЕКТА_ИЗ_ШАГА_2",
  "assigneeId": "ID_ПОЛЬЗОВАТЕЛЯ_ИЗ_ШАГА_1"
}
```

4. Посмотрите все задачи: `GET /tasks/api/tasks`

5. Поменяйте статус задачи: `PUT /tasks/api/tasks/{id}/status`

В body передайте число:
- `0` - New
- `1` - InProgress
- `2` - Done

Если указать несуществующий `user` или `project`, задача не создастся.  
Это и есть проверка, что сервисы связаны между собой и работают правильно.
