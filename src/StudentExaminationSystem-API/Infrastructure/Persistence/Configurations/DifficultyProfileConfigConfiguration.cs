using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class DifficultyProfileConfigConfiguration : IEntityTypeConfiguration<DifficultyProfileConfig>
{
    public void Configure(EntityTypeBuilder<DifficultyProfileConfig> builder)
    {
        builder.HasKey(dpc => new { dpc.DifficultyProfileId, dpc.DifficultyId });
        builder.Property(dpc => dpc.DifficultyPercentage)
            .IsRequired();
        
        builder.HasOne(dpc => dpc.DifficultyProfile)
            .WithMany(dp => dp.DifficultyProfileConfigs)
            .HasForeignKey(dpc => dpc.DifficultyProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(dpc => dpc.Difficulty)
            .WithMany()
            .HasForeignKey(dpc => dpc.DifficultyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
