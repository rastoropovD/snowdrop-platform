namespace Snowdrop.Auth.Models
{
    public record JwtAuthResult(string AccessToken, RefreshToken RefreshToken);
}