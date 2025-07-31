using Domain.Models;
using Domain.Models.Common;
using Domain.UserContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;
public sealed class UpdateAuditFieldsInterceptor(IUserContext userContext) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;
        if (dbContext is null)
        {
            return base.SavingChangesAsync(
                eventData,
                result,
                cancellationToken);
        }
        
        IEnumerable<EntityEntry<BaseEntity>> entries = dbContext
            .ChangeTracker
            .Entries<BaseEntity>();
        
        var isLoggedIn = userContext.IsAuthenticated;
        Guid? currentUserId = isLoggedIn ? userContext.UserId : null;
        
        foreach (var entityEntry in entries)
        {
            SetAuditFields(entityEntry, currentUserId.ToString());
        }
        return base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken);
    }

    void SetAuditFields(EntityEntry<BaseEntity> entityEntry,string? currentUserId)
    {
        if (entityEntry.State == EntityState.Added)
        {
            entityEntry.Property(e => e.CreatedAt).CurrentValue = DateTime.Now;
            entityEntry.Property(e => e.UpdatedAt).CurrentValue = DateTime.Now;
            if (entityEntry.Entity is not IAuditDateOnly)
            {
                entityEntry.Property(e => e.CreatedBy).CurrentValue = currentUserId;
                entityEntry.Property(e => e.UpdatedBy).CurrentValue = currentUserId;
            }
        }
        if (entityEntry.State == EntityState.Modified)
        {
            entityEntry.Property(e => e.UpdatedAt).CurrentValue = DateTime.Now;
            entityEntry.Property(e => e.CreatedAt).IsModified = false;
            entityEntry.Property(e => e.CreatedBy).IsModified = false;
            entityEntry.Property(e => e.UpdatedBy).CurrentValue = currentUserId;
        }
        if (entityEntry.State == EntityState.Deleted && entityEntry is not IHardDelete)
        {
            entityEntry.Property(e => e.IsDeleted).CurrentValue = true;
            entityEntry.State = EntityState.Modified;
            entityEntry.Property(e => e.UpdatedAt).CurrentValue = DateTime.Now;
            entityEntry.Property(e => e.DeletedBy).CurrentValue = currentUserId;
        }
    }
}
