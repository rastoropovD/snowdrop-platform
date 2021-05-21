using System.Security.Claims;

namespace Snowdrop.Infrastructure.Dto.Users
{
    public record UserResponse(string UserName, Claim[] Claims);
}