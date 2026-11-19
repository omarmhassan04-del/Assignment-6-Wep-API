using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using LibrarySystemApi.Entities;

namespace LibrarySystemApi.Data.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ApplySoftDelete(eventData);
            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            ApplySoftDelete(eventData);
            return ValueTask.FromResult(result);
        }

        private static void ApplySoftDelete(DbContextEventData eventData)
        {
            if (eventData.Context is null)
                return;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is null || entry.State != EntityState.Deleted)
                    continue;

                if (entry.Entity is BaseEntity entity)
                {
                    entry.State = EntityState.Modified;
                    entity.IsDeleted = true;
                    entity.ModifiedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is Loan loan)
                {
                    entry.State = EntityState.Modified;
                    loan.IsDeleted = true;
                    loan.ModifiedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
