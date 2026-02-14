using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2026_ruangkita_backend.Data;
using _2026_ruangkita_backend.Models;

namespace _2026_ruangkita_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowingsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BorrowingsController(AppDbContext context)
    {
        _context = context;
    }

    private async Task<bool> IsRoomBooked(int roomId, DateTime start, DateTime end, int? excludeId = null)
    {
        return await _context.Borrowings
            .AnyAsync(b => b.RoomId == roomId &&
                           b.Status == "Approved" &&
                           b.Id != excludeId &&
                           ((start >= b.BorrowDate && start < b.ReturnDate) ||
                            (end > b.BorrowDate && end <= b.ReturnDate) ||
                            (start <= b.BorrowDate && end >= b.ReturnDate)));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Borrowing>>> GetBorrowings(
    [FromQuery] string? status,
    [FromQuery] string? search,
    [FromQuery] int? userId,
    [FromQuery] string? role)
    {
        var query = _context.Borrowings
            .Include(b => b.Room)
            .Include(b => b.StatusHistories)
            .AsQueryable();


        if (role == "User" && userId.HasValue)
        {
            query = query.Where(b => b.UserId == userId.Value);
        }

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(b => b.Status == status);
        }

        if (!string.IsNullOrEmpty(search))
        {
            var searchTerm = search.ToLower().Trim();
            query = query.Where(b =>
                b.BorrowerName.ToLower().Contains(searchTerm) ||
                b.Purpose.ToLower().Contains(searchTerm));
        }

        return await query.OrderByDescending(b => b.BorrowDate).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Borrowing>> PostBorrowing(Borrowing borrowing)
    {
        if (await IsRoomBooked(borrowing.RoomId, borrowing.BorrowDate, borrowing.ReturnDate))
        {
            return BadRequest(new { message = "Gagal! Ruangan sudah dipesan orang lain pada jam tersebut." });
        }

        borrowing.Status = "Pending";

        borrowing.Room = null;

        var isConflict = await _context.Borrowings
        .AnyAsync(b => b.RoomId == borrowing.RoomId &&
                        b.Status == "Approved" && ((borrowing.BorrowDate >= b.BorrowDate && borrowing.BorrowDate < b.ReturnDate) ||
                        (borrowing.ReturnDate > b.BorrowDate && borrowing.ReturnDate <= b.ReturnDate) ||
                        (borrowing.BorrowDate <= b.BorrowDate && borrowing.ReturnDate >= b.ReturnDate)));

        if (isConflict)
        {
            return BadRequest(new { message = "Jadwal bentrok! Ruangan sudah dipesan pada jam tersebut." });
        }

        _context.Borrowings.Add(borrowing);
        await _context.SaveChangesAsync();

        var history = new StatusHistory
        {
            BorrowingId = borrowing.Id,
            Status = "Pending",
            ChangedAt = DateTime.Now
        };
        _context.StatusHistories.Add(history);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBorrowing), new { id = borrowing.Id }, borrowing);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Borrowing>> GetBorrowing(int id)
    {
        var borrowing = await _context.Borrowings
            .Include(b => b.Room)
            .Include(b => b.StatusHistories)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (borrowing == null) return NotFound();
        return borrowing;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutBorrowing(int id, Borrowing borrowing)
    {
        if (id != borrowing.Id) return BadRequest();

        if (await IsRoomBooked(borrowing.RoomId, borrowing.BorrowDate, borrowing.ReturnDate, id))
        {
            return BadRequest(new { message = "Perubahan gagal! Waktu baru bentrok dengan jadwal yang sudah ada." });
        }

        borrowing.Status = "Pending";

        _context.Entry(borrowing).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Borrowings.Any(e => e.Id == id)) return NotFound();
            else throw;
        }

        return Ok(new
        {
            message = "Perubahan disimpan! Status kembali ke Pending untuk ditinjau Admin.",
            data = borrowing
        });
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateDto statusDto)
    {
        var borrowing = await _context.Borrowings.FindAsync(id);
        if (borrowing == null) return NotFound();

        // Catat ke history
        var history = new StatusHistory
        {
            BorrowingId = id,
            Status = statusDto.Status,
            ChangedAt = DateTime.Now
        };
        _context.StatusHistories.Add(history);

        borrowing.Status = statusDto.Status;
        await _context.SaveChangesAsync();

        // Return data lengkap beserta history barunya
        var result = await _context.Borrowings
            .Include(b => b.Room)
            .Include(b => b.StatusHistories)
            .FirstOrDefaultAsync(b => b.Id == id);

        return Ok(result);
    }

    public class StatusUpdateDto { public string Status { get; set; } = string.Empty; }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBorrowing(int id)
    {
        var borrowing = await _context.Borrowings.FindAsync(id);
        if (borrowing == null) return NotFound();

        _context.Borrowings.Remove(borrowing);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}