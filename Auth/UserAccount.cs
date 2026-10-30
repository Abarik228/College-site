namespace CollegePortal.Auth;

public class UserAccount
{
    private readonly string _login;
    private readonly string _password;
    private bool _isLoggedIn;

    public UserAccount(string login, string password, User roleUser)
    {
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Логин и пароль обязательны.");
        }

        _login = login.Trim();
        _password = password;
        RoleUser = roleUser;
    }

    public string LoginName => _login;

    public bool IsLoggedIn => _isLoggedIn;

    public User RoleUser { get; }

    public bool Login(string login, string password)
    {
        _isLoggedIn = string.Equals(_login, login, StringComparison.OrdinalIgnoreCase)
                      && _password == password;
        if (_isLoggedIn)
        {
            RoleUser.MarkAuthenticated();
        }

        return _isLoggedIn;
    }

    public void Logout()
    {
        _isLoggedIn = false;
        RoleUser.MarkLoggedOut();
    }

    /// <summary>Пароль намеренно не включается в публичное описание.</summary>
    public string GetPublicInfo()
    {
        var status = _isLoggedIn ? "в системе" : "не авторизован";
        return $"Пользователь: {_login}, роль: {RoleUser.RoleName}, статус: {status}";
    }
}
