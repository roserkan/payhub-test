using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Domain;
using Shared.Persistence.Abstraction;
using Shared.Utils.Pagination;

namespace Shared.Persistence.EfCore;

public class RepositoryBase<TEntity, TContext> : IReadRepository<TEntity>, IWriteRepository<TEntity> 
    where TEntity: BaseEntity
    where TContext: DbContext
{
    private readonly TContext _context;
    
    public RepositoryBase(TContext context)
    {
        _context = context;
    }

    public IQueryable<TEntity> Query()
    {
        return _context.Set<TEntity>();
    }
    
    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        var query = Query();

        if (!enableTracking)
            query = query.AsNoTracking();
        
        // Apply filtering
        query = query.Where(predicate);

        // Apply includes for related entities
        if (include != null)
            query = include(query);

        // Eğer selector yoksa TEntity'yi TResult'a cast et
        return await query.FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<TResult?> GetWithSelectorAsync<TResult>(Expression<Func<TEntity, bool>> predicate, 
        Expression<Func<TEntity, TResult>> selector, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool enableTracking = false, CancellationToken cancellationToken = default)
    {
        var query = Query();

        if (!enableTracking)
            query = query.AsNoTracking();
        
        // Apply filtering
        query = query.Where(predicate);

        // Apply includes for related entities
        if (include != null)
            query = include(query);

        return await query.Select(selector).FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        var query = Query();

        if (!enableTracking)
            query = query.AsNoTracking();
        
        // Apply filtering
        if (predicate != null)
            query = query.Where(predicate);

        // Apply includes for related entities
        if (include != null)
            query = include(query);
        
        // Apply sorting
        if (orderBy != null)
            query = orderBy(query);

        return await query.ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<TResult>> GetAllWithSelectorAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = false, CancellationToken cancellationToken = default)
    {
        var query = Query();

        if (!enableTracking)
            query = query.AsNoTracking();
        
        // Apply filtering
        if (predicate != null)
            query = query.Where(predicate);

        // Apply includes for related entities
        if (include != null)
            query = include(query);
        
        // Apply sorting
        if (orderBy != null)
            query = orderBy(query);

        return await query.Select(selector).ToListAsync(cancellationToken);
    }

    
    public async Task<IPaginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0,
        int size = 10, bool isAll = false, bool enableTracking = false, CancellationToken cancellationToken = default)
    {
        var query = Query();

        if (!enableTracking)
            query = query.AsNoTracking();
        
        // Apply filtering
        if (predicate != null)
            query = query.Where(predicate);

        // Apply includes for related entities
        if (include != null)
            query = include(query);
        
        // Apply sorting
        if (orderBy != null)
            query = orderBy(query);

        return await query.ToPaginateAsync(index, size, 0, cancellationToken);
    }

    public async Task<IPaginate<TResult>> GetListWithSelectorAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool isAll = false, bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        var query = Query();

        if (!enableTracking)
            query = query.AsNoTracking();
        
        // Apply filtering
        if (predicate != null)
            query = query.Where(predicate);

        // Apply includes for related entities
        if (include != null)
            query = include(query);
        
        // Apply sorting
        if (orderBy != null)
            query = orderBy(query);

        return await query.Select(selector).ToPaginateAsync(index, size, 0, cancellationToken);
    }

  

    public async Task AddAsync(TEntity entity)
    {
        entity.CreatedDate = DateTime.Now;
        await _context.AddAsync(entity);
    }

    public async Task AddRangeAsync(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.CreatedDate = DateTime.Now;
        await _context.AddRangeAsync(entities);
    }

    public Task UpdateAsync(TEntity entity)
    {
        entity.UpdatedDate = DateTime.Now;
        _context.Update(entity);
        return Task.CompletedTask; 
    }

    public Task UpdateRangeAsync(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.UpdatedDate = DateTime.Now;
        _context.UpdateRange(entities);
        return Task.CompletedTask; 
    }

    public Task DeleteAsync(TEntity entity)
    {
        _context.Remove(entity);
        return Task.CompletedTask; 
    }

    public Task DeleteRangeAsync(ICollection<TEntity> entities)
    {
        _context.RemoveRange(entities);
        return Task.CompletedTask; 
    }
    
   

    

    

    
   

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, bool enableTracking = false, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.AnyAsync(cancellationToken);
    }
}