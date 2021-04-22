using Snowdrop.Data.Entites.Core;
using Snowdrop.Data.Enum;

namespace Snowdrop.Data.Entites
{
    public class Wallet : BaseEntity
    {
        public int ProjectId { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public virtual Project Project { get; set; }
    }
}