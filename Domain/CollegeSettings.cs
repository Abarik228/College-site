namespace CollegePortal.Domain;

/// <summary>
/// Настройки колледжа видны только внутри текущей сборки.
/// </summary>
internal class CollegeSettings
{
    private string _collegeName;
    private int _academicYear;

    internal CollegeSettings(string collegeName, int academicYear)
    {
        _collegeName = collegeName;
        _academicYear = academicYear;
    }

    internal string CollegeName => _collegeName;

    internal int AcademicYear => _academicYear;

    internal void SetCollegeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Название колледжа обязательно.");
        }

        _collegeName = name.Trim();
    }

    internal void SetAcademicYear(int year)
    {
        if (year < 2000 || year > 2100)
        {
            throw new ArgumentOutOfRangeException(nameof(year), "Некорректный учебный год.");
        }

        _academicYear = year;
    }
}
