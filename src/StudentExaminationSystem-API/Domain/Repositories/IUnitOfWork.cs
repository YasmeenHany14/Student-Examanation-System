using Domain.Models.Common;

namespace Domain.Repositories;

public interface IUnitOfWork
{
    IRefreshTokenRepository RefreshTokenRepository { get; }
    IUserRepository UserRepository { get; }
    IStudentRepository StudentRepository { get; }
    ISubjectRepository SubjectRepository { get; set; }
    IQuestionRepository QuestionRepository { get; set; }
    ISubjectExamConfigRepository SubjectExamConfigRepository { get; set; }
    IDifficultyProfileRepository DifficultyProfileRepository { get; set; }
    IExamRepository ExamHistoryRepository { get; set; }
    INotificationsRepository NotificationsRepository { get; }
    Task<int> SaveChangesAsync();
    IBaseRepository<TEntity> GetRepository<TEntity>()
        where TEntity : BaseEntity;
}
