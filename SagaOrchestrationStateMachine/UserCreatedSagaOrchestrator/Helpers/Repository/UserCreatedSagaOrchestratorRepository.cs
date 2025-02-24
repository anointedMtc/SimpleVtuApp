//using DomainSharedKernel.Interfaces;
//using InfrastructureSharedKernel.SpecificationHelper;
//using MassTransit.EntityFrameworkCoreIntegration;
//using Microsoft.EntityFrameworkCore;
//using SagaOrchestrationStateMachines.Common.Interfaces;

//namespace SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator.Helpers.Repository;

//public class UserCreatedSagaOrchestratorRepository : IMySagaRepository<UserCreatedSagaStateInstance> 
//{
//    //private readonly UserCreatedSagaDbContext _userCreatedSagaDbContext;

//    private readonly SagaDbContext _sagaDbContext;

//    public UserCreatedSagaOrchestratorRepository(SagaDbContext sagaDbContext)
//    {
//        //_userCreatedSagaDbContext = userCreatedSagaDbContext;
//        _sagaDbContext = sagaDbContext;
//    }

//    public async Task<IEnumerable<UserCreatedSagaStateInstance>> GetAllAsync(ISpecification<T> specification = null)
//    {
//        return ApplySpecification(specification);
//    }


//    public async Task<T?> FindAsync(ISpecification<T> specification = null)
//    {
//        return await ApplySpecification(specification).FirstOrDefaultAsync();
//    }

//    public async Task<int> CountAsync(ISpecification<T> specification)
//    {
//        return await ApplySpecification(specification).CountAsync();
//    }

//    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
//    {

//        return SpecificationEvaluator<T>.GetQuery(_sagaDbContext.Set<T>().AsQueryable(), spec);

//    }


//    public async Task<T> AddAsync(T entity)
//    {
//        await _sagaDbContext.Set<T>().AddAsync(entity);
//        await _sagaDbContext.SaveChangesAsync();
//        return entity;
//    }

//    public async Task UpdateAsync(T entity)
//    {
//        _sagaDbContext.Entry(entity).State = EntityState.Modified;
//        await _sagaDbContext.SaveChangesAsync();
//    }

//    public async Task DeleteAsync(T entity)
//    {
//        _sagaDbContext.Set<T>().Remove(entity);
//        await _sagaDbContext.SaveChangesAsync();
//    }


//    public async Task<T?> GetByIdAsync(Guid id)
//    {
//        T? result = await _sagaDbContext.Set<T>().FindAsync(id);
//        return result;
//    }


//    public async Task<IEnumerable<T>> GetAllAsync()
//    {
//        return await _sagaDbContext.Set<T>().ToListAsync();
//    }

//}
