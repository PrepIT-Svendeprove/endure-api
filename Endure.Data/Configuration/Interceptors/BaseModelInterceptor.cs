using Endure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Endure.Data.Configuration.Interceptors;

internal class BaseModelInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void ApplyAuditFields(DbContext? context)
    {
        if (context is null) return;

        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        foreach (var entry in context.ChangeTracker.Entries<BaseModel>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.UpdatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                // Dont save changes, if the CreatedAt property were changed for some reason.
                entry.Property(x => x.CreatedAt).IsModified = false;

                var hasRealChanges = entry.Properties.Any(p =>
                    p.IsModified &&
                    p.Metadata.Name != nameof(BaseModel.CreatedAt) &&
                    p.Metadata.Name != nameof(BaseModel.UpdatedAt));

                if (hasRealChanges)
                    entry.Entity.UpdatedAt = now;
            }
        }
    }
}
