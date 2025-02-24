using DomainSharedKernel.Interfaces;
using InfrastructureSharedKernel.SpecificationHelper;
using Microsoft.EntityFrameworkCore;
using SagaOrchestrationStateMachines.Domain.Interfaces;

namespace SagaOrchestrationStateMachines.Infrastructure.Persistence.Repository;

internal sealed class SagaStateMachineRepository<T> : ISagaStateMachineRepository<T> where T : class
{
    private readonly SagaStateMachineDbContext _sagaStateMachineDbContext;

    public SagaStateMachineRepository(SagaStateMachineDbContext sagaStateMachineDbContext)
    {
        _sagaStateMachineDbContext = sagaStateMachineDbContext;
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

        return SpecificationEvaluator<T>.GetQuery(_sagaStateMachineDbContext.Set<T>().AsQueryable(), spec);

    }


    public async Task<T> AddAsync(T entity)
    {
        await _sagaStateMachineDbContext.Set<T>().AddAsync(entity);
        await _sagaStateMachineDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _sagaStateMachineDbContext.Entry(entity).State = EntityState.Modified;
        await _sagaStateMachineDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _sagaStateMachineDbContext.Set<T>().Remove(entity);
        await _sagaStateMachineDbContext.SaveChangesAsync();
    }


    public async Task<T?> GetByIdAsync(Guid id)
    {
        T? result = await _sagaStateMachineDbContext.Set<T>().FindAsync(id);
        return result;
    }


    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _sagaStateMachineDbContext.Set<T>().ToListAsync();
    }

}
