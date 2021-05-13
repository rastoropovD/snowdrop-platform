using System.Threading.Tasks;
using Snowdrop.Auth.Models;

namespace Snowdrop.Auth.Managers.TokenStorage
{
    public interface ITokenStorage
    {
        Task RememberToken(RefreshToken refreshToken);
        void InvalidateToken(string userName);
        Task<RefreshToken> GetToken(string refreshToken);
    }
}