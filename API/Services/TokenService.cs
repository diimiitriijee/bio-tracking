using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain;
using Domain.src;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<Korisnik> _userManager;
    public TokenService(IConfiguration config, UserManager<Korisnik> userManager)
    {
        _userManager = userManager;
        _config = config;
    }
    public async Task<string> CreateToken(Korisnik korisnik)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, korisnik.UserName),
            new Claim(ClaimTypes.NameIdentifier, korisnik.Id.ToString()),//ToString je trenutni fix samo da probam da pokrenem
            new Claim(ClaimTypes.Email, korisnik.Email),

        };
        //TODO: Da se u zavisnosti od role da li je vodic napravi poseban token da ima sve atribute kao i vodic
        var roles = await _userManager.GetRolesAsync(korisnik);//nalazim role za korisnika da ih stavim u token, mora da bude async jer pristupa bazi

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));//stavljam ih u token

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
    public RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return new RefreshToken{Token = Convert.ToBase64String(randomNumber)};
    }
}