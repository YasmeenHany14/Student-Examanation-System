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
        IQuestionRepository questionRepository)
    {
        _dbContext = dbContext;
        RefreshTokenRepository = refreshTokenRepository;
        UserRepository = userRepository;
        StudentRepository = studentRepository;
        SubjectRepository = subjectRepository;
        QuestionRepository = questionRepository;
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
