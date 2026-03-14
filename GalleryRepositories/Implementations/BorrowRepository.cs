using GalleryBusiness;
using GalleryBusiness.Entities;
using GalleryRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GalleryRepositories.Implementations;

public class BorrowRepository : IBorrowRepository
{
    private readonly LibraryDbContext _context;

    public BorrowRepository(LibraryDbContext context) => _context = context;

    public async Task<BorrowRecord?> GetByIdAsync(int id)
        => await _context.BorrowRecords
            .Include(br => br.User)
            .Include(br => br.Book)
            .FirstOrDefaultAsync(br => br.ID == id);

    public async Task<List<BorrowRecord>> GetAllAsync()
        => await _context.BorrowRecords
            .Include(br => br.User)
            .Include(br => br.Book)
            .OrderByDescending(br => br.BorrowDate)
            .ToListAsync();

    public async Task<List<BorrowRecord>> GetByUserIdAsync(int userId)
        => await _context.BorrowRecords
            .Include(br => br.Book)
            .Where(br => br.UserID == userId)
            .OrderByDescending(br => br.BorrowDate)
            .ToListAsync();

    public async Task<BorrowRecord?> GetActiveBorrowAsync(int userId, int bookId)
        => await _context.BorrowRecords
            .FirstOrDefaultAsync(br => br.UserID == userId && br.BookID == bookId && !br.Returned);

    public async Task<BorrowRecord> AddAsync(BorrowRecord record)
    {
        _context.BorrowRecords.Add(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task UpdateAsync(BorrowRecord record)
    {
        _context.BorrowRecords.Update(record);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetActiveBorrowCountAsync()
        => await _context.BorrowRecords.CountAsync(br => !br.Returned);

    public async Task<int> GetOverdueCountAsync()
        => await _context.BorrowRecords.CountAsync(br => !br.Returned && br.DueDate < DateTime.Now);
}
