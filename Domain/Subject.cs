namespace CollegePortal.Domain;

public class Subject
{
    private string _title;
    private int _hours;
    private Teacher? _teacher;

    public Subject(string title, int hours)
    {
        _title = string.IsNullOrWhiteSpace(title)
            ? throw new ArgumentException("Название предмета обязательно.")
            : title.Trim();
        SetHours(hours);
    }

    public string Title => _title;

    public int Hours => _hours;

    public Teacher? Teacher => _teacher;

    public void SetHours(int hours)
    {
        if (hours < 8 || hours > 400)
        {
            throw new ArgumentOutOfRangeException(nameof(hours), "Количество часов должно быть от 8 до 400.");
        }

        _hours = hours;
    }

    public void AssignTeacher(Teacher teacher)
    {
        ArgumentNullException.ThrowIfNull(teacher);
        _teacher = teacher;
        _teacher.Subject = _title;
    }

    public string GetSubjectInfo()
    {
        var teacherName = _teacher is null ? "не назначен" : _teacher.GetFullName();
        return $"{_title}, {_hours} ч., преподаватель: {teacherName}";
    }
}
