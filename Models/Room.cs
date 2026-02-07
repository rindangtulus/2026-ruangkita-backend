namespace _2026_ruangkita_backend.Models;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Facility { get; set; } = string.Empty;
}