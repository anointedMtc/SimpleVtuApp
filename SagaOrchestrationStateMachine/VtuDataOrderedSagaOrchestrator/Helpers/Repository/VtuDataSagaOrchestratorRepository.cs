using DomainSharedKernel.Interfaces;
using InfrastructureSharedKernel.SpecificationHelper;
using Microsoft.EntityFrameworkCore;

namespace SagaOrchestrationStateMachines.VtuDataOrderedSagaOrchestrator.Helpers.Repository;

public class VtuDataSagaOrchestratorRepository<T> : IRepository<T> where T : class, IAggregateRoot
{
    private readonly VtuDataOrderedSagaDbContext _vtuDataOrderedSagaDbContext;

    public VtuDataSagaOrchestratorRepository(VtuDataOrderedSagaDbContext vtuDataOrderedSagaDbContext)
    {
        _vtuDataOrderedSagaDbContext = vtuDataOrderedSagaDbContext;
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

        return SpecificationEvaluator<T>.GetQuery(_vtuDataOrderedSagaDbContext.Set<T>().AsQueryable(), spec);

    }


    public async Task<T> AddAsync(T entity)
    {
        await _vtuDataOrderedSagaDbContext.Set<T>().AddAsync(entity);
        await _vtuDataOrderedSagaDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _vtuDataOrderedSagaDbContext.Entry(entity).State = EntityState.Modified;
        await _vtuDataOrderedSagaDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _vtuDataOrderedSagaDbContext.Set<T>().Remove(entity);
        await _vtuDataOrderedSagaDbContext.SaveChangesAsync();
    }


    public async Task<T?> GetByIdAsync(Guid id)
    {
        T? result = await _vtuDataOrderedSagaDbContext.Set<T>().FindAsync(id);
        return result;
    }


    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _vtuDataOrderedSagaDbContext.Set<T>().ToListAsync();
    }

}
