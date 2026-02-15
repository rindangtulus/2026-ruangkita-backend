using System.ComponentModel.DataAnnotations;

namespace _2026_ruangkita_backend.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty; // Nanti kita simpan versi teks atau hash

        [Required]
        public string Role { get; set; } = "User"; // Nilainya: "Admin" atau "User"

        public string FullName { get; set; } = string.Empty;
    }
}