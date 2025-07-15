using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Domain.Repositories;

namespace Application.Services;

public class UserService(
    IUnitOfWork unitOfWork
    ) : IUserService
{
    public async Task<Result<bool>> ToggleStatusAsync(string id)
    {
        var user = await unitOfWork.UserRepository.FindByIdAsync(id);
        if (user == null)
            return Result<bool>.Failure(CommonErrors.NotFound());
        
        user.IsActive = !user.IsActive;
        var result = await unitOfWork.UserRepository.UpdateAsync(user);
        if (!result.Succeeded)
            return Result<bool>.Failure(CommonErrors.InternalServerError());
        return Result<bool>.Success(user.IsActive);
    }
}
