namespace CollegePortal.Domain;

/// <summary>
/// Студент колледжа. Наследует Person, поэтому поля имени, фамилии и возраста не дублируются.
/// </summary>
public class Student : Person
{
    private static int _nextId = 1000;
    private readonly int _studentId;
    private string _groupName;

    public Student(string name, string surname, int age, string group)
        : base(name, surname, age)
    {
        ValidateStudentData(name, surname, age, group);
        _studentId = Interlocked.Increment(ref _nextId);
        SetAge(age);
        _groupName = group.Trim();
    }

    /// <summary>Идентификатор доступен только для чтения после создания объекта.</summary>
    public int StudentId => _studentId;

    public string Group
    {
        get => _groupName;
        set => _groupName = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Группа не может быть пустой.")
            : value.Trim();
    }

    public void SetAge(int age)
    {
        if (age < 15 || age > 70)
        {
            throw new ArgumentOutOfRangeException(nameof(age), "Возраст студента должен быть от 15 до 70 лет.");
        }

        _age = age;
    }

    private void ValidateStudentData(string name, string surname, int age, string group)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
        {
            throw new ArgumentException("Имя и фамилия обязательны.");
        }

        if (string.IsNullOrWhiteSpace(group))
        {
            throw new ArgumentException("Группа обязательна.");
        }

        if (age < 15 || age > 70)
        {
            throw new ArgumentOutOfRangeException(nameof(age), "Возраст студента должен быть от 15 до 70 лет.");
        }
    }

    public string GetStudentInfo()
    {
        var stats = CalculateStatistics();
        return $"{DisplayBasicInfo()} | ID: {StudentId} | группа: {_groupName} | показатель: {stats}";
    }
}
