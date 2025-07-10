using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(s => s.Code)
            .IsRequired()
            .HasMaxLength(5);
        
        builder.HasMany(s => s.Questions)
            .WithOne(q => q.Subject)
            .HasForeignKey(q => q.SubjectId);
    }
}

