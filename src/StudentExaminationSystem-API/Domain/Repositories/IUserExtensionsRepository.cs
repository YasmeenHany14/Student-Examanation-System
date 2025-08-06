namespace Domain.Repositories;

public interface IUserExtensionsRepository
{
    Task<IEnumerable<string>> GetAdminUserIdsAsync();
}
