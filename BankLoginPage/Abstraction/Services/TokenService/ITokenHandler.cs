using BankLoginPage.DTOs;
using BankLoginPage.Models.IdentityEntities;

namespace BankLoginPage.Abstraction.Services.TokenService
{
    public interface ITokenHandler
    {
        Task<TokenDTO> CreateAccessTokenAsync(int minute, AppUser user);
        string CreateRefreshToken();
    }
}
