using Shared.Domain;

namespace Shared.Persistence.Abstraction;

public interface IWriteRepository<TEntity> : IQuery<TEntity>
    where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity);

    Task AddRangeAsync(ICollection<TEntity> entities);

    Task UpdateAsync(TEntity entity);

    Task UpdateRangeAsync(ICollection<TEntity> entities);

    Task DeleteAsync(TEntity entity);

    Task DeleteRangeAsync(ICollection<TEntity> entities);
}