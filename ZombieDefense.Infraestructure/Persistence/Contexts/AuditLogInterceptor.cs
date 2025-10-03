using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using ZombieDefense.Domain.Entities;

namespace ZombieDefense.Infraestructure.Persistence.Contexts;

public class AuditLogInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var auditLogs = new List<AuditLog>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                auditLogs.Add(new AuditLog
                {
                    EliminatedId = entry.Entity is Eliminated eliminated ? eliminated.Id : Guid.Empty,
                    ActionType = "Added",
                    ActionDate = DateTime.UtcNow,
                    CreateAt = entry.Entity.GetType().Name,
                    NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject())
                });
            }
            else if (entry.State == EntityState.Modified)
            {
                auditLogs.Add(new AuditLog
                {
                    EliminatedId = entry.Entity is Eliminated eliminated ? eliminated.Id : Guid.Empty,
                    ActionType = "Modified",
                    ActionDate = DateTime.UtcNow,
                    CreateAt = entry.Entity.GetType().Name,
                    OldValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject()),
                    NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject())
                });
            }
            else if (entry.State == EntityState.Deleted)
            {
                auditLogs.Add(new AuditLog
                {
                    EliminatedId = entry.Entity is Eliminated eliminated ? eliminated.Id : Guid.Empty,
                    ActionType = "Deleted",
                    ActionDate = DateTime.UtcNow,
                    CreateAt = entry.Entity.GetType().Name,
                    OldValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject())
                });
            }
        }

        if (auditLogs.Any())
        {
            context.Set<AuditLog>().AddRange(auditLogs);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
