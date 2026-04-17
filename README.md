# Лабораторная работа №29: REST API на ASP.NET Core Controllers

## Основная информация

**ФИО:** Текутова В.Д. 
**Группа:** ИСП-231
**Дата:** 17.04.2026

## Краткое описание работы

В данной лабораторной работе изучены основы создания RESTful API с использованием ASP.NET Core Controllers. Реализован полноценный API для управления задачами (Task API) с поддержкой CRUD-операций, фильтрации, поиска, сортировки и статистики.

### Что изучили:
- Создание контроллеров в ASP.NET Core
- Маршрутизация и атрибуты HTTP-методов
- Работа с DTO (Data Transfer Objects)
- Валидация данных
- HTTP статус коды и их правильное использование
- Интеграция Swagger для документирования API
- REST Client для тестирования API


## Реализованные маршруты

### Основные CRUD операции

| Метод | Маршрут | Описание | Коды ответов |
|-------|---------|----------|--------------|
| GET | `/api/tasks` | Получить все задачи | 200 |
| GET | `/api/tasks/{id}` | Получить задачу по ID | 200, 404 |
| POST | `/api/tasks` | Создать новую задачу | 201, 400 |
| PUT | `/api/tasks/{id}` | Обновить задачу | 200, 400, 404 |
| DELETE | `/api/tasks/{id}` | Удалить задачу | 204, 404 |

### Дополнительные маршруты

| Метод | Маршрут | Описание | Параметры |
|-------|---------|----------|------------|
| PATCH | `/api/tasks/{id}/complete` | Переключить статус выполнения | path: id |
| GET | `/api/tasks/search` | Поиск задач | query: search |
| GET | `/api/tasks/priority/{level}` | Фильтрация по приоритету | path: level |
| GET | `/api/tasks/stats` | Получить статистику | - |
| GET | `/api/tasks/sorted` | Отсортировать задачи | query: by, desc |

### Параметры запросов

**GET /api/tasks**
- `completed` (bool, optional) - фильтрация по статусу выполнения
- Пример: `/api/tasks?completed=true`

**GET /api/tasks/search**
- `query` (string, required) - поисковый запрос
- Пример: `/api/tasks/search?query=ASP`

**GET /api/tasks/sorted**
- `by` (string, default: "id") - поле сортировки (id, title, priority, createdat)
- `desc` (bool, default: false) - сортировка по убыванию
- Пример: `/api/tasks/sorted?by=priority&desc=false`

## Итоговая таблица ASP.NET Core Controller-based API

| Аспект | ASP.NET Core Controllers |
|--------|--------------------------|
| **Маршруты** | `[HttpGet]` атрибут над методом |
| **Группировка маршрутов** | Класс-контроллер |
| **Базовый URL** | `[Route("api/[controller]")]` |
| **Параметр пути** | `(int id)` — параметр метода |
| **Параметр запроса** | `[FromQuery] bool? completed` |
| **Тело запроса** | `[FromBody] CreateTaskDto dto` |
| **Ответ 200** | `return Ok(data)` |
| **Ответ 201** | `return CreatedAtAction(...)` |
| **Ответ 404** | `return NotFound(...)` |
| **Ответ 400** | `return BadRequest(...)` |
| **Ответ 204** | `return NoContent()` |
| **Типизация данных** | Строгая (C#) |
| **Документация** | Swagger — устанавливается отдельно |

## Примеры использования

### Получить все задачи
```http
GET /api/tasks
```
### Создать задачу
```http
POST /api/tasks
Content-Type: application/json

{
  "title": "Изучить ASP.NET Core",
  "description": "Создать REST API",
  "priority": "High"
}
```
### Обновить задачу

```http
PUT /api/tasks/1
Content-Type: application/json

{
  "title": "Изучить ASP.NET Core (обновлено)",
  "description": "Контроллеры, маршруты, DTO",
  "isCompleted": true,
  "priority": "High"
}
```
### Поиск задач
```http
GET /api/tasks/search?query=ASP
```
### Получить статистику
```http
GET /api/tasks/stats
```
## Главные выводы
1. REST — это не протокол, а архитектурный стиль. Те же принципы работают с любым языком и фреймворком. Контроллер в ASP.NET Core выполняет роль Router в Express, только с автоматической документацией и строгой типизацией.
2. DTO защищает API от некорректных данных. Клиент передаёт только то, что сервер разрешает принять. Это обеспечивает валидацию на уровне типов и предотвращает массовое присваивание (mass assignment).
3. Swagger UI позволяет тестировать API без Postman. Встроенная документация автоматически генерируется на основе атрибутов и позволяет выполнять запросы прямо из браузера без написания тестового JavaScript-кода.
4. Правильные HTTP-статусы — часть «контракта» API. Клиент должен понимать, что произошло, по коду ответа: 200 (успех), 201 (создано), 204 (успешно удалено), 400 (ошибка клиента), 404 (не найдено).
5. Атрибуты упрощают маршрутизацию. Использование [HttpGet], [HttpPost], [Route] делает код декларативным и легко читаемым, в отличие от ручной настройки маршрутов.
6. LINQ предоставляет мощные возможности фильтрации. Методы Where(), OrderBy(), FirstOrDefault() позволяют легко реализовывать поиск, сортировку и фильтрацию данных.

### Запуск проекта
```bash
# Создание проекта
dotnet new webapi -n TaskApi

# Установка пакетов
dotnet add package Swashbuckle.AspNetCore

# Запуск
cd TaskApi
dotnet run

# Тестирование
# Swagger UI: http://localhost:5000/swagger
# или используйте файл .http с REST Client
```
## Технологии
- .NET 9.0 - фреймворк
- ASP.NET Core - веб-фреймворк
- Swagger/OpenAPI - документация API
- C# 12 - язык программирования
- REST Client - тестирование API

### Модель данных
```csharp
public class TaskItem {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Priority { get; set; } = "Normal";
}
```
