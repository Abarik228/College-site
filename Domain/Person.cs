namespace CollegePortal.Domain;

/// <summary>
/// Базовый класс человека. Поля с модификатором protected доступны наследникам.
/// </summary>
public abstract class Person
{
    protected string _name;
    protected string _surname;
    protected int _age;

    /// <summary>
    /// Закрытое поле: доступно только внутри Person, наследники его не видят.
    /// </summary>
    private string _internalNote = "Персональные служебные данные Person";

    protected Person(string name, string surname, int age)
    {
        _name = name;
        _surname = surname;
        _age = age;
    }

    public string Name
    {
        get => _name;
        set => _name = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Имя не может быть пустым.")
            : value.Trim();
    }

    public string Surname
    {
        get => _surname;
        set => _surname = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Фамилия не может быть пустой.")
            : value.Trim();
    }

    public int Age => _age;

    /// <summary>
    /// Защищённый метод: вызывается из Student и Teacher, снаружи класса недоступен.
    /// </summary>
    protected string DisplayBasicInfo()
    {
        return $"{_surname} {_name}, {_age} лет";
    }

    /// <summary>
    /// Доступен внутри текущей сборки и в классах-наследниках.
    /// </summary>
    protected internal string GetFullName()
    {
        return $"{_surname} {_name}";
    }

    /// <summary>
    /// Доступен только наследникам внутри этой же сборки.
    /// </summary>
    private protected int CalculateStatistics()
    {
        return _age > 0 ? _age : 0;
    }

    /// <summary>
    /// Публичный метод только для демонстрации ограничения доступа (п. 20).
    /// </summary>
    public string DescribeAccessRules()
    {
        return $"protected-поля имени доступны наследникам. private-поле заметки: {_internalNote}. " +
               $"GetFullName (protected internal): {GetFullName()}. " +
               $"CalculateStatistics (private protected): {CalculateStatistics()}.";
    }
}
