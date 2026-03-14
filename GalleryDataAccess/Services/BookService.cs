using GalleryBusiness.Entities;
using GalleryRepositories.Interfaces;

namespace GalleryDataAccess.Services;

public class BookService
{
    private readonly IBookRepository _bookRepo;

    public BookService(IBookRepository bookRepo) => _bookRepo = bookRepo;

    public async Task<List<Book>> GetAllBooksAsync()
        => await _bookRepo.GetAllAsync();

    public async Task<Book?> GetBookByIdAsync(int id)
        => await _bookRepo.GetByIdAsync(id);

    public async Task<List<Book>> SearchBooksAsync(string? title, string? author, string? category)
        => await _bookRepo.SearchAsync(title, author, category);

    public async Task<(bool Success, string Message)> AddBookAsync(Book book)
    {
        if (await _bookRepo.IsbnExistsAsync(book.ISBN))
            return (false, "A book with this ISBN already exists.");

        await _bookRepo.AddAsync(book);
        return (true, "Book added successfully.");
    }

    public async Task<(bool Success, string Message)> UpdateBookAsync(Book book)
    {
        var existing = await _bookRepo.GetByIdAsync(book.ID);
        if (existing == null)
            return (false, "Book not found.");

        existing.Title = book.Title;
        existing.Author = book.Author;
        existing.Category = book.Category;
        existing.ISBN = book.ISBN;
        existing.Quantity = book.Quantity;

        await _bookRepo.UpdateAsync(existing);
        return (true, "Book updated successfully.");
    }

    public async Task<(bool Success, string Message)> DeleteBookAsync(int id)
    {
        var book = await _bookRepo.GetByIdAsync(id);
        if (book == null)
            return (false, "Book not found.");

        await _bookRepo.DeleteAsync(id);
        return (true, "Book deleted successfully.");
    }
}
