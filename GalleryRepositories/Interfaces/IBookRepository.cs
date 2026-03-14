using GalleryBusiness.Entities;

namespace GalleryRepositories.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(int id);
    Task<List<Book>> GetAllAsync();
    Task<List<Book>> SearchAsync(string? title, string? author, string? category);
    Task<Book> AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);
    Task<bool> IsbnExistsAsync(string isbn);
}
