using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Snowdrop.Auth.Managers.TokenStorage;
using Snowdrop.Auth.Models;
using Snowdrop.Auth.Models.Configurations;

namespace Snowdrop.Auth.Managers.JwtAuthManager
{
    public sealed class JwtAuthManager : IJwtAuthManager
    {
        private readonly JwtConfig m_config = default;
        private readonly ITokenStorage m_tokenStorage = default;
        private readonly byte[] m_secret = default;
        
        public JwtAuthManager(JwtConfig config, ITokenStorage tokenStorage)
        {
            m_config = config;
            m_tokenStorage = tokenStorage;
            m_secret = Encoding.ASCII.GetBytes(m_config.Secret);
        }
        
        public void RemoveRefreshToken(string userName)
        {
            m_tokenStorage.InvalidateToken(userName);
        }

        public async Task<JwtAuthResult> GenerateToken(string userName, Claim[] claims)
        {
            DateTime now = DateTime.UtcNow;
            bool shouldAddAudienceClaim =
                string.IsNullOrEmpty(claims?.FirstOrDefault(p => p.Type == JwtRegisteredClaimNames.Aud)?.Value);

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                m_config.Issuer,
                shouldAddAudienceClaim ? m_config.Audience : string.Empty,
                claims,
                expires: now.AddMilliseconds(m_config.AccessTokenExpirationMs),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(m_secret), SecurityAlgorithms.HmacSha256Signature));

            string accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            RefreshToken refreshToken = new RefreshToken(userName, GenerateRefreshTokenString(),
                now.AddMilliseconds(m_config.RefreshTokenExpirationMs));

            await m_tokenStorage.RememberToken(refreshToken);

            return new JwtAuthResult(accessToken, refreshToken);
        }

        public async Task<JwtAuthResult> RefreshToken(string refreshToken, string accessToken)
        {
            DateTime now = DateTime.UtcNow;

            (ClaimsPrincipal principal, JwtSecurityToken jwtToken) = DecodeJwtToken(accessToken);
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                throw new SecurityTokenException("Invalid params");
            }

            string userName = principal.Identity?.Name;
            RefreshToken token = await m_tokenStorage.GetToken(refreshToken);
            if (!token.UserName.Equals(userName) || token.ExpiresAt < now)
            {
                throw new SecurityTokenException("Invalid params");
            }

            return await GenerateToken(userName, principal.Claims.ToArray());
        }
        
        private (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new SecurityTokenException("Invalid token");
            }
            ClaimsPrincipal principal = new JwtSecurityTokenHandler()
                .ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = m_config.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(m_secret),
                        ValidAudience = m_config.Audience,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30)
                    },
                    out SecurityToken validatedToken);
            return (principal, validatedToken as JwtSecurityToken);
        }
        
        private static string GenerateRefreshTokenString()
        {
            byte[] randomNumber = new byte[32];
            using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}