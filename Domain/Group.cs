namespace CollegePortal.Domain;

public class Group
{
    private readonly List<Student> _students = new();

    public Group(string name, int course)
    {
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Название группы обязательно.") : name.Trim();
        SetCourse(course);
    }

    public string Name { get; private set; }

    public int Course { get; private set; }

    public IReadOnlyList<Student> Students => _students.AsReadOnly();

    public void SetCourse(int course)
    {
        if (course < 1 || course > 4)
        {
            throw new ArgumentOutOfRangeException(nameof(course), "Курс должен быть от 1 до 4.");
        }

        Course = course;
    }

    public bool AddStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        if (_students.Any(s => s.StudentId == student.StudentId))
        {
            return false;
        }

        _students.Add(student);
        student.Group = Name;
        return true;
    }

    public bool RemoveStudent(int studentId)
    {
        var student = _students.FirstOrDefault(s => s.StudentId == studentId);
        if (student is null)
        {
            return false;
        }

        _students.Remove(student);
        return true;
    }
}
