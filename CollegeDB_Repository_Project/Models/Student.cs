namespace CollegeDB_Repository_Project.Models;

public class Student
{
    public int StudentId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? GroupId { get; set; }

    public override string ToString()
        => $"ID: {StudentId}; {LastName} {FirstName}; ID группы: {GroupId?.ToString() ?? "не назначена"}";
}
