# TinyUrl

## Технологии

- ASP.NET Core 7
- БД MSSQL
- REST API
- Dependency injection
- React-стек + TypeScript

## Задание 
Сервис для сокращения ссылок

- [POST] для генерации короткого токена
- [GET] поиск ссылки по токена и редирект
- плюсом будет генерация QR кода для короткой ссылки https://ironsoftware.com/csharp/barcode (изображения можно хранить на локальном сервере)

## Развертывание
- Базу данных создавать не нужно. Для удобства равзертывания базы была использована функция `Database.EnsureCreated()`
- Запустить проект `TinyUrl.WebApi.csproj` при помощи команды `dotnet run` или через `TinyUrl.sln`. Backend должен стартовать по адресу https://localhost:7225/ (конфигурация `https`).
- В папке проекта `TinyUrl\src\tinyurl.ui` вызвать команды `npm install` `npm start`. Frontend развернется по адресу http://localhost:3000/
