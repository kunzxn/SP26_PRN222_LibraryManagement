using GalleryBusiness.Entities;
using GalleryRepositories.Interfaces;

namespace GalleryDataAccess.Services;

public class BorrowService
{
    private readonly IBorrowRepository _borrowRepo;
    private readonly IBookRepository _bookRepo;

    public BorrowService(IBorrowRepository borrowRepo, IBookRepository bookRepo)
    {
        _borrowRepo = borrowRepo;
        _bookRepo = bookRepo;
    }

    public async Task<(bool Success, string Message)> BorrowBookAsync(int userId, int bookId)
    {
        // Rule 1: Check if quantity > 0
        var book = await _bookRepo.GetByIdAsync(bookId);
        if (book == null)
            return (false, "Book not found.");

        if (book.Quantity <= 0)
            return (false, "This book is currently out of stock.");

        // Rule 2: Check if user already borrowed this book without returning
        var existingBorrow = await _borrowRepo.GetActiveBorrowAsync(userId, bookId);
        if (existingBorrow != null)
            return (false, "You have already borrowed this book. Please return it first.");

        // Rule 3 & 4: BorrowDate = now, DueDate = now + 14 days
        var record = new BorrowRecord
        {
            UserID = userId,
            BookID = bookId,
            BorrowDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(14),
            Returned = false // Rule 5
        };

        await _borrowRepo.AddAsync(record);

        // Reduce book quantity
        book.Quantity -= 1;
        await _bookRepo.UpdateAsync(book);

        return (true, "Book borrowed successfully! Due date: " + record.DueDate.ToString("MMM dd, yyyy"));
    }

    public async Task<(bool Success, string Message)> ReturnBookAsync(int borrowId)
    {
        var record = await _borrowRepo.GetByIdAsync(borrowId);
        if (record == null)
            return (false, "Borrow record not found.");

        if (record.Returned)
            return (false, "This book has already been returned.");

        record.Returned = true;
        await _borrowRepo.UpdateAsync(record);

        // Increase book quantity back
        var book = await _bookRepo.GetByIdAsync(record.BookID);
        if (book != null)
        {
            book.Quantity += 1;
            await _bookRepo.UpdateAsync(book);
        }

        return (true, "Book returned successfully.");
    }

    public async Task<List<BorrowRecord>> GetAllRecordsAsync()
        => await _borrowRepo.GetAllAsync();

    public async Task<List<BorrowRecord>> GetUserRecordsAsync(int userId)
        => await _borrowRepo.GetByUserIdAsync(userId);

    public async Task<int> GetActiveBorrowCountAsync()
        => await _borrowRepo.GetActiveBorrowCountAsync();

    public async Task<int> GetOverdueCountAsync()
        => await _borrowRepo.GetOverdueCountAsync();
}
