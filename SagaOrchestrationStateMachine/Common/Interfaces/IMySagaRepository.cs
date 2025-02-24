//using DomainSharedKernel.Interfaces;
//using SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator;

//namespace SagaOrchestrationStateMachines.Common.Interfaces;

//public interface IMySagaRepository<UserCreatedSagaStateInstance>
//{
//    Task<IEnumerable<UserCreatedSagaStateInstance>> GetAllAsync(ISpecification<UserCreatedSagaStateInstance> specification = null);

//    Task<UserCreatedSagaStateInstance?> FindAsync(ISpecification<UserCreatedSagaStateInstance> specification = null);

//    Task<int> CountAsync(ISpecification<UserCreatedSagaStateInstance> specification);


//    // WRITE OPERATIONS
//    Task<UserCreatedSagaStateInstance> AddAsync(UserCreatedSagaStateInstance entity);
//    Task UpdateAsync(UserCreatedSagaStateInstance entity);
//    Task DeleteAsync(UserCreatedSagaStateInstance entity);


//    // For use within handlers
//    Task<UserCreatedSagaStateInstance?> GetByIdAsync(Guid id);

//    Task<IEnumerable<UserCreatedSagaStateInstance>> GetAllAsync();
//}
