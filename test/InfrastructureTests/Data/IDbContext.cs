using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InfrastructureTests.Data
{
    public interface IDbContext : IDisposable
    {
        List<T> Set<T>();
        //DbSet<T> Set<T>() where T : class;
        EntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
    }
}
