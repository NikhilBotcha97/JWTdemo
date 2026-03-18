using JWTdemo.Data;
using JWTdemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTdemo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserAccountController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserAccountController(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 GET ALL USERS
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new { u.Id, u.Username, u.Role })
                .ToListAsync();

            return Ok(users);
        }

        // 🔹 GET USER BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new { u.Id, u.Username, u.Role })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // 🔹 UPDATE USER (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] string username)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            user.Username = username;

            await _context.SaveChangesAsync();

            return Ok("User updated");
        }

        // 🔹 DELETE USER
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User deleted");
        }
    }
}