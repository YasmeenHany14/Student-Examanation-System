using Domain.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SoftDeleteConfiguration<T> : IEntityTypeConfiguration<T> where T : class, ISoftDelete
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(e => e.DeletedBy).HasMaxLength(30);
    }
}
