using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SubjectExamConfigConfiguration : IEntityTypeConfiguration<SubjectExamConfig>
{
    public void Configure(EntityTypeBuilder<SubjectExamConfig> builder)
    {
        builder.HasKey(sec => sec.SubjectId);
        builder.HasOne(sec => sec.Subject)
            .WithOne(s => s.SubjectExamConfig)
            .HasForeignKey<SubjectExamConfig>(sec => sec.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
