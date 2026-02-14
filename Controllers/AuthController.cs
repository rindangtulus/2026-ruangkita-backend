using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2026_ruangkita_backend.Data;
using _2026_ruangkita_backend.Models;

namespace _2026_ruangkita_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || user.Password != loginDto.Password)
            {
                return Unauthorized(new { message = "Username atau password salah!" });
            }

            return Ok(new
            {
                id = user.Id,
                username = user.Username,
                role = user.Role,
                fullName = user.FullName
            });
        }
    }
}