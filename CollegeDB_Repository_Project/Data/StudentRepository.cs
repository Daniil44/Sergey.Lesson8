using Microsoft.Data.SqlClient;
using CollegeDB_Repository_Project.Models;

namespace CollegeDB_Repository_Project.Data;

public class StudentRepository
{
    private readonly string _connectionString;

    public StudentRepository(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Строка подключения не может быть пустой.", nameof(connectionString));
        _connectionString = connectionString;
    }

    public List<Student> GetAll()
    {
        const string sql = @"
SELECT StudentId, FirstName, LastName, GroupId
FROM dbo.Students
ORDER BY StudentId;";
        var students = new List<Student>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        connection.Open();
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            int groupOrdinal = reader.GetOrdinal("GroupId");
            students.Add(new Student
            {
                StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                GroupId = reader.IsDBNull(groupOrdinal) ? null : reader.GetInt32(groupOrdinal)
            });
        }

        return students;
    }

    public Student? GetById(int id)
    {
        const string sql = @"
SELECT StudentId, FirstName, LastName, GroupId
FROM dbo.Students WHERE StudentId = @StudentId;";
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@StudentId", id);
        connection.Open();
        using var reader = command.ExecuteReader();

        if (!reader.Read()) return null;
        int groupOrdinal = reader.GetOrdinal("GroupId");
        return new Student
        {
            StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
            LastName = reader.GetString(reader.GetOrdinal("LastName")),
            GroupId = reader.IsDBNull(groupOrdinal) ? null : reader.GetInt32(groupOrdinal)
        };
    }

    public Student Add(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        if (string.IsNullOrWhiteSpace(student.FirstName) || string.IsNullOrWhiteSpace(student.LastName))
            throw new ArgumentException("Имя и фамилия студента обязательны.");

        const string sql = @"
INSERT INTO dbo.Students (FirstName, LastName, GroupId)
OUTPUT INSERTED.StudentId
VALUES (@FirstName, @LastName, @GroupId);";
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@FirstName", student.FirstName);
        command.Parameters.AddWithValue("@LastName", student.LastName);
        command.Parameters.AddWithValue("@GroupId", (object?)student.GroupId ?? DBNull.Value);
        connection.Open();
        student.StudentId = Convert.ToInt32(command.ExecuteScalar());
        return student;
    }
}
