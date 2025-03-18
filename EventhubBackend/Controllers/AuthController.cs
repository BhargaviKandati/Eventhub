using Microsoft.AspNetCore.Mvc;
using Eventhub.Models;
using BCrypt.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Eventhub.DTOs;

namespace Eventhub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly EFCoreDBContext _context; 
        private readonly IConfiguration _configuration;

        public AuthController(EFCoreDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(UserDto userDTO)
        {
            // Check if the username or email already exists
            if (await _context.Users.AnyAsync(u => u.UserName == userDTO.UserName || u.Email == userDTO.Email))
            {
                return BadRequest("Username or Email already exists.");
            }

            // Hash the password
            userDTO.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

            var user = new User
            {
                UserName = userDTO.UserName,
                FullName = userDTO.FullName,
                Email = userDTO.Email,
                Password = userDTO.Password,
                PhoneNumber = userDTO.PhoneNumber
            };

            // Add the user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Find the user by username
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            // Authentication successful
            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtSettings:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["jwtSettings:issuer"],
                audience: _configuration["jwtSettings:audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
