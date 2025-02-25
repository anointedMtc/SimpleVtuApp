using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Interfaces;
using SharedKernel.Infrastructure.SpecificationHelper;

namespace Wallet.Infrastructure.Persistence.Repositories;

public class WalletRepository<T> : IRepository<T> where T : class, IAggregateRoot
{
    private readonly WalletDbContext _walletDbContext;

    public WalletRepository(WalletDbContext walletDbContext)
    {
        _walletDbContext = walletDbContext;
    }


    public async Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification = null)
    {
        return ApplySpecification(specification);
    }


    public async Task<T?> FindAsync(ISpecification<T> specification = null)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {

        return SpecificationEvaluator<T>.GetQuery(_walletDbContext.Set<T>().AsQueryable(), spec);

    }


    public async Task<T> AddAsync(T entity)
    {
        await _walletDbContext.Set<T>().AddAsync(entity);
        await _walletDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _walletDbContext.Entry(entity).State = EntityState.Modified;
        await _walletDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _walletDbContext.Set<T>().Remove(entity);
        await _walletDbContext.SaveChangesAsync();
    }


    public async Task<T?> GetByIdAsync(Guid id)
    {
        T? result = await _walletDbContext.Set<T>().FindAsync(id);
        return result;
    }


    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _walletDbContext.Set<T>().ToListAsync();
    }

}
