using GalleryBusiness.Entities;

namespace GalleryRepositories.Interfaces;

public interface IBorrowRepository
{
    Task<BorrowRecord?> GetByIdAsync(int id);
    Task<List<BorrowRecord>> GetAllAsync();
    Task<List<BorrowRecord>> GetByUserIdAsync(int userId);
    Task<BorrowRecord?> GetActiveBorrowAsync(int userId, int bookId);
    Task<BorrowRecord> AddAsync(BorrowRecord record);
    Task UpdateAsync(BorrowRecord record);
    Task<int> GetActiveBorrowCountAsync();
    Task<int> GetOverdueCountAsync();
}
