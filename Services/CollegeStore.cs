using CollegePortal.Auth;
using CollegePortal.Domain;
using AuthStudent = CollegePortal.Auth.Student;
using AuthTeacher = CollegePortal.Auth.Teacher;
using DomainStudent = CollegePortal.Domain.Student;
using DomainTeacher = CollegePortal.Domain.Teacher;

namespace CollegePortal.Services;

public class CollegeStore
{
    private readonly CollegeSettings _settings;
    private readonly List<DomainStudent> _students = new();
    private readonly List<DomainTeacher> _teachers = new();
    private readonly List<Group> _groups = new();
    private readonly List<Subject> _subjects = new();
    private readonly List<UserAccount> _accounts = new();
    private readonly CollegeReports _reports = new();

    public CollegeStore()
    {
        _settings = new CollegeSettings("Городской политехнический колледж", 2026);
        GradeBook = new GradeBook();
        Attendance = new AttendanceManager();
        Seed();
    }

    public IReadOnlyList<DomainStudent> Students => _students;
    public IReadOnlyList<DomainTeacher> Teachers => _teachers;
    public IReadOnlyList<Group> Groups => _groups;
    public IReadOnlyList<Subject> Subjects => _subjects;
    public IReadOnlyList<UserAccount> Accounts => _accounts;
    public GradeBook GradeBook { get; }
    public AttendanceManager Attendance { get; }

    public string CollegeName => _settings.CollegeName;
    public int AcademicYear => _settings.AcademicYear;

    public void UpdateSettings(string name, int year)
    {
        _settings.SetCollegeName(name);
        _settings.SetAcademicYear(year);
    }

    public DomainStudent AddStudent(string name, string surname, int age, string groupName)
    {
        var student = new DomainStudent(name, surname, age, groupName);
        _students.Add(student);
        var group = _groups.FirstOrDefault(g => g.Name == groupName);
        group?.AddStudent(student);
        return student;
    }

    public DomainTeacher AddTeacher(string name, string surname, int age, string subject, int experience)
    {
        var teacher = new DomainTeacher(name, surname, age, subject, experience);
        _teachers.Add(teacher);
        return teacher;
    }

    public Group AddGroup(string name, int course)
    {
        if (_groups.Any(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Группа с таким названием уже существует.");
        }

        var group = new Group(name, course);
        _groups.Add(group);
        return group;
    }

    public UserAccount? FindAccount(string login) =>
        _accounts.FirstOrDefault(a => a.LoginName.Equals(login, StringComparison.OrdinalIgnoreCase));

    public string BuildStudentsReport() => _reports.StudentsReport(_students);

    public string BuildTeachersReport() => _reports.TeachersReport(_teachers);

    public string BuildAttendanceReport()
    {
        var stats = Attendance.GetStatistics();
        var text = $"Всего отметок: {stats.Total}{Environment.NewLine}" +
                   $"Присутствовали: {stats.Present}{Environment.NewLine}" +
                   $"Отсутствовали: {stats.Absent}{Environment.NewLine}" +
                   $"Посещаемость: {stats.Percent}%";
        return _reports.AttendanceReport(text);
    }

    private void Seed()
    {
        var is21 = AddGroup("ИС-21", 2);
        var pks22 = AddGroup("ПКС-22", 1);
        var po23 = AddGroup("ПО-23", 3);

        var t1 = AddTeacher("Елена", "Соколова", 42, "Программирование", 18);
        var t2 = AddTeacher("Игорь", "Белов", 38, "Базы данных", 12);
        var t3 = AddTeacher("Мария", "Орлова", 35, "Веб-разработка", 9);

        var programming = new Subject("Программирование", 144);
        programming.AssignTeacher(t1);
        var databases = new Subject("Базы данных", 108);
        databases.AssignTeacher(t2);
        var web = new Subject("Веб-разработка", 120);
        web.AssignTeacher(t3);
        _subjects.AddRange(new[] { programming, databases, web });

        var s1 = AddStudent("Анна", "Козлова", 18, is21.Name);
        var s2 = AddStudent("Павел", "Морозов", 19, is21.Name);
        var s3 = AddStudent("Дарья", "Лебедева", 17, pks22.Name);
        var s4 = AddStudent("Никита", "Волков", 20, po23.Name);
        var s5 = AddStudent("Ольга", "Новикова", 18, pks22.Name);

        GradeBook.AddGrade(s1.StudentId, s1.GetFullName(), "Программирование", 5);
        GradeBook.AddGrade(s1.StudentId, s1.GetFullName(), "Базы данных", 4);
        GradeBook.AddGrade(s2.StudentId, s2.GetFullName(), "Программирование", 4);
        GradeBook.AddGrade(s3.StudentId, s3.GetFullName(), "Веб-разработка", 5);
        GradeBook.AddGrade(s4.StudentId, s4.GetFullName(), "Базы данных", 3);

        Attendance.Add(s1.StudentId, s1.GetFullName(), "Программирование", DateTime.Today.AddDays(-2), true);
        Attendance.Add(s2.StudentId, s2.GetFullName(), "Программирование", DateTime.Today.AddDays(-2), false);
        Attendance.Add(s3.StudentId, s3.GetFullName(), "Веб-разработка", DateTime.Today.AddDays(-1), true);
        Attendance.Add(s4.StudentId, s4.GetFullName(), "Базы данных", DateTime.Today, true);
        Attendance.Add(s5.StudentId, s5.GetFullName(), "Веб-разработка", DateTime.Today, false);

        _accounts.Add(new UserAccount("admin", "admin123", new Admin("Елена Админова")));
        _accounts.Add(new UserAccount("teacher", "teacher123", new AuthTeacher("Елена Соколова")));
        _accounts.Add(new UserAccount("student", "student123", new AuthStudent("Анна Козлова")));
    }
}
