namespace CollegePortal.Domain;

public class ReportManager
{
    protected internal string GenerateReport(string title, string body)
    {
        return $"=== {title} ==={Environment.NewLine}{body}{Environment.NewLine}Сформировано: {DateTime.Now:dd.MM.yyyy HH:mm}";
    }
}

public class CollegeReports : ReportManager
{
    public string StudentsReport(IEnumerable<Student> students)
    {
        var lines = students.Select(s => s.GetStudentInfo());
        return GenerateReport("Отчёт по студентам", string.Join(Environment.NewLine, lines));
    }

    public string TeachersReport(IEnumerable<Teacher> teachers)
    {
        var lines = teachers.Select(t => t.GetTeacherInfo());
        return GenerateReport("Отчёт по преподавателям", string.Join(Environment.NewLine, lines));
    }

    public string AttendanceReport(string statisticsText)
    {
        return GenerateReport("Отчёт по посещаемости", statisticsText);
    }
}
