namespace CollegePortal.Auth;

/// <summary>
/// Базовый пользователь системы. Общие данные — protected, конфиденциальные — private.
/// </summary>
public abstract class User
{
    protected string DisplayName { get; set; }
    protected string Role { get; set; }
    private readonly string _securityStamp;
    private bool _authenticated;

    protected User(string displayName, string role)
    {
        DisplayName = displayName;
        Role = role;
        _securityStamp = Guid.NewGuid().ToString("N")[..8];
    }

    public string RoleName => Role;

    public string VisibleName => DisplayName;

    public bool IsAuthenticated => _authenticated;

    internal void MarkAuthenticated() => _authenticated = true;

    internal void MarkLoggedOut() => _authenticated = false;

    public string GetProfileInfo()
    {
        return $"{DisplayName} ({Role})";
    }

    /// <summary>Секретный штамп не выводится в интерфейсе.</summary>
    protected string GetSecurityHint() => $"stamp:{_securityStamp}";
}

public class Admin : User
{
    public Admin(string displayName) : base(displayName, "Admin")
    {
    }

    public string GetAdminPanelTitle() => $"Администратор {DisplayName}";
}

public class Teacher : User
{
    public Teacher(string displayName) : base(displayName, "Teacher")
    {
    }

    public string GetCabinetTitle() => $"Кабинет преподавателя {DisplayName}";
}

public class Student : User
{
    public Student(string displayName) : base(displayName, "Student")
    {
    }

    public string GetCabinetTitle() => $"Кабинет студента {DisplayName}";
}
