using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Snowdrop.Data.Entites.Core;

namespace Snowdrop.DAL.Configurations.Core
{
    internal abstract class BaseEntityTypeConfiguration<TEntity> : BaseTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        protected override Expression<Func<TEntity, object>> Key => entity => entity.Id;
        protected override void ConfigureEntity(EntityTypeBuilder<TEntity> builder)
        {
            builder
                .Property(entity => entity.CreatedAt)
                .HasColumnType("Date")
                .HasDefaultValueSql("GetUtcDate()")
                .IsRequired();
            
            builder
                .Property(entity => entity.ModifiedAt)
                .HasColumnType("Date")
                .HasDefaultValueSql("GetUtcDate()")
                .IsRequired();
        }
    }
}