using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(DataContext context)
    : BaseRepository<RefreshToken>(context), IRefreshTokenRepository
{
    public async Task<bool> CheckTokenExists(string token)
    {
        return await context.RefreshTokens.AnyAsync(x => x.Token == token);
    }

    public async Task<RefreshToken?> CheckTokenExistsByUserId(string token, string userId)
    {
        return await context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == token && x.UserId == userId);
    }

    public async Task<bool> RevokeToken(string token)
    {
        var result = await context.RefreshTokens
            .Where(x => x.Token == token)
            .ExecuteUpdateAsync(x => x.SetProperty(r => r.IsRevoked, true));
        return result > 0;
    }

    public void ReplaceToken(RefreshToken refreshToken, string newRefreshToken, DateTime newRefreshExpiresAt)
    {
        refreshToken.Token = newRefreshToken;
        refreshToken.ExpiryDate = newRefreshExpiresAt;
        refreshToken.IsRevoked = false;

        context.RefreshTokens.Update(refreshToken);
    }
}
