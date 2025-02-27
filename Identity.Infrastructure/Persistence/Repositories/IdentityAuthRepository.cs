using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Interfaces;
using SharedKernel.Infrastructure.SpecificationHelper;

namespace Identity.Infrastructure.Persistence.Repositories;

public class IdentityAuthRepository<T> : IRepository<T> where T : class, IAggregateRoot
{
    private readonly IdentityAuthDbContext _identityAuthDbContext; 

    public IdentityAuthRepository(IdentityAuthDbContext identityAuthDbContext)
    {
        _identityAuthDbContext = identityAuthDbContext;
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
        return SpecificationEvaluator<T>.GetQuery(_identityAuthDbContext.Set<T>().AsQueryable(), spec);
    }


    public async Task<T> AddAsync(T entity)
    {
        await _identityAuthDbContext.Set<T>().AddAsync(entity);
        await _identityAuthDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _identityAuthDbContext.Entry(entity).State = EntityState.Modified;
        await _identityAuthDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _identityAuthDbContext.Set<T>().Remove(entity);
        await _identityAuthDbContext.SaveChangesAsync();
    }


    public async Task<T?> GetByIdAsync(Guid id)
    {
        T? result = await _identityAuthDbContext.Set<T>().FindAsync(id);
        return result;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _identityAuthDbContext.Set<T>().ToListAsync();
    }

}
