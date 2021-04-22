using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Snowdrop.Data;
using Snowdrop.Data.Entites;

namespace Snowdrop.DAL.Context
{
    internal sealed class SnowdropContext : DbContext
    {
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        
        public SnowdropContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(SnowdropContext))!);
        }
    }
}