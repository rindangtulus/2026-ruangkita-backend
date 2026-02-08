using Microsoft.EntityFrameworkCore;
using _2026_ruangkita_backend.Models;

namespace _2026_ruangkita_backend.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Borrowing> Borrowings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>().HasData(
            new Room { Id = 1, Name = "Lab komputer", Capacity = 30, Facility = "AC, Proyektor, komputer" },
            new Room { Id = 2, Name = "B-201", Capacity = 40, Facility = "AC, Whiteboard" }
        );
    }
}
