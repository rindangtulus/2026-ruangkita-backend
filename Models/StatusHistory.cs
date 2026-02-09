using System.ComponentModel.DataAnnotations;

namespace _2026_ruangkita_backend.Models;

public class StatusHistory
{
    public int Id { get; set; }
    public int BorrowingId { get; set; }
    
    [Required]
    public string Status { get; set; } = string.Empty;
    
    public DateTime ChangedAt { get; set; } = DateTime.Now;

    public Borrowing? Borrowing { get; set; }

    public List<StatusHistory> StatusHistories { get; set; } = new();
}