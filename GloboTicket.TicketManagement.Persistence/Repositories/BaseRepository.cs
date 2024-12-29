
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.TicketManagement.Persistence.Repositories;
public class BaseRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : class
{
    protected readonly GloboTicketDbContext _dbContext;

    public BaseRepository(GloboTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public virtual Task<TEntity?> GetByIdAsync(Guid id)
    {
        return _dbContext.Set<TEntity>().FindAsync(id).AsTask();
    }
    public async Task<IReadOnlyList<TEntity>> ListAllAsync()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }
    public async Task UpdateAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }



}
