using Domain.Models.Common;
using Domain.Repositories;

namespace Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dbContext;
    public IRefreshTokenRepository RefreshTokenRepository { get; }
    public IUserRepository UserRepository { get; }
    public IStudentRepository StudentRepository { get; }
    public ISubjectRepository SubjectRepository { get; set; }
    public IQuestionRepository QuestionRepository { get; set; }
    public ISubjectExamConfigRepository SubjectExamConfigRepository { get; set; }
    public IDifficultyProfileRepository DifficultyProfileRepository { get; set; }
    public IExamRepository ExamHistoryRepository { get; set; }
    public IUserExtensionsRepository UserExtensionsRepository { get; set; }

    public INotificationsRepository NotificationsRepository { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
    
    public UnitOfWork(
        DataContext dbContext,
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IStudentRepository studentRepository, 
        ISubjectRepository subjectRepository,
        IQuestionRepository questionRepository,
        ISubjectExamConfigRepository subjectExamConfigRepository,
        IDifficultyProfileRepository difficultyProfileRepository,
        IExamRepository examHistoryRepository,
        INotificationsRepository notificationRepository,
        IUserExtensionsRepository userExtensionsRepository)
    {
        _dbContext = dbContext;
        RefreshTokenRepository = refreshTokenRepository;
        UserRepository = userRepository;
        StudentRepository = studentRepository;
        SubjectRepository = subjectRepository;
        QuestionRepository = questionRepository;
        SubjectExamConfigRepository = subjectExamConfigRepository;
        DifficultyProfileRepository = difficultyProfileRepository;
        ExamHistoryRepository = examHistoryRepository;
        NotificationsRepository = notificationRepository;
        UserExtensionsRepository = userExtensionsRepository;
    }
    
    public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
    {
        return new BaseRepository<TEntity>(_dbContext);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual async void Dispose(bool disposing)
    {
        if (disposing)
        {
            await _dbContext.DisposeAsync();
        }
    }
}
