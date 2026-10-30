namespace CollegePortal.Domain;

public class GradeRecord
{
    public int Id { get; init; }
    public int StudentId { get; init; }
    public string StudentName { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public int Value { get; init; }
    public DateTime Date { get; init; } = DateTime.Now;
}

public class GradeBook
{
    private readonly List<GradeRecord> _grades = new();
    private int _nextId = 1;

    public IReadOnlyList<GradeRecord> Grades => _grades.AsReadOnly();

    public GradeRecord AddGrade(int studentId, string studentName, string subject, int value)
    {
        if (value is < 2 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Оценка должна быть от 2 до 5.");
        }

        if (string.IsNullOrWhiteSpace(subject))
        {
            throw new ArgumentException("Предмет обязателен.");
        }

        var record = new GradeRecord
        {
            Id = _nextId++,
            StudentId = studentId,
            StudentName = studentName,
            Subject = subject.Trim(),
            Value = value,
            Date = DateTime.Now
        };
        _grades.Add(record);
        return record;
    }

    public bool RemoveGrade(int id)
    {
        var item = _grades.FirstOrDefault(g => g.Id == id);
        if (item is null)
        {
            return false;
        }

        _grades.Remove(item);
        return true;
    }

    public double AverageForStudent(int studentId)
    {
        var items = _grades.Where(g => g.StudentId == studentId).ToList();
        return items.Count == 0 ? 0 : items.Average(g => g.Value);
    }
}
