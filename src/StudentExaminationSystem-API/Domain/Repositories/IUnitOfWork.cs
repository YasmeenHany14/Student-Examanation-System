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
    Task<int> SaveChangesAsync();
    IBaseRepository<TEntity> GetRepository<TEntity>()
        where TEntity : BaseEntity;
}
