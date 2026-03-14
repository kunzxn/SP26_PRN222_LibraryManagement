using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GalleryBusiness.Entities;

public class Session
{
    [Key]
    public Guid SessionID { get; set; } = Guid.NewGuid();

    [Required]
    public int UserID { get; set; }

    [Required, MaxLength(20)]
    public string Role { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    // Navigation
    [ForeignKey("UserID")]
    public User? User { get; set; }
}
