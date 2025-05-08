
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using ResumeBuilder.Data;
using ResumeBuilder.DTOs;
using ResumeBuilder.Models;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<AuthResponseDto?> RegisterAsync(AuthRequestDto dto)
    {
        if (_context.Users.Any(u => u.Email == dto.Email)) return null;

        var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = hashed
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            Token = GenerateJwt(user)
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(AuthRequestDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
        if (user == null) return null;

        bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!valid) return null;

        return new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            Token = GenerateJwt(user)
        };
    }

    private string GenerateJwt(User user)
    {
        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // Add this claim
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
