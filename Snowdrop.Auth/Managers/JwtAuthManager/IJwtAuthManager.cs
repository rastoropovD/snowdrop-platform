using System.Security.Claims;
using System.Threading.Tasks;
using Snowdrop.Auth.Models;

namespace Snowdrop.Auth.Managers.JwtAuthManager
{
    public interface IJwtAuthManager
    {
        void RemoveRefreshToken(string userName);
        Task<JwtAuthResult> GenerateToken(string userName, Claim[] claims);
        Task<JwtAuthResult> RefreshToken(string refreshToken, string accessToken);
    }
}