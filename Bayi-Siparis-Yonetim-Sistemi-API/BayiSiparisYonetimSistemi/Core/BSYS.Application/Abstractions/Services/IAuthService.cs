using BSYS.Application.Abstractions.Services.Authentications;

namespace BSYS.Application.Abstractions.Services;

public interface IAuthService : IExternalAuthentication, IInternalAuthentication
{
    Task PasswordResetAsnyc(string email);
    Task<bool> VerifyResetTokenAsync(string resetToken, string userId);
}
