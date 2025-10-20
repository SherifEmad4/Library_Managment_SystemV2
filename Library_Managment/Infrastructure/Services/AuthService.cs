using Library_Managment.Application.DTOs;
using Library_Managment.Application.Interfaces;
using Library_Managment.Infrastructure.Data;
using Library_Managment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_Managment.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly LibraryDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(LibraryDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            // Check if email already exists
            if (await _context.Members.AnyAsync(m => m.Email == dto.Email))
                throw new Exception("Email already exists.");

            // Hash the password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // Create a new member
            var member = new Member
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = passwordHash,
                JoinDate = DateTime.UtcNow,
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = GenerateToken(member);
            return new AuthResponseDto { Token = token, Email = member.Email };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // Find member by email
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == dto.Email);
            if (member == null || !BCrypt.Net.BCrypt.Verify(dto.Password, member.PasswordHash))
                throw new Exception("Invalid credentials.");

            // Generate JWT token
            var token = GenerateToken(member);
            return new AuthResponseDto { Token = token, Email = member.Email };
        }

        private string GenerateToken(Member member)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                new Claim(ClaimTypes.Email, member.Email ?? string.Empty)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
