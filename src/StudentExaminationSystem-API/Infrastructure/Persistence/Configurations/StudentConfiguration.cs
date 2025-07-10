using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.HasOne(s => s.User)
            .WithOne()
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(s => s.GeneratedExams)
            .WithOne(e => e.Student)
            .HasForeignKey(e => e.StudentId);
        builder.HasMany(s => s.StudentSubjects)
            .WithOne(ss => ss.Student)
            .HasForeignKey(ss => ss.StudentId);
    }
}
