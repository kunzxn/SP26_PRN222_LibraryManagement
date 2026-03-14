using GalleryBusiness;
using GalleryBusiness.Entities;
using GalleryRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GalleryRepositories.Implementations;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context) => _context = context;

    public async Task<Book?> GetByIdAsync(int id)
        => await _context.Books.FindAsync(id);

    public async Task<List<Book>> GetAllAsync()
        => await _context.Books.OrderByDescending(b => b.CreatedAt).ToListAsync();

    public async Task<List<Book>> SearchAsync(string? title, string? author, string? category)
    {
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(b => b.Title.Contains(title));

        if (!string.IsNullOrWhiteSpace(author))
            query = query.Where(b => b.Author.Contains(author));

        if (!string.IsNullOrWhiteSpace(category) && category != "All Categories")
            query = query.Where(b => b.Category == category);

        return await query.OrderByDescending(b => b.CreatedAt).ToListAsync();
    }

    public async Task<Book> AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsbnExistsAsync(string isbn)
        => await _context.Books.AnyAsync(b => b.ISBN == isbn);
}
