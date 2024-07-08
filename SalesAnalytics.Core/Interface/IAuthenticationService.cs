using SalesAnalytics.Core.Entities;

namespace SalesAnalytics.API.Services
{
    public interface IAuthenticationService
    {
        Task<JwtTokenResponse> Authenticate(UserLoginModel loginModel);
        Task<bool> Register(UserLoginModel registrationModel);
    }
}
