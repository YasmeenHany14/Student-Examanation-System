using Domain.UserContext;

namespace Application.Helpers;

public static class AccessResourceIdFilter
{
    public static TKey? FilterResourceId<TKey>(IUserContext userContext)
    {
        var isAuthenticated = userContext.IsAuthenticated;
        if(!isAuthenticated)
            return default;
        
        var userId = userContext.UserId.ToString();
        var isAdmin = userContext.IsAdmin;

        if (isAdmin)
            return default;

        return (TKey)Convert.ChangeType(userId, typeof(TKey));
    }

    public static bool IsAdminOrCanAccess(string userId, IUserContext userContext)
    {
        //TODO: REMOVE THE IS AUTHENTICATED CHECK LATER
        var isAuthenticated = userContext.IsAuthenticated;
        if (!isAuthenticated)
            return true;

        var isAdmin = userContext.IsAdmin;
        if (isAdmin)
            return true;

        return userId == userContext.UserId.ToString();
    }
}
