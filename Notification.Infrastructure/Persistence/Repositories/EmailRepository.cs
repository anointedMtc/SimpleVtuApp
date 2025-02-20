using DomainSharedKernel.Interfaces;
using InfrastructureSharedKernel.SpecificationHelper;
using Microsoft.EntityFrameworkCore;

namespace Notification.Infrastructure.Persistence.Repositories;

public class EmailRepository<T> : IRepository<T> where T : class, IAggregateRoot
{
    private readonly EmailDbContext _emailDbContext;

    public EmailRepository(EmailDbContext emailDbContext )
    {
        _emailDbContext = emailDbContext;
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

        return SpecificationEvaluator<T>.GetQuery(_emailDbContext.Set<T>().AsQueryable(), spec);

    }


    public async Task<T> AddAsync(T entity)
    {
        await _emailDbContext.Set<T>().AddAsync(entity);
        await _emailDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _emailDbContext.Entry(entity).State = EntityState.Modified;
        await _emailDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _emailDbContext.Set<T>().Remove(entity);
        await _emailDbContext.SaveChangesAsync();
    }


    public async Task<T?> GetByIdAsync(Guid id)
    {
        T? result = await _emailDbContext.Set<T>().FindAsync(id);
        return result;
    }


    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _emailDbContext.Set<T>().ToListAsync();
    }

}