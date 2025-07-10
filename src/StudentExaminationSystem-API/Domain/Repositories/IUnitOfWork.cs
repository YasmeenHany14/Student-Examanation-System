namespace Domain.Repositories;

public interface IUnitOfWork
{
    IRefreshTokenRepository RefreshTokenRepository { get; }
    IUserRepository UserRepository { get; }
    Task<int> SaveChangesAsync();
}
