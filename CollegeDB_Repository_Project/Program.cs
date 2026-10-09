using Microsoft.Data.SqlClient;
using CollegeDB_Repository_Project.Data;
using CollegeDB_Repository_Project.Models;

const string connectionString =
    @"Server=(localdb)\MSSQLLocalDB;Database=CollegeDB;Trusted_Connection=True;TrustServerCertificate=True;";

var groupRepository = new GroupRepository(connectionString);

try
{
    Console.WriteLine("Группы в CollegeDB:");
    var groups = groupRepository.GetAll();

    if (groups.Count == 0)
    {
        Console.WriteLine("Групп пока нет. Добавляем примерную группу...");
        var created = groupRepository.Add(new Group
        {
            Name = "ИС-21",
            Course = 2
        });
        Console.WriteLine($"Добавлена группа: {created}");
    }
    else
    {
        foreach (Group group in groups)
            Console.WriteLine(group);
    }

    Console.WriteLine();
    Console.WriteLine("Демонстрация завершена. CRUD-методы доступны в GroupRepository.");
}
catch (Microsoft.Data.SqlClient.SqlException ex)
{
    Console.Error.WriteLine("Ошибка SQL Server. Проверьте строку подключения и выполните sql/CollegeDB.sql.");
    Console.Error.WriteLine(ex.Message);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Ошибка: {ex.Message}");
}
