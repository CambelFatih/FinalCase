using BSYS.Application.Abstractions.Services;
using BSYS.Application.Abstractions.Token;
using BSYS.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BSYS.Infrastructure.Services.Token;

public class TokenHandler : ITokenHandler
{
    private readonly IConfiguration _configuration;
    private readonly IUserRoleService _userRoleService;

    public TokenHandler(IConfiguration configuration, IUserRoleService userRoleService)
    {
        _configuration = configuration;
        _userRoleService = userRoleService;
    }

    public async Task<Application.DTOs.Token> CreateAccessToken(int second, AppUser user)
    {
        Application.DTOs.Token token = new();
        
        //Security Key'in simetriğini alıyoruz.
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

        //Şifrelenmiş kimliği oluşturuyoruz.
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
        var claims =  await GetClaims(user);
        //Oluşturulacak token ayarlarını veriyoruz.
        token.Expiration = DateTime.UtcNow.AddSeconds(second);
        JwtSecurityToken securityToken = new(
            audience: _configuration["Token:Audience"],
            issuer: _configuration["Token:Issuer"],
            expires: token.Expiration,
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials,
            claims: claims
            );

        //Token oluşturucu sınıfından bir örnek alalım.
        JwtSecurityTokenHandler tokenHandler = new();
        token.AccessToken = tokenHandler.WriteToken(securityToken);

        //string refreshToken = CreateRefreshToken();

        token.RefreshToken = CreateRefreshToken();
        return token;
    }
    private async Task<Claim[]> GetClaims(AppUser user)
    {
        string role = "";
        if (await _userRoleService.IsUserInRoleAsync(user.Id, "Admin")) role = "Admin"; 
        else if (await _userRoleService.IsUserInRoleAsync(user.Id, "Bayi")) role = "Bayi";
        else role = "User";
        // Define the claims for the user
        var claims = new[]
        {
            new(ClaimTypes.Name, user.UserName),
            new Claim("Id", user.Id.ToString()),
            new Claim("UserName", user.UserName.ToString()),
            new Claim("Email", user.Email.ToString()),
            new Claim("Role", role),
        };
        return claims;
    }

    public string CreateRefreshToken()
    {
        byte[] number = new byte[32];
        using RandomNumberGenerator random = RandomNumberGenerator.Create();
        random.GetBytes(number);
        return Convert.ToBase64String(number);
    }
}
