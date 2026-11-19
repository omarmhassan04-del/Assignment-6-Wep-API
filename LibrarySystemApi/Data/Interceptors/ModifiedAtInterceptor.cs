using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using LibrarySystemApi.Entities;

namespace LibrarySystemApi.Data.Interceptors
{
    public class ModifiedAtInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ApplyModifiedAt(eventData);
            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            ApplyModifiedAt(eventData);
            return ValueTask.FromResult(result);
        }

        private static void ApplyModifiedAt(DbContextEventData eventData)
        {
            if (eventData.Context is null)
                return;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is null || entry.State != EntityState.Modified)
                    continue;

                if (entry.Entity is BaseEntity entity)
                {
                    entity.ModifiedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is Loan loan)
                {
                    loan.ModifiedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
