using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class StudentSubjectConfiguration : IEntityTypeConfiguration<StudentSubject>
{
    public void Configure(EntityTypeBuilder<StudentSubject> builder)
    {
        builder.HasKey(ss => ss.Id);

        builder.HasIndex(ss => new { ss.StudentId, ss.SubjectId });
        
        builder.HasOne(ss => ss.Student)
            .WithMany(s => s.StudentSubjects)
            .HasForeignKey(ss => ss.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(ss => ss.Subject)
            .WithMany()
            .HasForeignKey(ss => ss.SubjectId);
    }
}
