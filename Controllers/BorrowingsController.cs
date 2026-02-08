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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Borrowing>>> GetBorrowings()
    {
        return await _context.Borrowings.Include(b => b.Room).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Borrowing>> PostBorrowing(Borrowing borrowing)
    {
        borrowing.Status = "Pending";
        
        _context.Borrowings.Add(borrowing);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBorrowings), new { id = borrowing.Id }, borrowing);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Borrowing>> GetBorrowing(int id)
    {
        var borrowing = await _context.Borrowings.Include(b => b.Room).FirstOrDefaultAsync(b => b.Id == id);

        if (borrowing == null) return NotFound();

        return borrowing;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutBorrowing(int id, Borrowing borrowing)
    {
        if (id != borrowing.Id) return BadRequest();

        _context.Entry(borrowing).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            if (!_context.Borrowings.Any(e => e.Id == id)) return NotFound();
            else throw;
        }

        return NoContent();
    }

        // PATCH: api/borrowings/5/status
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateDto statusDto)
    {
        var borrowing = await _context.Borrowings.FindAsync(id);

        if (borrowing == null) return NotFound();

        var allowedStatuses = new[] { "Approved", "Rejected", "Pending" };
        if (!allowedStatuses.Contains(statusDto.Status))
        {
            return BadRequest("Status tidak valid.");
        }

        borrowing.Status = statusDto.Status;
        await _context.SaveChangesAsync();

        var updatedData = await _context.Borrowings
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.Id == id);

        return Ok(new { message = "Status updated successfully", data = updatedData });
    }

    public class StatusUpdateDto
    {
        public string Status { get; set; } = string.Empty;
    }

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