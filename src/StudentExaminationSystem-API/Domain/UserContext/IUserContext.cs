namespace Domain.UserContext;

public interface IUserContext
{
    Guid UserId { get; }

    bool IsAuthenticated { get; }
    bool IsAdmin { get; }
}
