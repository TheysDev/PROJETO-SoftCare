using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SoftCare.Models;

namespace SoftCare.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;
    
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GeradorToken(User user)
    {
        var handler = new JwtSecurityTokenHandler();

        var chave = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        var credencial = new SigningCredentials(new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature);

        var descricaoToken = new SecurityTokenDescriptor
        {
            Subject = GeradorDeClaims(user),
            SigningCredentials = credencial,
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = handler.CreateToken(descricaoToken);

        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GeradorDeClaims(User user)
    {
        var ci = new ClaimsIdentity();
        ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));
        
        return ci;
    }
    
}