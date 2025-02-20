using DomainSharedKernel.Interfaces;
using InfrastructureSharedKernel.SpecificationHelper;
using Microsoft.EntityFrameworkCore;

namespace SagaOrchestrationStateMachines.VtuAirtimeOrderedSagaOrchestrator.Helpers.Repository;

public class VtuAirtimeSagaOrchestratorRepository<T> : IRepository<T> where T : class, IAggregateRoot
{
    private readonly VtuAirtimeOrderedSagaDbContext _vtuAirtimeOrderedSagaDbContext;

    public VtuAirtimeSagaOrchestratorRepository(VtuAirtimeOrderedSagaDbContext vtuAirtimeOrderedSagaDbContext)
    {
        _vtuAirtimeOrderedSagaDbContext = vtuAirtimeOrderedSagaDbContext;
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

        return SpecificationEvaluator<T>.GetQuery(_vtuAirtimeOrderedSagaDbContext.Set<T>().AsQueryable(), spec);

    }


    public async Task<T> AddAsync(T entity)
    {
        await _vtuAirtimeOrderedSagaDbContext.Set<T>().AddAsync(entity);
        await _vtuAirtimeOrderedSagaDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _vtuAirtimeOrderedSagaDbContext.Entry(entity).State = EntityState.Modified;
        await _vtuAirtimeOrderedSagaDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _vtuAirtimeOrderedSagaDbContext.Set<T>().Remove(entity);
        await _vtuAirtimeOrderedSagaDbContext.SaveChangesAsync();
    }


    public async Task<T?> GetByIdAsync(Guid id)
    {
        T? result = await _vtuAirtimeOrderedSagaDbContext.Set<T>().FindAsync(id);
        return result;
    }


    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _vtuAirtimeOrderedSagaDbContext.Set<T>().ToListAsync();
    }

}
