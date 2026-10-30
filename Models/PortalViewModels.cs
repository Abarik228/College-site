using CollegePortal.Domain;

namespace CollegePortal.Models;

public class GradePageViewModel
{
    public IReadOnlyList<GradeRecord> Grades { get; init; } = Array.Empty<GradeRecord>();
    public IReadOnlyList<Student> Students { get; init; } = Array.Empty<Student>();
    public IReadOnlyList<Subject> Subjects { get; init; } = Array.Empty<Subject>();
}

public class AttendancePageViewModel
{
    public IReadOnlyList<AttendanceRecord> Records { get; init; } = Array.Empty<AttendanceRecord>();
    public IReadOnlyList<Student> Students { get; init; } = Array.Empty<Student>();
    public IReadOnlyList<Subject> Subjects { get; init; } = Array.Empty<Subject>();
    public AttendanceStatistics Statistics { get; init; } = new();
}

public class AdminPageViewModel
{
    public string CollegeName { get; init; } = string.Empty;
    public int AcademicYear { get; init; }
    public int StudentsCount { get; init; }
    public int TeachersCount { get; init; }
    public int GroupsCount { get; init; }
    public int AccountsCount { get; init; }
}