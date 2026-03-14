using GalleryBusiness.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalleryBusiness;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<BorrowRecord> BorrowRecords { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Role).HasDefaultValue("Student");
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");
        });

        // Book configuration
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasIndex(b => b.ISBN).IsUnique();
            entity.Property(b => b.CreatedAt).HasDefaultValueSql("GETDATE()");
        });

        // BorrowRecord configuration
        modelBuilder.Entity<BorrowRecord>(entity =>
        {
            entity.HasOne(br => br.User)
                  .WithMany(u => u.BorrowRecords)
                  .HasForeignKey(br => br.UserID)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(br => br.Book)
                  .WithMany(b => b.BorrowRecords)
                  .HasForeignKey(br => br.BookID)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(br => br.Returned).HasDefaultValue(false);
        });

        // Session configuration
        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasOne(s => s.User)
                  .WithMany(u => u.Sessions)
                  .HasForeignKey(s => s.UserID)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed default admin
        modelBuilder.Entity<User>().HasData(new User
        {
            ID = 1,
            FullName = "Admin",
            Email = "admin@university.edu",
            Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Role = "Admin",
            CreatedAt = new DateTime(2024, 1, 1)
        });

        // Seed Sample Books
        modelBuilder.Entity<Book>().HasData(
            new Book { ID = 1, Title = "Clean Code", Author = "Robert C. Martin", Category = "Computer Science", ISBN = "978-0132350884", Quantity = 5, CreatedAt = DateTime.Now },
            new Book { ID = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Category = "Computer Science", ISBN = "978-0135957059", Quantity = 3, CreatedAt = DateTime.Now },
            new Book { ID = 3, Title = "Introduction to Algorithms", Author = "Thomas H. Cormen", Category = "Science & Tech", ISBN = "978-0262033848", Quantity = 2, CreatedAt = DateTime.Now },
            new Book { ID = 4, Title = "Design Patterns", Author = "Erich Gamma", Category = "Computer Science", ISBN = "978-0201633610", Quantity = 4, CreatedAt = DateTime.Now },
            new Book { ID = 5, Title = "Artificial Intelligence", Author = "Peter Norvig", Category = "Science & Tech", ISBN = "978-0134610993", Quantity = 0, CreatedAt = DateTime.Now }
        );
    }
}
