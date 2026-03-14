using System.ComponentModel.DataAnnotations;

namespace GalleryBusiness.Entities;

public class User
{
    [Key]
    public int ID { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Role { get; set; } = "Student"; // "Admin" or "Student"

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation
    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}
