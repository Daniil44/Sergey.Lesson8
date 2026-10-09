namespace CollegeDB_Repository_Project.Models;

public class Group
{
    public int GroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Course { get; set; }

    public override string ToString()
        => $"ID: {GroupId}; Название: {Name}; Курс: {Course}";
}
