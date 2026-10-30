namespace CollegePortal.Domain;

public class AttendanceRecord
{
    public int Id { get; init; }
    public int StudentId { get; init; }
    public string StudentName { get; init; } = string.Empty;
    public DateTime Date { get; internal set; }
    public bool Present { get; internal set; }
    public string Subject { get; set; } = string.Empty;
}

public class AttendanceStatistics
{
    public int Total { get; init; }
    public int Present { get; init; }
    public int Absent { get; init; }
    public double Percent { get; init; }
}

public class AttendanceManager
{
    private readonly List<AttendanceRecord> _records = new();
    private int _nextId = 1;

    public IReadOnlyList<AttendanceRecord> Records => _records.AsReadOnly();

    public AttendanceRecord Add(int studentId, string studentName, string subject, DateTime date, bool present)
    {
        var record = new AttendanceRecord
        {
            Id = _nextId++,
            StudentId = studentId,
            StudentName = studentName,
            Subject = subject.Trim(),
            Date = date,
            Present = present
        };
        _records.Add(record);
        return record;
    }

    public bool Update(int id, bool present, DateTime? date = null)
    {
        var record = _records.FirstOrDefault(r => r.Id == id);
        if (record is null)
        {
            return false;
        }

        record.Present = present;
        if (date.HasValue)
        {
            record.Date = date.Value;
        }

        return true;
    }

    public AttendanceStatistics GetStatistics(int? studentId = null)
    {
        var source = studentId is null ? _records : _records.Where(r => r.StudentId == studentId.Value).ToList();
        var total = source.Count;
        var present = source.Count(r => r.Present);
        var absent = total - present;
        var percent = total == 0 ? 0 : Math.Round(present * 100.0 / total, 1);
        return new AttendanceStatistics
        {
            Total = total,
            Present = present,
            Absent = absent,
            Percent = percent
        };
    }
}
