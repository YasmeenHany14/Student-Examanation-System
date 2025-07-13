using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Content)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(q => q.Difficulty)
            .HasConversion<int>();
        
        builder.HasOne(q => q.Subject)
            .WithMany(s => s.Questions)
            .HasForeignKey(q => q.SubjectId);
        
        builder.HasMany(q => q.Choices)
            .WithOne(c => c.Question)
            .HasForeignKey(c => c.QuestionId);
    }
}
