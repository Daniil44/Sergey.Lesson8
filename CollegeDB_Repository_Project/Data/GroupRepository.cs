using Microsoft.Data.SqlClient;
using CollegeDB_Repository_Project.Models;

namespace CollegeDB_Repository_Project.Data;

public class GroupRepository
{
    private readonly string _connectionString;

    public GroupRepository(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Строка подключения не может быть пустой.", nameof(connectionString));

        _connectionString = connectionString;
    }

    public List<Group> GetAll()
    {
        const string sql = "SELECT GroupId, Name, Course FROM dbo.Groups ORDER BY GroupId;";
        var result = new List<Group>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
            result.Add(Map(reader));

        return result;
    }

    public Group? GetById(int id)
    {
        const string sql = "SELECT GroupId, Name, Course FROM dbo.Groups WHERE GroupId = @GroupId;";
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@GroupId", id);
        connection.Open();

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public Group Add(Group group)
    {
        ArgumentNullException.ThrowIfNull(group);
        Validate(group);

        const string sql = @"
INSERT INTO dbo.Groups (Name, Course)
OUTPUT INSERTED.GroupId
VALUES (@Name, @Course);";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        AddParameters(command, group);
        connection.Open();
        group.GroupId = Convert.ToInt32(command.ExecuteScalar());
        return group;
    }

    public bool Update(Group group)
    {
        ArgumentNullException.ThrowIfNull(group);
        Validate(group);

        const string sql = @"
UPDATE dbo.Groups
SET Name = @Name, Course = @Course
WHERE GroupId = @GroupId;";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        AddParameters(command, group);
        command.Parameters.AddWithValue("@GroupId", group.GroupId);
        connection.Open();
        return command.ExecuteNonQuery() > 0;
    }

    public bool Delete(int id)
    {
        const string sql = "DELETE FROM dbo.Groups WHERE GroupId = @GroupId;";
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@GroupId", id);
        connection.Open();
        return command.ExecuteNonQuery() > 0;
    }

    private static Group Map(SqlDataReader reader) => new()
    {
        GroupId = reader.GetInt32(reader.GetOrdinal("GroupId")),
        Name = reader.GetString(reader.GetOrdinal("Name")),
        Course = reader.GetInt32(reader.GetOrdinal("Course"))
    };

    private static void AddParameters(SqlCommand command, Group group)
    {
        command.Parameters.AddWithValue("@Name", group.Name);
        command.Parameters.AddWithValue("@Course", group.Course);
    }

    private static void Validate(Group group)
    {
        if (string.IsNullOrWhiteSpace(group.Name))
            throw new ArgumentException("Название группы обязательно.");
        if (group.Name.Length > 50)
            throw new ArgumentException("Название группы должно быть не длиннее 50 символов.");
        if (group.Course is < 1 or > 6)
            throw new ArgumentOutOfRangeException(nameof(group.Course), "Курс должен быть от 1 до 6.");
    }
}
