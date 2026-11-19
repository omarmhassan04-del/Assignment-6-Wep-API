using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using LibrarySystemApi.Entities;

namespace LibrarySystemApi.Data.Interceptors
{
    public class CreatedAtInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ApplyCreatedAt(eventData);
            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            ApplyCreatedAt(eventData);
            return ValueTask.FromResult(result);
        }

        private static void ApplyCreatedAt(DbContextEventData eventData)
        {
            if (eventData.Context is null)
                return;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is null || entry.State != EntityState.Added)
                    continue;

                if (entry.Entity is BaseEntity entity)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.ModifiedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is Loan loan)
                {
                    loan.CreatedAt = DateTime.UtcNow;
                    loan.ModifiedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
