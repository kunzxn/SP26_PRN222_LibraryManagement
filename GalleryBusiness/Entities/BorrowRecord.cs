using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GalleryBusiness.Entities;

public class BorrowRecord
{
    [Key]
    public int ID { get; set; }

    [Required]
    public int UserID { get; set; }

    [Required]
    public int BookID { get; set; }

    public DateTime BorrowDate { get; set; } = DateTime.Now;

    public DateTime DueDate { get; set; } = DateTime.Now.AddDays(14);

    public bool Returned { get; set; } = false;

    // Navigation
    [ForeignKey("UserID")]
    public User? User { get; set; }

    [ForeignKey("BookID")]
    public Book? Book { get; set; }
}
