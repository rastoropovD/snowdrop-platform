namespace Snowdrop.Auth.Models.Configurations
{
    public sealed class JwtConfig
    {
        public string Secret { get; init; }
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public int AccessTokenExpirationMs { get; init; }
        public int RefreshTokenExpirationMs { get; init; }

        public JwtConfig()
        {
        }
    }
}