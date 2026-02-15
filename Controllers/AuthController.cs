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
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                return BadRequest(new { message = "Username sudah digunakan!" });
            }

            var user = new User
            {
                Username = registerDto.Username,
                Password = registerDto.Password,
                FullName = registerDto.FullName,
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registrasi berhasil! Silakan login." });
        }
        [HttpPut("update-profile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateProfileDTO dto) // Pakai UpdateProfileDTO
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "User tidak ditemukan" });

            user.FullName = dto.FullName;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.Password = dto.Password;
            }

            await _context.SaveChangesAsync();

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