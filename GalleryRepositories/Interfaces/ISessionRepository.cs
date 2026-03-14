using GalleryBusiness.Entities;

namespace GalleryRepositories.Interfaces;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(Guid sessionId);
    Task<Session> CreateAsync(Session session);
    Task DeleteAsync(Guid sessionId);
    Task DeleteExpiredAsync();
}
