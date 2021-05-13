using System.Threading.Tasks;
using Snowdrop.Auth.Models;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Snowdrop.Auth.Managers.TokenStorage
{
    public sealed class RedisTokenStorage : ITokenStorage
    {
        private readonly IRedisCacheClient m_client = default;

        public RedisTokenStorage(IRedisCacheClient client)
        {
            m_client = client;
        }

        public Task RememberToken(RefreshToken refreshToken)
        {
            return m_client.Db0.AddAsync(refreshToken.Token, refreshToken, refreshToken.ExpiresAt);
        }

        public async void InvalidateToken(string userName)
        {
            await m_client.Db0.RemoveAsync(userName);
        }

        public Task<RefreshToken> GetToken(string userName)
        {
            return m_client.Db0.GetAsync<RefreshToken>(userName);
        }
    }
}