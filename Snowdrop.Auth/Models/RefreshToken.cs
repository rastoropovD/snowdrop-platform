using System;

namespace Snowdrop.Auth.Models
{
    public record RefreshToken(string UserName, string Token, DateTime ExpiresAt);
}