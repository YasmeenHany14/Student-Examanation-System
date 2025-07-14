using Application.Common.ErrorAndResults;

namespace Application.Contracts;

public interface IUserService
{
    Task<Result<bool>> ToggleStatusAsync(string id);
}