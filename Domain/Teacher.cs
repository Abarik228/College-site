namespace CollegePortal.Domain;

public class Teacher : Person
{
    private string _subject;
    private int _experience;

    public Teacher(string name, string surname, int age, string subject, int experience)
        : base(name, surname, age)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
        {
            throw new ArgumentException("Имя и фамилия преподавателя обязательны.");
        }

        if (age < 22 || age > 80)
        {
            throw new ArgumentOutOfRangeException(nameof(age), "Возраст преподавателя должен быть от 22 до 80 лет.");
        }

        _subject = string.IsNullOrWhiteSpace(subject)
            ? throw new ArgumentException("Предмет обязателен.")
            : subject.Trim();
        UpdateExperience(experience);
    }

    public string Subject
    {
        get => _subject;
        set => _subject = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Предмет не может быть пустым.")
            : value.Trim();
    }

    /// <summary>Стаж можно изменить только через UpdateExperience().</summary>
    public int Experience => _experience;

    public void UpdateExperience(int years)
    {
        if (years < 0 || years > 55)
        {
            throw new ArgumentOutOfRangeException(nameof(years), "Стаж должен быть от 0 до 55 лет.");
        }

        _experience = years;
    }

    public string GetTeacherInfo()
    {
        return $"{DisplayBasicInfo()} | предмет: {_subject} | стаж: {_experience} лет | ФИО: {GetFullName()}";
    }
}
