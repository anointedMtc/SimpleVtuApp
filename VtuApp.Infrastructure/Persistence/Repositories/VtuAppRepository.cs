using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Interfaces;
using SharedKernel.Infrastructure.SpecificationHelper;
using VtuApp.Domain.Interfaces;

namespace VtuApp.Infrastructure.Persistence.Repositories;

public class VtuAppRepository<T> : IVtuAppRepository<T> where T : class, IAggregateRoot
{
    private readonly VtuDbContext _vtuDbContext;

    public VtuAppRepository(VtuDbContext vtuDbContext)
    {
        _vtuDbContext = vtuDbContext;
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

        return SpecificationEvaluator<T>.GetQuery(_vtuDbContext.Set<T>().AsQueryable(), spec);

    }


    public async Task<T> AddAsync(T entity)
    {
        await _vtuDbContext.Set<T>().AddAsync(entity);
        await _vtuDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _vtuDbContext.Entry(entity).State = EntityState.Modified;
        await _vtuDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _vtuDbContext.Set<T>().Remove(entity);
        await _vtuDbContext.SaveChangesAsync();
    }


    public async Task<T?> GetByIdAsync(Guid id)
    {
        T? result = await _vtuDbContext.Set<T>().FindAsync(id);
        return result;
    }


    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _vtuDbContext.Set<T>().ToListAsync();
    }

}