using System.Collections.Generic;
using Snowdrop.Data.Entites;
using Snowdrop.Data.Entites.Core;

namespace Snowdrop.Data
{
    public class Project : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }

        public virtual User Owner { get; set; }
        public virtual IReadOnlyCollection<ProjectMember> Team { get; set; }
        public virtual IReadOnlyCollection<Wallet> Wallets { get; set; }
    }
}