using Microsoft.IdentityModel.Tokens;
using NoteApp.Api.Domain.Entities;
using NoteApp.Api.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NoteApp.Api.Services;
public class JwtService(IConfiguration configuration) : IJwtService
{
	private readonly IConfiguration _configuration = configuration;
	public string GenerateToken(User user)
	{
		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
			new Claim(ClaimTypes.Email,user.Email),
			new Claim(ClaimTypes.Name, user.Username),
			new Claim(ClaimTypes.Role,user.Role),
			new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
		};

		var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
			?? throw new InvalidOperationException("JWT_KEY environment variable not set.");

		if (jwtKey.Length < 32)
			throw new InvalidOperationException("JWT_KEY must be at least 32 characters long for HMAC-SHA256.");

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var issuer = _configuration["Jwt:Issuer"] ?? "DefaultIssuer";

		var audience = _configuration["Jwt:Audience"] ?? "DefaultAudience";
		var expireHours = _configuration.GetValue<int>("Jwt:ExpireInHours");

		var token = new JwtSecurityToken(
			 issuer: issuer,
			 audience: audience,
			 claims: claims,
			 expires: DateTime.UtcNow.AddHours(expireHours),
			 signingCredentials: creds
			);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
