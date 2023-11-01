using BSYS.Domain.Entities.Identity;

namespace BSYS.Application.Abstractions.Token;

public interface ITokenHandler
{
    Task<DTOs.Token> CreateAccessToken(int second, AppUser appUser);
    string CreateRefreshToken();
}
