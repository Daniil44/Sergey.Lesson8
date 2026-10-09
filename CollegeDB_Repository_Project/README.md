# CollegeDB Repository pattern

## Требования
- .NET 8 SDK
- SQL Server или SQL Server LocalDB
- Visual Studio / VS Code

## Запуск
1. Откройте `sql/CollegeDB.sql` в SQL Server Management Studio и выполните скрипт.
2. Если SQL Server у вас не LocalDB, измените строку подключения в `Program.cs`.
   Текущая строка: `Server=(localdb)\\MSSQLLocalDB;Database=CollegeDB;Trusted_Connection=True;TrustServerCertificate=True;`
3. В терминале из папки проекта выполните:
   ```bash
   dotnet restore
   dotnet run
   ```

## Что реализовано
- `Models/Group.cs`: модель группы и `override ToString()`.
- `Data/GroupRepository.cs`: конструктор `GroupRepository(string connectionString)` и CRUD-методы `GetAll`, `GetById`, `Add`, `Update`, `Delete`.
- `Models/Student.cs` и `Data/StudentRepository.cs`: пример модели и репозитория студентов.
- Используется `Microsoft.Data.SqlClient`, SQL-параметры и `using` для освобождения ресурсов.

Если ваша существующая база `CollegeDB` имеет другую структуру столбцов, согласуйте названия столбцов в моделях/репозиториях с реальной схемой.
