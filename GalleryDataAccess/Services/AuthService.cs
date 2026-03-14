using GalleryBusiness.Entities;
using GalleryRepositories.Interfaces;

namespace GalleryDataAccess.Services;

public class AuthService
{
    private readonly IUserRepository _userRepo;
    private readonly ISessionRepository _sessionRepo;

    public AuthService(IUserRepository userRepo, ISessionRepository sessionRepo)
    {
        _userRepo = userRepo;
        _sessionRepo = sessionRepo;
    }

    public async Task<(bool Success, string Message, Session? Session, User? User)> LoginAsync(string email, string password)
    {
        var user = await _userRepo.GetByEmailAsync(email);
        if (user == null)
            return (false, "Invalid email or password.", null, null);

        bool isPasswordValid = false;
        try
        {
            isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
        }
        catch
        {
            // Fallback: If DB contains plain text password due to manual SQL edits
            isPasswordValid = (password == user.Password);
        }

        if (!isPasswordValid)
            return (false, "Invalid email or password.", null, null);

        var session = new Session
        {
            SessionID = Guid.NewGuid(),
            UserID = user.ID,
            Role = user.Role,
            ExpiresAt = DateTime.Now.AddHours(24)
        };

        await _sessionRepo.CreateAsync(session);
        return (true, "Login successful.", session, user);
    }

    public async Task<(bool Success, string Message)> RegisterAsync(string fullName, string email, string password)
    {
        if (await _userRepo.EmailExistsAsync(email))
            return (false, "An account with this email already exists.");

        var user = new User
        {
            FullName = fullName,
            Email = email,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            Role = "Student",
            CreatedAt = DateTime.Now
        };

        await _userRepo.AddAsync(user);
        return (true, "Registration successful! You can now log in.");
    }

    public async Task<Session?> ValidateSessionAsync(Guid sessionId)
    {
        return await _sessionRepo.GetByIdAsync(sessionId);
    }

    public async Task LogoutAsync(Guid sessionId)
    {
        await _sessionRepo.DeleteAsync(sessionId);
    }
}
