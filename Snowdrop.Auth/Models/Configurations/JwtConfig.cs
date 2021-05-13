namespace Snowdrop.Auth.Models.Configurations
{
    public record JwtConfig(string Secret, string Issuer, string Audience, int AccessTokenExpirationMs, int RefreshTokenExpirationMs);
}