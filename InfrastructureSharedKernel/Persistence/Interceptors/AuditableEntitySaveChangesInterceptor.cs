using DomainSharedKernel;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ApplicationSharedKernel.Interfaces;

namespace InfrastructureSharedKernel.Persistence.Interceptors;


public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IUserContext _userContext;
    private readonly IDateTimeService _dateTimeService;

    public AuditableEntitySaveChangesInterceptor(IUserContext userContext, 
        IDateTimeService dateTimeService)
    {
        _userContext = userContext;
        _dateTimeService = dateTimeService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _userContext.GetCurrentUser()?.Id;
                entry.Entity.Created = _dateTimeService.UtcNow;
                continue;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedBy = _userContext.GetCurrentUser()?.Id;
                entry.Entity.LastModified = _dateTimeService.UtcNow;
                continue;
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.Entity.DeletedBy = _userContext.GetCurrentUser()?.Id;
                entry.Entity.Deleted = _dateTimeService.UtcNow;
                entry.State = EntityState.Modified;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified)
        );
}
