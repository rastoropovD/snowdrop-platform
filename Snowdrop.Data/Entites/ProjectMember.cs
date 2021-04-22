using Snowdrop.Data.Enum;

namespace Snowdrop.Data.Entites
{
    public class ProjectMember
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public TeamMemberRole Role { get; set; }

        public virtual User User { get; set; }
        public virtual Project Project { get; set; }
    }
}