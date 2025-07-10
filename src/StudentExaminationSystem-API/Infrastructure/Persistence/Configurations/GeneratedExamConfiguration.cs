using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class GeneratedExamConfiguration : IEntityTypeConfiguration<GeneratedExam>
{
    public void Configure(EntityTypeBuilder<GeneratedExam> builder)
    {
        builder.HasKey(ge => ge.Id);
        builder.HasOne(ge => ge.Student)
            .WithMany(s => s.GeneratedExams)
            .HasForeignKey(ge => ge.StudentId);
        builder.HasOne(ge => ge.Subject)
            .WithMany(s => s.GeneratedExams)
            .HasForeignKey(ge => ge.SubjectId);

        // The current configuration already sets up the relationships between GeneratedExam, Student, and Subject via foreign keys.
        // No further configuration is needed for a 3-way relationship as EF Core will handle the navigation properties as defined in the models.
        // If you want to enforce uniqueness for the combination of StudentId and SubjectId, you can add a unique index:
        builder.HasIndex(ge => new { ge.StudentId, ge.SubjectId }).IsUnique(true); // Set to true if you want uniqueness
    }
}
