using System.Collections.Concurrent;

namespace WebApi.Services;

public interface IConnectionHelper
{
    void AddUserToGroup(string userId, string groupName);
    void RemoveUserFromGroup(string userId, string groupName);
    bool CheckUserInGroup(string userId, string groupName);
    
}

public class ConnectionHelper : IConnectionHelper
{
    private readonly ConcurrentDictionary<string, HashSet<string>> _groups = new();

    public void AddUserToGroup(string groupName, string userId)
    {
        _groups.AddOrUpdate(groupName, new HashSet<string> { userId },
            (key, existingUsers) =>
            {
                existingUsers.Add(userId);
                return existingUsers;
            }
        );
    }

    public void RemoveUserFromGroup(string groupName, string userId)
    {
        _groups.TryGetValue(groupName, out var existingGroup);
        if (existingGroup != null)
            existingGroup.Remove(userId);
    }

    public bool CheckUserInGroup(string groupName, string userId)
    {
        _groups.TryGetValue(groupName, out var existingGroup);
        if (existingGroup == null)
            return false;
        return existingGroup.Contains(userId);
    }
}
