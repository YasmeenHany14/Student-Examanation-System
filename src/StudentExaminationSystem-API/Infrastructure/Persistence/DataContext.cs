using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);


        FilterDeletedEntities(modelBuilder);
    }

    private void FilterDeletedEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Subject>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Question>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<QuestionChoice>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<DifficultyProfile>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Notification>().HasQueryFilter(e => !e.IsDeleted);
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionChoice> QuestionChoices { get; set; }
    public DbSet<DifficultyProfile> DifficultyProfiles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<AnswerHistory> AnswerHistories { get; set; }
    public DbSet<GeneratedExam> GeneratedExams { get; set; }
    public DbSet<StudentSubject> StudentSubjects { get; set; }
    public DbSet<SubjectExamConfig> SubjectExamConfigs { get; set; }
    public DbSet<Notification> Notifications { get; set; }
}
