using Snowdrop.Data.Entites.Core;
using Snowdrop.Data.Enum;

namespace Snowdrop.Data.Entites
{
    public class Balance : BaseEntity
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public virtual User User { get; set; }
    }
}