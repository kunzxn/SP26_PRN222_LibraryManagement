namespace Blazor_Server_App.Services;

public class UserSession
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsLoggedIn => UserId > 0;

    public void Login(int userId, string fullName, string role)
    {
        UserId = userId;
        FullName = fullName;
        Role = role;
    }

    public void Logout()
    {
        UserId = 0;
        FullName = string.Empty;
        Role = string.Empty;
    }
}
