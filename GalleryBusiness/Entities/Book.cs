using System.ComponentModel.DataAnnotations;

namespace GalleryBusiness.Entities;

public class Book
{
    [Key]
    public int ID { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Author { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation
    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}
