using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snowdrop.Auth.Models;

namespace Snowdrop.Auth.Managers.TokenStorage
{
    public sealed class MemoryTokenStorage : ITokenStorage
    {
        private readonly ConcurrentDictionary<string, RefreshToken> m_refreshTokens = default;

        public MemoryTokenStorage()
        {
            m_refreshTokens = new ConcurrentDictionary<string, RefreshToken>();
        }
        
        public Task RememberToken(RefreshToken refreshToken)
        {
            return Task.FromResult(
                m_refreshTokens.AddOrUpdate(refreshToken.Token, refreshToken, (_, _) => refreshToken));
        }

        public void InvalidateToken(string userName)
        {
            IEnumerable<KeyValuePair<string, RefreshToken>> tokens = m_refreshTokens
                .Where(p => p.Value.UserName.Equals(userName));

            foreach (KeyValuePair<string,RefreshToken> pair in tokens)
            {
                m_refreshTokens.TryRemove(pair.Key, out _);
            }
        }

        public Task<RefreshToken> GetToken(string refreshToken)
        {
            m_refreshTokens.TryGetValue(refreshToken, out RefreshToken token);
            return Task.FromResult(token);
        }
    }
}