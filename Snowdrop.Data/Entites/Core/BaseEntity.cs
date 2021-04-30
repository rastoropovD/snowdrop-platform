using System;
using System.ComponentModel.DataAnnotations;

namespace Snowdrop.Data.Entites.Core
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}