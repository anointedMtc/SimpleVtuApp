using DomainSharedKernel.Interfaces;
using InfrastructureSharedKernel.SpecificationHelper;
using Microsoft.EntityFrameworkCore;

namespace SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator.Helpers.Repository;

public class UserCreatedSagaOrchestratorRepository<T> : IRepository<T> where T : class, IAggregateRoot
{
    private readonly UserCreatedSagaDbContext _userCreatedSagaDbContext;

    public UserCreatedSagaOrchestratorRepository(UserCreatedSagaDbContext userCreatedSagaDbContext)
    {
        _userCreatedSagaDbContext = userCreatedSagaDbContext;
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

        return SpecificationEvaluator<T>.GetQuery(_userCreatedSagaDbContext.Set<T>().AsQueryable(), spec);

    }


    public async Task<T> AddAsync(T entity)
    {
        await _userCreatedSagaDbContext.Set<T>().AddAsync(entity);
        await _userCreatedSagaDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _userCreatedSagaDbContext.Entry(entity).State = EntityState.Modified;
        await _userCreatedSagaDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _userCreatedSagaDbContext.Set<T>().Remove(entity);
        await _userCreatedSagaDbContext.SaveChangesAsync();
    }


    public async Task<T?> GetByIdAsync(Guid id)
    {
        T? result = await _userCreatedSagaDbContext.Set<T>().FindAsync(id);
        return result;
    }


    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _userCreatedSagaDbContext.Set<T>().ToListAsync();
    }

}
