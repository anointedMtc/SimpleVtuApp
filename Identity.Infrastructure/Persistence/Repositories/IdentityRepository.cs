using DomainSharedKernel.Interfaces;
using InfrastructureSharedKernel.SpecificationHelper;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Repositories;

public class IdentityRepository<T> : IRepository<T> where T : class, IAggregateRoot
{
    private readonly ApplicationDbContext _applicationDbContext; 

    public IdentityRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }


    public async Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification = null)
    {
        // because I know i won't change the entites gotten from this...
        return ApplySpecification(specification).AsNoTracking();
    }


    public async Task<T?> FindAsync(ISpecification<T> specification = null)
    {
        // we may change the entity gotten from this... so no need to add AsNoTracking... besides, it's just one
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> specification)
    {
        // because I know i won't change the entites gotten from this...
        return await ApplySpecification(specification).AsNoTracking().CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        // it is possible to add the AsNoTracking here, but then that means it would also affect the FindAsyn() method... which we don't want
        return SpecificationEvaluator<T>.GetQuery(_applicationDbContext.Set<T>().AsQueryable(), spec);

    }


    public async Task<T> AddAsync(T entity)
    {
        await _applicationDbContext.Set<T>().AddAsync(entity);
        await _applicationDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _applicationDbContext.Entry(entity).State = EntityState.Modified;
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _applicationDbContext.Set<T>().Remove(entity);
        await _applicationDbContext.SaveChangesAsync();
    }


    public async Task<T?> GetByIdAsync(Guid id)
    {
        T? result = await _applicationDbContext.Set<T>().FindAsync(id);
        return result;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _applicationDbContext.Set<T>().AsNoTracking().ToListAsync();
    }

}
