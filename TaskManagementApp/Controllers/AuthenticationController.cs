using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TaskManagementApp.Data;
using TaskManagementApp.DTOs;
using TaskManagementApp.Models;
using Serilog;

namespace TaskManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Register a new user
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest userDto)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
                {
                    Log.Warning("User registration failed. Username '{Username}' already exists.", userDto.Username);
                    return BadRequest("Username already exists.");
                }

                var hashedPassword = HashPassword(userDto.Password!);
                var newUser = new UserEntity
                {
                    Username = userDto.Username,
                    Password = hashedPassword,
                    Role = userDto.Role
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                Log.Information("User '{Username}' registered successfully.", userDto.Username);
                return Ok(new { message = "User registered successfully!" });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while registering user '{Username}'.", userDto.Username);
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        // Login a user and return a success message
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserRequest userDto)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == userDto.Username);

                if (user == null || !VerifyPassword(userDto.Password!, user.Password!))
                {
                    Log.Warning("Login attempt failed for username '{Username}'. Invalid credentials.", userDto.Username);
                    return Unauthorized("Invalid username or password.");
                }

                Log.Information("User '{Username}' logged in successfully.", userDto.Username);
                return Ok(new
                {
                    message = "Login successful!",
                    user = new
                    {
                        user.Id,
                        user.Username,
                        user.Role
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during login for username '{Username}'.", userDto.Username);
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        // Helper method to hash the password
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        // Helper method to verify the password
        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return HashPassword(enteredPassword) == storedHash;
        }
    }
}
