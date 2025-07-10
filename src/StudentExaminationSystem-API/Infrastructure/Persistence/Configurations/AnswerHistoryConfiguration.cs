using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AnswerHistoryConfiguration : IEntityTypeConfiguration<AnswerHistory>
{
    public void Configure(EntityTypeBuilder<AnswerHistory> builder)
    {
        builder.HasKey(ah => new { ah.GeneratedExamId, ah.QuestionId});
        builder.HasOne(ah => ah.Question)
            .WithMany(q => q.AnswerHistories)
            .HasForeignKey(ah => ah.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(ah => ah.QuestionChoice)
            .WithMany(qc => qc.AnswerHistories)
            .HasForeignKey(ah => ah.QuestionChoiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
