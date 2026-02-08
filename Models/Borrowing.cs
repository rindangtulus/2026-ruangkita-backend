using System.ComponentModel.DataAnnotations;

namespace _2026_ruangkita_backend.Models;

public class Borrowing
{
    public int Id { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Nama peminjam wajib diisi")]
    public string BorrowerName { get; set; } = string.Empty;

    [Required]
    public DateTime BorrowDate { get; set; }

    [Required]
    public DateTime ReturnDate { get; set; }

    [Required]
    public string Purpose { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    public Room? Room { get; set; }
}