using Snowdrop.Data.Enum;

namespace Snowdrop.Infrastructure.Dto
{
    public record ProjectMemberDto(int UserId, int ProjectId, TeamMemberRole Role);
}