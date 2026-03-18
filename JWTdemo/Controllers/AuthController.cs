using JWTdemo.Data;
using JWTdemo.DTOs;
using JWTdemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTdemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _passwordHasher = new PasswordHasher<User>();
        }

        //  REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromForm] string username,
            [FromForm] string password)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Username == username);

            if (userExists)
                return BadRequest("User already exists");

            var user = new User
            {
                Username = username,
                Role = "User"
            };

            // 🔐 HASH PASSWORD
            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromForm] string username,
            [FromForm] string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                password
            );

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid credentials");

            var (token, expiresAt) = GenerateJwtToken(user);

            // calculate remaining minutes
            var expiresInMinutes = (int)(expiresAt - DateTime.Now).TotalMinutes;

            return Ok(new
            {
                token,
                expiresInMinutes
            });
        }

        // TOKEN GENERATION
        private (string token, DateTime expiresAt) GenerateJwtToken(User user)
        {
            var expires = DateTime.Now.AddMinutes(5); // change here if needed

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return (tokenString, expires);
        }
    }
}