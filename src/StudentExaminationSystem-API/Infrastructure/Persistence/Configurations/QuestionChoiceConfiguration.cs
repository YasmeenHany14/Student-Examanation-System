using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class QuestionChoiceConfiguration : IEntityTypeConfiguration<QuestionChoice>
{
    public void Configure(EntityTypeBuilder<QuestionChoice> builder)
    {
        builder.HasKey(qc => qc.Id);
        builder.Property(qc => qc.Content)
            .IsRequired()
            .HasMaxLength(30);
        
        builder.HasOne(qc => qc.Question)
            .WithMany(q => q.Choices)
            .HasForeignKey(qc => qc.QuestionId);
        builder.HasMany(qc => qc.AnswerHistories)
            .WithOne(ah => ah.QuestionChoice)
            .HasForeignKey(ah => ah.QuestionChoiceId);
    }
}
