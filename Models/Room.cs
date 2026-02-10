using System.ComponentModel.DataAnnotations;

namespace _2026_ruangkita_backend.Models;

public class Room
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nama ruangan wajib diisi!")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 200, ErrorMessage = "Kapasitas antara 1 hingga 200")]
    public int Capacity { get; set; }

    public string Facility { get; set; } = string.Empty;
}