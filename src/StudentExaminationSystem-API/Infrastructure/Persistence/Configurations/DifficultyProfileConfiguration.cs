using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class DifficultyProfileConfiguration : IEntityTypeConfiguration<DifficultyProfile>
{
    public void Configure(EntityTypeBuilder<DifficultyProfile> builder)
    {
        builder.HasKey(dp => dp.Id);

        builder.Property(dp => dp.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}
