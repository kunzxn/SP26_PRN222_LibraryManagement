using GalleryBusiness;
using GalleryBusiness.Entities;
using GalleryRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GalleryRepositories.Implementations;

public class SessionRepository : ISessionRepository
{
    private readonly LibraryDbContext _context;

    public SessionRepository(LibraryDbContext context) => _context = context;

    public async Task<Session?> GetByIdAsync(Guid sessionId)
        => await _context.Sessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.SessionID == sessionId && s.ExpiresAt > DateTime.Now);

    public async Task<Session> CreateAsync(Session session)
    {
        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task DeleteAsync(Guid sessionId)
    {
        var session = await _context.Sessions.FindAsync(sessionId);
        if (session != null)
        {
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteExpiredAsync()
    {
        var expired = await _context.Sessions.Where(s => s.ExpiresAt <= DateTime.Now).ToListAsync();
        _context.Sessions.RemoveRange(expired);
        await _context.SaveChangesAsync();
    }
}
